using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

// Same operation as "UpdatePerk.cs" with + or - identical methods
public class SaveOwTeam : MonoBehaviour
{
    public List<InputField> inputFields = new List<InputField>();
    public List<Slider> sliders = new List<Slider>();
    private string teamSequence, prefixKey;
    private int teamNumber;

    // Start is called before the first frame update
    void Start()
    {
        prefixKey = "Team_";
        teamNumber = getTeamNumber();
        teamSequence = ";1;;1;;1;;1;;1;;1";

        if(PlayerPrefs.HasKey(prefixKey + teamNumber.ToString())){
            teamSequence = PlayerPrefs.GetString(prefixKey + teamNumber.ToString());
        }
        getSavedData(teamSequence);
    }

    public void getSavedData(string teamSequence)
    {
        Debug.Log(teamSequence);
        string[] teamsArray = teamSequence.Split(char.Parse(";"));
        for (int i = 0; i < inputFields.Count + sliders.Count; i++)
        {
            if(i%2 == 0)
            {
                inputFields[i/2].text = teamsArray[i];
            }
            else
            {
                sliders[i/2+(1/2)].value = int.Parse(teamsArray[i]);
            }
        }
    }

    private int getTeamNumber()
    {
        foreach(Match m in Regex.Matches(this.name, "\\d+"))
        {
            //Debug.Log("Index : " + m.Index + " Value : " + m.Value);
            Debug.Log("Build Number : " + m.Value);
            return int.Parse(m.Value);
        }
        return -1;
    }

    public void saveTeam(){
        teamSequence = "";
        for(int i = 0; i < inputFields.Count-1; i++){
            teamSequence = teamSequence + inputFields[i].text + ";" + sliders[i].value + ";";
        }
        teamSequence = teamSequence + inputFields[inputFields.Count-1].text + ";" + sliders[inputFields.Count-1].value;
        Debug.Log(prefixKey + teamNumber.ToString() + " : " + teamSequence);
        PlayerPrefs.SetString(prefixKey + teamNumber.ToString(), teamSequence);
    }
}
