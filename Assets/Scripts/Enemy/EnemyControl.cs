using UnityEngine;
using System.Collections;

public class EnemyControl : MonoBehaviour {

    public GameObject m_Shot;
    public ParticleSystem m_Explosion;
    public float m_ShootSpeed;
    public float m_StartPos;
    public float m_ZoomInPos;
    public float m_MoveSpeed;
    public float m_Health;
    public float m_MoveTime;

    AudioSource m_ExplSFX;
    HUDUpdate HUD;
    EnemyManager EnMan;
    ShipControl ship;
    Rigidbody2D m_Body;
    public float m_WantedPos;

    void Start ()
    {
        m_Body = GetComponent<Rigidbody2D>();
        ship = GameObject.Find("Ship").GetComponent<ShipControl>();
        HUD = GameObject.Find("HUD").GetComponent<Canvas>().GetComponent<HUDUpdate>();
        StartCoroutine(Shoot());
        StartCoroutine(NewPos());
        EnMan = GameObject.Find("HUD").GetComponent<EnemyManager>();
        m_ExplSFX = GameObject.Find("EnemyKills").GetComponent<AudioSource>();
	}

    void Update()
    {
        if (m_Health <= 0)
        {
            HUD.enemykills++;
            EnMan.m_EnemiesAlive--;
            Destroy(gameObject);
        }
        Move();
        UpdateColor();
    }

    IEnumerator Shoot()
    {
        while(!ship.gameover)
        {
            yield return new WaitForSeconds(m_ShootSpeed);
            if (!ship.gameover)
            {
                Vector3 shotpos = transform.position + new Vector3(0, 0, -0.5f);
                Instantiate(m_Shot, shotpos, transform.rotation);
            }
        }
    }

    IEnumerator NewPos()
    {
        while (!ship.gameover)
        {
            yield return new WaitForSeconds(m_MoveSpeed + Random.Range(-0.5f,1.5f));
            if (!ship.gameover)
            {
                m_WantedPos = Random.Range(-3, 3);
                m_ZoomInPos = Random.Range(2.4f, 5.5f);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerShot")
        {
            Destroy(other.gameObject);
            m_Health -= 5;
            HUD.hits++;
        }
    }

    void UpdateColor()
    {
        GetComponent<SpriteRenderer>().color = new Color32(255, System.Convert.ToByte(150 + (105 * m_Health / 20)), System.Convert.ToByte(150 + (105 * m_Health / 20)), 255);
    }

    void OnDestroy()
    {
        m_Explosion.transform.parent = null;
        m_Explosion.Play();
        m_ExplSFX.Play();
        Destroy(m_Explosion.gameObject, m_Explosion.duration + 1f);
    }

    void Move()
    {
        Vector3 moveVel = m_Body.velocity;

        moveVel.y = 0;
        moveVel.x = 0;

        if (!ship.gameover)
        {
            moveVel.y = moveVel.y + (m_ZoomInPos - transform.position.y);
            moveVel.x = moveVel.x + (m_WantedPos - transform.position.x);
        }
        else
            moveVel.y = moveVel.y + (11 - transform.position.y);

        m_Body.velocity = moveVel;
    }
}
