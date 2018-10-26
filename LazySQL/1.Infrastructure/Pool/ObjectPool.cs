using System;
using System.Collections;

namespace LazySQL.Infrastructure
{
    public sealed class ObjectPool
    {
        private Int32 _nCapacity;
        private Int32 _nCurrentSize;
        private Hashtable _listObjects;
        private ArrayList _listFreeIndex;
        private ArrayList _listUsingIndex;
        private Type _typeObject;
        private Object _objCreateParam;

        public ObjectPool(Type type, Object create_param, Int32 init_size, Int32 capacity)
        {
            if (init_size < 0 || capacity < 1 || init_size > capacity)
            {
                throw (new Exception("Invalid parameter!"));
            }

            _nCapacity = capacity;
            _listObjects = new Hashtable(capacity);
            _listFreeIndex = new ArrayList(capacity);
            _listUsingIndex = new ArrayList(capacity);
            _typeObject = type;
            _objCreateParam = create_param;

            for (int i = 0; i < init_size; i++)
            {
                PoolItem pitem = new PoolItem(type, create_param);
                _listObjects.Add(pitem.InnerObjectHashcode, pitem);
                _listFreeIndex.Add(pitem.InnerObjectHashcode);
            }

            _nCurrentSize = _listObjects.Count;
        }

        public void Release()
        {
            lock (this)
            {
                foreach (DictionaryEntry de in _listObjects)
                {
                    ((PoolItem)de.Value).Release();
                }
                _listObjects.Clear();
                _listFreeIndex.Clear();
                _listUsingIndex.Clear();
            }
        }

        public Int32 CurrentSize
        {
            get { return _nCurrentSize; }
        }

        public Int32 ActiveCount
        {
            get { return _listUsingIndex.Count; }
        }

        public Object GetOne()
        {
            lock (this)
            {
                if (_listFreeIndex.Count == 0)
                {
                    if (_nCurrentSize == _nCapacity)
                    {
                        return null;
                    }
                    PoolItem pnewitem = new PoolItem(_typeObject, _objCreateParam);
                    _listObjects.Add(pnewitem.InnerObjectHashcode, pnewitem);
                    _listFreeIndex.Add(pnewitem.InnerObjectHashcode);
                    _nCurrentSize++;
                }

                Int32 nFreeIndex = (Int32)_listFreeIndex[0];
                PoolItem pitem = (PoolItem)_listObjects[nFreeIndex];
                _listFreeIndex.RemoveAt(0);
                _listUsingIndex.Add(nFreeIndex);

                if (!pitem.IsValidate)
                {
                    pitem.Recreate();
                }

                pitem.Using = true;
                return pitem.InnerObject;
            }
        }

        public void FreeObject(Object obj)
        {
            lock (this)
            {
                int key = obj.GetHashCode();
                if (_listObjects.ContainsKey(key))
                {
                    PoolItem item = (PoolItem)_listObjects[key];
                    item.Using = false;
                    _listUsingIndex.Remove(key);
                    _listFreeIndex.Add(key);
                }
            }
        }

        public Int32 DecreaseSize(Int32 size)
        {
            Int32 nDecrease = size;
            lock (this)
            {
                if (nDecrease <= 0)
                {
                    return 0;
                }
                if (nDecrease > _listFreeIndex.Count)
                {
                    nDecrease = _listFreeIndex.Count;
                }

                for (int i = 0; i < nDecrease; i++)
                {
                    _listObjects.Remove(_listFreeIndex[i]);
                }

                _listFreeIndex.Clear();
                _listUsingIndex.Clear();

                foreach (DictionaryEntry de in _listObjects)
                {
                    PoolItem pitem = (PoolItem)de.Value;
                    if (pitem.Using)
                    {
                        _listUsingIndex.Add(pitem.InnerObjectHashcode);
                    }
                    else
                    {
                        _listFreeIndex.Add(pitem.InnerObjectHashcode);
                    }
                }
            }
            _nCurrentSize -= nDecrease;
            return nDecrease;
        }
    }
}
