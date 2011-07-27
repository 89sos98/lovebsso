namespace CYQ.Data.Table
{
    using CYQ.Data;
    using CYQ.Data.SQL;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.Common;
    using System.Reflection;

    public class MDataTable : IDataReader, IDisposable, IDataRecord, IEnumerable, IListSource
    {
        private MDataColumn _Columns;
        private List<MDataRow> _Mdr;
        private int _Ptr;
        private string _TableName;

        public MDataTable()
        {
            this._TableName = string.Empty;
            this.Init("路过秋天");
        }

        public MDataTable(string tableName)
        {
            this._TableName = string.Empty;
            this.Init(tableName);
        }

        public void Bind(object control)
        {
            MBindUI.Bind(control, this);
        }

        public void Close()
        {
            this._Mdr.Clear();
        }

        public void Dispose()
        {
            this._Mdr.Clear();
            this._Mdr = null;
        }

        public bool GetBoolean(int i)
        {
            return (bool) this._Mdr[this._Ptr][i].Value;
        }

        public byte GetByte(int i)
        {
            return (byte) this._Mdr[this._Ptr][i].Value;
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public char GetChar(int i)
        {
            return (char) this._Mdr[this._Ptr][i].Value;
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IDataReader GetData(int i)
        {
            return this;
        }

        public string GetDataTypeName(int i)
        {
            return "";
        }

        public DateTime GetDateTime(int i)
        {
            return (DateTime) this._Mdr[this._Ptr][i].Value;
        }

        public decimal GetDecimal(int i)
        {
            return (decimal) this._Mdr[this._Ptr][i].Value;
        }

        public double GetDouble(int i)
        {
            return (double) this._Mdr[this._Ptr][i].Value;
        }

        public IEnumerator GetEnumerator()
        {
            return new DbEnumerator(this);
        }

        public Type GetFieldType(int i)
        {
            return this._Mdr[this._Ptr][i]._CellStruct.ValueType;
        }

        public float GetFloat(int i)
        {
            return (float) this._Mdr[this._Ptr][i].Value;
        }

        public Guid GetGuid(int i)
        {
            return (Guid) this._Mdr[this._Ptr][i].Value;
        }

        public short GetInt16(int i)
        {
            return (short) this._Mdr[this._Ptr][i].Value;
        }

        public int GetInt32(int i)
        {
            return (int) this._Mdr[this._Ptr][i].Value;
        }

        public long GetInt64(int i)
        {
            return (long) this._Mdr[this._Ptr][i].Value;
        }

        public IList GetList()
        {
            return this.Rows;
        }

        public string GetName(int i)
        {
            return this._Mdr[this._Ptr][i]._CellStruct.ColumnName;
        }

        public int GetOrdinal(string name)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public DataTable GetSchemaTable()
        {
            return null;
        }

        public string GetString(int i)
        {
            return Convert.ToString(this._Mdr[this._Ptr][i].Value);
        }

        public object GetValue(int i)
        {
            return this._Mdr[this._Ptr][i].Value;
        }

        public int GetValues(object[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = this._Mdr[this._Ptr - 1][i].Value;
            }
            return values.Length;
        }

        private void Init(string tableName)
        {
            this._Mdr = new List<MDataRow>();
            this._TableName = tableName;
            if (this._Columns == null)
            {
                this._Columns = new MDataColumn(this);
            }
        }

        public bool IsDBNull(int i)
        {
            return this._Mdr[this._Ptr][i]._CellValue.IsNull;
        }

        public static MDataTable LoadFromJson(string json)
        {
            JsonHelper helper = new JsonHelper();
            return helper.Load(json);
        }

        public MDataRow NewRow()
        {
            MDataRow row = new MDataRow();
            row.TableName = this._TableName;
            CellStruct dataStruct = null;
            for (int i = 0; i < this._Columns.Count; i++)
            {
                dataStruct = this._Columns[i];
                row.Add(new MDataCell(ref dataStruct));
            }
            return row;
        }

        public bool NextResult()
        {
            return (this._Ptr < (this._Mdr.Count - 1));
        }

        public static implicit operator MDataTable(DbDataReader sdr)
        {
            MDataTable table = new MDataTable("default");
            if (sdr != null)
            {
                MDataRow row = new MDataRow();
                int num = 0;
                while (sdr.Read())
                {
                    for (int i = 0; i < sdr.FieldCount; i++)
                    {
                        if (num == 0)
                        {
                            CellStruct dataStruct = new CellStruct(sdr.GetName(i), DataType.GetSqlType(sdr.GetFieldType(i)), false, true, -1, ParameterDirection.InputOutput);
                            MDataCell item = new MDataCell(ref dataStruct, sdr.GetValue(i));
                            row.Add(item);
                            table.Columns.Add(dataStruct);
                        }
                        if ((sdr[row[i]._CellStruct.ColumnName] == null) || (sdr[row[i]._CellStruct.ColumnName].ToString() == string.Empty))
                        {
                            row[i].Value = DBNull.Value;
                        }
                        else
                        {
                            row[i].Value = sdr[row[i]._CellStruct.ColumnName];
                        }
                    }
                    num++;
                    table.Rows.Add(row.Clone());
                }
                row.Clear();
                row = null;
                sdr.Close();
                sdr.Dispose();
                sdr = null;
            }
            return table;
        }

        public bool Read()
        {
            if (this._Ptr < this._Mdr.Count)
            {
                this._Ptr++;
                return true;
            }
            this._Ptr = 0;
            return false;
        }

        public DataTable ToDataTable()
        {
            DataTable table = new DataTable(this._TableName);
            if ((this.Columns != null) && (this.Columns.Count > 0))
            {
                foreach (CellStruct struct2 in this.Columns)
                {
                    table.Columns.Add(struct2.ColumnName);
                }
                foreach (MDataRow row in this.Rows)
                {
                    DataRow row2 = table.NewRow();
                    for (int i = 0; i < this.Columns.Count; i++)
                    {
                        row2[i] = row[i].Value;
                    }
                    table.Rows.Add(row2);
                }
            }
            return table;
        }

        public string ToJson()
        {
            JsonHelper helper = new JsonHelper();
            helper.Fill(this);
            return helper.ToString();
        }

        public List<T> ToList<T>()
        {
            List<T> list = new List<T>();
            if ((this.Rows != null) && (this.Rows.Count > 0))
            {
                foreach (MDataRow row in this.Rows)
                {
                    T local = (T) Activator.CreateInstance(typeof(T));
                    PropertyInfo[] properties = local.GetType().GetProperties();
                    object obj2 = null;
                    for (int i = 0; i < properties.Length; i++)
                    {
                        obj2 = row[properties[i].Name].Value;
                        if ((obj2 != null) && (obj2 != DBNull.Value))
                        {
                            properties[i].SetValue(local, obj2, null);
                        }
                    }
                    list.Add(local);
                }
            }
            return list;
        }

        public MDataColumn Columns
        {
            get
            {
                return this._Columns;
            }
        }

        public bool ContainsListCollection
        {
            get
            {
                return true;
            }
        }

        public int Depth
        {
            get
            {
                if (this._Mdr != null)
                {
                    return this._Mdr.Count;
                }
                return 0;
            }
        }

        public int FieldCount
        {
            get
            {
                if (this.Columns != null)
                {
                    return this.Columns.Count;
                }
                return 0;
            }
        }

        public bool IsClosed
        {
            get
            {
                return true;
            }
        }

        public object this[int i]
        {
            get
            {
                return this._Mdr[i];
            }
        }

        public object this[string name]
        {
            get
            {
                return null;
            }
        }

        public int RecordsAffected
        {
            get
            {
                return -1;
            }
        }

        public List<MDataRow> Rows
        {
            get
            {
                return this._Mdr;
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

