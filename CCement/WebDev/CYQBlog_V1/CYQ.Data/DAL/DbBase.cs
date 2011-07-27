namespace CYQ.Data.DAL
{
    using CYQ.Data.SQL;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.CompilerServices;

    internal abstract class DbBase : IDisposable
    {
        private DbCommand _com;
        private DbConnection _con;
        private DbProviderFactory _fac;
        internal string attachInfo = "";
        public DalType dalType;
        private IsolationLevel level = IsolationLevel.Unspecified;
        internal bool openTrans = true;
        private int returnValue = -1;
        private DbTransaction tran;
        public bool WriteLog = true;

        internal event OnException OnExceptionEvent;

        public DbBase(string conn, string providerName)
        {
            this.dalType = DalAction.GetDalType(providerName);
            this._fac = DbProviderFactories.GetFactory(providerName);
            this._con = this._fac.CreateConnection();
            this._con.ConnectionString = this.FormatConn(conn);
            this._com = this._con.CreateCommand();
            this._com.Connection = this._con;
        }

        internal virtual void AddCustomePara(string paraName, ParaType paraType)
        {
        }

        public void AddParameters(string parameterName, object value)
        {
            this.AddParameters(parameterName, value, DbType.String, -1, ParameterDirection.Input);
        }

        public virtual void AddParameters(string parameterName, object value, DbType dbType, int size, ParameterDirection direction)
        {
            if (this.dalType == DalType.Oracle)
            {
                parameterName = parameterName.Replace(":", "").Replace("@", "");
            }
            else
            {
                parameterName = (parameterName.Substring(0, 1) == "@") ? parameterName : ("@" + parameterName);
            }
            DbParameter newParameter = this.GetNewParameter();
            newParameter.ParameterName = parameterName;
            newParameter.Value = value;
            newParameter.DbType = dbType;
            if (size > -1)
            {
                newParameter.Size = size;
            }
            newParameter.Direction = direction;
            this.Com.Parameters.Add(newParameter);
        }

        public abstract void AddReturnPara();
        public void ClearParameters()
        {
            if ((this._com != null) && (this._com.Parameters != null))
            {
                this._com.Parameters.Clear();
            }
        }

        private void CloseCon()
        {
            try
            {
                if (this._con.State == ConnectionState.Open)
                {
                    if (this.tran != null)
                    {
                        this.openTrans = false;
                        this.tran.Commit();
                        this.tran = null;
                    }
                    this._con.Close();
                }
            }
            catch (DbException exception)
            {
                this.WriteError(exception.Message);
            }
        }

        public void Dispose()
        {
            if (this._con != null)
            {
                this.CloseCon();
                this._con = null;
            }
            if (this._com != null)
            {
                this._com = null;
            }
        }

        public void EndTransaction()
        {
            this.openTrans = false;
            if (this.tran != null)
            {
                try
                {
                    this.tran.Commit();
                }
                catch (Exception exception)
                {
                    this.WriteError(exception.Message);
                }
                this.tran = null;
            }
        }

        public DbDataReader ExeDataReader(string procName, bool isProc)
        {
            this.SetCommandText(procName, isProc);
            DbDataReader reader = null;
            try
            {
                this.OpenCon();
                reader = this._com.ExecuteReader();
                if ((reader != null) && !reader.HasRows)
                {
                    reader.Close();
                    reader = null;
                }
            }
            catch (DbException exception)
            {
                this.WriteError(exception.Message);
            }
            return reader;
        }

        public DataTable ExeDataTable(string procName, bool isProc)
        {
            this.SetCommandText(procName, isProc);
            DbDataAdapter adapter = this._fac.CreateDataAdapter();
            adapter.SelectCommand = this._com;
            DataTable dataTable = new DataTable();
            try
            {
                this.OpenCon();
                adapter.Fill(dataTable);
            }
            catch (DbException exception)
            {
                this.WriteError(exception.Message);
            }
            finally
            {
                adapter.Dispose();
                if (!this.openTrans)
                {
                    this.CloseCon();
                }
            }
            return dataTable;
        }

        public int ExeNonQuery(string procName, bool isProc)
        {
            this.SetCommandText(procName, isProc);
            int num = 1;
            try
            {
                this.OpenCon();
                this._com.ExecuteNonQuery();
            }
            catch (DbException exception)
            {
                num = 0;
                this.WriteError(exception.Message);
            }
            finally
            {
                if (!this.openTrans)
                {
                    this.CloseCon();
                }
            }
            return num;
        }

        public object ExeScalar(string procName, bool isProc)
        {
            this.SetCommandText(procName, isProc);
            object obj2 = null;
            try
            {
                this.OpenCon();
                obj2 = this._com.ExecuteScalar();
            }
            catch (DbException exception)
            {
                this.WriteError(exception.Message);
            }
            finally
            {
                if (!this.openTrans)
                {
                    this.CloseCon();
                }
            }
            return obj2;
        }

        private string FormatConn(string connString)
        {
            if (this.dalType != DalType.Access)
            {
                string str = connString.ToLower();
                int index = str.IndexOf("provider");
                if (index > -1)
                {
                    int num2 = str.IndexOf(';', index);
                    if (num2 > index)
                    {
                        connString = str.Remove(index, (num2 - index) + 1);
                    }
                }
            }
            return connString;
        }

        public abstract DbParameter GetNewParameter();
        private void OpenCon()
        {
            try
            {
                if (this._con.State == ConnectionState.Closed)
                {
                    this._con.Open();
                    if (this.openTrans)
                    {
                        this.tran = this._con.BeginTransaction(this.level);
                        this._com.Transaction = this.tran;
                    }
                }
            }
            catch (DbException exception)
            {
                this.WriteError(exception.Message);
            }
        }

        public void RollBack()
        {
            if (this.tran != null)
            {
                this.tran.Rollback();
            }
        }

        private void SetCommandText(string commandText, bool isProc)
        {
            this._com.CommandText = isProc ? commandText : SQLString.FormatDal(commandText, this.dalType, false);
            this._com.CommandType = isProc ? CommandType.StoredProcedure : CommandType.Text;
            if ((isProc && commandText.Contains("SelectBase")) && !this._com.Parameters.Contains("ReturnValue"))
            {
                this.AddReturnPara();
            }
            else
            {
                string str = commandText.ToLower();
                if ((str.IndexOf("table") > -1) && (((str.IndexOf("delete") > -1) || (str.IndexOf("drop") > -1)) || (str.IndexOf("truncate") > -1)))
                {
                    Log.WriteLog(commandText);
                }
            }
            this.attachInfo = this.attachInfo + "<br><hr>SQL:<br> " + commandText;
            foreach (DbParameter parameter in this._com.Parameters)
            {
                object attachInfo = this.attachInfo;
                this.attachInfo = string.Concat(new object[] { attachInfo, "<br>Para: ", parameter.ParameterName, "->", parameter.Value });
            }
            this.attachInfo = this.attachInfo + "<hr>";
        }

        public void SetLevel(IsolationLevel tranLevel)
        {
            this.level = tranLevel;
        }

        private void WriteError(string err)
        {
            if (this.OnExceptionEvent != null)
            {
                this.OnExceptionEvent(err);
            }
            if (this.tran != null)
            {
                this.tran.Rollback();
                this.tran = null;
            }
            if (this.WriteLog)
            {
                Log.WriteLog(err + this.attachInfo);
            }
        }

        public DbCommand Com
        {
            get
            {
                return this._com;
            }
        }

        public DbConnection Con
        {
            get
            {
                return this._con;
            }
        }

        public virtual string Pre
        {
            get
            {
                return "@";
            }
        }

        public int ReturnValue
        {
            get
            {
                if (((this.returnValue == -1) && (this._com != null)) && (this._com.Parameters != null))
                {
                    int.TryParse(Convert.ToString(this._com.Parameters[this._com.Parameters.Count - 1].Value), out this.returnValue);
                }
                return this.returnValue;
            }
            set
            {
                this.returnValue = value;
            }
        }

        internal delegate void OnException(string msg);
    }
}

