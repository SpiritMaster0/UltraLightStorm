using UnityEngine;
using TMPro;

public class TargetCircleData
{
    public Vector2 position;
    public float angle;
    public float delay;
    public int lifespan;
    public float size;
    public int matchFrame;
    public bool shrinkingWhiteCircle;

    public TargetCircleData(Vector2 position, float angle, float delay, int lifespan, float size, int matchFrame, bool shrinkingWhiteCircle)
    {
        this.position = position;
        this.angle = angle;
        this.delay = delay;
        this.lifespan = lifespan;
        this.size = size;
        this.matchFrame = matchFrame;
        this.shrinkingWhiteCircle = shrinkingWhiteCircle; 
    }
}

public class TargetCircle : MonoBehaviour
{
    public static TargetCircle Instance { get; private set; }
    public GameObject scoreSplashEffectPrefab; 
    private int circleLifeSpan;
    private int maxLifeSpan;
    private float circleTransparency;
    public float updateInterval = 1f / 30f; 
    private float timer;
    private SpriteRenderer spriteRenderer;
    public GameObject shrinkingCircle;
    private float initialShrinkingCircleSize;
    private float targetCircleSize;
    private int matchFrame;
    public float scoreMultiplier;
    public bool isShrinkingWhiteCircle;
    private float targetCircleWhite = 0.2f;
    public GameObject judgementPrefab;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        timer = updateInterval;
        circleTransparency = 0f;
        spriteRenderer.color = new Color(1f, 1f, 1f, circleTransparency);
    }

    void Awake()
    {
        Instance = this;
    }

    public void Initialize(Vector2 position, float angle, int lifeSpan, float size, int matchFrame, bool shrinkingWhiteCircle)
    {
        maxLifeSpan = lifeSpan;
        circleLifeSpan = 0;
        transform.position = position;
        transform.localScale = new Vector2(size, size);
        transform.rotation = Quaternion.Euler(0, 0, angle);
        targetCircleSize = size;
        this.matchFrame = matchFrame;

        BoxCollider2D circleCollider = GetComponent<BoxCollider2D>();
        circleCollider.isTrigger = true;

        Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
        rb.isKinematic = true;
        isShrinkingWhiteCircle = shrinkingWhiteCircle;

        gameObject.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "LineCollider" && other.GetComponent<BoxCollider2D>() != null)
        {
            Vector2[] linePoints = DrawingManager.Instance.linePoints;

            if (linePoints.Length > 1)
            {
                Vector2 directionToFirstPoint = linePoints[0] - linePoints[1];
                float angle = Vector2.Angle(transform.right, directionToFirstPoint);

                if (angle <= 30f)
                {
                    Debug.Log("Success");
                    CalculateScore();
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("Failed: Angle too large");
                }
            }
        }
    }

    void Update()
    {
        if (TargetCircleManager.Instance.pausedGame) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            circleLifeSpan++;
            if (circleLifeSpan <= 5)
            {
                circleTransparency += 1f / 5f;
                circleTransparency = Mathf.Clamp(circleTransparency, 0f, 1f);
                spriteRenderer.color = new Color(1f, 1f, 1f, circleTransparency);
            }
            else if (circleLifeSpan >= maxLifeSpan - 3 && circleLifeSpan <= maxLifeSpan)
            {
                circleTransparency -= 1f / 3f;
                circleTransparency = Mathf.Clamp(circleTransparency, 0f, 1f);
                spriteRenderer.color = new Color(1f, 1f, 1f, circleTransparency);
            }
            if (circleLifeSpan >= maxLifeSpan)
            {

                Debug.Log("Despawned.");
                string judgementState = "Miss"; 
                HealthManager.Instance.UpdateHealth(-10f); 
                GameObject judgementObject = Instantiate(judgementPrefab, transform.position, Quaternion.identity);
                JudgementManager judgementManager = judgementObject.GetComponent<JudgementManager>();
                if (judgementManager != null)
                {
                    judgementManager.Initialize(transform.position, judgementState);
                }
                else
                {
                    Debug.LogError("JudgementManager component not found on the instantiated prefab.");
                }
                scoreMultiplier = 1;
                Destroy(gameObject);
            }
            if (matchFrame == circleLifeSpan && !isShrinkingWhiteCircle)
            {
                shrinkingCircle.SetActive(true);
            }
            else if (isShrinkingWhiteCircle)
            {
                if (matchFrame - circleLifeSpan == 19)
                {
                    shrinkingCircle.SetActive(true);
                    shrinkingCircle.transform.localScale = new Vector2(0.2f, 0.2f);
                }
                if (matchFrame - circleLifeSpan > 0 && matchFrame - circleLifeSpan <= 21)
                {
                    targetCircleWhite -= 0.00701754385f;
                    shrinkingCircle.transform.localScale = new Vector2(targetCircleWhite, targetCircleWhite);
                }
            }

            timer = updateInterval;
        }
    }

    private void CalculateScore()
    {
        int frameDifference = circleLifeSpan - matchFrame; 
        int windowSize = isShrinkingWhiteCircle ? 3 : 5; 
        string judgementState = "Bad"; 
        Debug.Log(frameDifference);

        if (frameDifference <= -8) 
{
    Debug.Log("Bad");
    ScoreManager.Instance.AddScore(100 * scoreMultiplier);
    HealthManager.Instance.UpdateHealth(-6f); 
    scoreMultiplier = 1;
    judgementState = "Bad";
}
else if (frameDifference <= -5) 
{
    Debug.Log("Okay");
    ScoreManager.Instance.AddScore(100 * scoreMultiplier);
    scoreMultiplier = 1;
    judgementState = "Okay";
}
else if (frameDifference <= -3) 
{
    Debug.Log("Good");
    ScoreManager.Instance.AddScore(100 * scoreMultiplier);
    HealthManager.Instance.UpdateHealth(5f); 
    scoreMultiplier = 1;
    judgementState = "Good";
}
else if (Mathf.Abs(frameDifference) <= windowSize) 
{
    Debug.Log("Perfect");
    ScoreManager.Instance.AddScore(500 * scoreMultiplier);
    scoreMultiplier = Mathf.Min(scoreMultiplier + 0.5f, 8);
    HealthManager.Instance.UpdateHealth(7f); 
    judgementState = "Perfect";
}
else if (Mathf.Abs(frameDifference) <= windowSize + 5) 
{
    Debug.Log("Good");
    ScoreManager.Instance.AddScore(300 * scoreMultiplier);
    scoreMultiplier = Mathf.Min(scoreMultiplier + 0.5f, 8);
    HealthManager.Instance.UpdateHealth(5f); 
    judgementState = "Good";
}
else if (Mathf.Abs(frameDifference) <= windowSize + 8)
{
    Debug.Log("Okay");
    ScoreManager.Instance.AddScore(200 * scoreMultiplier);
    scoreMultiplier = Mathf.Min(scoreMultiplier + 0.5f, 8);
    judgementState = "Okay";
}
else
{
    Debug.Log("Bad");
    ScoreManager.Instance.AddScore(100 * scoreMultiplier);
    HealthManager.Instance.UpdateHealth(-6f); 
    scoreMultiplier = 1;
    judgementState = "Bad";
}

        // Instantiate the judgementPrefab and initialize it
        GameObject judgementObject = Instantiate(judgementPrefab, transform.position, Quaternion.identity);
        JudgementManager judgementManager = judgementObject.GetComponent<JudgementManager>();
        if (judgementManager != null)
        {
            judgementManager.Initialize(transform.position, judgementState);
        }
        else
        {
            Debug.LogError("JudgementManager component not found on the instantiated prefab.");
        }
    }
}
