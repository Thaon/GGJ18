using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagSocket : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "TAG")
        {
            GetComponent<MeshRenderer>().sharedMaterial.color = Color.yellow;
            transform.GetComponentInParent<BoxCustomizer>().m_hasTag = true;
            Destroy(other.gameObject);
        }
    }
}
