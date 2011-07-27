namespace CYQ.Data
{
    using CYQ.Data.Aop;
    using CYQ.Data.Cache;
    using CYQ.Data.DAL;
    using CYQ.Data.SQL;
    using CYQ.Data.Table;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;

    public class MAction : IDisposable
    {
        private IAop _Aop;
        private string _ConnectionString;
        private DbBase _DataSqlHelper;
        private bool _IsFillComplete;
        private bool _IsInsertCommand;
        private CacheManage _PropertyCache;
        private MDataRow _Row;
        private SQLString _SQLString;
        private string _TableName;
        private MActionUI _UI;
        private string debugInfo;

        public MAction(object tableNamesEnum)
        {
            this._Aop = new CYQ.Data.Aop.Aop();
            this.debugInfo = string.Empty;
            string conn = string.Empty;
            if (tableNamesEnum is Enum)
            {
                string name = tableNamesEnum.GetType().Name;
                if ((name != "TableNames") && (name != "ViewNames"))
                {
                    conn = name.Substring(2).Replace("Enum", "Conn");
                }
            }
            this.Init(tableNamesEnum.ToString(), conn);
        }

        public MAction(object tableNamesEnum, string conn)
        {
            this._Aop = new CYQ.Data.Aop.Aop();
            this.debugInfo = string.Empty;
            this.Init(tableNamesEnum.ToString(), conn);
        }

        private void _DataSqlHelper_OnExceptionEvent(string msg)
        {
            this._Aop.OnError(msg);
        }

        public MAction Bind(object control)
        {
            return this.Bind(control, string.Empty, MBindUI.GetID(control).Substring(3), "ID");
        }

        public MAction Bind(object control, string where)
        {
            return this.Bind(control, where, MBindUI.GetID(control).Substring(3), "ID");
        }

        public MAction Bind(object control, string where, object text, object value)
        {
            MDataTable source = this._DataSqlHelper.ExeDataReader(this._SQLString.GetBindSql(where, text, value), false);
            if ((source != null) && (source.Rows.Count > 0))
            {
                MBindUI.BindList(control, source);
            }
            return this;
        }

        public void Close()
        {
            this.Dispose();
            if (this._Aop != null)
            {
                this._Aop = null;
            }
        }

        public bool Delete()
        {
            return this.Delete(this._Row[0].Value, new object[0]);
        }

        public bool Delete(object where, params object[] aopInfo)
        {
            this._Aop.Begin(AopEnum.Delete, this._TableName, aopInfo);
            object id = where;
            this._DataSqlHelper.ClearParameters();
            bool success = this._DataSqlHelper.ExeNonQuery(this._SQLString.GetDeleteSql(where), false) > 0;
            if (success)
            {
                return true;
            }
            this.OnError();
            this._Aop.End(AopEnum.Delete, success, id, aopInfo);
            return success;
        }

        public void Dispose()
        {
            this.EndFormat();
            if (this._DataSqlHelper != null)
            {
                if (this._Row != null)
                {
                    this._Row.Clear();
                    this._Row = null;
                }
                this._DataSqlHelper.Dispose();
                if (this._DataSqlHelper != null)
                {
                    this.debugInfo = this.debugInfo + this._DataSqlHelper.attachInfo;
                    this._DataSqlHelper = null;
                }
                if (this._SQLString != null)
                {
                    this._SQLString = null;
                }
                if (this._UI != null)
                {
                    this._UI.Dispose();
                }
            }
        }

        public void EndFormat()
        {
            ValueFormat.Reset();
        }

        public void EndTransation()
        {
            if ((this._DataSqlHelper != null) && this._DataSqlHelper.openTrans)
            {
                this._DataSqlHelper.EndTransaction();
            }
        }

        public bool Fill(object where, params object[] aopInfo)
        {
            this._Aop.Begin(AopEnum.Fill, this._TableName, aopInfo);
            bool success = false;
            if (this._DataSqlHelper != null)
            {
                this._DataSqlHelper.ClearParameters();
                MDataTable table = this._DataSqlHelper.ExeDataReader(this._SQLString.GetTopOneSql(where), false);
                success = (table != null) && (table.Rows.Count > 0);
                if (success)
                {
                    this._Row = table.Rows[0];
                    this._Row.TableName = this.TableName;
                    where = this._Row[0].Value;
                    this.ResetRow();
                }
                else
                {
                    this.OnError();
                }
            }
            this._Aop.End(AopEnum.Fill, success, where, aopInfo);
            return success;
        }

        public void FormatAllInputValue(string format)
        {
            ValueFormat.IsFormat = true;
            ValueFormat.formatString = format;
        }

        public T Get<T>(object key)
        {
            return this._Row.Get<T>(key);
        }

        public int GetCount(string where, params object[] aopInfo)
        {
            this._Aop.Begin(AopEnum.GetCount, this._TableName, aopInfo);
            this._DataSqlHelper.ClearParameters();
            object obj2 = this._DataSqlHelper.ExeScalar(this._SQLString.GetCountSql(where), false);
            bool success = (obj2 != null) && (Convert.ToInt32(obj2) > 0);
            this._Aop.End(AopEnum.GetCount, success, obj2, aopInfo);
            if (!success)
            {
                return 0;
            }
            return Convert.ToInt32(obj2);
        }

        public void GetFrom(object control)
        {
            this._UI.Get(control, null);
        }

        public void GetFrom(object control, object value)
        {
            this._UI.Get(control, value);
        }

        private bool GetPropertysByTableName()
        {
            if (this.GetPropertysFromCache())
            {
                return true;
            }
            if ((this._TableName.IndexOf('(') > -1) && (this._TableName.IndexOf(')') > -1))
            {
                this._TableName = SQLString.FormatDal(this._TableName, this._DataSqlHelper.dalType, false);
                this._Row.TableName = this._TableName;
                return true;
            }
            try
            {
                MDataColumn mdcs = OutPutData.GetColumn(this._TableName, ref this._DataSqlHelper);
                this._Row = this.ToDataRow(mdcs);
                this._Row.TableName = this._TableName;
                this._PropertyCache.Add(this._TableName, mdcs.Clone());
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool GetPropertysFromCache()
        {
            bool flag = false;
            if ((this._PropertyCache != null) && this._PropertyCache.Contains(this._TableName))
            {
                try
                {
                    this._Row = this.ToDataRow(this._PropertyCache.Get(this._TableName) as MDataColumn);
                    flag = this._Row.Count > 0;
                }
                catch
                {
                }
            }
            return flag;
        }

        private void Init(string tableName, string conn)
        {
            this._TableName = tableName;
            this.InitSqlHelper(conn);
            this._Row = new MDataRow();
            this._PropertyCache = CacheManage.Instance;
            if (!this.GetPropertysByTableName())
            {
                throw new Exception("数据库字段加载失败!请检查数据库链接及表名(" + this.TableName + ")是否存在!");
            }
            this._SQLString = new SQLString(ref this._Row, ref this._DataSqlHelper);
            this._UI = new MActionUI(ref this._Row);
            IAop fromConfig = this._Aop.GetFromConfig();
            if (fromConfig != null)
            {
                this.SetAop(fromConfig);
            }
            if (this._DataSqlHelper.dalType == DalType.Access)
            {
                this.EndTransation();
            }
        }

        private void InitSqlHelper(string conn)
        {
            if (this._DataSqlHelper == null)
            {
                this._DataSqlHelper = DalAction.GetHelper(conn);
                this._ConnectionString = this._DataSqlHelper.Con.ConnectionString;
            }
        }

        public bool Insert()
        {
            return this.Insert(false, new object[0]);
        }

        public bool Insert(bool AutoSetValue, params object[] aopInfo)
        {
            this._Aop.Begin(AopEnum.Insert, this._TableName, aopInfo);
            if (AutoSetValue)
            {
                this._UI.AutoSetColumnValue(true);
            }
            this._IsInsertCommand = true;
            this._DataSqlHelper.ClearParameters();
            bool success = this.InsertOrUpdate(this._SQLString.GetInsertSql());
            object id = 0;
            if (!success)
            {
                this.OnError();
            }
            else
            {
                id = this._Row[0].Value;
            }
            this._Aop.End(AopEnum.Insert, success, id, aopInfo);
            return success;
        }

        private bool InsertOrUpdate(string SqlCommandText)
        {
            bool flag = false;
            if (this._SQLString._IsCanDo)
            {
                if (this._IsInsertCommand)
                {
                    object obj2;
                    this._IsInsertCommand = false;
                    if (this._DataSqlHelper.dalType == DalType.Sql)
                    {
                        obj2 = this._DataSqlHelper.ExeScalar(SqlCommandText, false);
                    }
                    else
                    {
                        obj2 = this._DataSqlHelper.ExeNonQuery(SqlCommandText, false);
                        if ((obj2 != null) && (Convert.ToInt32(obj2) > 0))
                        {
                            this._DataSqlHelper.ClearParameters();
                            obj2 = this._DataSqlHelper.ExeScalar(this._SQLString.GetMaxID(), false);
                        }
                    }
                    if (obj2 != null)
                    {
                        flag = this._IsFillComplete = this.Fill(obj2, new object[0]);
                    }
                }
                else
                {
                    flag = this._DataSqlHelper.ExeNonQuery(SqlCommandText, false) > 0;
                }
            }
            if (flag)
            {
                this._SQLString._IsCanDo = false;
            }
            return flag;
        }

        internal void OnError()
        {
            if ((this._DataSqlHelper != null) && this._DataSqlHelper.openTrans)
            {
                this.Dispose();
            }
        }

        public void ReOpenTransation()
        {
            this._DataSqlHelper.openTrans = true;
        }

        private void ResetRow()
        {
            if (this._SQLString != null)
            {
                this._SQLString.SetRow(ref this._Row);
                this._UI._Row = this._Row;
            }
        }

        public bool ResetTable(object tableName)
        {
            this._TableName = tableName.ToString();
            if (!this.GetPropertysByTableName())
            {
                this.OnError();
                return false;
            }
            this.ResetRow();
            return true;
        }

        public void RollBack()
        {
            if ((this._DataSqlHelper != null) && this._DataSqlHelper.openTrans)
            {
                this._DataSqlHelper.RollBack();
            }
        }

        public MDataTable Select(params object[] aopInfo)
        {
            int num;
            return this.Select(0, 0, "", out num, aopInfo);
        }

        public MDataTable Select(int PageIndex, int pageSize, string where, out int rowCount, params object[] aopInfo)
        {
            this._Aop.Begin(AopEnum.Select, this._TableName, aopInfo);
            rowCount = 0;
            MDataTable table = null;
            this._DataSqlHelper.ClearParameters();
            DbDataReader reader = null;
            if (this._SQLString != null)
            {
                where = this._SQLString.FormatWhere(where);
            }
            else
            {
                where = SQLString.FormatDal(where, this._DataSqlHelper.dalType, true);
            }
            switch (this._DataSqlHelper.dalType)
            {
                case DalType.Sql:
                    this._DataSqlHelper.AddParameters("@PageIndex", PageIndex, DbType.Int32, -1, ParameterDirection.Input);
                    this._DataSqlHelper.AddParameters("@PageSize", pageSize, DbType.Int32, -1, ParameterDirection.Input);
                    this._DataSqlHelper.AddParameters("@TableName", this._TableName, DbType.String, -1, ParameterDirection.Input);
                    this._DataSqlHelper.AddParameters("@Where", where, DbType.String, -1, ParameterDirection.Input);
                    reader = this._DataSqlHelper.ExeDataReader("SelectBase", true);
                    break;

                case DalType.Access:
                {
                    string procName = Pager.GetAccess(PageIndex, pageSize, where, this._TableName);
                    reader = this._DataSqlHelper.ExeDataReader(procName, false);
                    break;
                }
                case DalType.Oracle:
                    this._DataSqlHelper.AddParameters("PageIndex", PageIndex, DbType.Int32, -1, ParameterDirection.Input);
                    this._DataSqlHelper.AddParameters("PageSize", pageSize, DbType.Int32, -1, ParameterDirection.Input);
                    this._DataSqlHelper.AddParameters("TableName", this._TableName, DbType.String, -1, ParameterDirection.Input);
                    this._DataSqlHelper.AddParameters("WhereStr", where, DbType.String, -1, ParameterDirection.Input);
                    reader = this._DataSqlHelper.ExeDataReader("MyPackage.SelectBase", true);
                    break;
            }
            bool success = false;
            table = reader;
            if (reader != null)
            {
                reader.Close();
                success = true;
                switch (this._DataSqlHelper.dalType)
                {
                    case DalType.Sql:
                    case DalType.Oracle:
                        rowCount = this._DataSqlHelper.ReturnValue;
                        break;

                    case DalType.Access:
                        rowCount = this.GetCount(where, new object[0]);
                        break;
                }
            }
            this._DataSqlHelper.ClearParameters();
            this._Aop.End(AopEnum.Select, success, (int) rowCount, aopInfo);
            return table;
        }

        public void Set(object key, object value)
        {
            this._Row[key].Value = value;
        }

        public void SetAop(IAop aop)
        {
            this._Aop = aop;
            this._DataSqlHelper.OnExceptionEvent += new DbBase.OnException(this._DataSqlHelper_OnExceptionEvent);
        }

        public void SetAutoPrefix(string autoPrefix, params string[] otherPrefix)
        {
            this._UI.SetAutoPrefix(autoPrefix, otherPrefix);
        }

        public void SetNoAop()
        {
            this._Aop = new CYQ.Data.Aop.Aop();
        }

        public void SetTo(object control)
        {
            this._UI.Set(control, null, true);
        }

        public void SetTo(object control, object value)
        {
            this._UI.Set(control, value, true);
        }

        public void SetTo(object control, object value, bool isControlEnabled)
        {
            this._UI.Set(control, value, isControlEnabled);
        }

        public void SetTransLevel(IsolationLevel level)
        {
            this._DataSqlHelper.SetLevel(level);
        }

        internal MDataRow ToDataRow(MDataColumn mdcs)
        {
            MDataRow row = new MDataRow();
            for (int i = 0; i < mdcs.Count; i++)
            {
                CellStruct dataStruct = mdcs[i];
                MDataCell item = new MDataCell(ref dataStruct);
                row.Add(item);
            }
            row.TableName = this._TableName;
            return row;
        }

        public bool Update()
        {
            return this.Update(this._Row[0].Value, false, new object[0]);
        }

        public bool Update(object where)
        {
            return this.Update(where, false, new object[0]);
        }

        public bool Update(object where, bool AutoSetValue, params object[] aopInfo)
        {
            this._Aop.Begin(AopEnum.Update, this._TableName, aopInfo);
            if (AutoSetValue)
            {
                this._UI.AutoSetColumnValue(false);
            }
            if ((where == null) && (this._Row[0].Value != null))
            {
                where = this._Row[0].Value;
            }
            object id = where;
            this._DataSqlHelper.ClearParameters();
            string updateSql = this._SQLString.GetUpdateSql(where);
            bool success = this.InsertOrUpdate(updateSql);
            if (!success)
            {
                this.OnError();
            }
            this._Aop.End(AopEnum.Update, success, id, aopInfo);
            return success;
        }

        public string ConnectionString
        {
            get
            {
                return this._ConnectionString;
            }
            set
            {
                this._ConnectionString = value;
            }
        }

        public MDataRow Data
        {
            get
            {
                return this._Row;
            }
        }

        public string DebugInfo
        {
            get
            {
                if (this._DataSqlHelper != null)
                {
                    return this._DataSqlHelper.attachInfo;
                }
                return this.debugInfo;
            }
        }

        public int ReturnValue
        {
            get
            {
                if (this._DataSqlHelper != null)
                {
                    return this._DataSqlHelper.ReturnValue;
                }
                return 0;
            }
        }

        public string TableName
        {
            get
            {
                return this._TableName;
            }
            set
            {
                this._TableName = value;
            }
        }
    }
}

