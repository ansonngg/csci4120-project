using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public GameObject laserLight;
    public GameObject laserParticle;
    public GameObject laserBeam;
    public LaserDamage laserdamage;

    // Start is called before the first frame update
    void Start()
    {
        ToggleAll(false);
    }

    public void ToggleLight(bool b)
	{
        laserLight.SetActive(b);
	}

    public void ToggleParticle(bool b)
    {
        laserParticle.SetActive(b);
    }

    public void ToggleBeam(bool b)
    {
        laserBeam.SetActive(b);
        laserdamage.enabled = b;
    }

    public void ToggleAll(bool b)
	{
        laserLight.SetActive(b);
        laserParticle.SetActive(b);
        laserBeam.SetActive(b);
        laserdamage.enabled = b;
    }
}
