using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFireable
{
    public void Fire(float force, Vector3 direction, float damage, LayerMask targetLayer);
}
