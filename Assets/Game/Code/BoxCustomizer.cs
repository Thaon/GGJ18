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

    private Box m_selectedBox;
    private Material m_mat;
    private int m_waypointsCounter = 0;

    #endregion

    void Start ()
    {
        CLI cli = FindObjectOfType<CLI>();
        m_selectedBox = cli.m_selectedBox;
        m_mat = GetComponent<MeshRenderer>().sharedMaterial;


        SetSize(cli.m_sizes[m_selectedBox.m_features[0]]);
        SetColour(cli.m_colours[m_selectedBox.m_features[2]]);
        m_nameTxt.text = GetRandomInitial(cli.m_names[m_selectedBox.m_features[3]]).ToString();
        m_iconImg.sprite = m_images[m_selectedBox.m_features[4]];
        m_productNameTxt.text = cli.m_productNames[m_selectedBox.m_features[5]];
    }
	
	void Update ()
    {
		if (m_canMove)
        {
            if (Vector3.Distance(transform.position, m_waypoints[m_waypointsCounter].transform.position) > .5f)
                transform.position = Vector3.MoveTowards(transform.position, m_waypoints[m_waypointsCounter].position, 5 * Time.deltaTime);
            else
            {
                m_waypointsCounter ++;
                m_canMove = false;
            }
        }

        if (Vector3.Distance(transform.position, m_waypoints[1].transform.position) < 1.0f)
        {
            //we reached the last point, check if there is a tag on the box and if it is correct
            if (m_hasTag && FindObjectOfType<CLI>().m_correctlyGuessed)
                FindObjectOfType<CLI>().m_points += 100;
            else
                FindObjectOfType<CLI>().m_points -= 50;

            //remove the box
            FindObjectOfType<BoxSpawner>().m_boxInstance = null;
            Destroy(this.gameObject);
        }
	}

    private void SetSize(string size)
    {
        switch (size)
        {
            case "small":
                transform.localScale /= 2;
                break;

            case "big":
                transform.localScale *= 2;
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

            case "white":
                m_mat.color = Color.white;
                break;

            case "grey":
                m_mat.color = Color.grey;
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
