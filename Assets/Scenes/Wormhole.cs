using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class WormholeData : TargetCircleData
{
    public WormholeData(List<Vector2> wormholePoints, float delay, int lifespan, float size, int matchFrame, bool shrinkingWhiteCircle) : base(Vector2.zero, 0f, delay, lifespan, size, matchFrame, shrinkingWhiteCircle)
    {
        this.wormholePoints = wormholePoints;
    }

    public List<Vector2> wormholePoints;
}

public class Wormhole : MonoBehaviour
{
    public static Wormhole Instance { get; private set; }
    public Vector2[] wormholePoints;
    public float wormholeSize = 1f;
    private BoxCollider2D boxCollider;
    private LineRenderer lineRenderer;
    private WormholeEndPoint wormholeEndPoint;
    public Color TravelColor;
    public Color OtherColor;
    private float timer;
    public float updateInterval = 1f / 30f;
    public int wormholePointTracker = 0;
    public int wormholeLifeSpan = 0;
    public int wormholePointTime = 13;
    public float wormholeTransparency = 0f;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        InitializeComponents();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            wormholeLifeSpan++;
            if (wormholeLifeSpan <= 5)
            {
                wormholeTransparency += 1 / 5f;
                wormholeTransparency = Mathf.Clamp(wormholeTransparency, 0f, 1f);
                spriteRenderer.color = new Color(1f, 1f, 1f, wormholeTransparency);
            }
            if (wormholeLifeSpan == 300)
            {
                Destroy(lineRenderer);
                Debug.Log("Despawned wormhole.");
            }
            timer = updateInterval;
        }
    }

    private void InitializeComponents()
    {
        GameObject lineColliderObject = new GameObject("WormholeCollider");
        lineColliderObject.transform.parent = transform;
        boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider == null)
        {
            boxCollider = gameObject.AddComponent<BoxCollider2D>();
        }

        // Find the WormholeEndPoint script in the scene
        wormholeEndPoint = FindObjectOfType<WormholeEndPoint>();

        // Initialize SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }

        // Initialize LineRenderer
        lineRenderer = lineColliderObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = wormholeSize;
        lineRenderer.endWidth = wormholeSize;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = TravelColor;
        lineRenderer.endColor = TravelColor;
        lineRenderer.numCornerVertices = 6;
    }

    public void Initialize(List<Vector2> futureWormholePoints, float travelTime)
{
    wormholePoints = futureWormholePoints.ToArray();
    wormholePointTracker = 0;
    UpdateCollider();
}


    public void UpdateCollider()
    {
        if (wormholePointTracker >= wormholePoints.Length)
        {
            Destroy(lineRenderer);
            return;
        }

        Vector2 startPoint2D = wormholePoints[wormholePointTracker];
        Vector2 endPoint2D = wormholePoints[wormholePointTracker];

        Vector3 startPoint = new Vector3(startPoint2D.x, startPoint2D.y, 0);
        Vector3 endPoint = new Vector3(endPoint2D.x, endPoint2D.y, 0);

        Vector3 center = (startPoint + endPoint) / 2f;
        float length = Vector3.Distance(startPoint, endPoint);
        boxCollider.size = new Vector2(length + 5, wormholeSize + 2);
        boxCollider.offset = Vector2.zero;
        boxCollider.transform.position = center;

        Vector3 direction = endPoint - startPoint;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        boxCollider.transform.rotation = Quaternion.Euler(0, 0, angle);

        lineRenderer.positionCount = wormholePoints.Length;
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            lineRenderer.SetPosition(i, wormholePoints[i]);
        }

        lineRenderer.startColor = TravelColor;
        lineRenderer.endColor = TravelColor;

        wormholeEndPoint?.UpdateEndPointCollider(startPoint2D);

        wormholePointTracker++;
        wormholeLifeSpan = 0;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (wormholePoints != null && wormholePoints.Length > 1)
        {
            for (int i = 0; i < wormholePoints.Length - 1; i++)
            {
                Gizmos.DrawLine(wormholePoints[i], wormholePoints[i + 1]);
            }
        }
    }
}