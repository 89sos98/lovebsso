namespace CYQ.Data.DAL
{
    using System;

    internal class PageSQL
    {
        private const string AccessPage = "select top {0} * from {1}\r\n            where ID >= \r\n\t            (SELECT max(ID) FROM \r\n\t\t            (select top {2} ID from {1} where {3} ) as t\r\n\t            ) and {3}";
        private const string AccesspageAll = "select * from {0} where {1}";

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
            return string.Format("select top {0} * from {1}\r\n            where ID >= \r\n\t            (SELECT max(ID) FROM \r\n\t\t            (select top {2} ID from {1} where {3} ) as t\r\n\t            ) and {3}", new object[] { pageSize, tableName, num, where });
        }
    }
}

