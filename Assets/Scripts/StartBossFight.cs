using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBossFight : MonoBehaviour
{
    public GameObject bossEntrance;
    public GameObject particles;
    AudioSource bossMusic;
    bool isClosed = false;

    public RobotArmAction Arm1;
    public RobotArmAction Arm2;

    // Start is called before the first frame update
    void Start()
    {
        bossMusic = GetComponent<AudioSource>();
        Arm1.enabled = false;
        Arm2.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !isClosed)
        {
            bossEntrance.SetActive(true);
            Instantiate(particles, new Vector3(393.38f, 4.311822f, 259.94f), Quaternion.identity);
            Instantiate(particles, new Vector3(393.38f, 4.311822f, 263.866f), Quaternion.identity);
            Instantiate(particles, new Vector3(393.38f, 4.311822f, 267.792f), Quaternion.identity);
            Instantiate(particles, new Vector3(393.38f, 4.311822f, 256.014f), Quaternion.identity);
            Instantiate(particles, new Vector3(393.38f, 4.311822f, 252.088f), Quaternion.identity);
            bossMusic.PlayDelayed(1.7143f);
            isClosed = true;

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                Destroy(enemy);
            }

            Arm1.enabled = true;
            Arm1.GetComponent<RobotArmHealth>().canTakeDamage = true;
            Arm2.enabled = true;
            Arm2.GetComponent<RobotArmHealth>().canTakeDamage = true;
        }
    }
}
