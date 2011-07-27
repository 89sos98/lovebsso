namespace CYQ.Data.SQL
{
    using System;
    using System.Text;

    internal class ProcedureSql
    {
        private static StringBuilder sqlText = new StringBuilder();

        internal static string GetCreateLogTableSql()
        {
            return "\r\n    CREATE TABLE [dbo].[ErrorLogs](\r\n\t[ID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,\r\n\t[PageUrl] [nvarchar](2000)  NULL,\r\n\t[ErrorMessage] [nvarchar](4000) NULL,\r\n\t[LogTime] [datetime] NULL  DEFAULT (getdate())\r\n\t)";
        }

        internal static string GetCreateLogTableSqlForOracle()
        {
            return "create table ErrorLogs(ID number not null,PageUrl  nvarchar2(2000),ErrorMessage nvarchar2(4000),LogTime  date default sysdate,CONSTRAINT errorlogs_id_pk PRIMARY KEY (ID)) tablespace USERS";
        }

        internal static string GetTableColumnsByMSSQL()
        {
            sqlText.Remove(0, sqlText.Length);
            sqlText.Append("select s1.name as ColumnName,case s2.name when 'nvarchar' then s1.[prec] WHEN 'uniqueidentifier' THEN 36 ");
            sqlText.Append("WHEN 'ntext' THEN -1 WHEN 'text' THEN -1 WHEN 'image' THEN -1 else s1.[length] end  as [MaxSize],");
            sqlText.Append("isnullable as [IsNullable],colstat as [ReadOnly],s2.name as [SqlType] ");
            sqlText.Append("from syscolumns s1 right join systypes s2 on s2.xtype =s1.xtype  ");
            sqlText.Append("where id=object_id(@TableName)  and s2.name<>'sysname' order by ReadOnly desc  ");
            return sqlText.ToString();
        }

        public static string GetTableColumnsByOracle(params string[] flag)
        {
            if ((flag.Length > 0) && (flag[0] == "p"))
            {
                return " select argument_Name as ColumnName,-1 as MaxSize,0 as IsNullable,0 as ReadOnly,'int' as SqlType from user_arguments where object_name=upper(:TableName)";
            }
            return "select COLUMN_NAME as ColumnName,\r\n                    Data_length*2 as MaxSize,\r\n                    case NULLABLE when 'Y' then 1 else 0 end as IsNullable,\r\n                    0 as ReadOnly,\r\n                    DATA_TYPE as SqlType\r\n                    from USER_TAB_COLS where TABLE_NAME=upper(:TableName) order by COLUMN_ID";
        }

        internal static string IsLogTableExist2000()
        {
            return "SELECT count(*) FROM sysobjects WHERE id = OBJECT_ID(N'[dbo].[ErrorLogs]') AND xtype in (N'U')";
        }

        internal static string IsLogTableExist2005()
        {
            return "SELECT count(*) FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ErrorLogs]') AND type in (N'U')";
        }

        internal static string IsLogTableExistOracle()
        {
            return "Select count(*)  From user_objects Where object_type='TABLE' and object_name=upper('ErrorLogs')";
        }
    }
}

