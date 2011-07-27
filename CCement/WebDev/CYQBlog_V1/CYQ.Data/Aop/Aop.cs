namespace CYQ.Data.Aop
{
    using CYQ.Data;
    using CYQ.Data.Cache;
    using System;
    using System.Reflection;

    internal class Aop : IAop
    {
        private CacheManage _Cache;

        public void Begin(AopEnum action, string objName, params object[] aopInfo)
        {
        }

        public void End(AopEnum action, bool success, object id, params object[] aopInfo)
        {
        }

        public IAop GetFromConfig()
        {
            string aop = AppConfig.Aop;
            if (aop != null)
            {
                this._Cache = CacheManage.Instance;
                if (this._Cache.Contains("Aop"))
                {
                    return (this._Cache.Get("Aop") as IAop);
                }
                string[] strArray = aop.Split(new char[] { ',' });
                if (strArray.Length == 2)
                {
                    try
                    {
                        Assembly assembly = Assembly.Load(strArray[0]);
                        if (assembly != null)
                        {
                            object obj2 = assembly.CreateInstance(strArray[1]);
                            if (obj2 != null)
                            {
                                this._Cache.Add("Aop", obj2);
                                return (obj2 as IAop);
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        throw new Exception(exception.Message + "--标题Aop配置为[程序集名称,名称空间.类名]如:<add key=\"Aop\" value=\"CYQ.Data.Test,CYQ.Data.Test.MyAop\" />");
                    }
                }
            }
            return null;
        }

        public void OnError(string msg)
        {
        }
    }
}

