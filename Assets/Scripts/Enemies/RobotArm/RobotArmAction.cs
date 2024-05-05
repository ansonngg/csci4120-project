using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTreeFPS;

public class RobotArmAction : MonoBehaviour
{
    private RobotArmControl control;
    private IEnumerator currentAction;
    public bool inAction = false;
    private IEnumerator currentMove;
    public bool inMove = false;

    public float defaultBaseRot = 0;
    public Vector3 defaultClawPos;
    public float defaultClawRot = 180;
    public float defaultClawAngle = 30;

    public LaserController Laser;
    public Transform target;
    public Transform ClawPos;

    public GameObject throwProjectile;
    private GameObject projectile;

    private float CooldownChance = 0;
    public float CooldownRate = 5;

    public float railMin;
    public float railMax;
    public float railSpeed = 0.2f;

    public int slamDamage = 20;
    public int throwDamage = 20;
    public int laserDamage = 30;

    public AudioSource slamSound;
    public AudioSource laserSound;
    public AudioSource armSound;
    // Start is called before the first frame update
    void Start()
    {
        control = GetComponent<RobotArmControl>();
        Laser.laserdamage.damage = laserDamage;
    }

    // Update is called once per frame
    void Update()
    {
		if (!inAction)
		{
            float rand = Random.Range(0, 9 + CooldownChance);

            if (rand > 9)
		    {
                //Debug.Log(gameObject.name + " Cooldown");
                Cooldown();
                CooldownChance = 0;
                
		    }else if(rand > 6)
			{
                //Debug.Log(gameObject.name + " laser");

                MoveTo(target.position.z + Random.Range(-15, 15), 2.5f);
                FireLaser();
                CooldownChance += CooldownRate;
            }
            else if(rand > 3)
			{
                //Debug.Log(gameObject.name + " Slam");

                FollowPlayer(1.5f);
                Slam();
                CooldownChance += CooldownRate;
            }
			else
			{
                //Debug.Log(gameObject.name + " Throw");

                MoveTo(Random.Range(railMin, railMax), 1);
                ThrowObject();
                CooldownChance += CooldownRate;
            }

		}
    }

    private void PlayClipAtPitch(AudioSource audio, float pitch, float volume)
	{
        audio.pitch = pitch;

        audio.PlayOneShot(audio.clip, volume);
	}

    public void RestoreDefault()
	{
		if (inAction)
		{
            StopCoroutine(currentAction);
		}

        currentAction = RestoreDefaultCoroutine();
        StartCoroutine(currentAction);
	}

    public void ThrowObject()
    {
        if (inAction)
        {
            StopCoroutine(currentAction);
        }

        currentAction = ThrowObjectCoroutine();
        StartCoroutine(currentAction);
    }

    public void Slam()
    {
        if (inAction)
        {
            StopCoroutine(currentAction);
        }

        currentAction = SlamCoroutine();
        StartCoroutine(currentAction);
    }

    public void FireLaser()
    {
        if (inAction)
        {
            StopCoroutine(currentAction);
        }

        currentAction = FireLaserCoroutine();
        StartCoroutine(currentAction);
    }

    public void Flinch()
    {
        if (inAction)
        {
            StopCoroutine(currentAction);
        }

        if(projectile != null)
		{
            Destroy(projectile);
		}

        currentAction = FlinchCoroutine();
        StartCoroutine(currentAction);
    }

    public void Cooldown()
    {
        if (inAction)
        {
            StopCoroutine(currentAction);
        }

        currentAction = CooldownCoroutine(Random.Range(2, 5));
        StartCoroutine(currentAction);
    }

    public void MoveTo(float zPos, float duration)
	{
		if (inMove)
		{
            StopCoroutine(currentMove);
		}

        currentMove = MoveToCoroutine(zPos, duration);
        StartCoroutine(currentMove);
	}

    public void FollowPlayer(float duration)
    {
        if (inMove)
        {
            StopCoroutine(currentMove);
        }

        currentMove = FollowPlayerCoroutine(duration);
        StartCoroutine(currentMove);
    }

    private IEnumerator RestoreDefaultCoroutine()
	{
        inAction = true;
        PlayClipAtPitch(armSound, 1, 0.3f);

        control.rotateBaseTo(defaultBaseRot, 1.5f);
        control.moveClawTo(defaultClawPos, 1.5f);
        control.rotateClawTo(defaultClawRot, 1.5f);
        control.openClawTo(defaultClawAngle, 1.5f);
        yield return new WaitForSeconds(1.5f);

        inAction = false;
    }

