namespace CYQ.Data.Xml
{
    using CYQ.Data.Table;
    using System;
    using System.Runtime.CompilerServices;
    using System.Xml;

    public class XmlHelper : XmlBase
    {
        private MDataRow _Row;
        private MDataTable _Table;

        public event SetForeachEventHandler OnForeach;

        public XmlHelper()
        {
        }

        public XmlHelper(bool forHtml)
        {
            if (forHtml)
            {
                base.LoadNameSpace(base.htmlNameSpace);
            }
        }

        public XmlHelper(string nameSpaceUrl)
        {
            base.LoadNameSpace(nameSpaceUrl);
        }

        public void AppendNode(XmlNode parentNode, XmlNode childNode)
        {
            if ((parentNode != null) && (childNode != null))
            {
                parentNode.AppendChild(childNode);
            }
        }

        public void AppendNode(XmlNode parentNode, XmlNode childNode, int Position)
        {
            if (parentNode.ChildNodes.Count >= Position)
            {
                parentNode.ChildNodes[Position].AppendChild(childNode);
            }
        }

        public void Clear(XmlNode node)
        {
            node.RemoveAll();
        }

        public XmlNode CreateNode(string tag, string text, params string[] attrAndValue)
        {
            XmlElement element = base.Create(tag);
            try
            {
                element.InnerXml = text;
            }
            catch
            {
                element.InnerXml = base.SetCDATA(text);
            }
            if ((attrAndValue != null) && ((attrAndValue.Length % 2) == 0))
            {
                string name = "";
                string str2 = "";
                for (int i = 0; i < attrAndValue.Length; i++)
                {
                    name = attrAndValue[i];
                    i++;
                    str2 = attrAndValue[i];
                    element.SetAttribute(name, str2);
                }
            }
            return element;
        }

        public XmlNode Get(string tag, string attr, string value, XmlNode parent)
        {
            return base.Fill(base.GetXPath(tag, attr, value), parent);
        }

        public XmlNode GetByID(string id)
        {
            return base.Fill(base.GetXPath("*", "id", id), null);
        }

        public XmlNode GetByID(string id, XmlNode node)
        {
            return base.Fill(base.GetXPath("*", "id", id), node);
        }

        public XmlNode GetByName(string name)
        {
            return base.Fill(base.GetXPath("*", "name", name), null);
        }

        public XmlNode GetByName(string name, XmlNode node)
        {
            return base.Fill(base.GetXPath("*", "name", name), node);
        }

        public XmlNodeList GetList(string tag)
        {
            return base.Select(base.GetXPath(tag, null, null), null);
        }

        public XmlNodeList GetList(string tag, string attr)
        {
            return base.Select(base.GetXPath(tag, attr, null), null);
        }

        public XmlNodeList GetList(string tag, XmlNode node)
        {
            return base.Select(base.GetXPath(tag, null, null), node);
        }

        public XmlNodeList GetList(string tag, string attr, string value)
        {
            return base.Select(base.GetXPath(tag, attr, value), null);
        }

        public XmlNodeList GetList(string tag, string attr, XmlNode node)
        {
            return base.Select(base.GetXPath(tag, attr, null), node);
        }

        public XmlNodeList GetList(string tag, string attr, string value, XmlNode node)
        {
            return base.Select(base.GetXPath(tag, attr, value), node);
        }

        private string GetRowValue(string id)
        {
            string str = "";
            if (this._Row != null)
            {
                MDataCell cell = this._Row[id.Substring(3)];
                if (cell == null)
                {
                    cell = this._Row[id];
                }
                if (cell != null)
                {
                    str = Convert.ToString(cell.Value);
                }
            }
            return str;
        }

        public void InsertAfter(XmlNode newNode, XmlNode refNode)
        {
            XmlNode oldNode = this.CreateNode(newNode.Name, "", new string[0]);
            this.ReplaceNode(newNode, oldNode);
            refNode.ParentNode.InsertAfter(oldNode, refNode);
        }

