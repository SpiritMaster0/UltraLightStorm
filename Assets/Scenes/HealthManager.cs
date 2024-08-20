using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance { get; private set; }
    public Slider healthSlider; 
    public SpriteRenderer revealImage; 
    public GameObject targetCircleManager; 

    private float maxHealth = 100f;
    private float currentHealth;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        revealImage.color = new Color(revealImage.color.r, revealImage.color.g, revealImage.color.b, 0f); // Start fully transparent
    }

    public void UpdateHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0f, maxHealth);
        healthSlider.value = currentHealth;

        if (currentHealth <= 0)
        {
            HandleHealthDepletion();
        }
        else if (currentHealth <= maxHealth * 0.5f)
        {
            float transparency = Mathf.InverseLerp(0, maxHealth * 0.5f, currentHealth) * 0.25f;
            revealImage.color = new Color(revealImage.color.r, revealImage.color.g, revealImage.color.b, transparency);
        }
    }

    private void HandleHealthDepletion()
    {
        TargetCircleManager.Instance.StopSpawningCircles();
    }
}