    private IEnumerator ThrowObjectCoroutine()
	{
        inAction = true;

        control.rotateBaseTo(180, 1.5f);
        control.openClawTo(120, 1.5f);
        yield return new WaitForSeconds(1);

        control.moveClawTo(new Vector3(4, 1, 0), 1);
        control.rotateClawTo(180, 1);
        control.openClawTo(45, 1);
        PlayClipAtPitch(armSound, 1.1f, 1);
        yield return new WaitForSeconds(1);

        control.moveClawTo(new Vector3(4, 4, 0), 1.5f);
        PlayClipAtPitch(armSound, 0.8f, 1f);

        projectile = PoolManager.Instance.InstantatieFromPool("box", ClawPos.position, ClawPos.rotation);
        projectile.GetComponent<ThrownProjectile>().StartAttach(ClawPos);
        yield return new WaitForSeconds(1.5f);

        yield return new WaitForSeconds(0.2f);

        float x = target.position.x - transform.position.x;
        float z = target.position.z - transform.position.z;
        bool clockwise = Vector3.Dot(transform.up, Vector3.Cross(new Vector3(x, 0, z), transform.right)) < 0;
        Vector3 targetPos = target.position;

        control.rotateBaseTo(0, 0.2f, clockwise);
        control.moveClawTo(new Vector3(5.5f, 4, 0), 0.2f);
        control.rotateClawTo(220, 0.2f);
        control.openClawTo(150, 0.2f);
        PlayClipAtPitch(armSound, 1.1f, 0.75f);
        yield return new WaitForSeconds(0.1f);

        //proj.layer = gameObject.layer;
        projectile.GetComponent<ThrownProjectile>().ThrowProjectile(targetPos);

        yield return new WaitForSeconds(0.1f);

        inAction = false;
    }

    private IEnumerator SlamCoroutine()
	{
        inAction = true;

        control.LookPlayer(3.35f);
        control.moveClawTo(new Vector3(3, 8, 0), 1.5f);
        control.rotateClawTo(300, 1.5f);
        control.openClawTo(30, 1.5f);
        PlayClipAtPitch(armSound, 0.5f, 1f);
        yield return new WaitForSeconds(1.5f);

        yield return new WaitForSeconds(2f);

        //Debug.Log(Vector3.Distance(target.position, transform.position));
        control.moveClawTo(new Vector3(Mathf.Clamp(Vector3.Distance(target.position, transform.position) / 2, 2, 8), 1.35f, 0), 0.15f);
        control.rotateClawTo(230, 0.15f);
        yield return new WaitForSeconds(0.15f);

        slamSound.PlayOneShot(slamSound.clip, slamSound.volume);
        if(Vector3.Distance(target.position, ClawPos.position) < 4)
		{
            PlayerStats stat = target.GetComponent<PlayerStats>();
            if(stat != null)
			{
                stat.ApplyDamage(slamDamage);
			}
		}

        inAction = false;
    }

    private IEnumerator FireLaserCoroutine()
	{
        inAction = true;

        control.LookPlayer(2.5f);
        control.moveClawTo(new Vector3(2, 5, 0), 1.5f);
        control.rotateClawTo(180, 1.5f);
        control.openClawTo(15, 1.5f);
        Laser.ToggleLight(true);
        yield return new WaitForSeconds(0.2f);

        laserSound.PlayOneShot(laserSound.clip, laserSound.volume);
        yield return new WaitForSeconds(1.3f);

        Laser.ToggleBeam(true);
        Laser.ToggleParticle(true);
        yield return new WaitForSeconds(1f);

        control.moveClawTo(new Vector3(3.5f, 5.5f, 0), 0.3f);
        control.rotateClawTo(225, 0.3f);
        yield return new WaitForSeconds(0.3f);

        control.moveClawTo(new Vector3(5, 6, 0), 0.2f);
        control.rotateClawTo(270, 0.15f);
        yield return new WaitForSeconds(0.2f);

        Laser.ToggleAll(false);
        yield return new WaitForSeconds(0.2f);
        inAction = false;
    }

    private IEnumerator FlinchCoroutine()
	{
        inAction = true;

        control.moveClawTo(new Vector3(1, 9, 0), 0.1f);
        control.rotateClawTo(180, 0.1f);
        control.openClawTo(120, 0.1f);
        yield return new WaitForSeconds(0.1f);

        control.moveClawTo(new Vector3(2, 8, 0), 1f);
        control.openClawTo(defaultClawAngle, 1f);
        yield return new WaitForSeconds(1);

        inAction = false;
    }

    private IEnumerator CooldownCoroutine(float time)
	{
        inAction = true;
        control.rotateBaseTo(defaultBaseRot, 1.5f);
        control.moveClawTo(defaultClawPos, 1.5f);
        control.rotateClawTo(defaultClawRot, 1.5f);
        control.openClawTo(defaultClawAngle, 1.5f);
        yield return new WaitForSeconds(time);

        inAction = false;
    }

    private IEnumerator MoveToCoroutine(float pos, float duration)
	{
        inMove = true;

        pos = Mathf.Clamp(pos, railMin, railMax);
        float finishTime = Time.time + duration;
        while (Time.time < finishTime)
        {
            if(transform.position.z - pos > railSpeed)
		    {
                transform.position -= Vector3.forward * railSpeed;
		    }else if(transform.position.z - pos < -railSpeed)
		    {
                transform.position += Vector3.forward * railSpeed;
		    }
		    else
		    {
                transform.position = new Vector3(transform.position.x, transform.position.y, pos);
                break;
		    }
            yield return null;
		}

        inMove = false;
    }

    private IEnumerator FollowPlayerCoroutine(float duration)
    {
        inMove = true;

        float finishTime = Time.time + duration;
        while (Time.time < finishTime)
        {
            float pos = Mathf.Clamp(target.position.z, railMin, railMax);

            if (transform.position.z - pos > railSpeed)
            {
                transform.position -= Vector3.forward * railSpeed;
            }
            else if (transform.position.z - pos < -railSpeed)
            {
                transform.position += Vector3.forward * railSpeed;
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, pos);
            }
            yield return null;
        }

        inMove = false;
    }
}
