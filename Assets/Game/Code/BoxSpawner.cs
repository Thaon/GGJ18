using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour {

    #region member variables

    [HideInInspector]
    public GameObject m_boxInstance;
    public Transform m_spawningPosition;
    public Transform[] m_waypoints;

    private GameObject m_boxPrefab;
    private bool m_gameStarted = false;

    #endregion


    void Start ()
    {
        m_boxPrefab = Resources.Load("BoxRB") as GameObject;
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
        m_boxInstance.GetComponent<BoxCustomizer>().m_waypoints = m_waypoints;
        m_gameStarted = true;
    }
}
