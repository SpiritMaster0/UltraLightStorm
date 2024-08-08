using UnityEngine;
using System.Collections.Generic;

public class DrawingManager : MonoBehaviour
{
    public static DrawingManager Instance { get; private set; }

    public GameObject brushPrefab;
    public float brushSize = 0.1f;
    public List<Color> lineColors; // List of colors for drawing lines
    public int maxLinePoints = 7; // Maximum number of line points to keep in history
    public float updateInterval = 1f / 30f; // Update coordinates every 1/30 seconds (30 FPS)
    public Vector2[] linePoints;

    private GameObject currentBrush;
    private LineRenderer currentLineRenderer;
    private float timer;
    private int currentColorIndex = 0; // Index of the current line color

    // BoxCollider variables
    private BoxCollider2D lineCollider;
    private GameObject lineColliderObject;

void Start()
{
    if (Instance == null)
    {
        Instance = this;
    }

    timer = updateInterval;
    linePoints = new Vector2[maxLinePoints];

    lineColliderObject = new GameObject("LineCollider");
    lineColliderObject.transform.parent = transform; 
    lineCollider = lineColliderObject.AddComponent<BoxCollider2D>();
    Rigidbody2D rb = lineColliderObject.AddComponent<Rigidbody2D>();
    rb.bodyType = RigidbodyType2D.Kinematic;
    rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
}


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)) {
            SetMaxLinePoints(20);
            updateInterval = 1f / 120f;
        }
        if (Input.GetKeyDown(KeyCode.C)) {
            SetMaxLinePoints(5);
            updateInterval = 1f / 30f;
        }

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Vector2 mousePosition = GetEffectiveMousePosition();

            for (int i = linePoints.Length - 1; i > 0; i--)
            {
                linePoints[i] = linePoints[i - 1];
            }

            linePoints[0] = mousePosition;

            timer = updateInterval;

            DrawLines(lineColors[currentColorIndex]);
            // Update CircleFollower immediately after updating line points
            CircleFollower.Instance?.UpdateFollowerPosition();
        }
    }

    void LateUpdate() {
        UpdateLineCollider();
    }

    private void SetMaxLinePoints(int newMaxLinePoints)
    {
        Vector2[] oldLinePoints = linePoints;
        linePoints = new Vector2[newMaxLinePoints];

        if (newMaxLinePoints > oldLinePoints.Length)
        {
            Vector2 lastPoint = oldLinePoints[oldLinePoints.Length - 1];
            for (int i = 0; i < newMaxLinePoints - oldLinePoints.Length; i++)
            {
                linePoints[i] = lastPoint;
            }
            for (int i = newMaxLinePoints - oldLinePoints.Length; i < newMaxLinePoints; i++)
            {
                linePoints[i] = oldLinePoints[i - (newMaxLinePoints - oldLinePoints.Length)];
            }
        }
        else
        {
            for (int i = 0; i < newMaxLinePoints; i++)
            {
                linePoints[i] = oldLinePoints[oldLinePoints.Length - newMaxLinePoints + i];
            }
        }

        maxLinePoints = newMaxLinePoints;
    }

    private Vector2 GetEffectiveMousePosition()
    {
        if (CircleFollower.Instance != null && CircleFollower.Instance.isLocked)
        {
            return CircleFollower.Instance.transform.position;
        }
        else
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    public Vector2 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void DrawLines(Color lineColor)
    {
        if (currentBrush == null)
        {
            currentBrush = Instantiate(brushPrefab, Vector3.zero, Quaternion.identity);
            currentLineRenderer = currentBrush.AddComponent<LineRenderer>();
            currentLineRenderer.startWidth = brushSize;
            currentLineRenderer.endWidth = brushSize;
            currentLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        }

        currentLineRenderer.startColor = lineColor;
        currentLineRenderer.endColor = lineColor;
        currentLineRenderer.numCornerVertices = 3;
        currentLineRenderer.numCapVertices = 3;

        currentLineRenderer.positionCount = Mathf.Min(maxLinePoints, linePoints.Length);

        for (int i = 0; i < currentLineRenderer.positionCount; i++)
        {
            currentLineRenderer.SetPosition(i, linePoints[i]);
        }

        currentColorIndex = (currentColorIndex + 1) % lineColors.Count;
    }

    private void UpdateLineCollider()
    {
        if (linePoints.Length < 2)
            return;

        Vector3 startPoint = new Vector3(linePoints[0].x, linePoints[0].y, 0);
        Vector3 endPoint = new Vector3(linePoints[1].x, linePoints[1].y, 0);

        Vector3 center = (startPoint + endPoint) / 2f;

        float length = Vector3.Distance(startPoint, endPoint);

        lineCollider.size = new Vector2(length, brushSize); 
        lineCollider.offset = Vector2.zero;

        lineColliderObject.transform.position = center;

        Vector3 direction = endPoint - startPoint;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        lineColliderObject.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // Draw Gizmos to visualize the BoxCollider in the Scene view
    void OnDrawGizmos()
    {
        if (lineCollider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.matrix = lineColliderObject.transform.localToWorldMatrix;
            Gizmos.DrawWireCube(lineCollider.offset, lineCollider.size);
        }
    }
}