        public void InsertBefore(XmlNode newNode, XmlNode refNode)
        {
            XmlNode oldNode = this.CreateNode(newNode.Name, "", new string[0]);
            this.ReplaceNode(newNode, oldNode);
            refNode.ParentNode.InsertBefore(oldNode, refNode);
        }

        public void InterChange(XmlNode xNodeFirst, XmlNode xNodeLast)
        {
            if ((xNodeFirst != null) && (xNodeLast != null))
            {
                if ((xNodeFirst.ParentNode != null) && (xNodeLast.ParentNode != null))
                {
                    xNodeFirst.ParentNode.ReplaceChild(xNodeLast.Clone(), xNodeFirst);
                    xNodeLast.ParentNode.ReplaceChild(xNodeFirst.Clone(), xNodeLast);
                }
                else
                {
                    base.xmlDoc.DocumentElement.ReplaceChild(xNodeLast.Clone(), xNodeFirst);
                    base.xmlDoc.DocumentElement.ReplaceChild(xNodeFirst.Clone(), xNodeLast);
                }
            }
        }

        public void LoadData(MDataRow row)
        {
            this._Row = row;
        }

        public void LoadData(MDataTable table)
        {
            this._Table = table;
            if (this._Table.Rows.Count > 0)
            {
                this._Row = this._Table.Rows[0];
            }
        }

        public void Remove(string id)
        {
            XmlNode byID = this.GetByID(id);
            if (byID != null)
            {
                byID.ParentNode.RemoveChild(byID);
            }
        }

        public void Remove(XmlNode node)
        {
            node.ParentNode.RemoveChild(node);
        }

        public void RemoveChild(string id, int index)
        {
            XmlNode byID = this.GetByID(id);
            if (byID != null)
            {
                this.RemoveChild(byID, index);
            }
        }

        public void RemoveChild(XmlNode node, int index)
        {
            if (node.ChildNodes.Count >= index)
            {
                node.RemoveChild(node.ChildNodes[index - 1]);
            }
        }

        public void ReplaceNode(XmlNode newNode, XmlNode oldNode)
        {
            if ((newNode != null) && (oldNode != null))
            {
                oldNode.RemoveAll();
                oldNode.InnerXml = newNode.InnerXml;
                XmlAttributeCollection attributes = newNode.Attributes;
                if ((attributes != null) && (attributes.Count > 0))
                {
                    for (int i = 0; i < attributes.Count; i++)
                    {
                        ((XmlElement) oldNode).SetAttribute(attributes[i].Name, attributes[i].Value);
                    }
                }
            }
        }

        public void Set(string id, string value)
        {
            XmlNode byID = this.GetByID(id);
            this.Set(byID, SetType.InnerXml, new string[] { value });
        }

        public void Set(string id, SetType setType, params string[] values)
        {
            XmlNode byID = this.GetByID(id);
            this.Set(byID, setType, values);
        }

        public void Set(XmlNode node, SetType setType, params string[] values)
        {
            if (node != null)
            {
                string str;
                switch (setType)
                {
                    case SetType.InnerXml:
                        node.InnerXml = this.SetValue(node.InnerXml, values[0], true);
                        return;

                    case SetType.InnerText:
                        node.InnerText = this.SetValue(node.InnerText, values[0], false);
                        return;

                    case SetType.Value:
                    case SetType.Href:
                    case SetType.Src:
                    case SetType.Class:
                    case SetType.Disabled:
                        str = setType.ToString().ToLower();
                        this.SetAttrValue(node, str, values[0]);
                        return;

                    case SetType.A:
                        node.InnerXml = this.SetValue(node.InnerXml, values[0], true);
                        if (values.Length > 1)
                        {
                            this.SetAttrValue(node, "href", values[1]);
                        }
                        return;

                    case SetType.Img:
                    case SetType.Input:
                        return;

                    case SetType.Select:
                        foreach (XmlNode node2 in node.ChildNodes)
                        {
                            if ((node2.Attributes["value"] != null) && (node2.Attributes["value"].Value == values[0]))
                            {
                                this.SetAttrValue(node2, "selected", "selected");
                                break;
                            }
                        }
                        return;

                    case SetType.Checked:
                        if ((values[0] == "1") || (values[0].ToLower() == "true"))
                        {
                            str = setType.ToString().ToLower();
                            this.SetAttrValue(node, str, str);
                        }
                        return;
                }
            }
        }

