using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class GetJsonData : MonoBehaviour
{
    private string url = "https://raw.githubusercontent.com/TakeUpTech/Projects/main/MyGamesCompanionData/DeadByDaylightApi.json"; // Api containing DBD data
    private string jsonFile;
    [SerializeField] private List<string> urlPerksList = new List<string>();
    [SerializeField] private List<string> rolePerksList = new List<string>();
    private bool loaded = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(downloadJsonApi(url)); // Launch the method downloadJsonApi with a wait
    }

    IEnumerator downloadJsonApi(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest(); // Waits for the end of the web request to move on
            jsonFile = webRequest.downloadHandler.text; // Download the text retrieved by the web request
            Debug.Log("Download json finished");
        }
        addUrlToList();
    }

    private void addUrlToList()
    {
        JSONNode data = JSON.Parse(jsonFile); // Cutting the json with the SimpleJSON library
        foreach(JSONNode perk in data) // For each variable associated with the recovered data
        {
            if(!urlPerksList.Contains(perk["icon"].Value)) // If the variable has not already been added to the list then
            {
                urlPerksList.Add(perk["icon"].Value); // Adds to the list the variable that has as an antecedent the data name "icon".
                rolePerksList.Add(perk["role"].Value); // Adds to the list the variable has as an antecedent the data name "role".
            }
        }
        //urlPerksList.Sort();
        Debug.Log("Adding url to list finished");
        loaded = true; // Tells everyone (other scripts) who has completed data recovery
    }

    public string getUrlFromList(int index)
    {
        return urlPerksList[index]; // Returns the url variable of the index image i from the list
    }

    public string getRoleFromList(int index)
    {
        return rolePerksList[index]; // Returns the role variable of a perk of index i from the list
    }

    public int getUrlListSize()
    {
        return urlPerksList.Count; // Returns the number of perks in memory
    }

    public bool isLoaded()
    {
        return loaded; // Returns the state of the loaded variable
    }
}