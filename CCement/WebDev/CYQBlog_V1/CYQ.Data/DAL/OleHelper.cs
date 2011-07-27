namespace CYQ.Data.DAL
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Data.OleDb;

    internal class OleHelper : DbBase
    {
        public OleHelper(string conn, string providerName) : base(conn, providerName)
        {
        }

        public override void AddParameters(string parameterName, object value, DbType dbType, int size, ParameterDirection direction)
        {
            parameterName = (parameterName.Substring(0, 1) == "@") ? parameterName : ("@" + parameterName);
            OleDbParameter parameter = new OleDbParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = value;
            if (dbType == DbType.DateTime)
            {
                parameter.OleDbType = OleDbType.DBTimeStamp;
                parameter.Value = Convert.ToString(value);
            }
            else
            {
                parameter.DbType = dbType;
                parameter.Value = value;
            }
            base.Com.Parameters.Add(parameter);
        }

        public override void AddReturnPara()
        {
        }

        public override DbParameter GetNewParameter()
        {
            return new OleDbParameter();
        }
    }
}

