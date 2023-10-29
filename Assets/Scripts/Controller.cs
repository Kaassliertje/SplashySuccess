using UnityEngine;

//deze code zorgt voor de communicatie tussen mijn controller en de game
public class Controller : MonoBehaviour
{
    public float potentiometer = 0;
    public bool knopje = false;
    [SerializeField]private bool isVirtueel = true;

    //Echte arduino 
    [SerializeField]private Arduino arduino;

    //Virtuele arduino
    private bool virtueelKnopje = false;
    private float virtuelePotentiometer = 0;

    private void Start()
    {
        arduino.OnCommunicationReceived.AddListener(UpdateVanArduino); //Laat onze functie luisteren naar het data ontvangen event. (Dus dat onze functie word aangeroepen als we data ontvangen)
    }

    private void OnDestroy()
    {
        arduino.OnCommunicationReceived.RemoveListener(UpdateVanArduino);
    }

    private void Update()
    {
        if(isVirtueel)
        {
            UpdateVanToetsenbord();
        }
    }

    private void UpdateVanArduino(string data)
    {
        if(isVirtueel == false) 
        {
            //Stel er gaat iets fout. Dan geven we dit terug
            bool arduinoKnopje = false;
            float arduinoPotentiometer = 0;

            if(data.Length > 0) //Hebben we iets ontvangen?
            {
                //Onze data ziet er uit als "knopje/potentiometer" dus bv "1/0.343253"
                string[] splitData = data.Split("/"); //x[] -> Lijst van x
                //Parse is een prgrammeer woord voor tekst veranderen in iets anders zodat de computer het begrijpt
                //Probeert het eerste gedeelte van de gesplitste data te veranderen in een int en slaat die op in resultaatknopje
                if(int.TryParse(splitData[0], out int resultaatKnopje)) //Wordt alleen uitgevoerd als het gelukt is.
                {
                    //Verandert een nummer in een bool
                    if(resultaatKnopje > 0)
                    {
                        arduinoKnopje = true;
                    } else
                    {
                        arduinoKnopje = false;
                    }
                }
                //Probeert het tweede gedeelte van de gesplitste data te veranderen in een float en slaat die op in resultaatPotentiometer. 
                if (float.TryParse(splitData[1], out float resultaatPotentiometer)) //Wordt alleen uitgevoerd als het gelukt is.
                {
                    arduinoPotentiometer = resultaatPotentiometer;
                }
            }

            //Het doet de waarde van de arduino in de controller
            knopje = arduinoKnopje;
            potentiometer = arduinoPotentiometer / 100f;
        }
    }

    private void UpdateVanToetsenbord()
    {
        //Regel 48 geeft aan dat als je op spatie drukt dat er dan iets gebeurt. Namelijk als de checkbox is aangevinkt gaat deze uit, anders gaat deze aan
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (virtueelKnopje == true)
            {
                virtueelKnopje = false;
            }
            else
            {
                virtueelKnopje = true;
            }
        }

        //Regel 61 geeft aan dat als je op pijltje omhoog drukt dat er dan iets gebeurt. Namelijk dat er 0.1 per seconde aan de rotatie van de virtuele potentiometer toegevoegd wordt
        if(Input.GetKey(KeyCode.UpArrow))
        {
            virtuelePotentiometer = virtuelePotentiometer + 0.5f * Time.deltaTime;
        }

        //Regel 67 geeft aan dat als je op pijltje omlaag drukt dat er dan iets gebeurt. Namelijk dat er 0.1 per seconde van de rotatie van de virtuele potentiometer afgehaald wordt
        if (Input.GetKey(KeyCode.DownArrow))
        {
            virtuelePotentiometer = virtuelePotentiometer - 0.5f * Time.deltaTime;
        }

        //Het zorgt ervoor dat de waarde van de virtuele potentiometer niet onder de nul en boven de een kan komen
        if (virtuelePotentiometer > 1f) virtuelePotentiometer = 1f;
        if (virtuelePotentiometer < 0f) virtuelePotentiometer = 0f;

        //Het doet de waarde van de virutele arduino in de controller
        knopje = virtueelKnopje;
        potentiometer = virtuelePotentiometer;
    }
}
