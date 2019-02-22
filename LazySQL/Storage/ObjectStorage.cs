using LazySQL.Extends;
using System.Collections.Concurrent;

namespace LazySQL.Storage
{
    public class ObjectStorage
    {
        public ConcurrentDictionary<string, IPool> poolSave;

        public ObjectStorage()
        {
            poolSave = new ConcurrentDictionary<string, IPool>();
        }
    }
}
