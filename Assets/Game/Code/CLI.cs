using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public GameObject m_CommandsDescription;
    public int m_maxResults;
    public List<string> m_sizes;
    public List<string> m_shapes;
    public List<string> m_colours;
    public List<string> m_names; //add initials only to box label
    public List<string> m_icons; //image
    public List<string> m_productNames;
    public Box m_selectedBox;
    public bool m_correctlyGuessed = false;
    private bool m_started = false;
    public GameObject m_pcCamera;

    public int m_points = 0;
    private Text m_pointsTxt;
    private Text m_playerInfoTxt;

    private int m_trueBox;
    private int[] m_guessedFeatures = { -1, -1, -1, -1, -1, -1 };
    private char m_guessedInitial = ' ';
    private int m_commandParsing = -1;

    #endregion


    void Start ()
    {
        DontDestroyOnLoad(this.gameObject);

        //fill in lists
        m_sizes = ReadFeaturesFromFile("sizes.txt");
        m_shapes = ReadFeaturesFromFile("shapes.txt");
        m_colours = ReadFeaturesFromFile("colours.txt");
        m_names = ReadFeaturesFromFile("names.txt");
        m_icons = ReadFeaturesFromFile("icons.txt");
        m_productNames = ReadFeaturesFromFile("productnames.txt");

        m_pointsTxt = GameObject.Find("PointsText").GetComponent<Text>();
        m_playerInfoTxt = GameObject.Find("PlayerInfo").GetComponent<Text>();

        m_CLIinput.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (m_pointsTxt != null)
        {
            m_pointsTxt.text = "Points: " + m_points;
            if (FindObjectOfType<BoxCustomizer>())
                m_playerInfoTxt.text = "Time Left : " + Mathf.RoundToInt(FindObjectOfType<BoxCustomizer>().m_timer) + " / Attempts Left : " + FindObjectOfType<BoxSpawner>().m_lives;
            else
                m_playerInfoTxt.text = "Attempts Left : " + FindObjectOfType<BoxSpawner>().m_lives;
        }

        if (SceneManager.GetActiveScene().name == "GameOverScene")
        {
            FindObjectOfType<CLI>().m_CLItext.text = "Game Over, your final score is: " + FindObjectOfType<CLI>().m_points.ToString();
            return;
        }

        if (m_commandParsing == -1) //we have no command
        {
            if (Input.GetKeyDown(KeyCode.S)) //size
            {
                m_commandParsing = 0;
            }

            if (Input.GetKeyDown(KeyCode.C)) //colour
            {
                m_commandParsing = 1;
            }

            if (Input.GetKeyDown(KeyCode.N)) //name initial
            {
                m_commandParsing = 2;
            }

            if (Input.GetKeyDown(KeyCode.I)) //image
            {
                m_commandParsing = 3;
            }

            if (Input.GetKeyDown(KeyCode.P)) //prodct name
            {
                m_commandParsing = 4;
            }

            if (Input.GetKeyDown(KeyCode.Return)) //start
            {
                if (!m_started)
                {
                    GenerateNewBox();
                    FindObjectOfType<BoxSpawner>().SpawnBox();
                    m_started = true;
                }
            }

            if (Input.GetKeyDown(KeyCode.H)) //help
            {
                m_commandParsing = 6;
            }

            if (Input.GetKeyDown(KeyCode.R)) //reset
            {
                m_commandParsing = 7;
            }

            if (Input.GetKeyDown(KeyCode.Tab)) //print tag
            {
                m_commandParsing = 8;
            }
        }
        else
        {
            //remove overlay and activate input box
            m_CommandsDescription.SetActive(false);
            m_CLIinput.gameObject.SetActive(true);
            m_CLIinput.Select();
            m_CLIinput.ActivateInputField();
        }
    }

    public void GenerateNewBox()
    {
        m_pcCamera.SetActive(true);

        //reset CLI variables
        m_guessedInitial = ' ';
        m_guessedFeatures = new int[] { -1, -1, -1, -1, -1, -1 };
        m_CLItext.text = "New Box Incoming!";

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
        bool commandFound = false;

        switch (m_commandParsing)
        {
            case 6:
                m_CLItext.text = "The following filters can be added to the search algorithm: \n - SIZE [S] \n  - COLOUR [C] \n - NAME [N] \n - ICON [I] \n - PRODUCT [P] \n - PRINT [TAB] \n - RESET FILTERS [R]";
                commandFound = true;
                break;

            case 7:
                m_guessedInitial = ' ';
                m_guessedFeatures = new int[] { -1, -1, -1, -1, -1, -1 };
                commandFound = true;
                break;

            case 0:
                m_guessedFeatures[0] = GetIdFromString(command.ToLower(), m_sizes);
                commandFound = true;
                break;

            //case "shape":
            //    m_guessedFeatures[1] = GetIdFromString(comm[1].ToLower(), m_shapes);
            //    commandFound = true;
            //    break;

            case 1:
                m_guessedFeatures[2] = GetIdFromString(command.ToLower(), m_colours);
                commandFound = true;
                break;

            case 2:
                //initials to full name mapping
                m_guessedInitial = command.ToLower()[0];
                m_guessedFeatures[3] = MapInitials(m_guessedInitial);
                commandFound = true;
                break;

            case 3:
                m_guessedFeatures[4] = GetIdFromString(command.ToLower(), m_icons);
                commandFound = true;
                break;

            case 4:
                m_guessedFeatures[5] = GetIdFromString(command.ToLower(), m_productNames);
                commandFound = true;
                break;

            case 8:
                int toPrint = int.Parse(command);
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
                GameObject tagLocation = GameObject.Find("TagSpawnerLocation");
                Instantiate((GameObject)Resources.Load("Tag"), tagLocation.transform.position, tagLocation.transform.rotation);
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
            return m_selectedBox.m_features[3];
        }
        else
        {
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
                    }
                    else if (m_guessedFeatures[j] == -1 && m_guessedInitial != ' ')
                        if (j == 3) //name
                        {
                            b.m_features[j] = GetNameWithInitial();
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
        ParseCommand();
        GenerateResults();
        m_CommandsDescription.SetActive(true);
        m_CLIinput.gameObject.SetActive(false);
        m_commandParsing = -1;
    }
}
