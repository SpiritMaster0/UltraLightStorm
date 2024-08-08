using UnityEngine;

public class CircleFollower : MonoBehaviour
{
    public static CircleFollower Instance { get; private set; }

    public float updateInterval = 1f / 30f; // Update coordinates every 1/30 seconds (30 FPS)
    private float timer;

    public bool isLocked = false; // To manage the lock state
    public Vector2 lockedMousePosition; // To store the position of the mouse when locked

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        timer = updateInterval;
    }

    void Update()
    {
        if (!isLocked)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                UpdateFollowerPosition();
                timer = updateInterval;
            }
        }
        else
        {
            // When locked, the CircleFollower doesn't update its position based on mouse
        }
    }

    public void UpdateFollowerPosition()
    {
        Vector2[] linePoints = DrawingManager.Instance.linePoints;

        if (linePoints != null && linePoints.Length > 1)
        {
            transform.position = linePoints[1];
            Vector2 directionToFirstPoint = linePoints[0] - (Vector2)transform.position;
            transform.right = directionToFirstPoint;
            transform.position = linePoints[0];
        }
    }

    public void LockFollower()
    {
        isLocked = true;
        lockedMousePosition = DrawingManager.Instance.GetMousePosition();
    }

    public void UnlockFollower()
    {
        isLocked = false;
    }
}
