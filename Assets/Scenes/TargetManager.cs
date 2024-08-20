using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TargetCircleManager : MonoBehaviour
{
    public static TargetCircleManager Instance { get; private set; }
    public GameObject targetCirclePrefab;
    public GameObject wormholePrefab;
    public List<List<TargetCircleData>> targetCircleDataLists;
    public int SongChoice { get; private set; }
    public List<Vector2> wormholePathPoints;
    public float travelTime = 5f;
    private Wormhole wormhole;
    public bool pausedGame = false;
    public bool tutorialShowUp = true;
    public TextMeshProUGUI tutorialText;
    public AnimatedCircleBackground animatedCircleBackground;
    
    public List<string> tutorialTexts = new List<string>();
    public int currentTextIndex = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        wormhole = FindObjectOfType<Wormhole>();
        if (animatedCircleBackground == null)
        {
            animatedCircleBackground = FindObjectOfType<AnimatedCircleBackground>();
        }

        targetCircleDataLists = new List<List<TargetCircleData>>();
        //JotaroTheme data list


        tutorialTexts.Add("Welcome to Lightstorm! Allow me to instruct you how to play. (Press Space)");
        tutorialTexts.Add("Try moving your mouse around, you'll notice you have an Arrow following you around!");
        tutorialTexts.Add("This Arrow is how you play the game, you see that circle that just appeared?");
        tutorialTexts.Add("You have to move your mouse, and by extension, your arrow, through the Circle when the White Circle closes on it");
        tutorialTexts.Add("You may also notice there are arrows on the circles. You need to go through the circles in that direction.");
        tutorialTexts.Add("Depending on how fast you react, you can get a 'Bad...', and 'Okay.', a 'Good!' and a 'Perfect!!!'");
        tutorialTexts.Add("Try to aim for 'Perfect!!!' as it the best. And see how high your score is!");
        tutorialTexts.Add("When your ready, just go through both of the notes! Blaze past them, making a long trail of Light! Bulldoze through the circles!");

        targetCircleDataLists.Add(new List<TargetCircleData>
        {
            new TargetCircleData(new Vector2(0, 1), 0.1f, 0f, 10, 3, 15, false),
            new TargetCircleData(new Vector2(2, -3), 90f, 2f, 10, 3, 15, false),
            new TargetCircleData(new Vector2(3, -1), 180f, 3f, 10, 3, 15, false),
        });
        // HydroCity data list
        targetCircleDataLists.Add(new List<TargetCircleData>
        {
            new TargetCircleData(new Vector2(4, -1), 0f, 0f, 23, 3, 10, false),
            new TargetCircleData(new Vector2(8, 2), 90f, 1.1f, 23, 3, 15, false),
            new TargetCircleData(new Vector2(5f, 1f), 220f, 1.5f, 23, 3.5f, 8, false),
            new TargetCircleData(new Vector2(2f, -1f), 140f, 1.75f, 23, 3.4f, 8, false),
            new TargetCircleData(new Vector2(-1f, 1f), 220f, 1.9f, 23, 3.4f, 8, false),
            new TargetCircleData(new Vector2(-4f, -1f), 140f, 2.2f, 23, 3.4f, 8, false),
            new TargetCircleData(new Vector2(-7f, 3f), 90f, 2.35f, 23, 3, 8, false),

            new TargetCircleData(new Vector2(-4f, 4.5f), 0f, 2.4f, 35, 4, 7, false),
            new TargetCircleData(new Vector2(0f, 4.5f), 0f, 2.4f, 35, 4, 8, false),
            new TargetCircleData(new Vector2(4f, 4.5f), 0f, 2.4f, 35, 4, 9, false),
            new TargetCircleData(new Vector2(6f, 0f), 240f, 2.55f, 40, 5, 12, false),

            new TargetCircleData(new Vector2(-2f, -3.5f), 180f, 5f, 23, 3, 10, false),
            new TargetCircleData(new Vector2(-6f, -3.5f), 140f, 5.1f, 23, 4, 10, false),
            new TargetCircleData(new Vector2(-9f, 2f), 110f, 5.2f, 23, 5, 10, false),
        
            new TargetCircleData(new Vector2(-7f, 3f), 290f, 5.9f, 20, 3, 10, false),
            new TargetCircleData(new Vector2(-6.5f, 0f), 290f, 5.9f, 20, 3, 11, false),
            new TargetCircleData(new Vector2(-3f, -3f), 70f, 6.15f, 20, 3, 13, false),
            new TargetCircleData(new Vector2(-2.5f, 0f), 70f, 6.15f, 20, 3, 15, false),
            new TargetCircleData(new Vector2(1f, 3f), 290f, 6.4f, 20, 3, 13, false),
            new TargetCircleData(new Vector2(1.5f, 0f), 290f, 6.4f, 20, 3, 15, false),
            new TargetCircleData(new Vector2(5f, -3f), 70f, 6.65f, 20, 3, 13, false),
            new TargetCircleData(new Vector2(5.5f, 0f), 70f, 6.65f, 20, 3, 15, false),

            new TargetCircleData(new Vector2(0f, 2f), 180f, 7f, 23, 3, 15, false),
            new TargetCircleData(new Vector2(-4f, 0f), 270f, 7.1f, 23, 4, 15, false),
            new TargetCircleData(new Vector2(0f, -4f), 0f, 7.2f, 23, 5, 15, false),

            new TargetCircleData(new Vector2(4f, 0f), 70f, 9f, 23, 3, 10, false),
            new TargetCircleData(new Vector2(4f, 0f), 250f, 9.6f, 23, 3, 10, false),
            new TargetCircleData(new Vector2(1.75f, 0f), 110f, 10.2f, 23, 3, 10, false),

            new TargetCircleData(new Vector2(-2f, 0f), 240, 11f, 23, 3, 10, false),
            new TargetCircleData(new Vector2(-4f, 0f), 60, 11.25f, 23, 3, 10, false),
            new TargetCircleData(new Vector2(-6f, 0f), 240, 11.5f, 23, 3, 10, false),
            new TargetCircleData(new Vector2(-8f, 0f), 60, 11.75f, 23, 3, 10, false),
            new TargetCircleData(new Vector2(-4f, 3f), 0, 12f, 23, 3, 10, false),
            new TargetCircleData(new Vector2(-3f, 0f), 240, 12.25f, 23, 3, 10, false),
            new TargetCircleData(new Vector2(-6f, 0f), 90, 12.5f, 23, 3, 10, false),
            
            new TargetCircleData(new Vector2(-6f, 2f), 270, 14.4f, 23, 3, 10, false),
            new TargetCircleData(new Vector2(-5.5f, -1f), 290, 14.55f, 23, 3, 10, false),
            new TargetCircleData(new Vector2(-1.3f, -4f), 310, 14.7f, 23, 3, 10, false),

            new TargetCircleData(new Vector2(3f, 0f), 90, 15.7f, 23, 3, 10, false),
            new TargetCircleData(new Vector2(0f, 3f), 180, 15.85f, 23, 3, 10, false),
            new TargetCircleData(new Vector2(-3f, 0f), 270, 16f, 23, 3, 10, false),
            new TargetCircleData(new Vector2(0f, -3f), 0, 16.15f, 23, 3, 10, false),

            new TargetCircleData(new Vector2(0f, 0f), 120, 16.3f, 23, 3, 15, false),
            new TargetCircleData(new Vector2(-3f, 3f), 290, 16.5f, 25, 3, 10, false),
            new TargetCircleData(new Vector2(3f, -3f), 120, 16.8f, 30, 3, 20, false),

            new TargetCircleData(new Vector2(0f, 2f), 0, 18.5f, 23, 3, 10, false),
            new TargetCircleData(new Vector2(0f, 2f), 180, 19.1f, 23, 3, 10, false),
            new TargetCircleData(new Vector2(0f, 2f), 0, 19.7f, 23, 3, 10, false),
        });

        //Diamonds data list
        targetCircleDataLists.Add(new List<TargetCircleData>
        {
            new TargetCircleData(new Vector2(0f, 2f), 90f, 0.3f, 23, 3f, 21, true),
            new TargetCircleData(new Vector2(0f, 3f), 90f, 0.3f, 23, 3f, 20, false),
            new TargetCircleData(new Vector2(4f, 0f), 320, 1f, 123, 3f, 78, true),
            new TargetCircleData(new Vector2(0f, 0f), 135f, 5.8f, 33, 3f, 31, true),
            new TargetCircleData(new Vector2(0f, 0f), 340f, 8.8f, 60, 3f, 55, true),
            new TargetCircleData(new Vector2(6f, 0f), 90f, 11f, 35, 3f, 22, true),
            new TargetCircleData(new Vector2(5f, -2f), 250f, 12f, 23, 3f, 21, true),
            new TargetCircleData(new Vector2(2.5f, 2f), 110f, 12f, 41, 3f, 40, true),
            new TargetCircleData(new Vector2(0f, -2f), 250f, 12f, 59, 3f, 58, true),
            new TargetCircleData(new Vector2(-2.5f, 2f), 110f, 12f, 77, 3f, 75, true),
            new TargetCircleData(new Vector2(-5f, -2f), 250f, 12f, 95, 3f, 94, true),
        
            new TargetCircleData(new Vector2(0f, 0f), 0f, 16f, 60, 3f, 30, true),
            new TargetCircleData(new Vector2(0f, -3f), 200f, 17f, 38, 3f, 25, true),
            new TargetCircleData(new Vector2(-3f, -1f), 100f, 17f, 53, 3f, 40, true),
            new TargetCircleData(new Vector2(0f, 0f), 340f, 18.2f, 38, 3f, 25, true),
            new TargetCircleData(new Vector2(3f, 2f), 80f, 18.2f, 53, 3f, 40, true),
            new TargetCircleData(new Vector2(4f, 3f), 315f, 20f, 40, 3f, 30, true),
            new TargetCircleData(new Vector2(0f, 1f), 135f, 21f, 40, 3f, 22, true),
            new TargetCircleData(new Vector2(-4f, -1f), 270f, 21.67f, 30, 3f, 22, true),
            new TargetCircleData(new Vector2(-4f, -2f), 270f, 21.67f, 30, 3f, 22, true),
            new TargetCircleData(new Vector2(-4f, -3f), 270f, 21.67f, 30, 3f, 22, true),

            new TargetCircleData(new Vector2(0f, -3f), 0f, 22.5f, 70, 3f, 21, true),
            new TargetCircleData(new Vector2(0f, 0f), 180f, 22.5f, 70, 3f, 34, true),
            new TargetCircleData(new Vector2(0f, 3f), 0f, 22, 80, 3f, 62, true),
            new TargetCircleData(new Vector2(3f, -3f), 240f, 24, 40, 3f, 27, true),


            new TargetCircleData(new Vector2(0f, 0f), 90f, 24.5f, 40, 3f, 30, true),
            new TargetCircleData(new Vector2(3f, 3f), 0f, 25f, 40, 3f, 30, true),
            new TargetCircleData(new Vector2(0f, 3f), 180f, 26f, 30, 3f, 21, true),
            new TargetCircleData(new Vector2(-3f, 3f), 180f, 26f, 30, 3f, 21, false),
            new TargetCircleData(new Vector2(-4f, 0f), 270f, 27f, 30, 3f, 21, true),

            new TargetCircleData(new Vector2(0f, 0f), 45f, 27.5f, 30, 3f, 21, true),
            new TargetCircleData(new Vector2(3f, 0f), 315f, 28f, 73, 3f, 52, true),
            new TargetCircleData(new Vector2(5f, 0f), 90f, 29.8f, 73, 3f, 51, true),
            new TargetCircleData(new Vector2(2f, 3f), 180f, 30.9f, 53, 3f, 42, true),
            new TargetCircleData(new Vector2(-2f, 0f), 215f, 32f, 30, 3f, 21, true),
            new TargetCircleData(new Vector2(-2f, 0f), 45f, 33.2f, 30, 3f, 21, true),
            new TargetCircleData(new Vector2(0f, 3f), 0f, 34f, 30, 3f, 21, true),
            new TargetCircleData(new Vector2(0f, 0f), 180f, 34.4f, 39, 3f, 21, true),
            new TargetCircleData(new Vector2(0f, -3f), 0f, 34.85315f, 30, 3f, 21, true),
            new TargetCircleData(new Vector2(3f, 0f), 90f, 35.50329f, 30, 3f, 21, true),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 36.10702f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 36.87327f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 37.477f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 39.03273f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 39.40425f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 40.00798f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 41.912f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 42.70148f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 43.39808f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 44.02502f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 44.74483f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 45.32533f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 45.92906f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 46.57923f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 47.15973f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 47.76344f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 49.73713f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 51.17677f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 51.80371f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 52.40744f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 54.35789f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 55.10094f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 56.81923f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 57.63192f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 59.90748f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 60.37188f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 61.25423f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 61.67219f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 61.97404f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 62.29913f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 62.32235f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 62.83319f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 64.76044f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 65.43383f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 67.05923f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 67.43075f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 68.08089f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 70.35646f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 70.68154f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 71.16914f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 71.88898f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 72.6088f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 72.91064f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 73.25896f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 75.27908f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 75.62737f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 75.95246f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 77.90294f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 78.22802f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 78.5531f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 80.759f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 81.08408f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 81.64136f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 82.4076f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 83.05777f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 83.38285f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 83.75437f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 85.68163f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 86.02991f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 86.37823f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 88.35191f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 88.65377f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 88.97885f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 91.23119f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 91.53307f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 92.81014f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 93.48352f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 93.78539f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 94.15691f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 95.38757f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 96.10738f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 96.75755f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 97.40771f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 98.70802f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 99.26529f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 100.5424f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 101.2622f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 101.8659f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 103.2824f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 104.5827f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 105.256f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 105.8598f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 106.2313f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 106.5331f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 107.1833f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 107.8799f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 108.4604f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 109.1802f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 109.8304f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 110.5038f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 111.1539f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 112.4078f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 113.7778f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 114.4047f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 115.0781f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 115.7282f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 116.3087f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 116.9357f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 117.6323f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 118.3057f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 119.0023f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 119.5595f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 120.1865f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 120.8831f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 122.7871f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 124.1803f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 124.8305f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 125.4806f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 126.154f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 127.4079f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 128.058f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 130.055f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 130.6587f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 133.2825f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 134.6525f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 135.3027f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 135.9296f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 137.8801f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 138.507f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 140.1788f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 140.5039f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 141.1541f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 143.4529f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 144.73f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 145.0318f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 145.4266f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 145.6588f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 146.0535f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 146.3089f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 147.2145f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 148.3291f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 148.6541f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 148.9792f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 149.9545f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 150.9529f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 151.2316f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 151.6031f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 153.8554f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 154.1805f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 154.6681f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 155.179f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 155.4808f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 156.131f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 156.456f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 156.7811f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 157.7796f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 158.7084f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 159.0799f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 159.4282f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 160.357f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 161.3555f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 161.6805f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 162.0056f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 162.9344f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 164.3276f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 164.6527f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 165.0939f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 165.6279f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 165.9762f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 166.6032f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 167.1837f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 168.3214f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 169.1341f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 169.7843f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 177.7255f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 178.3525f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 178.9794f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 179.6528f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 180.2797f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 180.9067f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 181.6265f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 182.2534f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 182.8339f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 183.577f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 184.1807f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 184.8308f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 185.4346f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 186.8742f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 187.1528f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 188.0584f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 189.9857f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 190.659f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 192.3541f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 192.6095f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 193.3293f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 193.3293f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 195.6281f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 195.93f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 196.8588f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 197.2071f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 197.8572f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 198.1823f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 198.5538f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 200.5043f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 200.8062f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 201.1313f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 203.0817f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 203.3836f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 203.7319f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 204.8f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 205.0786f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 205.3805f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 205.5663f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 205.7288f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 207.331f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 209.9084f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 210.2335f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 210.5585f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 210.9069f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 211.2784f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 211.6034f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 212.4626f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 212.8805f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 213.5539f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 213.879f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 214.1808f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 215.2025f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 215.5044f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 216.1313f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 217.0601f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 217.4084f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 217.7335f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 218.0818f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 218.3837f, 23, 3f, 10, false),
            new TargetCircleData(new Vector2(0f, 0f), 0f, 219.3357f, 23, 3f, 10, false),
        });

        // wormholePathPoints = new List<Vector2>
        // {
        //     new Vector2(-5, 0),
        //     new Vector2(0, 5),
        //     new Vector2(5, 0),
        //     new Vector2(0, -5)
        // };

        // // Initialize wormhole once with path points
        // wormhole.Initialize(wormholePathPoints, travelTime);
    }

    void Update()
    {
        if (tutorialShowUp)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ShowNextTutorialText();
            }
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                ShowPreviousTutorialText();
            }
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SongChoice = 2;
            SpawnCirclesForList(SongChoice);
            StartCoroutine(PauseGameWhenScoreIs500());
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            // Trigger the thrust pattern when 'T' key is pressed
            animatedCircleBackground.TriggerThrustPattern();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            // Stop any active pattern when 'X' key is pressed
            animatedCircleBackground.StopPattern();
        }
    }
    private IEnumerator HandleTutorial()
    {
        tutorialText.text = tutorialTexts[currentTextIndex];
        tutorialText.gameObject.SetActive(true);

        yield return new WaitUntil(() => ScoreManager.Instance.score >= 500);

        tutorialShowUp = false;
        pausedGame = false;
        tutorialText.gameObject.SetActive(false);
    }

    private void ShowNextTutorialText()
    {
        if (currentTextIndex < tutorialTexts.Count - 1)
        {
            currentTextIndex++;
            tutorialText.text = tutorialTexts[currentTextIndex];
        }
    }

    private void ShowPreviousTutorialText()
    {
        if (currentTextIndex > 0)
        {
            currentTextIndex--;
            tutorialText.text = tutorialTexts[currentTextIndex];
        }
    }

    private IEnumerator PauseGameWhenScoreIs500()
    {
        yield return new WaitForSeconds(1f);

        if (SongChoice == 2)
        {
            pausedGame = true;
            tutorialShowUp = true;
            tutorialText.gameObject.SetActive(true);

            yield return new WaitUntil(() => ScoreManager.Instance.score >= 500);

            pausedGame = false;
            tutorialShowUp = false;
            tutorialText.gameObject.SetActive(false);
        }
    }

    public void StopSpawningCircles()
    {
        StopAllCoroutines();
        pausedGame = true;
    }

    

