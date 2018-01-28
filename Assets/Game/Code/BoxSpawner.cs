using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoxSpawner : MonoBehaviour {

    #region member variables

    [HideInInspector]
    public GameObject m_boxInstance;
    public Transform m_spawningPosition;
    public Transform[] m_waypoints;
    public float m_timeUntilDestruction;
    public int m_lives = 3;

    private GameObject m_boxPrefab;
    private bool m_gameStarted = false;


    public AudioClip m_incomingClip;
    private AudioSource m_source;

    #endregion


    void Start ()
    {
        m_boxPrefab = Resources.Load("BoxRB") as GameObject;
        m_source = GetComponent<AudioSource>();
    }
	
	void Update ()
    {
		if (m_gameStarted)
        {
            if (m_boxInstance == null)
                SpawnBox();
        }
	}

    public void SpawnBox()
    {
        m_source.PlayOneShot(m_incomingClip);
        FindObjectOfType<CLI>().GenerateNewBox();
        m_boxInstance = Instantiate(m_boxPrefab, m_spawningPosition.position, Quaternion.identity);
        m_boxInstance.GetComponent<BoxCustomizer>().m_waypoints = m_waypoints;
        m_boxInstance.GetComponent<BoxCustomizer>().m_timeUntilDestruction = m_timeUntilDestruction;
        m_timeUntilDestruction -= 5.0f;
        m_gameStarted = true;
    }

    public void RemoveLife()
    {
        m_lives--;
        if (m_lives <= 0)
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }
}
