using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace grimhawk.tools.objectpool
{
    public class ObjectPool<T> : IPool<T> where T : MonoBehaviour, IPoolable<T>
    {
        #region Private Variables
        private System.Action<T> onPullObject;
        private System.Action<T> onPushObject;
        private Stack<T> pool = new Stack<T>();
        private GameObject blueprint;
        #endregion

        #region Public Variables
        public int PooledCount => pool.Count;
        #endregion

        #region Constructors
        public ObjectPool(GameObject pooledObject, int numberToSpawn = 0)
        {
            this.blueprint = pooledObject;
            Populate(numberToSpawn);
        }
        public ObjectPool(GameObject pooledObject, Action<T> onPullObject, Action<T> onPushObject, int numberToSpawn = 0)
        {
            this.blueprint = pooledObject;
            this.onPushObject = onPushObject;
            this.onPullObject = onPullObject;
            Populate(numberToSpawn);
        }
        #endregion

        #region Monobehavior
        private void Populate(int number)
        {
            T t;
            for (int i = 0; i < number; i++)
            {
                t = GameObject.Instantiate(blueprint).GetComponent<T>();
                pool.Push(t);
                t.gameObject.SetActive(false);
            }
        }
        public GameObject PullGameObject() => Pull().gameObject;

        public GameObject PullGameObject(Vector3 position)
        {
            GameObject go = Pull().gameObject;
            go.transform.position = position;
            return go;
        }
        public GameObject PullGameObject(Vector3 position, Quaternion rotation)
        {
            GameObject go = Pull().gameObject;
            go.transform.SetLocalPositionAndRotation(position, rotation);
            return go;
        }
        #endregion

        #region Inherited Methods
        public T Pull()
        {
            T t;
            if (PooledCount > 0)
                t = pool.Pop();
            else
                t = GameObject.Instantiate(blueprint).GetComponent<T>();

            t.gameObject.SetActive(true);
            t.Initialize(Push);

            onPullObject?.Invoke(t);

            return t;
        }
        public T Pull(Vector3 position)
        {
            T t = Pull();
            t.transform.position = position;
            return t;
        }
        public T Pull(Vector3 position, Quaternion rotation)
        {
            T t = Pull();
            t.transform.SetPositionAndRotation(position, rotation);
            return t;
        }
       
        public void Push(T t)
        {
            pool.Push(t);
            onPushObject?.Invoke(t);
            t.gameObject.SetActive(false);

        }
        #endregion
    }
    public interface IPool<T>
    {
        T Pull();
        void Push(T t);
    }
    public interface IPoolable<T>
    {
        void Initialize(System.Action<T> returnAction);
        void ReturnToPool();
    }
}

