using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

	[System.Serializable]
	public class Pool
	{
		public string tag;
		public GameObject prefab;
		public int count;
	}

	public List<Pool> pools;
	public Dictionary<string, Queue<GameObject>> poolDictionary;

	private void Awake()
	{
        Instance = this;
	}
	// Start is called before the first frame update
	void Start()
    {
		poolDictionary = new Dictionary<string, Queue<GameObject>>();

		foreach(Pool pool in pools)
		{
			Queue<GameObject> objectPool = new Queue<GameObject>();

			for(int i = 0; i < pool.count; i++)
			{
				GameObject obj = Instantiate(pool.prefab);
				obj.SetActive(false);
				objectPool.Enqueue(obj);
			}

			poolDictionary.Add(pool.tag, objectPool);
		}
    }

	public GameObject InstantatieFromPool(string tag, Vector3 position, Quaternion rotation)
	{
		if (!poolDictionary.ContainsKey(tag))
		{
			Debug.Log(tag + "Key Doesn't Exist");
			return null;
		}
		GameObject obj = poolDictionary[tag].Dequeue();
		obj.SetActive(true);
		obj.transform.position = position;
		obj.transform.rotation = rotation;

		IPooledObject pooledObject = obj.GetComponent<IPooledObject>();

		if(pooledObject != null)
		{
			pooledObject.OnObjectInstantiate();
		}

		poolDictionary[tag].Enqueue(obj);

		return obj;
	}

}
