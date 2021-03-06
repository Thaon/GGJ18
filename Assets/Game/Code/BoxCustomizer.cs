﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BoxCustomizer : MonoBehaviour {

    #region member variables

    public Text m_nameTxt;
    public Text m_productNameTxt;
    public Image m_iconImg;
    public Sprite[] m_images;
    public Transform[] m_waypoints;
    public bool m_canMove = true;
    public bool m_hasTag = false;
    public float m_timeUntilDestruction;
    public GameObject m_effect;

    private Box m_selectedBox;
    private Material m_mat;
    private int m_waypointsCounter = 0;
    private Text m_timerText;
    public float m_timer = 0;

    public AudioClip m_conveyorClip;
    public AudioClip m_stoppedClip;
    public AudioClip m_rightClip;
    public AudioClip m_wrongClip;
    private bool m_rightGuess = false;

    private AudioSource m_source;

    #endregion

    void Start ()
    {
        CLI cli = FindObjectOfType<CLI>();
        m_selectedBox = cli.m_selectedBox;
        m_mat = GetComponentInChildren<MeshRenderer>().sharedMaterial;

        SetSize(cli.m_sizes[m_selectedBox.m_features[0]]);
        SetColour(cli.m_colours[m_selectedBox.m_features[2]]);
        m_nameTxt.text = GetRandomInitial(cli.m_names[m_selectedBox.m_features[3]]).ToString();
        m_iconImg.sprite = m_images[m_selectedBox.m_features[4]];
        m_productNameTxt.text = cli.m_productNames[m_selectedBox.m_features[5]];
        m_source = GetComponent<AudioSource>();

        m_timerText = GameObject.Find("TimerText").GetComponent<Text>();
        m_timer = m_timeUntilDestruction;
    }

    private void OnDestroy()
    {
        GameObject go = Instantiate(m_effect, transform.GetComponentInChildren<BoxCollider>().gameObject.transform.position, Quaternion.identity);
        if (m_rightGuess)
            go.GetComponent<AudioSource>().PlayOneShot(m_rightClip);
        else
            go.GetComponent<AudioSource>().PlayOneShot(m_wrongClip);
        Destroy(go, 3);
    }

    void Update ()
    {
		if (m_canMove)
        {
            if (Vector3.Distance(transform.position, m_waypoints[m_waypointsCounter].transform.position) > .1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, m_waypoints[m_waypointsCounter].position, 5 * Time.deltaTime);
                if (!m_source.isPlaying)
                    m_source.PlayOneShot(m_conveyorClip);
            }
            else
            {
                m_waypointsCounter++;
                m_canMove = false;
                if (m_source.isPlaying)
                {
                    m_source.Stop();
                    m_source.PlayOneShot(m_stoppedClip);
                    StartCoroutine(StopSounds());
                }
            }
        }

        if (Vector3.Distance(transform.position, m_waypoints[1].transform.position) < 1.0f)
        {
            //we reached the last point, check if there is a tag on the box and if it is correct
            if (m_hasTag && FindObjectOfType<CLI>().m_correctlyGuessed)
            {
                FindObjectOfType<CLI>().m_points += 100;
                m_rightGuess = true;
            }
            else
            {
                FindObjectOfType<BoxSpawner>().RemoveLife();
                FindObjectOfType<CLI>().m_points -= 50;
            }

            Debug.Log("Points: " + FindObjectOfType<CLI>().m_points);

            //remove the box
            FindObjectOfType<BoxSpawner>().m_boxInstance = null;
            FindObjectOfType<CLI>().GenerateNewBox();
            Destroy(this.gameObject);
        }

        if (m_waypointsCounter > 0)
        {
            m_timer -= Time.deltaTime;
            m_timerText.text = Mathf.RoundToInt(m_timer).ToString();

            if (m_timer <= 0)
            {
                FindObjectOfType<BoxSpawner>().RemoveLife();
                Destroy(this.gameObject);
            }
        }
	}

    IEnumerator StopSounds()
    {
        yield return new WaitForSeconds(0.5f);
        m_source.Stop();
    }

    private void SetSize(string size)
    {
        switch (size)
        {
            case "small":
                transform.localScale /= 2;
                break;

            case "big":
                transform.localScale *= 1.5f;
                break;
        }
    }

    private void SetColour(string colour)
    {
        switch (colour)
        {
            case "yellow":
                m_mat.color = Color.yellow;
                break;

            case "blue":
                m_mat.color = Color.blue;
                break;

            case "red":
                m_mat.color = Color.red;
                break;

            case "green":
                m_mat.color = Color.green;
                break;

            case "black":
                m_mat.color = Color.black;
                break;

            case "purple":
                m_mat.color = new Color(130, 0, 186);
                break;
        }
    }

    private char GetRandomInitial(string name)
    {
        string[] nameSections = name.Split(' ');
        int ran = Random.Range(0, 2);
        return nameSections[ran][0];
    }

    public void SendBox()
    {
        m_canMove = true;
    }
}
