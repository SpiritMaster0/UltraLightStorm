using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimatedCircleBackground : MonoBehaviour
{
    public int numberOfCircles = 150;
    public float minRadius = 0.1f;
    public float maxRadius = 0.5f;
    public float minSpeed = 0.1f;
    public float maxSpeed = 0.5f;
    public float peakSpeedDuration = 2f;
    public float initialBoost = 1f;
    public AnimationCurve speedCurve;
    public Color[] colors;
    public List<Color> glowColors = new List<Color>(); // List of glow colors
    public float spiralSpeed = 1f;
    public float thrustAmount = 2f;
    public float minSpinSpeed = 0.5f;
    public float maxSpinSpeed = 2.0f;

    private GameObject[] circles;
    private Color[] originalColors;
    private float cameraWidth;
    private float cameraHeight;
    private float[] initialSpeeds;
    private float[] normalSpeeds;
    private Vector3[] startPositions;
    private Vector3[] targetPositions;
    private float elapsedTime = 0f;
    private bool speedUpPhase = true;
    private bool patternActive = false;
    private bool glowActive = false;
    private int currentGlowIndex = 0;
    private float glowDuration = 0f;
    private System.Action currentPattern;

    void Start()
    {
        cameraHeight = Camera.main.orthographicSize;
        cameraWidth = cameraHeight * Camera.main.aspect;

        initialSpeeds = new float[numberOfCircles];
        normalSpeeds = new float[numberOfCircles];
        startPositions = new Vector3[numberOfCircles];
        targetPositions = new Vector3[numberOfCircles];
        circles = new GameObject[numberOfCircles];
        originalColors = new Color[numberOfCircles];

        for (int i = 0; i < numberOfCircles; i++)
        {
            initialSpeeds[i] = Random.Range(minSpeed, maxSpeed);
            normalSpeeds[i] = Random.Range(minSpeed, maxSpeed);
            circles[i] = CreateCircle();
            originalColors[i] = circles[i].GetComponent<SpriteRenderer>().color; // Store the original color
            startPositions[i] = new Vector3(Random.Range(-cameraWidth, cameraWidth), -cameraHeight, 0);
            targetPositions[i] = new Vector3(circles[i].transform.position.x, Random.Range(-cameraHeight, cameraHeight), 0);

            circles[i].transform.position = startPositions[i];
        }
    }

    void Update()
    {
        if (speedUpPhase)
        {
            // Speed up phase
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / peakSpeedDuration);

            for (int i = 0; i < numberOfCircles; i++)
            {
                GameObject circle = circles[i];
                float speedMultiplier = speedCurve.Evaluate(progress);
                float speed = Mathf.Lerp(initialSpeeds[i] + initialBoost, normalSpeeds[i], speedMultiplier);
                circle.transform.position += Vector3.up * speed * Time.deltaTime;

                float yOffset = Mathf.Lerp(startPositions[i].y, targetPositions[i].y, speedMultiplier);
                circle.transform.position = new Vector3(circle.transform.position.x, yOffset, 0);
            }

            if (progress >= 1f)
            {
                speedUpPhase = false;
            }
        }
        else if (patternActive)
        {
            // Apply current pattern
            currentPattern?.Invoke();
        }
        else
        {
            // Normal phase
            for (int i = 0; i < numberOfCircles; i++)
            {
                GameObject circle = circles[i];
                circle.transform.position += Vector3.up * normalSpeeds[i] * Time.deltaTime;

                if (circle.transform.position.y > cameraHeight)
                {
                    circle.transform.position = new Vector3(circle.transform.position.x, -cameraHeight, 0);
                }
            }
        }

        // Handle glowing effect
        if (glowActive)
        {
            glowDuration -= Time.deltaTime;
            if (glowDuration <= 0f)
            {
                NextGlow();
            }
        }

        // Check for input to trigger glow
        if (Input.GetKeyDown(KeyCode.I))
        {
            TriggerGlow(2.0f); // Call with a desired duration
        }
    }

    private void NextGlow()
    {
        currentGlowIndex = (currentGlowIndex + 1) % glowColors.Count;
        Color glowColor = glowColors[currentGlowIndex];
        
        for (int i = 0; i < numberOfCircles; i++)
        {
            circles[i].GetComponent<SpriteRenderer>().color = glowColor;
        }

        glowDuration = 0.5f; // Duration between glow color changes
    }

    public void TriggerGlow(float duration)
    {
        glowDuration = duration;
        glowActive = true;
        currentGlowIndex = -1; // Start with the first glow color
        NextGlow(); // Initialize with the first glow color
    }

    private void ThrustPattern()
    {
        for (int i = 0; i < numberOfCircles; i++)
        {
            GameObject circle = circles[i];
            float thrust = Mathf.Sin(Time.time * minSpinSpeed) * thrustAmount;
            circle.transform.position += Vector3.right * thrust * Time.deltaTime;
        }
    }

    public void TriggerThrustPattern()
    {
        currentPattern = ThrustPattern;
        patternActive = true;
    }

    public void StopPattern()
    {
        patternActive = false;
        currentPattern = null;
        glowActive = false; // Turn off glowing
        RestoreColors(); // Restore original colors if glow was active
    }

    private void RestoreColors()
    {
        for (int i = 0; i < numberOfCircles; i++)
        {
            circles[i].GetComponent<SpriteRenderer>().color = originalColors[i];
        }
    }

    private GameObject CreateCircle()
    {
        GameObject circle = new GameObject("Circle");
        circle.transform.parent = transform;
        float radius = Random.Range(minRadius, maxRadius);
        circle.transform.localScale = new Vector3(radius, radius, 1);

        SpriteRenderer spriteRenderer = circle.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = CreateCircleSprite(radius);
        spriteRenderer.color = colors[Random.Range(0, colors.Length)];
        circle.transform.position = new Vector3(Random.Range(-cameraWidth, cameraWidth), Random.Range(-cameraHeight, cameraHeight), 0);

        return circle;
    }

    private Sprite CreateCircleSprite(float radius)
    {
        int textureSize = 64;
        Texture2D texture = new Texture2D(textureSize, textureSize);
        Color[] pixels = new Color[textureSize * textureSize];

        int center = textureSize / 2;
        float radiusSquared = radius * radius;

        for (int y = 0; y < textureSize; y++)
        {
            for (int x = 0; x < textureSize; x++)
            {
                float xCoord = (x - center) / (float)center;
                float yCoord = (y - center) / (float)center;
                if (xCoord * xCoord + yCoord * yCoord <= radiusSquared)
                {
                    pixels[y * textureSize + x] = Color.white;
                }
                else
                {
                    pixels[y * textureSize + x] = Color.clear;
                }
            }
        }

        texture.SetPixels(pixels);
        texture.Apply();
        return Sprite.Create(texture, new Rect(0, 0, textureSize, textureSize), new Vector2(0.5f, 0.5f));
    }
}
