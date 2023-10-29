using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    [SerializeField]private List<GameObject> schermen = new List<GameObject>();

    private void Awake()
    {
        KiesScherm(0); //Wanneer het spel begint, krijg je het main menu te zien
    }

    //Wisselt het scherm dat wordt weergegeven naar het scherm met het nummer 'schermnummer' (wat via de parameter bepaald is) in de lijst.
    public void KiesScherm(int schermNummer)
    {
        for (int i = 0; i < schermen.Count; i++) 
        //Maakt een variable genaamt i met de waarde 0. Daarna voert hij de code hieronder uit totdat i niet langer minder is dan het aantal schermen in de lijst. Na elke keer dat de code wordt uitgevoerd wordt i verhoogd met 1 door de i++
        {
            GameObject scherm = schermen[i]; //Slaat het scherm i uit de lijst op in de variabele scherm (Bijvoorbeeld voor namen[] = 1 Katharina, 2 Gerard, 3 Obongulus is namen[2] -> Gerard)
            scherm.SetActive(i == schermNummer); //Zet het scherm in de variable scherm aan als i gelijk is aan het schermnummer van de parameter. Anders zet het scherm uit.
        }
    }
}
