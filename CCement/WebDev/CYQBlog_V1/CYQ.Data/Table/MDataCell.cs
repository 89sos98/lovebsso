namespace CYQ.Data.Table
{
    using CYQ.Data.SQL;
    using System;

    public class MDataCell
    {
        internal CellStruct _CellStruct;
        internal CellValue _CellValue;

        internal MDataCell(ref CellStruct dataStruct)
        {
            this.Init(dataStruct, null);
        }

        internal MDataCell(ref CellStruct dataStruct, object value)
        {
            this.Init(dataStruct, value);
        }

        private void Init(CellStruct dataStruct, object value)
        {
            this._CellValue = new CellValue();
            this._CellStruct = dataStruct;
            this._CellValue.Value = value;
        }

        public object Value
        {
            get
            {
                return this._CellValue.Value;
            }
            set
            {
                this._CellValue.IsChange = true;
                this._CellValue.IsNull = false;
                if (ValueFormat.IsFormat && (DataType.GetGroupID(this._CellStruct.SqlType) == 0))
                {
                    this._CellValue.Value = ValueFormat.Format(value);
                }
                else
                {
                    this._CellValue.Value = value;
                }
            }
        }
    }
}

