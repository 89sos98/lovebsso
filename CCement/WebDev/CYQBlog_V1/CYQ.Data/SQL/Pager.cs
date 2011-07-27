namespace CYQ.Data.SQL
{
    using System;
    using System.Text;

    internal class Pager
    {
        private const string AccessPage = "select top {0} * from {1}\r\n            where ID >= \r\n\t            (SELECT max(ID) FROM \r\n\t\t            (select top {2} ID from {1} where {3} ) as t\r\n\t            ) and {4}";
        private const string AccesspageAll = "select * from {0} where {1}";
        private static StringBuilder sqlText = new StringBuilder();

        public static string GetAccess(int pageIndex, int pageSize, string where, string tableName)
        {
            if (string.IsNullOrEmpty(where))
            {
                where = "1=1";
            }
            if ((pageIndex == 0) || (pageSize == 0))
            {
                return string.Format("select * from {0} where {1}", tableName, where);
            }
            int num = ((pageIndex - 1) * pageSize) + 1;
            return string.Format("select top {0} * from {1}\r\n            where ID >= \r\n\t            (SELECT max(ID) FROM \r\n\t\t            (select top {2} ID from {1} where {3} ) as t\r\n\t            ) and {4}", new object[] { pageSize, tableName, num, RemoveOrderBy(where), where });
        }

        public static string GetPackageBodyForOracle()
        {
            return "\r\ncreate or replace package Body MyPackage is \r\n procedure SelectBase(pageIndex int,pageSize int,tableName varchar2,whereStr varchar2,\r\n  resultCount out int, resultCursor out MyCursor)\r\n  is\r\n  rowStart  int;\r\n  rowEnd    int;\r\n  mySql varchar2(8000);\r\n  whereOnly varchar2(8000);\r\n  OrderOnly varchar2(400);\r\n  begin\r\n    mySql:='select count(*) from '||tableName;\r\n    whereOnly:=whereStr;\r\n    rowStart:=instr(whereStr,'order by');\r\n    if whereStr is not null and  rowStart>0 \r\n      then\r\n        whereOnly:=substr(whereStr, 1,rowStart-1);\r\n        OrderOnly:=substr(whereStr,rowStart, length(whereStr)-rowStart+1);\r\n        end if;  \r\n    if length(whereOnly)>1\r\n      then\r\n       whereOnly:=' where '|| whereOnly;\r\n       mySql:=mySql||whereOnly;\r\n        end if;\r\n        execute immediate mySql into resultCount;\r\n\r\n        if pageIndex=0 and pageSize=0\t\r\n        then \r\n        mySql:='select * from '||tableName||whereOnly||OrderOnly;\r\n       else\r\n\r\n        rowStart:=(pageIndex-1)*pageSize+1; \r\n        rowEnd:=rowStart+pageSize-1;\r\n        mySql:='select * from (select t.*,RowNum as rn from ('||tableName||') t'||whereOnly||OrderOnly||') where rn between '||rowStart||' and '||rowEnd; \r\n        end if;\r\n    open ResultCursor for mySql;\r\n    end SelectBase;\r\n  end MyPackage;";
        }

        public static string GetPackageHeadForOracle()
        {
            return "\r\n           create or replace package MyPackage as \r\ntype MyCursor is ref cursor;\r\nprocedure SelectBase(pageIndex int,pageSize int,tableName varchar2,whereStr varchar2,\r\n  resultCount out int, resultCursor out MyCursor);\r\nend MyPackage;";
        }

        public static string GetSelectBaseOutPutToHtmlForOracle()
        {
            return (GetPackageHeadForOracle() + GetPackageBodyForOracle());
        }

        public static string GetSelectBaseOutPutToHtmlForSql2000()
        {
            sqlText.Remove(0, sqlText.Length);
            sqlText.Append("create procedure SelectBase <br />");
            sqlText.Append("@PageIndex\t\t       int, <br />");
            sqlText.Append("@PageSize\t\t\t   int, <br />");
            sqlText.Append("@TableName    nvarchar(4000), <br />");
            sqlText.Append("@Where \t   nvarchar(2000)='' <br />");
            sqlText.Append("as <br />");
            sqlText.Append("Declare @rowcount  \t\tint <br />");
            sqlText.Append("Declare @intStart  \t\tint <br />");
            sqlText.Append("Declare @intEnd         int <br />");
            sqlText.Append("declare @Column1 varchar(32) <br />");
            sqlText.Append("Declare @Sql nvarchar(2000), @WhereR nvarchar(1000), @OrderBy nvarchar(1000) <br />");
            sqlText.Append("set @rowcount=0 <br />");
            sqlText.Append("set nocount on <br />");
            sqlText.Append("if @Where<>'' <br />");
            sqlText.Append("begin <br />");
            sqlText.Append("set @Where=' and '+@Where <br />");
            sqlText.Append("end <br />");
            sqlText.Append("if CHARINDEX('order by', @Where)>0 <br />");
            sqlText.Append("begin <br />");
            sqlText.Append("set @WhereR=substring(@Where, 1, CHARINDEX('order by',@Where)-1)\t--取得条件 <br />");
            sqlText.Append("set @OrderBy=substring(@Where, CHARINDEX('order by',@Where), Len(@Where))\t--取得排序方式(order by 字段 方式) <br />");
            sqlText.Append("end <br />");
            sqlText.Append("else <br />");
            sqlText.Append("begin <br />");
            sqlText.Append("set @WhereR=@Where <br />");
            sqlText.Append("set @OrderBy=' order by id asc' <br />");
            sqlText.Append("end <br />");
            sqlText.Append("set @Sql='SELECT @rowcount=count(*) from '+cast(@TableName as varchar(4000))+' where 1=1 '+@WhereR <br />");
            sqlText.Append("exec sp_executeSql @Sql,N'@rowcount int output',@rowcount output <br />");
            sqlText.Append("if @PageIndex=0 and @PageSize=0\t--不进行分页,查询所有数据列表 <br />");
            sqlText.Append("begin <br />");
            sqlText.Append("set @Sql='SELECT * from '+cast(@TableName as varchar(4000))+' where 1=1 '+@Where <br />");
            sqlText.Append("end <br />");
            sqlText.Append("else\t--进行分页查询数据列表 <br />");
            sqlText.Append("begin <br />");
            sqlText.Append("set @intStart=(@PageIndex-1)*@PageSize+1; <br />");
            sqlText.Append("set @intEnd=@intStart+@PageSize-1 <br />");
            sqlText.Append("set @Column1=col_name(object_id(@tableName),1) <br />");
            sqlText.Append("set @Sql='Create table #tem(tempID int identity(1,1) not null,Row int) '  <br />");
            sqlText.Append("set @Sql=@Sql+'insert #tem(Row) select '+@Column1+' from '+@TableName+' where 1=1 '+@Where  <br />");
            sqlText.Append("set @Sql=@Sql+' select t.* from '+@TableName+' t left join #tem  on t.'+@Column1+'=#tem.Row '  <br />");
            sqlText.Append("set @Sql=@Sql+' where  #tem.tempID between '+cast(@intStart as varchar)+' and '+cast(@intEnd as varchar) <br />");
            sqlText.Append("end <br />");
            sqlText.Append("exec sp_executeSql @Sql <br />");
            sqlText.Append("return @rowcount <br />");
            sqlText.Append("set nocount off <br />");
            return sqlText.ToString();
        }

        public static string GetSelectBaseOutPutToHtmlForSql2005()
        {
            sqlText.Remove(0, sqlText.Length);
            sqlText.Append("create procedure SelectBase <br />");
            sqlText.Append("@PageIndex\t\t       int, <br />");
            sqlText.Append("@PageSize\t\t\t   int, <br />");
            sqlText.Append("@TableName    nvarchar(4000), <br />");
            sqlText.Append("@Where \t   nvarchar(max)='' <br />");
            sqlText.Append("as <br />");
            sqlText.Append("Declare @rowcount  \t\tint <br />");
            sqlText.Append("Declare @intStart  \t\tint <br />");
            sqlText.Append("Declare @intEnd         int <br />");
            sqlText.Append("Declare @Sql nvarchar(max), @WhereR nvarchar(max), @OrderBy nvarchar(max) <br />");
            sqlText.Append("set @rowcount=0 <br />");
            sqlText.Append("set nocount on <br />");
            sqlText.Append("if @Where<>'' <br />");
            sqlText.Append("begin <br />");
            sqlText.Append("set @Where=' and '+@Where <br />");
            sqlText.Append("end <br />");
            sqlText.Append("if CHARINDEX('order by', @Where)>0 <br />");
            sqlText.Append("begin <br />");
            sqlText.Append("set @WhereR=substring(@Where, 1, CHARINDEX('order by',@Where)-1)\t--取得条件 <br />");
            sqlText.Append("set @OrderBy=substring(@Where, CHARINDEX('order by',@Where), Len(@Where))\t--取得排序方式(order by 字段 方式) <br />");
            sqlText.Append("end <br />");
            sqlText.Append("else <br />");
            sqlText.Append("begin <br />");
            sqlText.Append("set @WhereR=@Where <br />");
            sqlText.Append("set @OrderBy=' order by id asc' <br />");
            sqlText.Append("end <br />");
            sqlText.Append("set @Sql='SELECT @rowcount=count(*) from '+cast(@TableName as varchar(4000))+' where 1=1 '+@WhereR <br />");
            sqlText.Append("exec sp_executeSql @Sql,N'@rowcount int output',@rowcount output <br />");
            sqlText.Append("if @PageIndex=0 and @PageSize=0\t--不进行分页,查询所有数据列表 <br />");
            sqlText.Append("begin <br />");
            sqlText.Append("set @Sql='SELECT * from '+cast(@TableName as varchar(4000))+' where 1=1 '+@Where <br />");
            sqlText.Append("end <br />");
            sqlText.Append("else\t--进行分页查询数据列表 <br />");
            sqlText.Append("begin <br />");
            sqlText.Append("set @intStart=(@PageIndex-1)*@PageSize+1; <br />");
            sqlText.Append("set @intEnd=@intStart+@PageSize-1 <br />");
            sqlText.Append("set @Sql='select * from(select *,ROW_NUMBER() OVER('+cast(@OrderBy as nvarchar)+') as row from ' <br />");
            sqlText.Append("set @Sql=@Sql+@TableName+' where 1=1 '+@WhereR+') as a where row between '+cast(@intStart as varchar)+' and '+cast(@intEnd as varchar) <br />");
            sqlText.Append("end <br />");
            sqlText.Append("exec sp_executeSql @Sql <br />");
            sqlText.Append("return @rowcount <br />");
            sqlText.Append("set nocount off <br />");
            return sqlText.ToString();
        }

        private static string RemoveOrderBy(string where)
        {
            int index = where.ToLower().IndexOf("order by");
            if (index > -1)
            {
                where = where.Remove(index);
            }
            return where;
        }
    }
}

