using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [Header("Vis")]
    public float snelheidVis; //pixels per seconde
    public float maximaleHoogteVis;
    public float minimaleHoogteVis;
    public float maximaleSnelheidVis;
    public float minimaleSnelheidVis;
    public float maximaleVisWisselTijd;
    public float minimaleVisWisselTijd;

    [Header("Balk")]
    public float minimaleHoogteBalk;
    public float maximaleHoogteBalk;
    public float balkGroote;

    [Header("Game")]
    public float totaleVangTijd = 2;

    [Header("Sleep dingen")]
    [SerializeField]private Controller controller;
    [SerializeField]private RectTransform vis;
    [SerializeField]private RectTransform playerBalk;
    [SerializeField]private GameObject spelerKanVangenVisual;
    [SerializeField]private Slider progressieBalk; 
    [SerializeField]private TMP_Text inGameScore;
    [SerializeField]private TMP_Text eindScore;
    [SerializeField]private TMP_Text timerText;
    [SerializeField]private ScreenManager screenManager;
    
    private int score = 0;
    private float timer = 0;
    private float progressieBalkPercentage = 0;
    private bool isPaused = true;
    private float visWisselTijd = 0;
    private float vangTijd = 0;
    private float vangTijdVermindering = 0.25f;
    private bool laatsteKnopje = false;

    public void StartGame()
    {
        score = 0;
        timer = 60;
        progressieBalkPercentage = 0;
        isPaused = false;
        laatsteKnopje = controller.knopje;
    }

    private void Update()
    {
        if(isPaused == false)
        {
            timer = timer - Time.deltaTime;
            if (timer <= 0)
            {
                GameOver();
            }
            timerText.text = Mathf.FloorToInt(timer).ToString();
            inGameScore.text = score.ToString();
            BeweegVis();
            BeweegBalk();
            UpdateProgressieBalk();
            UpdateVangen();
        }

    }

    private void UpdateVangen()
    {
        if(vangTijd / totaleVangTijd >= 1)
        {
            if(controller.knopje != laatsteKnopje) //Knopje is gewisseld
            {
                VangVis();
                laatsteKnopje = controller.knopje;
            }
            spelerKanVangenVisual.SetActive(true);
        } else
        {
            laatsteKnopje = controller.knopje; //Zodat je niet al van te voren kan klikken
            spelerKanVangenVisual.SetActive(false);
        }
    }

    private void VangVis()
    {
        score = score + 100;
        vangTijd = 0;
        //Verplaats de vis naar een nieuwe plek zodat het een nieuwe vis lijkt.
        Vector3 visPositie = vis.localPosition;
        visPositie.y = Random.Range(minimaleHoogteVis,maximaleHoogteVis);
        vis.localPosition = visPositie;
    }

    private void UpdateProgressieBalk()
    {
        vangTijd -= vangTijdVermindering * Time.deltaTime;
        if(Mathf.Abs(vis.localPosition.y-playerBalk.localPosition.y) < balkGroote)
        {
            vangTijd += Time.deltaTime;
        }
        if (vangTijd < 0) vangTijd = 0; //Voorkom een negatieve vangtijd
        float vangPercentage = vangTijd / totaleVangTijd;
        if (vangPercentage > 1) vangPercentage = 1; //Als de speler de vis meer dan de minimale vangtijd vast heeft wordt dit getal groter dan 100%, we moeten dit voor de progressie balk maximaal 100% maken zodat de balk niet verder dan vol gaat
        progressieBalk.value = vangPercentage;
    }

    private void BeweegBalk()
    {
        Vector3 balkPositie = playerBalk.localPosition;
        balkPositie.y = (minimaleHoogteBalk * (1 - controller.potentiometer)) + (maximaleHoogteBalk * controller.potentiometer);
        playerBalk.localPosition = balkPositie;
    }

    private void BeweegVis()
    {
        visWisselTijd = visWisselTijd - Time.deltaTime;
        if (visWisselTijd <= 0)
        {
            int richting = 1;
            if (Random.Range(0f, 1f) <= 0.5f) richting = -1;
            snelheidVis = Random.Range(minimaleSnelheidVis, maximaleSnelheidVis) * richting;
            visWisselTijd = Random.Range(minimaleVisWisselTijd, maximaleVisWisselTijd);
        }

        Vector3 visPositie = vis.localPosition;
        float nieuweHoogte = visPositie.y + snelheidVis * Time.deltaTime;
        if(nieuweHoogte < maximaleHoogteVis && nieuweHoogte > minimaleHoogteVis)
        {
            visPositie.y = nieuweHoogte;
        } else
        {
            visPositie.y = visPositie.y - snelheidVis * Time.deltaTime;
            snelheidVis = -snelheidVis;
        }
        vis.localPosition = visPositie;
    }

    private void GameOver()
    {
        screenManager.KiesScherm(2);
        eindScore.text = score.ToString();

    }
}
