namespace CYQ.Data.DAL
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;

    internal class SQLHelper : DbBase
    {
        public SQLHelper(string conn, string providerName) : base(conn, providerName)
        {
        }

        internal override void AddCustomePara(string paraName, ParaType paraType)
        {
            SqlParameter parameter;
            switch (paraType)
            {
                case ParaType.OutPut:
                case ParaType.ReturnValue:
                    parameter = new SqlParameter();
                    parameter.ParameterName = paraName;
                    parameter.SqlDbType = SqlDbType.Int;
                    if (paraType != ParaType.OutPut)
                    {
                        parameter.Direction = ParameterDirection.ReturnValue;
                        break;
                    }
                    parameter.Direction = ParameterDirection.Output;
                    break;

                default:
                    return;
            }
            base.Com.Parameters.Add(parameter);
        }

        public void AddParameters(string parameterName, object value, DbType dbType, int size)
        {
            this.AddParameters(parameterName, value, dbType, size, ParameterDirection.Input);
        }

        public override void AddReturnPara()
        {
            this.AddParameters("ReturnValue", null, DbType.Int32, 0x20, ParameterDirection.ReturnValue);
        }

        public override DbParameter GetNewParameter()
        {
            return new SqlParameter();
        }
    }
}

