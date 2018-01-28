using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DramaticAudio : MonoBehaviour {

    private AudioSource m_source;
    private BoxCustomizer m_box;

	void Start ()
    {
        m_source = GetComponent<AudioSource>();
        if (m_box)
            m_source.volume = m_box.m_timer / m_box.m_timeUntilDestruction;
        else
            m_source.volume = 0;
	}
	
	void Update ()
    {
        if (m_box)
            m_source.volume = 1 - (m_box.m_timer / m_box.m_timeUntilDestruction);
        else
        {
            m_box = FindObjectOfType<BoxCustomizer>();
            m_source.volume = 0;
        }
    }
}
