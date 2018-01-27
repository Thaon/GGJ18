using UnityEngine;
using UnityEngine.UI;

public class BoxCustomizer : MonoBehaviour {

    #region member variables

    public Text m_nameTxt;
    public Text m_productNameTxt;
    public Image m_iconImg;
    public Sprite[] m_images;

    private Box m_selectedBox;
    private Material m_mat;

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
}
