using System;

namespace LazySQL.Infrastructure
{
    interface IDynamicObject
    {
        void Create(Object param);
        Object GetInnerObject();
        bool IsValidate();
        void Release();
    }
}
