using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Box
{
    public int[] m_features;

    public Box(int siz, int sh, int col, int nam, int ico, int pnam)
    {
        m_features = new int[6] { siz, sh, col, nam, ico, pnam };
    }

    public int[] DescribeBox()
    {
        return m_features;
    }
}

public class CLI : MonoBehaviour {

    #region member variables

    public List<string> m_sizes;
    public List<string> m_shapes;
    public List<string> m_colours;
    public List<string> m_names; //add initials only to box label
    public List<string> m_icons; //image
    public List<string> m_productNames;

    private Box m_selectedBox;
    private int[] m_guessedFeatures = { -1, -1, -1, -1, -1, -1 };
    private Text m_text;

    #endregion


    void Start ()
    {
        int size = Random.Range(0, m_sizes.Count);
        int shape = Random.Range(0, m_shapes.Count);
        int colour = Random.Range(0, m_colours.Count);
        int name = Random.Range(0, m_names.Count);
        int icon = Random.Range(0, m_icons.Count);
        int pName = Random.Range(0, m_productNames.Count);

        m_selectedBox = new Box(size, shape, colour, name, icon, pName);

        GenerateResults();
    }

    void Update ()
    {
		
	}

    public void ReadFeaturesFromFile(string filename, List<string> featureList)
    {
        string[] lines = File.ReadAllLines("textfiles/" + filename);
        foreach (string line in lines)
        {
            featureList.Add(line);
        }
    }

    public int ParseCommand(string value, string command)
    {
        string[] comm = command.Split(' ');

        switch (comm[0].ToLower())
        {
            case "reset":
                m_guessedFeatures = new int[] { -1, -1, -1, -1, -1, -1 };
                break;

            case "size":
                return GetIdFromString(comm[1].ToLower(), m_sizes);
                break;

            case "shape":
                return GetIdFromString(comm[1].ToLower(), m_shapes);
                break;
        }
        return -1;
    }

    public int GetIdFromString(string value, List<string> list)
    {
        if (list.Contains(value))
            return list.IndexOf(value);
        else
            return -1;
    }

    public string DescribeBox(int[] features)
    {
        return m_sizes[features[0]] + ", " + m_shapes[features[1]] + ", " + m_colours[features[2]] + ", " + m_names[features[3]] + ", " + m_icons[features[4]] + ", " + m_productNames[features[5]] + ". \n";
    }

    public void GenerateResults()
    {
        List<Box> results = new List<Box>(5);
        int trueBox = Random.Range(0, results.Capacity);
        for (int i = 0; i < results.Capacity; i++)
        {
            if (i == trueBox)
                results.Add(m_selectedBox);
            else
            {
                //create random box with the same requested box feature id
                int size = Random.Range(0, m_sizes.Count);
                int shape = Random.Range(0, m_shapes.Count);
                int colour = Random.Range(0, m_colours.Count);
                int name = Random.Range(0, m_names.Count);
                int icon = Random.Range(0, m_icons.Count);
                int pName = Random.Range(0, m_productNames.Count);
                Box b = new Box(size, shape, colour, name, icon, pName);
                for (int j = 0 ; j < m_guessedFeatures.Length ; j++)
                {
                    if ( m_guessedFeatures[j] != -1 )
                    {
                        b.m_features[j] = m_guessedFeatures[j];
                    }
                }
                results.Add(b);
            }
        }

        foreach(Box box in results)
        {
            Debug.Log(DescribeBox(box.m_features));
        }
    }
}
