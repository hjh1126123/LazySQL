using System;

namespace LazySQL.Infrastructure
{
    public interface IDynamicObject
    {
        void Create(Object param);
        Object GetInnerObject();
        bool IsValidate();
        void Release();
    }
}
