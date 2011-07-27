namespace CYQ.Data.Xml
{
    using CYQ.Data;
    using System;
    using System.Xml;

    internal class XhtmlUrlResolver : XmlUrlResolver
    {
        private string dtdUri;

        public override Uri ResolveUri(Uri baseUri, string relativeUri)
        {
            if (relativeUri.Contains("xhtml1-transitional.dtd") && (this.DtdUri != null))
            {
                relativeUri = this.DtdUri;
            }
            return base.ResolveUri(baseUri, relativeUri);
        }

        public string DtdUri
        {
            get
            {
                if (this.dtdUri == null)
                {
                    this.dtdUri = AppConfig.DtdUri;
                }
                return this.dtdUri;
            }
        }
    }
}

