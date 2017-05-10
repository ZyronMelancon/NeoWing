using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControl : MonoBehaviour {

    public GameObject m_ShotType; //The shot prefab
    public int m_ShotDamage; //Amount of damage each shot prefab does
    public int m_ShotNumber; //The amount of shot prefabs per blast
    public float m_ShotSpeed; //The speed of the shot prefab
    public float m_ShotCooldown; //The amount of time between each blast
    public float m_Spread; //The angle of spread
    public bool m_SpreadRandom; //Random angle for each blast? Or uniform?
    public float m_ShotDestroyTime; //Time until destroying the shot prefab
    public AudioSource m_ShootSound; //Sound to play at each blast

    ShipControl ship;
    float hinput;

	// Use this for initialization
	void Start ()
    {
		ship = GameObject.Find("Ship").GetComponent<ShipControl>();
        StartCoroutine(Shoot());
    }
	
	// Update is called once per frame
	void Update ()
    {
        hinput = Input.GetAxisRaw("Horizontal");
    }

    IEnumerator Shoot()
    {
        while(!ship.gameover)
        {
            if (Input.GetAxisRaw("Fire1") == 1)
            {
                for (int i = m_ShotNumber; i > 0; i--)
                {
                    //Make a shot
                    GameObject thisshot = Instantiate(m_ShotType, transform.position, transform.rotation);
                    //Set damage
                    thisshot.GetComponent<ShotControl>().m_ShotDamage = m_ShotDamage;

                    //Currently, m_SpreadRandom does nothing, but I will add that in the next update.

                    //Here is where we send the shot in the direction it shall go
                    Vector2 shotvec = new Vector2(-Mathf.Sin((ship.transform.rotation.z + Random.Range(-m_Spread / 100, m_Spread / 100)) * 2), Mathf.Cos((ship.transform.rotation.z + Random.Range(-m_Spread / 100, m_Spread / 100)) * 2));
                    thisshot.GetComponent<Rigidbody2D>().AddForce(shotvec * (m_ShotSpeed * 100));

                    //Destroy it if it hasn't hit anything in the amount of time set
                    Destroy(thisshot, m_ShotDestroyTime);
                }
                m_ShootSound.Play();
                ship.m_ShotsFired++;
            }
            yield return new WaitForSeconds(m_ShotCooldown / 10);
        }
    }
}