        private void SetAttrValue(XmlNode node, string key, string value)
        {
            if (node.Attributes[key] == null)
            {
                XmlAttribute attribute = base.xmlDoc.CreateAttribute(key);
                node.Attributes.Append(attribute);
            }
            node.Attributes[key].Value = this.SetValue(node.Attributes[key].InnerXml, value, false);
        }

        public void SetFor(string id)
        {
            this.SetFor(id, SetType.InnerXml);
        }

        public void SetFor(string id, SetType setType)
        {
            this.SetFor(id, setType, new string[] { this.GetRowValue(id) });
        }

        public void SetFor(string id, SetType setType, params string[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i].Contains("[#new]"))
                {
                    values[i] = values[i].Replace("[#new]", this.GetRowValue(id));
                }
            }
            XmlNode byID = this.GetByID(id);
            this.Set(byID, setType, values);
        }

        public void SetForeach(string id, SetType setType, params object[] formatValues)
        {
            string text = string.Empty;
            XmlNode byID = this.GetByID(id);
            if (byID != null)
            {
                switch (setType)
                {
                    case SetType.InnerXml:
                        text = byID.InnerXml;
                        break;

                    case SetType.InnerText:
                        text = byID.InnerText;
                        break;

                    case SetType.Value:
                    case SetType.Href:
                    case SetType.Src:
                    case SetType.Class:
                    {
                        string str2 = setType.ToString().ToLower();
                        if (byID.Attributes[str2] != null)
                        {
                            text = byID.Attributes[str2].Value;
                        }
                        break;
                    }
                }
                this.SetForeach(byID, text, formatValues);
            }
        }

        public void SetForeach(string id, string text, params object[] formatValues)
        {
            XmlNode byID = this.GetByID(id);
            this.SetForeach(byID, text, formatValues);
        }

        private void SetForeach(XmlNode node, string text, params object[] formatValues)
        {
            if (((node != null) && (this._Table != null)) && (this._Table.Rows.Count > 0))
            {
                string str = "";
                object[] values = new object[formatValues.Length];
                string format = text;
                for (int i = 0; i < this._Table.Rows.Count; i++)
                {
                    for (int j = 0; j < values.Length; j++)
                    {
                        MDataCell cell = this._Table.Rows[i][formatValues[j].ToString()];
                        if ((cell == null) && (formatValues[j].ToString().ToLower() == "row"))
                        {
                            values[j] = i + 1;
                        }
                        else
                        {
                            values[j] = cell.Value;
                        }
                    }
                    if (this.OnForeach != null)
                    {
                        format = this.OnForeach(text, values, i);
                    }
                    str = str + string.Format(format, values);
                }
                try
                {
                    node.InnerXml = str;
                }
                catch
                {
                    node.InnerXml = base.SetCDATA(str);
                }
            }
            if (this.OnForeach != null)
            {
                this.OnForeach = null;
            }
        }

        private string SetValue(string sourceValue, string newValue, bool addCData)
        {
            if (string.IsNullOrEmpty(newValue))
            {
                return sourceValue;
            }
            if (addCData)
            {
                return base.SetCDATA(newValue.Replace("[#source]", sourceValue));
            }
            return newValue.Replace("[#source]", sourceValue);
        }

        public delegate string SetForeachEventHandler(string text, object[] values, int row);
    }
}

