using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPooledObject
{
    public void OnObjectInstantiate();

    //public void SetPool();

    public void ReturnToPool();
}
