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
    private int circleLifeSpan;
    private int maxLifeSpan;
    private float circleTransparency;
    public float updateInterval = 1f / 30f; // Update coordinates every 1/30 seconds (30 FPS)
    private float timer;
    private SpriteRenderer spriteRenderer;
    public GameObject shrinkingCircle;
    private float initialShrinkingCircleSize;
    private float targetCircleSize;
    private int matchFrame;
    public float scoreMultiplier;
    public bool isShrinkingWhiteCircle;
    private float targetCircleWhite = 0.2f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        timer = updateInterval;
        circleTransparency = 0f;
        spriteRenderer.color = new Color(1f, 1f, 1f, circleTransparency);
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
            Destroy(gameObject);
            Debug.Log("Despawned.");
            scoreMultiplier = 1;
        }
        Debug.Log(matchFrame - circleLifeSpan);

        if (matchFrame == circleLifeSpan && isShrinkingWhiteCircle == false)
        {
            shrinkingCircle.SetActive(true);
        }
        else if (isShrinkingWhiteCircle == true)
        {
            if (matchFrame - circleLifeSpan == 19)
            {
                shrinkingCircle.SetActive(true);
                shrinkingCircle.transform.localScale = new Vector2(0.2f, 0.2f);
            }
            if (matchFrame - circleLifeSpan > 0 && matchFrame - circleLifeSpan <= 21)
            {
                targetCircleWhite = targetCircleWhite - 0.00701754385f;
                shrinkingCircle.transform.localScale = new Vector2((float)targetCircleWhite, (float)targetCircleWhite);
            }
        }

        timer = updateInterval;
    }
}

    private void CalculateScore()
    {
        int frameDifference = Mathf.Abs(circleLifeSpan - matchFrame);

        if (frameDifference <= 5)
        {
            Debug.Log("Perfect");
            ScoreManager.Instance.AddScore(500 * scoreMultiplier);
            scoreMultiplier = Mathf.Min(scoreMultiplier + 0.5f, 8);
        }
        else if (frameDifference <= 7)
        {
            Debug.Log("Good");
            ScoreManager.Instance.AddScore(300 * scoreMultiplier);
            scoreMultiplier = Mathf.Min(scoreMultiplier + 0.5f, 8);
        }
        else if (frameDifference <= 10)
        {
            Debug.Log("Okay");
            ScoreManager.Instance.AddScore(200 * scoreMultiplier);
            scoreMultiplier = Mathf.Min(scoreMultiplier + 0.5f, 8);
        }
        else
        {
            Debug.Log("Bad");
            ScoreManager.Instance.AddScore(100 * scoreMultiplier);
            scoreMultiplier = 1;
        }
    }
}