using RichPackage.GuardClauses;
using System;
using System.Collections.Generic;

namespace RichPackage.Pooling
{
    public class Cache : Cache<object> { } // simple definition

    public class Cache<T>
    {
        private readonly Dictionary<Type, Pool> cache;

        public Cache()
        {
            cache = new Dictionary<Type, Pool>();
        }

        private Pool GetPool<U>() where U : T, new()
        {
            // lazy create pool
            if (!cache.TryGetValue(typeof(U), out Pool pool))
            {
                pool = new Pool(factoryMethod: () => new U());
                cache.Add(typeof(U), pool);
            }

            return pool;
        }

        public U Rent<U>() where U : T, new()
        {
            return (U)GetPool<U>().Depool();
        }

        public void Return<U>(U props) where U : T, new()
        {
            GuardAgainst.ArgumentIsNull(props, nameof(props));

            GetPool<U>().Enpool(props);
        }

        private class Pool : ObjectPool<T>
        {
            public Pool(Func<T> factoryMethod)
                : base(Infinite_Pool_Size, 0, 0, factoryMethod)
            {
            }
        }
    }
}
