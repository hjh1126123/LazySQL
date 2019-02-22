using System;

namespace LazySQL.Extends
{
    public interface IDynamicObject
    {
        void Create(Object param);
        Object GetInnerObject();
        bool IsValidate();
        void Release();
    }
}
