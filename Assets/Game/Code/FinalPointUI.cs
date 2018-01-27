using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalPointUI : MonoBehaviour {

	void Start ()
    {
        GetComponent<Text>().text = "GAME OVER!\nFinal Score: " + FindObjectOfType<CLI>().m_points;
    }
}
