namespace CYQ.Data.Table
{
    using CYQ.Data.SQL;
    using System;
    using System.Data;

    public class CellStruct
    {
        public string ColumnName;
        public bool IsCanNull;
        public bool IsReadOnly;
        public int MaxSize;
        public string Operator = "=";
        public ParameterDirection ParaDirection;
        public SqlDbType SqlType;
        internal Type ValueType;

        public CellStruct(string columnName, SqlDbType sqlType, bool isReadOnly, bool isCanNull, int maxSize, ParameterDirection paraDirection)
        {
            this.ColumnName = columnName;
            this.SqlType = sqlType;
            this.IsReadOnly = isReadOnly;
            this.IsCanNull = isCanNull;
            this.MaxSize = maxSize;
            this.ParaDirection = paraDirection;
            this.ValueType = DataType.GetType(sqlType);
        }
    }
}

