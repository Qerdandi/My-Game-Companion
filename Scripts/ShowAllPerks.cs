using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/*public static class PerkButtonAction
{
    public static void AddEventListener<T>(this Button button, T parm, Action<T> OnClick)
    {
        button.onClick.AddListener(delegate() {
            OnClick(parm);
        });
    }
}*/

public class ShowAllPerks : MonoBehaviour
{
    public List<Sprite> allPerksList = new List<Sprite>();
    public GetJsonData getJsonData;
    private bool loaded = false;

    // Use this for initialization
    void Start () 
    {
        StartCoroutine(beforeStart()); // Launches beforeStart method with a wait
    }

    IEnumerator beforeStart() 
    {
        yield return new WaitUntil(() => getJsonData.isLoaded()); // Waits for getJsonData to finish retrieving all data
        createPerks();
    }

    void createPerks()
    {
        float originY = 380f; // Position of the perk object model
        float originX = -850f;
        int size = getJsonData.getUrlListSize(); // Retrieves the total number of perks in memory

        GameObject perkPrefab = transform.GetChild(0).gameObject; // Use the object already created in Unity positioned in the 1st sub-folder as a template
        GameObject perkClone;

        Debug.Log("Starting creating clones");
        for(int i = 0; i < size; i++)
        {
            if(i%16 == 0 && i!= 0) // Manages the position of each perk to avoid overlapping
            {
                originY=originY-110;
                originX=originX-16*110;
            }
            perkClone = Instantiate(perkPrefab, transform); // Created an object identical to the perkPrefab template
            Davinci.get() // Use the Davinci library to download the image of the perks
                    .load(getJsonData.getUrlFromList(i)) // Load the image whose url is at index i of the list in getJsonData
                    .setFadeTime(0) // The appearance is instant : no fade animation
                    .into(perkClone.transform.GetComponent<Image>()) // The image is put in the dedicated component of the created clone
                    .start();
            perkClone.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = i.ToString(); // Addition of the reference index under the perk to be able to enter it quickly in the input spaces
            perkClone.transform.localPosition = new Vector2(originX+110*i, originY); // Modification of the position of the created perk
            //perkClone.GetComponent<Button>().AddEventListener(i, ShowIndexPerk);
        }
        Destroy(perkPrefab); //On supprime le modèle d'origine
        loaded = true;
    }

    public bool isLoaded() { return loaded; }
}
