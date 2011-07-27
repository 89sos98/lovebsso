namespace CYQ.Data.Xml
{
    using System;
    using System.Xml;

    internal class ResolverDtd
    {
        public static void Resolver(ref XmlDocument xmlDoc)
        {
            xmlDoc.XmlResolver = new XhtmlUrlResolver();
        }
    }
}

