using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace Acoolaum.Core.Services
{
    public class Pool : IDisposable
    {
        private PoolableMonoBehaviour _prefab;
        private Stack<PoolableMonoBehaviour> _items;

        public Pool(PoolableMonoBehaviour prefab, int startAmount = 10)
        {
            _prefab = prefab;
            _items = new Stack<PoolableMonoBehaviour>(startAmount);
            for (int i = 0; i < startAmount; i++)
            {
                var newItem = CreateNew();
                _items.Push(newItem);
            }
        }

        public PoolableMonoBehaviour Get()
        {
            if (_items.Count > 0)
            {
                var item = _items.Pop();
                return item;
            }

            return CreateNew();
        }

        public void Dispose()
        {
            while (_items.Count > 0)
            {
                var pick = _items.Peek();
                pick.OnDispose();
            }
        }
        
        private PoolableMonoBehaviour CreateNew()
        {
            var newItem = Object.Instantiate(_prefab);
            newItem.OnRecycle();
            return newItem;
        }
    }
}