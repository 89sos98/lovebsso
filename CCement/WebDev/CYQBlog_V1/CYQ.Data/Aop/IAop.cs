namespace CYQ.Data.Aop
{
    using System;

    public interface IAop
    {
        void Begin(AopEnum action, string objName, params object[] aopInfo);
        void End(AopEnum action, bool success, object id, params object[] aopInfo);
        IAop GetFromConfig();
        void OnError(string msg);
    }
}

