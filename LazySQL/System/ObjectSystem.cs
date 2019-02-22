using LazySQL.Extends;
using LazySQL.Storage;

namespace LazySQL.System
{
    public class ObjectSystem : ISystem
    {
        public ObjectStorage objectStorage;
        public ObjectSystem(SystemMediator systemMediator) : base(systemMediator)
        {
            objectStorage = new ObjectStorage();
        }
        
        public void PoolAdd(string key, IPool pool)
        {
            if (!objectStorage.poolSave.ContainsKey(key))
            {
                objectStorage.poolSave.AddOrUpdate(key, pool, (k, o_v) => pool);
            }
        }
        public void PoolRemove(string key, out IPool pool)
        {
            if (objectStorage.poolSave.ContainsKey(key))
            {
                objectStorage.poolSave.TryRemove(key, out pool);
            }
            else
            {
                pool = null;
            }
        }
    }
}
