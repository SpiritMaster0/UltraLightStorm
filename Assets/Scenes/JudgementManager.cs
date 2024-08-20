using UnityEngine;
using System.Collections;

public class JudgementManager : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; 
    public Sprite badSprite;
    public Sprite okaySprite;
    public Sprite goodSprite;
    public Sprite perfectSprite;
    public Sprite missSprite;
    public float destroyAfter = 1f; 
    public float fadeOutDuration = 0.5f; // Duration for the fade out

    private Coroutine fadeOutCoroutine;

    public void ChangeSprite(string state)
    {
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer is not assigned.");
            return;
        }

        switch (state)
        {
            case "Bad":
                spriteRenderer.sprite = badSprite;
                break;
            case "Okay":
                spriteRenderer.sprite = okaySprite;
                break;
            case "Good":
                spriteRenderer.sprite = goodSprite;
                break;
            case "Perfect":
                spriteRenderer.sprite = perfectSprite;
                break;
            case "Miss":
                spriteRenderer.sprite = missSprite;
                break;
            default:
                Debug.LogWarning("Unknown state: " + state);
                // Optionally assign a default sprite here
                break;
        }
    }

    public void Initialize(Vector2 position, string judgement)
    {
        transform.position = position;
        gameObject.SetActive(true);
        ChangeSprite(judgement);

        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
        }
        fadeOutCoroutine = StartCoroutine(HandleJudgement());
    }

    private IEnumerator HandleJudgement()
    {
        yield return new WaitForSeconds(destroyAfter);

        float elapsedTime = 0f;
        Color startColor = spriteRenderer.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f); // Fully transparent

        while (elapsedTime < fadeOutDuration)
        {
            spriteRenderer.color = Color.Lerp(startColor, endColor, elapsedTime / fadeOutDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        spriteRenderer.color = endColor;
        Destroy(gameObject);
    }
}
