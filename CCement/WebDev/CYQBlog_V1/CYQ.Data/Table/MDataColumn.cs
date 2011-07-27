namespace CYQ.Data.Table
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    public class MDataColumn : List<CellStruct>
    {
        private MDataTable _Table;

        public MDataColumn()
        {
        }

        internal MDataColumn(MDataTable table)
        {
            this._Table = table;
        }

        public void Add(string columnName)
        {
            this.Add(columnName, SqlDbType.NVarChar);
        }

        public void Add(string columnName, SqlDbType SqlType)
        {
            CellStruct item = new CellStruct(columnName, SqlType, false, true, 0, ParameterDirection.Input);
            base.Add(item);
        }

        public MDataColumn Clone()
        {
            MDataColumn column = new MDataColumn();
            for (int i = 0; i < base.Count; i++)
            {
                CellStruct item = base[i];
                column.Add(item);
            }
            return column;
        }

        public void Remove(string columnName)
        {
            int index = -1;
            for (int i = 0; i < base.Count; i++)
            {
                if (base[i].ColumnName == columnName)
                {
                    index = i;
                    break;
                }
            }
            if (index > -1)
            {
                this.RemoveAt(index);
            }
        }

        public void RemoveAt(int index)
        {
            if (this._Table != null)
            {
                foreach (MDataRow row in this._Table.Rows)
                {
                    row.RemoveAt(index);
                }
            }
            base.RemoveAt(index);
        }
    }
}

