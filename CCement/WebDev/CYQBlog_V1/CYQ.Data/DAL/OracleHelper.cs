namespace CYQ.Data.DAL
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Data.OracleClient;

    internal class OracleHelper : DbBase
    {
        public OracleHelper(string conn, string providerName) : base(conn, providerName)
        {
        }

        internal override void AddCustomePara(string paraName, ParaType paraType)
        {
            OracleParameter parameter = new OracleParameter();
            parameter.ParameterName = paraName;
            switch (paraType)
            {
                case ParaType.Cursor:
                case ParaType.OutPut:
                    if (paraType != ParaType.Cursor)
                    {
                        parameter.OracleType = OracleType.Int32;
                        break;
                    }
                    parameter.OracleType = OracleType.Cursor;
                    break;

                case ParaType.ReturnValue:
                    parameter.OracleType = OracleType.Int32;
                    parameter.Direction = ParameterDirection.ReturnValue;
                    goto Label_004F;

                default:
                    goto Label_004F;
            }
            parameter.Direction = ParameterDirection.Output;
        Label_004F:
            base.Com.Parameters.Add(parameter);
        }

        public override void AddReturnPara()
        {
            if (!base.Com.Parameters.Contains("ResultCursor"))
            {
                this.AddCustomePara("ResultCursor", ParaType.Cursor);
            }
            if (!base.Com.Parameters.Contains("ResultCount"))
            {
                this.AddParameters("ResultCount", null, DbType.Int32, -1, ParameterDirection.Output);
            }
        }

        public override DbParameter GetNewParameter()
        {
            return new OracleParameter();
        }

        public override string Pre
        {
            get
            {
                return ":";
            }
        }
    }
}

