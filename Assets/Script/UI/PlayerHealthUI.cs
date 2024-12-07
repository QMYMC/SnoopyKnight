using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    Text levelText;
    Image healthSlider;
    Image expSlider;

    private void Awake()
    {
        levelText = transform.GetChild(2).GetComponent<Text>();
        healthSlider = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        expSlider = transform.GetChild(1).GetChild(0).GetComponent<Image>();
    }

    void Update()
    {
        levelText.text = "Level" + GameManager.Instance.playStats.characterData.currentLevel.ToString("00");
        UpdateHealth();
        UpdateExp();

    }

    void UpdateHealth()
    {
        float sliderPercent = (float)GameManager.Instance.playStats.CurrentHealth / GameManager.Instance.playStats.MaxHealth;
        healthSlider.fillAmount = sliderPercent;
    }

    void UpdateExp()
    { 
        float sliderPercent = (float)GameManager.Instance.playStats.characterData.currentExp / GameManager.Instance.playStats.characterData.baseExp;
        expSlider.fillAmount = sliderPercent;
    }
}
