namespace CYQ.Data.Table
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Reflection;

    public class MDataRow : List<MDataCell>, IDataRecord, ICustomTypeDescriptor
    {
        private string _TableName;
        private int index;
        private PropertyDescriptorCollection properties;

        public MDataRow Clone()
        {
            MDataRow row = new MDataRow();
            for (int i = 0; i < base.Count; i++)
            {
                MDataCell item = new MDataCell(ref base[i]._CellStruct);
                item.Value = base[i].Value;
                item._CellValue.IsChange = false;
                row.Add(item);
            }
            row.TableName = this.TableName;
            return row;
        }

        public T Get<T>(object key)
        {
            object obj2 = this[key].Value;
            if ((obj2 == null) || (obj2 == DBNull.Value))
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

        public AttributeCollection GetAttributes()
        {
            return null;
        }

        public string GetClassName()
        {
            return "MDataRow";
        }

        public string GetComponentName()
        {
            return "by  路过秋天";
        }

        public TypeConverter GetConverter()
        {
            throw new Exception("The method or operation is not implemented..-- by 路过秋天");
        }

        public EventDescriptor GetDefaultEvent()
        {
            throw new Exception("The method or operation is not implemented..-- by 路过秋天");
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            throw new Exception("The method or operation is not implemented..-- by 路过秋天");
        }

        public object GetEditor(Type editorBaseType)
        {
            throw new Exception("The method or operation is not implemented..-- by 路过秋天");
        }

        public EventDescriptorCollection GetEvents()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            throw new Exception("The method or operation is not implemented..-- by 路过秋天");
        }

        public PropertyDescriptorCollection GetProperties()
        {
            return this.GetProperties(null);
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            if (this.index != 1)
            {
                this.index++;
                this.properties = new PropertyDescriptorCollection(null);
                foreach (MDataCell cell in this)
                {
                    this.properties.Add(new MDataProperty(cell, null));
                }
            }
            return this.properties;
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        bool IDataRecord.GetBoolean(int i)
        {
            return (bool) this[i].Value;
        }

        byte IDataRecord.GetByte(int i)
        {
            return (byte) this[i].Value;
        }

        long IDataRecord.GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        char IDataRecord.GetChar(int i)
        {
            return (char) this[i].Value;
        }

        long IDataRecord.GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return (long) this[i].Value;
        }

        IDataReader IDataRecord.GetData(int i)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        string IDataRecord.GetDataTypeName(int i)
        {
            return "";
        }

        DateTime IDataRecord.GetDateTime(int i)
        {
            return (DateTime) this[i].Value;
        }

        decimal IDataRecord.GetDecimal(int i)
        {
            return (decimal) this[i].Value;
        }

        double IDataRecord.GetDouble(int i)
        {
            return (double) this[i].Value;
        }

        Type IDataRecord.GetFieldType(int i)
        {
            return this[i]._CellStruct.ValueType;
        }

        float IDataRecord.GetFloat(int i)
        {
            return (float) this[i].Value;
        }

        Guid IDataRecord.GetGuid(int i)
        {
            return (Guid) this[i].Value;
        }

        short IDataRecord.GetInt16(int i)
        {
            return (short) this[i].Value;
        }

        int IDataRecord.GetInt32(int i)
        {
            return (int) this[i].Value;
        }

        long IDataRecord.GetInt64(int i)
        {
            return (long) this[i].Value;
        }

        string IDataRecord.GetName(int i)
        {
            return (string) this[i].Value;
        }

        int IDataRecord.GetOrdinal(string name)
        {
            return (int) this[name].Value;
        }

        string IDataRecord.GetString(int i)
        {
            return (string) this[i].Value;
        }

        object IDataRecord.GetValue(int i)
        {
            return this[i].Value;
        }

        int IDataRecord.GetValues(object[] values)
        {
            return 0;
        }

        bool IDataRecord.IsDBNull(int i)
        {
            return (this[i].Value == DBNull.Value);
        }

        public MDataCell this[object filed]
        {
            get
            {
                if ((filed is Enum) || (filed is int))
                {
                    return base[(int) filed];
                }
                return this[filed.ToString()];
            }
        }

        public MDataCell this[string Key]
        {
            get
            {
                for (int i = 0; i < base.Count; i++)
                {
                    if (base[i]._CellStruct.ColumnName.ToLower() == Key.ToLower())
                    {
                        return base[i];
                    }
                }
                return null;
            }
        }

        int IDataRecord.FieldCount
        {
            get
            {
                return base.Count;
            }
        }

        object IDataRecord.this[string name]
        {
            get
            {
                return this[name].Value;
            }
        }

        object IDataRecord.this[int i]
        {
            get
            {
                return this[i].Value;
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

