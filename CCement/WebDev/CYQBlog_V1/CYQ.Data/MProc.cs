namespace CYQ.Data
{
    using CYQ.Data.Aop;
    using CYQ.Data.DAL;
    using CYQ.Data.Table;
    using System;
    using System.Data;

    public class MProc : IDisposable
    {
        private IAop _Aop;
        private string debugInfo;
        private DbBase helper;
        private bool isProc;
        private string procName;

        public MProc(object procNamesEnum)
        {
            this._Aop = new CYQ.Data.Aop.Aop();
            this.procName = string.Empty;
            this.isProc = true;
            this.debugInfo = string.Empty;
            this.Init(procNamesEnum, string.Empty);
        }

        public MProc(object procNamesEnum, string conn)
        {
            this._Aop = new CYQ.Data.Aop.Aop();
            this.procName = string.Empty;
            this.isProc = true;
            this.debugInfo = string.Empty;
            this.Init(procNamesEnum, conn);
        }

        public void Clear()
        {
            this.helper.ClearParameters();
        }

        public void Close()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            if (this.helper != null)
            {
                this.debugInfo = this.helper.attachInfo;
                this.helper.Dispose();
                this.helper = null;
            }
        }

        public void EndTransation()
        {
            if ((this.helper != null) && this.helper.openTrans)
            {
                this.helper.EndTransaction();
            }
        }

        public MDataTable ExeMDataTable(params object[] aopInfo)
        {
            this._Aop.Begin(AopEnum.ExeMDataTable, this.procName, aopInfo);
            MDataTable table = this.helper.ExeDataReader(this.procName, this.isProc);
            this._Aop.End(AopEnum.ExeMDataTable, table != null, null, aopInfo);
            return table;
        }

        public int ExeNonQuery(params object[] aopInfo)
        {
            this._Aop.Begin(AopEnum.ExeNonQuery, this.procName, aopInfo);
            int id = this.helper.ExeNonQuery(this.procName, this.isProc);
            this._Aop.End(AopEnum.ExeNonQuery, id > 0, id, aopInfo);
            return id;
        }

        public T ExeScalar<T>(params object[] aopInfo)
        {
            this._Aop.Begin(AopEnum.ExeScalar, this.procName, aopInfo);
            object obj2 = this.helper.ExeScalar(this.procName, this.isProc);
            bool success = obj2 != null;
            this._Aop.End(AopEnum.ExeScalar, success, success, aopInfo);
            if (!success)
            {
                return default(T);
            }
            string name = typeof(T).Name;
            if (name != null)
            {
                if (!(name == "Int32"))
                {
                    if (name == "String")
                    {
                        obj2 = obj2.ToString();
                    }
                }
                else
                {
                    obj2 = Convert.ToInt32(obj2);
                }
            }
            return (T) obj2;
        }

        private void helper_OnExceptionEvent(string msg)
        {
            this._Aop.OnError(msg);
        }

        public void Init(object procNamesEnum, string conn)
        {
            if (procNamesEnum is Enum)
            {
                string name = procNamesEnum.GetType().Name;
                if (name != "ProcNames")
                {
                    conn = name.Substring(2).Replace("Enum", "Conn");
                }
            }
            this.procName = procNamesEnum.ToString().Trim();
            if (this.procName.Contains(" ") && (this.procName.Length > 0x10))
            {
                this.isProc = false;
            }
            this.helper = DalAction.GetHelper(conn);
            IAop fromConfig = this._Aop.GetFromConfig();
            if (fromConfig != null)
            {
                this.SetAop(fromConfig);
            }
        }

        public void ReOpenTransation()
        {
            this.helper.openTrans = true;
        }

        public void ResetProc(object procNamesEnum)
        {
            this.helper.ClearParameters();
            this.procName = procNamesEnum.ToString().Trim();
            if (this.procName.Contains(" ") && (this.procName.Length > 0x10))
            {
                this.isProc = false;
            }
        }

        public void RollBack()
        {
            if ((this.helper != null) && this.helper.openTrans)
            {
                this.helper.RollBack();
            }
        }

        public void Set(object paraName, object value)
        {
            this.helper.AddParameters(Convert.ToString(paraName), value);
        }

        public void Set(object paraName, object value, DbType dbType)
        {
            this.helper.AddParameters(Convert.ToString(paraName), value, dbType, -1, ParameterDirection.Input);
        }

        public void SetAop(IAop aop)
        {
            this._Aop = aop;
            this.helper.OnExceptionEvent += new DbBase.OnException(this.helper_OnExceptionEvent);
        }

        public void SetCustom(object paraName, ParaType paraType)
        {
            this.helper.AddCustomePara(Convert.ToString(paraName), paraType);
        }

        public void SetNoAop()
        {
            this._Aop = new CYQ.Data.Aop.Aop();
        }

        public void SetTransLevel(IsolationLevel level)
        {
            this.helper.SetLevel(level);
        }

        public string DebugInfo
        {
            get
            {
                if (this.helper != null)
                {
                    return this.helper.attachInfo;
                }
                return this.debugInfo;
            }
        }

        public int ReturnValue
        {
            get
            {
                return this.helper.ReturnValue;
            }
        }
    }
}

