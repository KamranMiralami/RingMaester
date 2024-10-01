using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private readonly Queue<T> _pool = new Queue<T>();
    private readonly T _prefab;
    private readonly Transform _parentTransform;

    public ObjectPool(T prefab, int initialSize, Transform parentTransform = null)
    {
        _prefab = prefab;
        _parentTransform = parentTransform;

        for (int i = 0; i < initialSize; i++)
        {
            T obj = CreateNewObject();
            _pool.Enqueue(obj);
        }
    }

    private T CreateNewObject()
    {
        T obj = Object.Instantiate(_prefab, _parentTransform);
        obj.gameObject.SetActive(false);
        return obj;
    }

    public T Get()
    {
        if (_pool.Count > 0)
        {
            T obj = _pool.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            return CreateNewObject();
        }
    }

    public void Release(T obj)
    {
        obj.gameObject.SetActive(false);
        _pool.Enqueue(obj);
    }

    public int Count => _pool.Count;
}
