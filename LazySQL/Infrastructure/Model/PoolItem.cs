using LazySQL.Extends;
using System;

namespace LazySQL.Infrastructure
{
    public class PoolItem
    {
        private IDynamicObject _object;
        private bool _bUsing;
        private Type _type; 
        private Object _CreateParam;

        public PoolItem(Type type, Object param)
        {
            _type = type;
            _CreateParam = param;
            Create();
        }

        private void Create()
        {
            _bUsing = false;
            _object = (IDynamicObject)Activator.CreateInstance(_type);
            _object.Create(_CreateParam);
        }

        public void Recreate()
        {
            _object.Release();
            Create();
        }

        public void Release()
        {
            _object.Release();
        }

        public Object InnerObject
        {
            get { return _object.GetInnerObject(); }
        }

        public int InnerObjectHashcode
        {
            get { return InnerObject.GetHashCode(); }
        }

        public bool IsValidate
        {
            get { return _object.IsValidate(); }
        }

        public bool Using
        {
            get { return _bUsing; }
            set { _bUsing = value; }
        }
    }
}
