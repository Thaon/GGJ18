using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Mapper : MonoBehaviour {

    public GameObject m_mapperReference;

    private VRTK_ControllerEvents m_eventMgr;
    private bool m_pressingTrigger = false;

	void Start ()
    {
        m_eventMgr = GetComponent<VRTK_ControllerEvents>();

        m_eventMgr.TriggerPressed += TriggerDown;
        m_eventMgr.TriggerReleased += TriggerReleased;
    }

    void Update ()
    {
        if (m_pressingTrigger)
            Instantiate(m_mapperReference, transform.position, Quaternion.identity);
    }

    private void TriggerDown(object sender, ControllerInteractionEventArgs e)
    {
        m_pressingTrigger = true;
    }

    private void TriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        m_pressingTrigger = true;
    }
}