void SpawnCirclesForList(int index)
{
    if (index < 0 || index >= targetCircleDataLists.Count)
    {
        Debug.LogError("Invalid index for targetCircleDataLists.");
        return;
    }

    var dataList = targetCircleDataLists[index];
    Debug.Log(dataList);
    
    foreach (var circleData in dataList)
    {
        if (IsWormhole(dataList))
        {
            StartCoroutine(SpawnWormhole(wormholePathPoints, circleData.delay));
        }
        else
        {
            StartCoroutine(SpawnCircles(circleData.position, circleData.angle, circleData.delay, circleData.lifespan, circleData.size, circleData.matchFrame, circleData.shrinkingWhiteCircle));
        }
    }
}

bool IsWormhole(List<TargetCircleData> dataList)
{
    // Implement your logic to determine if the dataList is for a wormhole
    return dataList.Count > 0 && dataList[0] is WormholeData;
}

IEnumerator SpawnCircles(Vector2 position, float angle, float delay, int lifespan, float size, int matchFrame, bool shrinkingWhiteCircle)
{
    float startTime = Time.time;

    while (Time.time - startTime < delay)
    {
        if (pausedGame)
        {
            yield return null; 
            startTime += Time.deltaTime; 
        }
        else
        {
            yield return null; 
        }
    }
    GameObject circle = Instantiate(targetCirclePrefab, position, Quaternion.identity);
    TargetCircle targetCircle = circle.GetComponent<TargetCircle>();
    targetCircle.Initialize(position, angle, lifespan, size, matchFrame, shrinkingWhiteCircle);
}




    IEnumerator SpawnWormhole(List<Vector2> wormholePathPointsHere, float delay)
    {
        if (pausedGame) yield break;
        yield return new WaitForSeconds(delay);
        wormhole.Initialize(wormholePathPointsHere, travelTime);
    }
}
