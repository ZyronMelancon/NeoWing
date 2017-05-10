using UnityEngine;
using System.Collections;

public class ShotControl : MonoBehaviour {
    public ParticleSystem m_ShotFX;
    public int m_ShotDamage = 5;

    Rigidbody2D m_shotPos;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update ()
    {

	}

    void OnDestroy()
    {
        m_ShotFX.transform.parent = null;
        m_ShotFX.Play();
        Destroy(m_ShotFX.gameObject, m_ShotFX.duration);
    }
}
