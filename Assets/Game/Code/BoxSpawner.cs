using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour {

    #region member variables

    public GameObject m_boxInstance;
    public Transform m_spawningPosition;

    private GameObject m_boxPrefab;
    private bool m_gameStarted = false;

    #endregion


    void Start ()
    {
        m_boxPrefab = Resources.Load("Box") as GameObject;
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
        m_boxInstance = Instantiate(m_boxPrefab, m_spawningPosition.position, Quaternion.identity);
    }
}
