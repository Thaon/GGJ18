﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagSocket : MonoBehaviour {

    public Material m_attachedmaterial;
    public AudioClip m_clip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "TAG")
        {
            GetComponent<AudioSource>().PlayOneShot(m_clip);
            GetComponent<MeshRenderer>().sharedMaterial = m_attachedmaterial;
            transform.GetComponentInParent<BoxCustomizer>().m_hasTag = true;
            Destroy(other.gameObject);
        }
    }
}
