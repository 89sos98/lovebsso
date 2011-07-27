namespace CYQ.Data.SQL
{
    using System;
    using System.Data;

    internal class DataType
    {
        public static DbType GetDbType(string typeName)
        {
            switch (typeName.ToLower())
            {
                case "char":
                case "ansistring":
                    return DbType.AnsiString;

                case "ansistringfixedlength":
                    return DbType.AnsiStringFixedLength;

                case "varbinary":
                case "binary":
                case "image":
                case "timestamp":
                case "byte[]":
                    return DbType.Binary;

                case "bit":
                case "boolean":
                    return DbType.Boolean;

                case "tinyint":
                case "byte":
                    return DbType.Byte;

                case "smallmoney":
                case "currency":
                    return DbType.Currency;

                case "date":
                    return DbType.Date;

                case "smalldatetime":
                case "datetime":
                    return DbType.DateTime;

                case "numeric":
                case "decimal":
                    return DbType.Decimal;

                case "money":
                case "double":
                    return DbType.Double;

                case "uniqueidentifier":
                case "guid":
                    return DbType.Guid;

                case "smallint":
                case "int16":
                case "uint16":
                    return DbType.Int16;

                case "int":
                case "int32":
                case "uint32":
                    return DbType.Int32;

                case "bigint":
                case "int64":
                case "uint64":
                    return DbType.Int64;

                case "variant":
                case "object":
                    return DbType.Object;

                case "sbyte":
                    return DbType.SByte;

                case "float":
                case "single":
                    return DbType.Single;

                case "text":
                case "string":
                case "varchar":
                case "nvarchar":
                    return DbType.String;

                case "nchar":
                case "stringfixedlength":
                    return DbType.StringFixedLength;

                case "time":
                    return DbType.Time;

                case "varnumeric":
                    return DbType.VarNumeric;

                case "xml":
                    return DbType.Xml;
            }
            return DbType.Object;
        }

        public static DbType GetDbType(Type type)
        {
            return GetDbType(type.Name.ToString());
        }

        internal static int GetGroupID(SqlDbType sqlDbType)
        {
            switch (sqlDbType)
            {
                case SqlDbType.BigInt:
                case SqlDbType.Decimal:
                case SqlDbType.Float:
                case SqlDbType.Int:
                case SqlDbType.Money:
                case SqlDbType.Real:
                case SqlDbType.SmallInt:
                case SqlDbType.TinyInt:
                    return 1;

                case SqlDbType.Char:
                case SqlDbType.NChar:
                case SqlDbType.NVarChar:
                case SqlDbType.UniqueIdentifier:
                case SqlDbType.Text:
                case SqlDbType.VarChar:
                    return 0;

                case SqlDbType.DateTime:
                case SqlDbType.SmallDateTime:
                    return 2;
            }
            return 0x3e7;
        }

        public static SqlDbType GetSqlType(string typeName)
        {
            switch (typeName.ToLower())
            {
                case "char":
                case "ansistring":
                    return SqlDbType.Char;

                case "varchar":
                case "nvarchar":
                case "nvarchar2":
                case "ansistringfixedlength":
                case "stringfixedlength":
                case "string":
                    return SqlDbType.NVarChar;

                case "binary":
                case "image":
                case "varbinary":
                case "timestamp":
                    return SqlDbType.Binary;

                case "bit":
                case "boolean":
                    return SqlDbType.Bit;

                case "tinyint":
                case "byte":
                case "sbyte":
                    return SqlDbType.TinyInt;

                case "money":
                case "currency":
                    return SqlDbType.Money;

                case "datetime":
                case "date":
                    return SqlDbType.DateTime;

                case "decimal":
                case "numeric":
                    return SqlDbType.Decimal;

                case "real":
                case "double":
                    return SqlDbType.Real;

                case "uniqueidentifier":
                case "guid":
                    return SqlDbType.UniqueIdentifier;

                case "smallint":
                case "int16":
                case "uint16":
                    return SqlDbType.SmallInt;

                case "int":
                case "int32":
                case "uint32":
                case "number":
                    return SqlDbType.Int;

                case "bigint":
                case "int64":
                case "uint64":
                case "varnumeric":
                    return SqlDbType.BigInt;

                case "variant":
                case "object":
                    return SqlDbType.Variant;

                case "float":
                case "single":
                    return SqlDbType.Float;

                case "smalldatetime":
                case "time":
                    return SqlDbType.SmallDateTime;

                case "xml":
                    return SqlDbType.Xml;

                case "ntext":
                    return SqlDbType.NText;

                case "text":
                    return SqlDbType.Text;
            }
            return SqlDbType.NVarChar;
        }

        public static SqlDbType GetSqlType(Type type)
        {
            return GetSqlType(type.Name.ToString());
        }

        internal static Type GetType(SqlDbType sqlType)
        {
            switch (sqlType)
            {
                case SqlDbType.BigInt:
                    return typeof(long);

                case SqlDbType.Binary:
                case SqlDbType.Image:
                case SqlDbType.Timestamp:
                case SqlDbType.VarBinary:
                    return typeof(byte[]);

                case SqlDbType.Bit:
                    return typeof(bool);

                case SqlDbType.Char:
                case SqlDbType.NChar:
                case SqlDbType.NText:
                case SqlDbType.NVarChar:
                case SqlDbType.Text:
                case SqlDbType.VarChar:
                    return typeof(string);

                case SqlDbType.DateTime:
                case SqlDbType.SmallDateTime:
                case SqlDbType.Date:
                case SqlDbType.DateTime2:
                case SqlDbType.DateTimeOffset:
                    return typeof(DateTime);

                case SqlDbType.Decimal:
                case SqlDbType.Money:
                case SqlDbType.SmallMoney:
                    return typeof(decimal);

                case SqlDbType.Float:
                    return typeof(double);

                case SqlDbType.Int:
                    return typeof(int);

                case SqlDbType.Real:
                    return typeof(float);

                case SqlDbType.UniqueIdentifier:
                    return typeof(Guid);

                case SqlDbType.SmallInt:
                    return typeof(short);

                case SqlDbType.TinyInt:
                    return typeof(byte);
            }
            return typeof(object);
        }
    }
}

