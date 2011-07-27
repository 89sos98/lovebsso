namespace CYQ.Data
{
    using CYQ.Data.SQL;
    using CYQ.Data.Table;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Windows.Forms;

    internal class MActionUI : IDisposable
    {
        public MDataRow _Row;
        private List<string> autoPrefixList;

        public MActionUI(ref MDataRow row)
        {
            this._Row = row;
        }

        public void AutoSetColumnValue(bool containsID)
        {
            int num = 0;
            if (containsID || !this._Row[0]._CellValue.IsNull)
            {
                num = 1;
            }
            while (num < this._Row.Count)
            {
                if (!this._Row[num]._CellValue.IsChange)
                {
                    try
                    {
                        foreach (string str in this.autoPrefixList)
                        {
                            string str2 = HttpContext.Current.Request[str + this._Row[num]._CellStruct.ColumnName];
                            switch (str2)
                            {
                                case null:
                                {
                                    continue;
                                }
                                case "on":
                                    if (this._Row[num]._CellStruct.SqlType == SqlDbType.Bit)
                                    {
                                        this._Row[num].Value = true;
                                    }
                                    else
                                    {
                                        this._Row[num].Value = 1;
                                    }
                                    goto Label_01B6;
                            }
                            if ((str2.Length == 0) && (DataType.GetGroupID(this._Row[num]._CellStruct.SqlType) == 1))
                            {
                                this._Row[num].Value = 0;
                            }
                            else
                            {
                                this._Row[num].Value = TypeDescriptor.GetConverter(this._Row[num]._CellStruct.ValueType).ConvertFrom(str2.Trim());
                            }
                            goto Label_01B6;
                        }
                    }
                    catch
                    {
                    }
                }
            Label_01B6:
                num++;
            }
        }

        public void Dispose()
        {
            if (this.autoPrefixList != null)
            {
                this.autoPrefixList.Clear();
                this.autoPrefixList = null;
            }
        }

        public void Get(object ct, object value)
        {
            if (ct is System.Web.UI.Control)
            {
                this.GetFrom(ct as System.Web.UI.Control, value);
            }
            else
            {
                this.GetFrom(ct as System.Windows.Forms.Control, value);
            }
        }

        public void GetFrom(System.Web.UI.Control ct, object value)
        {
            string str2;
            string str = ct.ID.Substring(3);
            if ((value == null) && ((str2 = ct.GetType().Name) != null))
            {
                if (!(str2 == "TextBox"))
                {
                    if (str2 == "Literal")
                    {
                        value = ((Literal) ct).Text;
                    }
                    else if (str2 == "Label")
                    {
                        value = ((System.Web.UI.WebControls.Label) ct).Text;
                    }
                    else if (str2 == "HiddenField")
                    {
                        value = ((HiddenField) ct).Value;
                    }
                    else if (str2 == "DropDownList")
                    {
                        value = ((DropDownList) ct).SelectedValue;
                    }
                    else if (str2 == "CheckBox")
                    {
                        value = ((System.Web.UI.WebControls.CheckBox) ct).Checked;
                    }
                }
                else
                {
                    value = ((System.Web.UI.WebControls.TextBox) ct).Text.Trim();
                }
            }
            this._Row[str].Value = value;
        }

        public void GetFrom(System.Windows.Forms.Control ct, object value)
        {
            string str = ct.Name.Substring(3);
            if (value == null)
            {
                switch (ct.GetType().Name)
                {
                    case "TextBox":
                        value = ((System.Windows.Forms.TextBox) ct).Text.Trim();
                        break;

                    case "ComboBox":
                        value = ((ComboBox) ct).Text;
                        break;

                    case "Label":
                        value = ((System.Windows.Forms.Label) ct).Text;
                        break;

                    case "DateTimePicker":
                        value = ((DateTimePicker) ct).Value;
                        break;

                    case "ListBox":
                        value = ((System.Windows.Forms.ListBox) ct).Text;
                        break;

                    case "CheckBox":
                        value = ((System.Windows.Forms.CheckBox) ct).Checked;
                        break;

                    case "NumericUpDown":
                        value = ((NumericUpDown) ct).Value;
                        break;

                    case "RichTextBox":
                        value = ((RichTextBox) ct).Text;
                        break;
                }
            }
            this._Row[str].Value = value;
        }

        public void Set(object ct, object value, bool isControlEnabled)
        {
            if (ct is System.Web.UI.Control)
            {
                this.SetTo(ct as System.Web.UI.Control, value, isControlEnabled);
            }
            else
            {
                this.SetTo(ct as System.Windows.Forms.Control, value, isControlEnabled);
            }
        }

        public void SetAutoPrefix(string autoPrefix, params string[] otherPrefix)
        {
            this.autoPrefixList = new List<string>();
            this.autoPrefixList.Add(autoPrefix);
            foreach (string str in otherPrefix)
            {
                this.autoPrefixList.Add(str);
            }
        }

        public void SetTo(System.Web.UI.Control ct, object value, bool isControlEnabled)
        {
            string str = ct.ID.Substring(3);
            if (value == null)
            {
                value = this._Row[str].Value;
            }
            string name = ct.GetType().Name;
            if (name != null)
            {
                if (!(name == "TextBox"))
                {
                    if (!(name == "Literal"))
                    {
                        if (!(name == "Label"))
                        {
                            if (!(name == "HiddenField"))
                            {
                                if (!(name == "DropDownList"))
                                {
                                    if (name == "CheckBox")
                                    {
                                        bool flag;
                                        if (Convert.ToString(value) == "1")
                                        {
                                            flag = true;
                                        }
                                        else
                                        {
                                            bool.TryParse(Convert.ToString(value), out flag);
                                        }
                                        ((System.Web.UI.WebControls.CheckBox) ct).Checked = flag;
                                        ((System.Web.UI.WebControls.CheckBox) ct).Enabled = isControlEnabled;
                                    }
                                    return;
                                }
                                ((DropDownList) ct).SelectedValue = Convert.ToString(value);
                                ((DropDownList) ct).Enabled = isControlEnabled;
                                return;
                            }
                            ((HiddenField) ct).Value = Convert.ToString(value);
                            return;
                        }
                        ((System.Web.UI.WebControls.Label) ct).Text = Convert.ToString(value);
                        return;
                    }
                }
                else
                {
                    ((System.Web.UI.WebControls.TextBox) ct).Text = Convert.ToString(value);
                    ((System.Web.UI.WebControls.TextBox) ct).Enabled = isControlEnabled;
                    return;
                }
                ((Literal) ct).Text = Convert.ToString(value);
            }
        }

        public void SetTo(System.Windows.Forms.Control ct, object value, bool isControlEnabled)
        {
            bool flag;
            string str = ct.Name.Substring(3);
            if (value == null)
            {
                value = this._Row[str].Value;
            }
            switch (ct.GetType().Name)
            {
                case "TextBox":
                    ((System.Windows.Forms.TextBox) ct).Text = Convert.ToString(value);
                    ((System.Windows.Forms.TextBox) ct).Enabled = isControlEnabled;
                    return;

                case "ComboBox":
                    ((ComboBox) ct).Items.Add(value);
                    return;

                case "Label":
                    ((System.Windows.Forms.Label) ct).Text = Convert.ToString(value);
                    return;

                case "DateTimePicker":
                    DateTime time;
                    if (DateTime.TryParse(Convert.ToString(value), out time))
                    {
                        ((DateTimePicker) ct).Value = time;
                    }
                    return;

                case "ListBox":
                    ((System.Windows.Forms.ListBox) ct).Items.Add(value);
                    return;

                case "CheckBox":
                    if (!(Convert.ToString(value) == "1"))
                    {
                        bool.TryParse(Convert.ToString(value), out flag);
                        break;
                    }
                    flag = true;
                    break;

                case "NumericUpDown":
                {
                    decimal result = 0M;
                    if (decimal.TryParse(Convert.ToString(value), out result))
                    {
                        ((NumericUpDown) ct).Value = result;
                    }
                    return;
                }
                case "RichTextBox":
                    ((System.Windows.Forms.ListBox) ct).Text = Convert.ToString(value);
                    return;

                default:
                    return;
            }
            ((System.Windows.Forms.CheckBox) ct).Checked = flag;
            ((System.Windows.Forms.CheckBox) ct).Enabled = isControlEnabled;
        }
    }
}

