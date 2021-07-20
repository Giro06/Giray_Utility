using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Giroo.Utility
{
    [System.Serializable]
    public class PoolInfo
    {
        public string poolName;
        public GameObject prefab;
        public int poolSize;
        public bool fixedSize;
    }

    class Pool
    {
        private Stack<PoolObject> availableObjStack = new Stack<PoolObject>();

        private bool fixedSize;
        private GameObject poolObjectPrefab;
        private int poolSize;
        private string poolName;
        public GameObject root;

        public Pool(string poolName, GameObject poolObjectPrefab, int initialCount, bool fixedSize)
        {
            this.poolName = poolName;
            this.poolObjectPrefab = poolObjectPrefab;
            this.poolSize = initialCount;
            this.fixedSize = fixedSize;

            root = new GameObject(poolName);
            //populate the pool
            for (int index = 0; index < initialCount; index++)
            {
                AddObjectToPool(NewObjectInstance(root));
            }
        }

        private void AddObjectToPool(PoolObject po)
        {
            //add to pool
            po.gameObject.SetActive(false);
            availableObjStack.Push(po);
            po.gameObject.transform.SetParent(po.root.transform);
            po.isPooled = true;
        }

        private PoolObject NewObjectInstance(GameObject root)
        {
            GameObject go = (GameObject) GameObject.Instantiate(poolObjectPrefab);
            go.transform.SetParent(root.transform);
            PoolObject po = go.GetComponent<PoolObject>();
            if (po == null)
            {
                po = go.AddComponent<PoolObject>();
            }

            //set name
            po.poolName = poolName;
            po.root = root;
            return po;
        }

        public GameObject NextAvailableObject(Vector3 position, Quaternion rotation)
        {
            PoolObject po = null;
            if (availableObjStack.Count > 0)
            {
                po = availableObjStack.Pop();
            }
            else if (fixedSize == false)
            {
                //increment size var, this is for info purpose only
                poolSize++;
                Debug.Log(string.Format("Growing pool {0}. New size: {1}", poolName, poolSize));
                //create new object
                po = NewObjectInstance(root);
            }
            else
            {
                Debug.LogWarning("No object available & cannot grow pool: " + poolName);
            }

            GameObject result = null;
            if (po != null)
            {
                po.isPooled = false;
                result = po.gameObject;
                result.SetActive(true);

                result.transform.position = position;
                result.transform.rotation = rotation;
            }

            return result;
        }

        public void ReturnObjectToPool(PoolObject po)
        {
            if (poolName.Equals(po.poolName))
            {
                /* we could have used availableObjStack.Contains(po) to check if this object is in pool.
                 * While that would have been more robust, it would have made this method O(n) 
                 */
                if (po.isPooled)
                {
                    Debug.LogWarning(po.gameObject.name + " is already in pool. Why are you trying to return it again? Check usage.");
                }
                else
                {
                    AddObjectToPool(po);
                }
            }
            else
            {
                Debug.LogError(string.Format("Trying to add object to incorrect pool {0} {1}", po.poolName, poolName));
            }
        }
    }

    public class GiroPool : MonoBehaviour
    {
        public static GiroPool instance;

        public PoolInfo[] poolInfos;

        private Dictionary<string, Pool> poolDictionary = new Dictionary<string, Pool>();

        void Start()
        {
            //set instance
            instance = this;
            //check for duplicate names
            CheckForDuplicatePoolNames();
            //create pools
            CreatePools();
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void CheckForDuplicatePoolNames()
        {
            for (int index = 0; index < poolInfos.Length; index++)
            {
                string poolName = poolInfos[index].poolName;
                if (poolName.Length == 0)
                {
                    Debug.LogError(string.Format("Pool {0} does not have a name!", index));
                }

                for (int internalIndex = index + 1; internalIndex < poolInfos.Length; internalIndex++)
                {
                    if (poolName.Equals(poolInfos[internalIndex].poolName))
                    {
                        Debug.LogError(string.Format("Pool {0} & {1} have the same name. Assign different names.", index, internalIndex));
                    }
                }
            }
        }

        private void CreatePools()
        {
            foreach (PoolInfo currentPoolInfo in poolInfos)
            {
                Pool pool = new Pool(currentPoolInfo.poolName, currentPoolInfo.prefab,
                    currentPoolInfo.poolSize, currentPoolInfo.fixedSize);
                
                pool.root.transform.SetParent(transform);
                Debug.Log("Creating pool: " + currentPoolInfo.poolName);
               
                poolDictionary[currentPoolInfo.poolName] = pool;
            }
        }

        public void CreatePool(PoolInfo poolInfo)
        {
            if (poolDictionary.ContainsKey(poolInfo.poolName))
            {
                Debug.LogError("Pool key is already used !");
            }

            Pool pool = new Pool(poolInfo.poolName, poolInfo.prefab, poolInfo.poolSize, poolInfo.fixedSize);
            pool.root.transform.SetParent(transform);
            poolDictionary[poolInfo.poolName] = pool;
        }

        public GameObject GetObjectFromPool(string poolName, Vector3 position, Quaternion rotation)
        {
            GameObject result = null;

            if (poolDictionary.ContainsKey(poolName))
            {
                Pool pool = poolDictionary[poolName];
                result = pool.NextAvailableObject(position, rotation);
                 
                if (result == null)
                {
                    Debug.LogWarning("No object available in pool. Consider setting fixedSize to false.: " + poolName);
                }
            }
            else
            {
                Debug.LogError("Invalid pool name specified: " + poolName);
            }

            return result;
        }

        public GameObject GetObjectFromPool(string poolName, Vector3 position, Quaternion rotation, float timer)
        {
            GameObject result = null;

            if (poolDictionary.ContainsKey(poolName))
            {
                Pool pool = poolDictionary[poolName];
                result = pool.NextAvailableObject(position, rotation);
                //scenario when no available object is found in pool
                if (result == null)
                {
                    Debug.LogWarning("No object available in pool. Consider setting fixedSize to false.: " + poolName);
                }
            }
            else
            {
                Debug.LogError("Invalid pool name specified: " + poolName);
            }

            TimedPoolObject timedPoolObject = result.AddComponent<TimedPoolObject>();
            timedPoolObject.SetTimer(timer);
            return result;
        }

        public void ReturnObjectToPool(GameObject go)
        {
            PoolObject po = go.GetComponent<PoolObject>();
            if (po == null)
            {
                Debug.LogWarning("Specified object is not a pooled instance: " + go.name);
            }
            else
            {
                if (poolDictionary.ContainsKey(po.poolName))
                {
                    Pool pool = poolDictionary[po.poolName];
                    pool.ReturnObjectToPool(po);
                }
                else
                {
                    Debug.LogWarning("No pool available with name: " + po.poolName);
                }
            }
        }
    }
}