using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPerksBuild : MonoBehaviour
{
    public GameObject elementPrefab;
    public int yDelay;
    public string playerPrefsKey;
    private int currentNbOfElements = 1;
    private int previousNbOfElements;
    private float originX = 960;
    private float originY = -440;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey(playerPrefsKey)) // Checks if several builds have already been created otherwise remains at 1
        {
            currentNbOfElements = PlayerPrefs.GetInt(playerPrefsKey); //DbD : NumberOfBuilds, Ow : NumberOfTeams
        }
        previousNbOfElements = currentNbOfElements; // Keeps the value in memory for comparison when adding or deleting in the future
        checkNumberOfBuilds();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentNbOfElements > previousNbOfElements)
        {
            previousNbOfElements = currentNbOfElements;
            createPerksBuild(currentNbOfElements-1);
        }
        else if(currentNbOfElements < previousNbOfElements)
        {
            previousNbOfElements = currentNbOfElements;
            if(currentNbOfElements != 0)
            {
                Destroy(GameObject.Find(elementPrefab.name.Split(' ')[0] + " (" + currentNbOfElements + ")"));  
            }
        }
    }

    private void createPerksBuild(int index)
    {
        GameObject perkClone;
        perkClone = Instantiate(elementPrefab, transform); // Create a clone based on elementPrefab
        perkClone.transform.name = elementPrefab.name.Split(' ')[0] + " (" + index + ")"; // Changes its name based on the model name only the index is changed
        perkClone.transform.parent = elementPrefab.transform.parent; // Indicates that the clone must have the same parent object as the model
        perkClone.transform.localPosition = new Vector2(originX, originY-yDelay*index); // Modification of its position to avoid clone superpositions
    }

    private void checkNumberOfBuilds()
    {
        for(int index = 0; index < currentNbOfElements; index ++)
        {
            if(index != 0)
            {
                createPerksBuild(index); // Created as many builds as the number previously retrieved
            }
        }  
    }

    public void AddBuild()
    {
        if(currentNbOfElements < 9){ // Max number of build otherwise leave the page
            currentNbOfElements++; // Increase the current number of builds by 1
            Debug.Log("Add --> " + currentNbOfElements);
        }
        PlayerPrefs.SetInt(playerPrefsKey, currentNbOfElements); // Updates the number of builds
        
    }

    public void DeleteBuild()
    {
        if(currentNbOfElements > 1) // Max number of builds otherwise no more models and superpositions
        {
            currentNbOfElements--; // Reduces the current number of builds by 1
            PlayerPrefs.DeleteKey(elementPrefab.name.Split(' ')[0] + "_" + currentNbOfElements); // Delete the build save
            Debug.Log("Remove --> " + currentNbOfElements);
        }
        PlayerPrefs.SetInt(playerPrefsKey, currentNbOfElements); // Updates the number of builds
    }
}
