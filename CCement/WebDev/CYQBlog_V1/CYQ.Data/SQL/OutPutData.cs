namespace CYQ.Data.SQL
{
    using CYQ.Data.DAL;
    using CYQ.Data.Table;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class OutPutData
    {
        private string databaseName;
        private DbBase helper;
        private DbBase sqColumnlHelper;

        public OutPutData()
        {
            this.databaseName = string.Empty;
            this.helper = DalAction.GetHelper(string.Empty);
            this.sqColumnlHelper = DalAction.GetHelper(string.Empty);
            this.databaseName = this.helper.Con.Database;
        }

        public OutPutData(string conn)
        {
            this.databaseName = string.Empty;
            this.helper = DalAction.GetHelper(conn);
            this.sqColumnlHelper = DalAction.GetHelper(conn);
            this.databaseName = (this.helper.Con.Database == "") ? this.helper.Con.DataSource : this.helper.Con.Database;
        }

        public bool ExeCreateLogTable(DataBaseType dataBaseType)
        {
            this.helper.WriteLog = false;
            string procName = "";
            string createLogTableSql = "";
            switch (dataBaseType)
            {
                case DataBaseType.Sql2000:
                    procName = ProcedureSql.IsLogTableExist2000();
                    createLogTableSql = ProcedureSql.GetCreateLogTableSql();
                    break;

                case DataBaseType.Sql2005:
                    procName = ProcedureSql.IsLogTableExist2005();
                    createLogTableSql = ProcedureSql.GetCreateLogTableSql();
                    break;

                case DataBaseType.Oracle:
                    procName = ProcedureSql.IsLogTableExistOracle();
                    createLogTableSql = ProcedureSql.GetCreateLogTableSqlForOracle();
                    break;
            }
            object obj2 = this.helper.ExeScalar(procName, false);
            if ((obj2 == null) || (Convert.ToInt32(obj2) > 0))
            {
                this.helper.Dispose();
                throw new Exception("数据库表 ErrorLogs 已存在！");
            }
            int num = this.helper.ExeNonQuery(createLogTableSql, false);
            this.helper.Dispose();
            return (num > 0);
        }

        public bool ExeCreateProc(DataBaseType dataBaseType)
        {
            string procName = "";
            string str2 = "";
            switch (dataBaseType)
            {
                case DataBaseType.Sql2000:
                    procName = GetSelectBaseOutPutToHtmlForSql2000().Replace("<br />", "\r\n");
                    break;

                case DataBaseType.Sql2005:
                    procName = GetSelectBaseOutPutToHtmlForSql2005().Replace("<br />", "\r\n");
                    break;

                case DataBaseType.Oracle:
                    procName = Pager.GetPackageHeadForOracle().Replace("\r\n", "");
                    str2 = Pager.GetPackageBodyForOracle().Replace("\r\n", "");
                    break;
            }
            if (procName == "")
            {
                return false;
            }
            this.helper.WriteLog = false;
            int num = this.helper.ExeNonQuery(procName, false);
            if ((num > 0) && (str2 != ""))
            {
                this.helper.ExeNonQuery(str2, false);
            }
            this.helper.Dispose();
            return (num > 0);
        }

        internal static MDataColumn GetColumn(string tableName, ref DbBase helper)
        {
            MDataColumn column = new MDataColumn();
            CellStruct item = null;
            switch (helper.dalType)
            {
                case DalType.Sql:
                case DalType.Oracle:
                {
                    string procName = (helper.dalType == DalType.Sql) ? ProcedureSql.GetTableColumnsByMSSQL() : ProcedureSql.GetTableColumnsByOracle(new string[] { helper.attachInfo });
                    helper.AddParameters("TableName", tableName, DbType.String, 50, ParameterDirection.Input);
                    DbDataReader reader = helper.ExeDataReader(procName, false);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            item = new CellStruct(reader["ColumnName"].ToString(), DataType.GetSqlType(reader["SqlType"].ToString()), Convert.ToBoolean(reader["ReadOnly"]), Convert.ToBoolean(reader["IsNullable"]), Convert.ToInt32(reader["MaxSize"]), ParameterDirection.InputOutput);
                            column.Add(item);
                        }
                        reader.Close();
                    }
                    break;
                }
                case DalType.Access:
                {
                    DataTable table = helper.ExeDataTable(string.Format("select * from {0} where 1=2", tableName), false);
                    if (table != null)
                    {
                        foreach (DataColumn column2 in table.Columns)
                        {
                            item = new CellStruct(column2.ColumnName, DataType.GetSqlType(column2.DataType), column2.ReadOnly, column2.AllowDBNull, column2.MaxLength, ParameterDirection.InputOutput);
                            column.Add(item);
                        }
                    }
                    break;
                }
            }
            helper.ClearParameters();
            return column;
        }

        public static string GetSelectBaseOutPutToHtmlForOracle()
        {
            return Pager.GetSelectBaseOutPutToHtmlForOracle().Replace("\r\n", "<br>");
        }

        public static string GetSelectBaseOutPutToHtmlForSql2000()
        {
            return Pager.GetSelectBaseOutPutToHtmlForSql2000();
        }

        public static string GetSelectBaseOutPutToHtmlForSql2005()
        {
            return Pager.GetSelectBaseOutPutToHtmlForSql2005();
        }

        public string OutPutAllTableEnum(TableType tableType, DataBaseType dataBaseType, bool isMutilDataBase)
        {
            string str3;
            DbDataReader reader;
            string str = string.Format("namespace CYQ.Entity.{0} {{<br>", this.databaseName);
            if (tableType == TableType.V)
            {
                str = str + (isMutilDataBase ? string.Format(" public enum V_{0}Enum {{", this.databaseName) : "public enum ViewNames{");
            }
            else if (tableType == TableType.P)
            {
                str = str + (isMutilDataBase ? string.Format(" public enum P_{0}Enum {{", this.databaseName) : "public enum ProcNames{");
            }
            else
            {
                str = str + (isMutilDataBase ? string.Format(" public enum U_{0}Enum {{", this.databaseName) : "public enum TableNames{");
            }
            StringBuilder builder = new StringBuilder();
            string procName = "";
            switch (dataBaseType)
            {
                case DataBaseType.Sql2000:
                    procName = "select name from sysobjects where status>0 and  xtype='" + tableType.ToString() + "'";
                    goto Label_0132;

                case DataBaseType.Sql2005:
                    procName = "select name from sys.objects where type='" + tableType.ToString() + "'";
                    goto Label_0132;

                case DataBaseType.Access:
                    procName = "select name from MSysObjects where flags=0 and Type=1";
                    goto Label_0132;

                case DataBaseType.Oracle:
                    str3 = "";
                    switch (tableType)
                    {
                        case TableType.V:
                            str3 = "VIEW";
                            break;

                        case TableType.U:
                            str3 = "TABLE";
                            break;

                        case TableType.P:
                            str3 = "PROCEDURE";
                            break;
                    }
                    break;

                default:
                    goto Label_0132;
            }
            procName = string.Format("Select object_name as name From user_objects Where object_type='{0}'", str3);
        Label_0132:
            reader = this.helper.ExeDataReader(procName, false);
            if (reader != null)
            {
                while (reader.Read())
                {
                    string tableName = Convert.ToString(reader["name"]);
                    if ((((tableName.ToLower() != "sysdiagrams") && (tableName.ToLower() != "selectbase")) && (!tableName.Contains("sp_") || !tableName.Contains("diagram"))) && !tableName.Contains("$"))
                    {
                        str = str + tableName + ",";
                        builder.Append(this.OutPutSingleTableFiledEnum(tableType, tableName, false));
                        this.sqColumnlHelper.ClearParameters();
                    }
                }
                reader.Close();
            }
            this.sqColumnlHelper.Dispose();
            this.helper.Dispose();
            str = str.TrimEnd(new char[] { ',' }) + "}";
            if (!string.IsNullOrEmpty(builder.ToString()))
            {
                str = str + "<br> #region 枚举 <br>" + builder.ToString() + " #endregion";
            }
            return (str + "<br>}");
        }

        public string OutPutSingleTableFiledEnum(TableType tableType, object tableName)
        {
            return this.OutPutSingleTableFiledEnum(tableType, tableName, true);
        }

        private string OutPutSingleTableFiledEnum(TableType tableType, object tableName, bool dispose)
        {
            string str = " public enum " + Convert.ToString(tableName) + " { ";
            if ((tableType == TableType.P) && (this.sqColumnlHelper.dalType == DalType.Oracle))
            {
                this.sqColumnlHelper.attachInfo = "p";
            }
            MDataColumn column = GetColumn(Convert.ToString(tableName), ref this.sqColumnlHelper);
            if (column.Count > 0)
            {
                for (int i = 0; i < column.Count; i++)
                {
                    str = str + column[i].ColumnName + ",";
                }
                str = str.TrimEnd(new char[] { ',' }) + "}<br>";
            }
            else
            {
                str = str + "}<br>";
            }
            if (dispose)
            {
                this.sqColumnlHelper.Dispose();
                this.helper.Dispose();
            }
            if (tableType == TableType.P)
            {
                str = str.Replace("@", "");
            }
            return str;
        }
    }
}

