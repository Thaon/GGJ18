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

    public Text m_CLItext;
    public InputField m_CLIinput;
    public int m_maxResults;
    public List<string> m_sizes;
    public List<string> m_shapes;
    public List<string> m_colours;
    public List<string> m_names; //add initials only to box label
    public List<string> m_icons; //image
    public List<string> m_productNames;
    public Box m_selectedBox;
    public bool m_correctlyGuessed = false;

    public int m_points = 0;


    private int m_trueBox;
    private int[] m_guessedFeatures = { -1, -1, -1, -1, -1, -1 };
    private char m_guessedInitial = ' ';

    #endregion


    void Start ()
    {
        //fill in lists
        m_sizes = ReadFeaturesFromFile("sizes.txt");
        m_shapes = ReadFeaturesFromFile("shapes.txt");
        m_colours = ReadFeaturesFromFile("colours.txt");
        m_names = ReadFeaturesFromFile("names.txt");
        m_icons = ReadFeaturesFromFile("icons.txt");
        m_productNames = ReadFeaturesFromFile("productnames.txt");

        GenerateNewBox();
    }

    public void GenerateNewBox()
    {
        //reset CLI variables
        m_guessedInitial = ' ';
        m_guessedFeatures = new int[] { -1, -1, -1, -1, -1, -1 };

        //generate selected box
        int size = Random.Range(0, m_sizes.Count);
        int shape = Random.Range(0, m_shapes.Count);
        int colour = Random.Range(0, m_colours.Count);
        int name = Random.Range(0, m_names.Count);
        int icon = Random.Range(0, m_icons.Count);
        int pName = Random.Range(0, m_productNames.Count);

        m_selectedBox = new Box(size, shape, colour, name, icon, pName);
    }

    public List<string> ReadFeaturesFromFile(string filename)
    {
        List<string> featureList = new List<string>();
        string[] lines = File.ReadAllLines("textfiles/" + filename);
        foreach (string line in lines)
        {
            featureList.Add(line);
        }
        return featureList;
    }

    public bool ParseCommand()
    {
        string command = m_CLIinput.text;
        string[] comm = command.Split(' ');
        bool commandFound = false;

        switch (comm[0].ToLower())
        {
            case "start":
                FindObjectOfType<BoxSpawner>().SpawnBox();
                commandFound = true;
                break;

            case "help":
                m_CLItext.text = "The following filters can be added to the search algorithm: \n - SIZE [small | medium | big] \n - SHAPE [box | tube | pyramid] \n - COLOUR [ multiple ] \n - NAME [ multiple, initials ] \n - ICON [ multiple ] \n - PRODUCT [ multiple ]";
                commandFound = true;
                break;

            case "reset":
                m_guessedInitial = ' ';
                m_guessedFeatures = new int[] { -1, -1, -1, -1, -1, -1 };
                commandFound = true;
                break;

            case "size":
                m_guessedFeatures[0] = GetIdFromString(comm[1].ToLower(), m_sizes);
                commandFound = true;
                break;

            case "shape":
                m_guessedFeatures[1] = GetIdFromString(comm[1].ToLower(), m_shapes);
                commandFound = true;
                break;

            case "colour":
                m_guessedFeatures[2] = GetIdFromString(comm[1].ToLower(), m_colours);
                commandFound = true;
                break;

            case "name":
                //initials to full name mapping
                m_guessedInitial = comm[1].ToLower()[0];
                m_guessedFeatures[3] = MapInitials(comm[1].ToLower()[0]);
                commandFound = true;
                break;

            case "icon":
                m_guessedFeatures[4] = GetIdFromString(comm[1].ToLower(), m_icons);
                commandFound = true;
                break;

            case "product":
                m_guessedFeatures[5] = GetIdFromString(comm[1].ToLower(), m_productNames);
                commandFound = true;
                break;

            case "print":
                int toPrint = int.Parse(comm[1]);
                m_CLItext.text = "Printing tag for box number " + toPrint;

                if (toPrint == m_trueBox)
                {
                    m_correctlyGuessed = true;
                    Debug.Log("Correct guess");
                }
                else
                {
                    m_correctlyGuessed = false;
                    Debug.Log("Wrong guess");
                }

                //reset features and display an empty screen
                m_guessedInitial = ' ';
                m_guessedFeatures = new int[] { -1, -1, -1, -1, -1, -1 };
                m_CLItext.text = "Printing tag...";

                commandFound = true;
                break;
        }
        m_CLIinput.text = "";
        return commandFound;
    }

    public int GetIdFromString(string value, List<string> list)
    {
        if (list.Contains(value))
            return list.IndexOf(value);
        else
        {
            return -1;
        }
    }

    public int MapInitials(char value)
    {
        string[] name = m_names[m_selectedBox.m_features[3]].Split(' ');
        if (name[0][0] == value || name[1][0] == value)
        {
            m_guessedInitial = value;
            return m_selectedBox.m_features[3];
        }
        else
        {
            m_guessedInitial = value;
            return -1;
        }
    }

    public int GetNameWithInitial()
    {
        //get a list of similar names
        List<int> namesWithMatchingInitials = new List<int>();
        foreach (string name in m_names)
        {
            string[] nameSections = name.Split(' ');
            if (nameSections[0].ToLower()[0] == m_guessedInitial || nameSections[1].ToLower()[0] == m_guessedInitial)
            {
                namesWithMatchingInitials.Add(m_names.FindIndex(X => X == name));
                Debug.Log("Found");
            }
        }
        //get a random one and return it
        return namesWithMatchingInitials[Random.Range(0, namesWithMatchingInitials.Count)];
    }

    public string DescribeBox(int[] features)
    {
        return m_sizes[features[0]] + ", " + m_shapes[features[1]] + ", " + m_colours[features[2]] + ", " + m_names[features[3]] + ", " + m_icons[features[4]] + ", " + m_productNames[features[5]] + ". \n";
    }

    public void GenerateResults()
    {
        List<Box> results = new List<Box>();
        m_trueBox = Random.Range(0, m_maxResults);

        //check wether I should hide the correct box based on guesses
        bool hide = false;
        for (int j = 0; j < m_guessedFeatures.Length; j++)
        {
            if (m_guessedFeatures[j] != -1)
            {
                if (m_guessedFeatures[j] != m_selectedBox.m_features[j])
                {
                    hide = true;
                    m_trueBox = -1;
                    break;
                }
            }
        }

        for (int i = 0; i < m_maxResults; i++)
        {
            if (i == m_trueBox && !hide)
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
                for (int j = 0; j < m_guessedFeatures.Length; j++)
                {
                    if (m_guessedFeatures[j] != -1)
                    {
                        b.m_features[j] = m_guessedFeatures[j];

                        if (j == 3) //name
                        {
                            b.m_features[j] = GetNameWithInitial();
                        }
                    }
                }
                results.Add(b);
            }
        }

        m_CLItext.text = "";
        for(int i = 0; i < results.Count; i++)
        {
            m_CLItext.text += i + "> " + DescribeBox(results[i].m_features);
        }
        Debug.Log("Selected box is number " + (m_trueBox).ToString());
    }

    public void ExecuteCommand()
    {
        string command = m_CLIinput.text;
        string[] comm = command.Split(' ');
        if (comm[0].ToLower() != "help")
        {
            if (!ParseCommand())
                m_CLItext.text = "Could not recognise the syntax or the command, type help to visualize a list of commands.";
            else
            {
                GenerateResults();
            }
        }
        else
            ParseCommand();
    }
}
