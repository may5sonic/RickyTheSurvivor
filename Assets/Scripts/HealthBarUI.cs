using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Slider slider;
    public TMP_Text label;

    void Awake()
    {
        if (slider != null)
        {
            slider.minValue = 0f;
            slider.maxValue = 1f;
        }
    }

    void Update()
    {
        if (playerHealth == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                playerHealth = playerObject.GetComponentInChildren<PlayerHealth>();
            }
        }

        if (playerHealth == null) return;

        float value = playerHealth.HealthPercent;
        if (slider != null) slider.value = value;
        if (label != null) label.text = $"{playerHealth.CurrentHealth}/{playerHealth.maxHealth}";
    }
}
