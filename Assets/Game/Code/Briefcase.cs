using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Briefcase : MonoBehaviour {

    #region member variables

    public GameObject m_controller1, m_controller2;

    #endregion


    void Start ()
    {
		
	}
	
	void Update ()
    {
		transform.position = ( m_controller1.transform.position + m_controller2.transform.position ) / 2;
	}
}
