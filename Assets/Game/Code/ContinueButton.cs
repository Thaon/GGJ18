using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MonoBehaviour {


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Controller")
        {
            FindObjectOfType<BoxCustomizer>().m_canMove = true;
        }
    }
}
