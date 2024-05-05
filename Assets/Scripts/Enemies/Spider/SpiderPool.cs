using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderPool : MonoBehaviour
{
    public GameObject prefab;
    public Transform player;
    public int count;
    public int maxCount;

    public static SpiderPool Instance;

    private Queue<GameObject> queue;
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        queue = new Queue<GameObject>();

        for(int i = 0; i < count; i++)
		{
            GameObject obj = Instantiate(prefab);
            obj.GetComponent<SpiderAI>().player = player;
            obj.SetActive(false);

            queue.Enqueue(obj);
		}
    }


    public GameObject InstantiateFromPool(Vector3 position, Quaternion rotation)
	{
        GameObject obj;

        if (queue.Count > 0)
		{
            obj = queue.Dequeue();
		}
		else if(count >= maxCount)
		{
            return null;
		}
		else
		{
            obj = Instantiate(prefab);
            obj.GetComponent<SpiderAI>().player = player;
            count += 1;
		}

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);
        obj.GetComponent<SpiderHealth>().OnObjectInstantiate();

        return obj;
	}

    public void BackToPool(SpiderHealth obj)
	{
        queue.Enqueue(obj.gameObject);
	}
}
