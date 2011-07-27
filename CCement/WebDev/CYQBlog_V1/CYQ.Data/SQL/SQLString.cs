namespace CYQ.Data.SQL
{
    using CYQ.Data;
    using CYQ.Data.DAL;
    using CYQ.Data.Table;
    using System;
    using System.Data;
    using System.Text;

    internal class SQLString
    {
        private static string _AutoID = null;
        private DbBase _DataSqlHelper;
        internal bool _IsCanDo;
        internal MDataRow _Row;
        private string _TableName;
        public static string[] filterKey = new string[] { 
            "select", "master", "delete", "drop", "update", "truncate", "create", "exists", "insert", "asc(", "while", "xp_cmdshell", "add", "declare", "exec", "ch", 
            "ch(", "delay", "waitfor", "sleep"
         };

        public SQLString(ref MDataRow row, ref DbBase helper)
        {
            this._DataSqlHelper = helper;
            this.SetRow(ref row);
        }

        private static string FilterChar(string text)
        {
            text = text.Replace("--", "").Replace(";", "").Replace("&", "").Replace("*", "").Replace("||", "");
            string str = text.ToLower();
            string[] strArray = str.Split(new char[] { ' ' });
            if ((strArray.Length == 1) && (str.Length > 30))
            {
                Log.WriteLog(text);
                if (text.IndexOf("%20") > -1)
                {
                    return string.Empty;
                }
            }
            bool flag = false;
            string str2 = string.Empty;
            for (int i = 0; i < filterKey.Length; i++)
            {
                if (str.IndexOf(filterKey[i]) <= -1)
                {
                    continue;
                }
                foreach (string str3 in strArray)
                {
                    str2 = str3.Trim(new char[] { '\'', ')', '|', '!', '%', '^', '(' });
                    if ((str2.IndexOf(filterKey[i]) > -1) && (str2.Length > filterKey[i].Length))
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    Log.WriteLog(filterKey[i] + text);
                    return string.Empty;
                }
                flag = false;
            }
            return text;
        }

        public static string FormatCharIndex(string text, bool sql, string key)
        {
            int index = text.IndexOf(key);
            if (index > -1)
            {
                if (!sql)
                {
                    int num2 = 0;
                    do
                    {
                        int num3 = index + key.Length;
                        text = text.Insert(index + 2, "_");
                        num2 = text.IndexOf(')', index + 4);
                        string[] strArray = text.Substring(num3 + 2, (num2 - num3) - 2).Split(new char[] { ',' });
                        text = text.Remove(num3 + 2, (num2 - num3) - 2);
                        text = text.Insert(num3 + 2, strArray[1] + "," + strArray[0]);
                        index = text.IndexOf(key);
                    }
                    while (index > -1);
                    text = text.Replace("#_", "#");
                }
                text = sql ? text.Replace(key, "CHARINDEX") : text.Replace(key, "instr");
            }
            return text;
        }

        internal static string FormatDal(string text, DalType dalType, bool filter)
        {
            if (filter)
            {
                text = FilterChar(text);
            }
            if (!string.IsNullOrEmpty(text))
            {
                text = ReplaceFunA(text, dalType);
                switch (dalType)
                {
                    case DalType.Sql:
                        text = ReplaceValue(text, false);
                        text = ReplaceFunB(text, true);
                        text = ReplaceFunC(text, true);
                        return text;

                    case DalType.Access:
                        text = ReplaceValue(text, true);
                        text = ReplaceFunB(text, true);
                        text = ReplaceFunC(text, false);
                        return text;

                    case DalType.Oracle:
                        text = ReplaceValue(text, false);
                        text = ReplaceFunB(text, false);
                        text = ReplaceFunC(text, false);
                        return text;
                }
            }
            return text;
        }

        private static string FormatDate(string text, bool AorS, string key, string format, string func)
        {
            int index = text.IndexOf(key);
            if (index > -1)
            {
                if (!AorS)
                {
                    int startIndex = 0;
                    do
                    {
                        text = text.Insert(index + 2, "_");
                        startIndex = text.IndexOf(')', index + 4);
                        text = text.Insert(startIndex, format);
                        index = text.IndexOf(key);
                    }
                    while (index > -1);
                    text = text.Replace("#_", "#");
                }
                text = AorS ? text.Replace(key, func) : text.Replace(key, "to_char");
            }
            return text;
        }

        public static string FormatDateDiff(string text, DalType dalType)
        {
            string str = "[#DATEDIFF]";
            if (text.IndexOf(str) > -1)
            {
                text = text.Replace(str, "DateDiff");
                string[] strArray = new string[] { "yyyy", "q", "m", "y", "d", "h", "ww", "n", "s" };
                switch (dalType)
                {
                    case DalType.Sql:
                        text = text.Replace("[#h]", "hh");
                        text = text.Replace("[#GETDATE]", "getdate()");
                        foreach (string str3 in strArray)
                        {
                            text = text.Replace("[#" + str3 + "]", str3);
                        }
                        return text;

                    case DalType.Access:
                        text = text.Replace("[#GETDATE]", "now()");
                        foreach (string str2 in strArray)
                        {
                            text = text.Replace("[#" + str2 + "]", "'" + str2 + "'");
                        }
                        return text;

                    case DalType.Oracle:
                        text = text.Replace("[#GETDATE]", "sysdate");
                        foreach (string str4 in strArray)
                        {
                            text = text.Replace("[#" + str4 + "]", "'" + str4 + "'");
                        }
                        return text;
                }
            }
            return text;
        }

        internal string FormatWhere(object beFormatWhere)
        {
            string text = Convert.ToString(beFormatWhere);
            string str2 = text.ToLower();
            if (text == "")
            {
                return "";
            }
            text = FormatDal(text, this._DataSqlHelper.dalType, true);
            if (((string.IsNullOrEmpty(text) || (str2.IndexOfAny(new char[] { '=', '>', '<' }) != -1)) || (str2.Contains("like") || str2.Contains("between"))) || str2.Contains("in"))
            {
                return text;
            }
            if (DataType.GetGroupID(this._Row[0]._CellStruct.SqlType) == 1)
            {
                return (this._Row[0]._CellStruct.ColumnName + "=" + text);
            }
            return (this._Row[0]._CellStruct.ColumnName + "='" + text + "'");
        }

        internal string GetBindSql(string where, object text, object value)
        {
            if (!string.IsNullOrEmpty(where))
            {
                where = " where " + where;
            }
            return string.Format("select distinct {0},{1} from {2} {3}", new object[] { text.ToString(), value.ToString(), this._TableName, this.FormatWhere(where) });
        }

        internal string GetCountSql(string where)
        {
            if (!string.IsNullOrEmpty(where))
            {
                where = " where " + where;
            }
            where = this.RemoveOrderBy(where);
            return ("select count(*) from " + this._TableName + this.FormatWhere(where));
        }

        internal string GetDeleteSql(object where)
        {
            return ("delete  from " + this._TableName + " Where " + this.FormatWhere(where));
        }

        internal string GetInsertSql()
        {
            this._IsCanDo = true;
            StringBuilder builder = new StringBuilder();
            StringBuilder builder2 = new StringBuilder();
            builder.Append("insert into " + this._TableName + "(");
            builder2.Append(") Values(");
            int groupID = DataType.GetGroupID(this._Row[0]._CellStruct.SqlType);
            if ((groupID == 0) && (this._Row[0].Value == null))
            {
                this._Row[0].Value = Guid.NewGuid();
            }
            for (int i = groupID; i < this._Row.Count; i++)
            {
                if (!this._Row[i]._CellStruct.IsCanNull && this._Row[i]._CellValue.IsNull)
                {
                    this._IsCanDo = false;
                    break;
                }
                if (this._Row[i]._CellValue.IsChange && !this._Row[i]._CellValue.IsNull)
                {
                    builder.Append("[" + this._Row[i]._CellStruct.ColumnName + "],");
                    builder2.Append(this._DataSqlHelper.Pre + this._Row[i]._CellStruct.ColumnName + ",");
                    this._DataSqlHelper.AddParameters(this._DataSqlHelper.Pre + this._Row[i]._CellStruct.ColumnName, this._Row[i]._CellValue.Value, DataType.GetDbType(this._Row[i]._CellStruct.ValueType), this._Row[i]._CellStruct.MaxSize, ParameterDirection.Input);
                }
            }
            if (this._DataSqlHelper.dalType == DalType.Oracle)
            {
                builder = builder.Replace("[", "").Replace("]", "");
                builder.Append(this._Row[0]._CellStruct.ColumnName + ",");
                builder2.Append(AutoID + ".nextval,");
            }
            string str = builder.ToString().TrimEnd(new char[] { ',' }) + builder2.ToString().TrimEnd(new char[] { ',' }) + ")";
            if (this._DataSqlHelper.dalType == DalType.Sql)
            {
                str = str + ((groupID == 1) ? " select cast(scope_Identity() as int) as OutPutValue" : string.Format(" select '{0}' as OutPutValue", this._Row[0].Value));
            }
            return str;
        }

        internal string GetMaxID()
        {
            switch (this._DataSqlHelper.dalType)
            {
                case DalType.Access:
                    return ("select max(ID) from " + this._TableName);

                case DalType.Oracle:
                    return string.Format("select {0}.currval from dual", AutoID);
            }
            return "";
        }

        internal string GetTopOneSql(object where)
        {
            switch (this._DataSqlHelper.dalType)
            {
                case DalType.Sql:
                case DalType.Access:
                    return ("select top 1 * from " + this._TableName + " where " + this.FormatWhere(where));

                case DalType.Oracle:
                    return ("select * from " + this._TableName + " where rownum=1 and " + this.FormatWhere(where));
            }
            return "";
        }

        internal string GetUpdateSql(object where)
        {
            this._IsCanDo = true;
            StringBuilder builder = new StringBuilder();
            builder.Append("Update " + this._TableName + " set ");
            for (int i = 1; i < this._Row.Count; i++)
            {
                if (this._Row[i]._CellValue.IsChange && !this._Row[i]._CellValue.IsNull)
                {
                    this._DataSqlHelper.AddParameters(this._DataSqlHelper.Pre + this._Row[i]._CellStruct.ColumnName, this._Row[i]._CellValue.Value, DataType.GetDbType(this._Row[i]._CellStruct.ValueType), this._Row[i]._CellStruct.MaxSize, ParameterDirection.Input);
                    builder.Append("[" + this._Row[i]._CellStruct.ColumnName + "]=" + this._DataSqlHelper.Pre + this._Row[i]._CellStruct.ColumnName + ",");
                }
            }
            if (this._DataSqlHelper.dalType == DalType.Oracle)
            {
                builder = builder.Replace("[", "").Replace("]", "");
            }
            builder = builder.Remove(builder.Length - 1, 1);
            builder.Append(" where " + this.FormatWhere(where));
            return builder.ToString();
        }

        internal string RemoveOrderBy(string where)
        {
            where = where.ToLower();
            int index = where.IndexOf("order by");
            if (index > 0)
            {
                where = where.Substring(0, index);
            }
            return where;
        }

        private static string ReplaceFunA(string text, DalType dalType)
        {
            text = FormatDateDiff(text, dalType);
            switch (dalType)
            {
                case DalType.Sql:
                case DalType.Access:
                    return text.Replace("[#LEN]", "len").Replace("[#SUBSTRING]", "substring").Replace("[#GETDATE]", (dalType == DalType.Access) ? "now()" : "getdate()");

                case DalType.Oracle:
                    return text.Replace("[#LEN]", "length").Replace("[#SUBSTRING]", "substr").Replace("[#GETDATE]", "to_char(current_date,'dd-mon-yyyy hh:mi:ss')");
            }
            return text;
        }

        private static string ReplaceFunB(string text, bool AorS)
        {
            text = FormatDate(text, AorS, "[#YEAR]", ",'yyyy'", "Year");
            text = FormatDate(text, AorS, "[#MONTH]", ",'MM'", "Month");
            text = FormatDate(text, AorS, "[#DAY]", ",'dd'", "Day");
            return text;
        }

        private static string ReplaceFunC(string text, bool sql)
        {
            return FormatCharIndex(text, sql, "[#CHARINDEX]");
        }

        private static string ReplaceValue(string text, bool access)
        {
            if (!access)
            {
                return text.Replace("[#TRUE]", "1").Replace("[#FALSE]", "0").Replace("[#DESC]", "desc").Replace("[#ASC]", "asc");
            }
            return text.Replace("[#TRUE]", "true").Replace("[#FALSE]", "false").Replace("[#DESC]", "asc").Replace("[#ASC]", "desc");
        }

        public void SetRow(ref MDataRow row)
        {
            this._Row = row;
            this._TableName = row.TableName;
        }

        public static string AutoID
        {
            get
            {
                if (_AutoID == null)
                {
                    _AutoID = AppConfig.AutoID;
                    if (string.IsNullOrEmpty(_AutoID))
                    {
                        _AutoID = "AutoID";
                    }
                }
                return _AutoID;
            }
        }
    }
}

