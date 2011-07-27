namespace CYQ.Data
{
    using CYQ.Data.Table;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;

    internal class JsonHelper
    {
        private List<string> arrData = new List<string>();
        private int count;
        private string errorMsg = "";

        public void addItem(string name, string value)
        {
            this.arrData.Add("\"" + name + "\":\"" + value + "\"");
        }

        public void addItemOk()
        {
            this.arrData.Add("<br>");
        }

        public void Fill(MDataTable table)
        {
            if (table == null)
            {
                this.ErrorMsg = "查询对象为Null";
            }
            else
            {
                this.Count = table.Rows.Count;
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        this.addItem(table.Columns[j].ColumnName, Convert.ToString(table.Rows[i][j].Value));
                    }
                    this.addItemOk();
                }
            }
        }

        public MDataTable Load(string json)
        {
            MDataTable table = new MDataTable("loadFromJson");
            if (((!string.IsNullOrEmpty(json) && (json.Length > 30)) && (json.StartsWith("{") && (json.IndexOf(',') > -1))) && json.EndsWith("}"))
            {
                try
                {
                    int startIndex = json.IndexOf(":[{") + 2;
                    string str = json.Substring(startIndex, json.LastIndexOf("]}") - startIndex).Replace(@"\}", "#100#").Replace(@"\,", "#101#").Replace(@"\:,", "#102#");
                    bool flag = false;
                    if (string.IsNullOrEmpty(str))
                    {
                        return table;
                    }
                    string[] strArray = str.Replace("{", string.Empty).Split(new char[] { '}' });
                    string str2 = string.Empty;
                    string columnName = string.Empty;
                    string str4 = string.Empty;
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        str2 = strArray[i].Replace("#100#", @"\}").Trim(new char[] { ',' });
                        if (!string.IsNullOrEmpty(str2))
                        {
                            string[] strArray2 = str2.Split(new char[] { ',' });
                            string str5 = string.Empty;
                            if (i == 0)
                            {
                                for (int j = 0; j < strArray2.Length; j++)
                                {
                                    columnName = strArray2[j].Replace("#101#", @"\,").Split(new char[] { ':' })[0].Trim(new char[] { '\'', '"' });
                                    table.Columns.Add(columnName, SqlDbType.NVarChar);
                                }
                                flag = true;
                            }
                            if (flag)
                            {
                                MDataRow item = table.NewRow();
                                for (int k = 0; k < strArray2.Length; k++)
                                {
                                    str5 = strArray2[k].Replace("#101#", @"\,");
                                    if (str5.IndexOf(':') > -1)
                                    {
                                        str4 = str5.Substring(str5.IndexOf(':') + 1).Replace("#102#", @"\:").Trim(new char[] { '\'', '"' });
                                        item[k].Value = str4;
                                    }
                                }
                                table.Rows.Add(item);
                            }
                        }
                    }
                }
                catch
                {
                    return table;
                }
            }
            return table;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            builder.Append("\"count\":\"" + this.count + "\",");
            builder.Append("\"error\":\"" + this.errorMsg + "\",");
            builder.Append("\"success\":\"" + (this.Success ? "true" : "") + "\",");
            builder.Append("\"data\":[");
            int num = 0;
            builder.Append("{");
            if (this.arrData.Count <= 0)
            {
                builder.Append("}]");
            }
            else
            {
                foreach (string str in this.arrData)
                {
                    num++;
                    if (str != "<br>")
                    {
                        builder.Append(str + ",");
                    }
                    else
                    {
                        builder = builder.Replace(",", "", builder.Length - 1, 1);
                        builder.Append("},");
                        if (num < this.arrData.Count)
                        {
                            builder.Append("{");
                        }
                    }
                }
                builder = builder.Replace(",", "", builder.Length - 1, 1);
                builder.Append("]");
            }
            builder.Append("}");
            return builder.ToString();
        }

        public int Count
        {
            get
            {
                return this.count;
            }
            set
            {
                this.count = value;
            }
        }

        public string ErrorMsg
        {
            get
            {
                return this.errorMsg;
            }
            set
            {
                this.errorMsg = value;
            }
        }

        public bool Success
        {
            get
            {
                return (this.count > 0);
            }
        }
    }
}

