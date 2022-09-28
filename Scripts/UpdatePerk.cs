using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;

public class UpdatePerk : MonoBehaviour
{
    public List<InputField> inputFields = new List<InputField>();
    public List<GameObject> perkSlots = new List<GameObject>();
    public GetJsonData getJsonData;
    private int buildNumber, sizeList;
    private string perksSequence, prefixKey;

    // Start is called before the first frame update
    void Start()
    {
        prefixKey = "Build_";
        buildNumber = getBuildNumber();
        perksSequence = ("0;0;0;0");

        if(PlayerPrefs.HasKey(prefixKey + buildNumber.ToString())) // Verify the existence of a save
        {
            perksSequence = PlayerPrefs.GetString(prefixKey + buildNumber.ToString()); // If yes, get it back
        }

        StartCoroutine(BeforeStart());
    }

    IEnumerator BeforeStart() 
    {
        yield return new WaitUntil(() => getJsonData.isLoaded()); // Start the method after the end of the data url recovery

        sizeList = getJsonData.getUrlListSize();

        string[] perksArray = perksSequence.Split(';'); // We cut the recovered sequence in 4 values for the 4 slots of perks
        for(int i = 0; i < perksArray.Length; i++)
        {
            putImageOnSlot(int.Parse(perksArray[i]), perkSlots[i]); // We add the image whose url is that of the index i (value in the sequence) of the array of urls of the perks
            inputFields[i].text = perksArray[i]; // Note in the text bar the reference value of the perk
        }
    }

    private void putImageOnSlot(int i, GameObject gameObject)
    {
        Davinci.get()
                .load(getJsonData.getUrlFromList(i)) // Load the image of the given url
                .setFadeTime(0) // No fade animation
                .into(gameObject.GetComponent<Image>()) // Put the image in the gameObject that already has an image component
                .start();
    }

    private int getBuildNumber()
    {
        foreach(Match m in Regex.Matches(this.name, "\\d+")) // Allows you to retrieve only the build number from its name in the form: "Build (0)".
        {
            //Debug.Log("Index : " + m.Index + " Value : " + m.Value);
            Debug.Log("Build Number : " + m.Value);
            return int.Parse(m.Value); // Convert string value to int
        }
        return -1; // Returns -1 if error. In my opinion, if there is an error, the -1 will break everything
    }

    public void ChangePerk() // Allows to change the perk according to the number entered in the text field
    {
        List<int> interList = new List<int>();

        for(int i = 0; i < perkSlots.Count; i++)
        {
            int index = 0;

            try{
                index = int.Parse(inputFields[i].text); // Try to transform the string into a number. If the user writes something other than a number, the default perk 0 will be used
                
                if(index < 0 || index >= sizeList) // We check that the value entered corresponds to an existing perk
                {
                    index = 0;
                }
            }catch{}

            putImageOnSlot(index, perkSlots[i]); // We update the image
            interList.Add(index); // We update the values of the sequence

        }
        perksSequence=interList[0]+";"+interList[1]+";"+interList[2]+";"+interList[3]; // Final sequence
        Debug.Log(perksSequence);
    }

    public void RandomizeBuild()
    {
        List<int> interList = new List<int>();

        int randomIndexRole = UnityEngine.Random.Range(0, sizeList); // We choose a random value to define the role of the build: survivor or killer
        string randomRole = getJsonData.getRoleFromList(randomIndexRole);  // We choose a role corresponding to the above index

        for(int i = 0; i < perkSlots.Count; i++) // For each perk slot
        {
            bool perkFound = false;
            while (!perkFound) // Continue until a valid perk has been found (good role)
            {
                int randomIndexUrl = UnityEngine.Random.Range(0, sizeList); // Choose a random url index

                if(getJsonData.getRoleFromList(randomIndexUrl).Equals(randomRole)) // If url corresponds to a perk of a role identical to the one defined previously
                {
                    inputFields[i].text = randomIndexUrl.ToString(); // The text of the field takes the value random
                    putImageOnSlot(randomIndexUrl, perkSlots[i]); // We update the image
                    interList.Add(randomIndexUrl); // We update the sequence
                    perkFound = true; // We signal that a perk has been found to move to the next perk
                }
            }
        }
        perksSequence=interList[0]+";"+interList[1]+";"+interList[2]+";"+interList[3]; // Final sequence
        Debug.Log(perksSequence);
    }

    public void SaveCombinaison()
    {
        PlayerPrefs.SetString(prefixKey + buildNumber.ToString(), perksSequence); // Save the sequence
        Debug.Log("Saved Data");
    }
}