using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagSocket : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "TAG")
        {
            GetComponent<MeshRenderer>().sharedMaterial.color = Color.yellow;
            Destroy(other.gameObject);
            transform.parent.GetComponent<BoxCustomizer>().m_hasTag = true;
        }
    }
}
