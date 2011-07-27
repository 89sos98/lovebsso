namespace CYQ.Data.Table
{
    using System;
    using System.ComponentModel;

    internal class MDataProperty : PropertyDescriptor
    {
        private MDataCell cell;

        public MDataProperty(MDataCell mdc, Attribute[] attrs) : base(mdc._CellStruct.ColumnName, attrs)
        {
            this.cell = mdc;
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override object GetValue(object component)
        {
            return ((MDataRow) component)[this.cell._CellStruct.ColumnName].Value;
        }

        public override void ResetValue(object component)
        {
        }

        public override void SetValue(object component, object value)
        {
            this.cell.Value = value;
        }

        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }

        public override Type ComponentType
        {
            get
            {
                return typeof(MDataCell);
            }
        }

        public override bool IsBrowsable
        {
            get
            {
                return true;
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public override Type PropertyType
        {
            get
            {
                return this.cell._CellStruct.ValueType;
            }
        }
    }
}

