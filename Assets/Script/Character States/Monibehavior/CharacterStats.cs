using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CharacterStats : MonoBehaviour
{
    public event Action<int,int> UpdateHealthBarOnattack;

    public CharacterData_SO templateData;

    public CharacterData_SO characterData;

    public AttackData_SO attackData;

    [HideInInspector]
    public bool isCritical;

    void Awake()
    {
        if (templateData != null)
            characterData = Instantiate(templateData); 
    }

    public int MaxHealth 
    {
        get
        {
            if (characterData != null)
                return characterData.maxHealth;
            else return 0;
        }
        set 
        {
            characterData.maxHealth = value;
        }
    }

    public int CurrentHealth
    {
        get
        {
            if (characterData != null)
                return characterData.currentHealth;
            else return 0;
        }
        set
        {
            characterData.currentHealth = value;
        }
    }

    public int BaseDefence
    {
        get
        {
            if (characterData != null)
                return characterData.baseDefence;
            else return 0;
        }
        set
        {
            characterData.baseDefence = value;
        }
    }

    public int CurrentDefence
    {
        get
        {
            if (characterData != null)
                return characterData.currentDefence;
            else return 0;
        }
        set
        {
            characterData.currentDefence = value;
        }
    }

    #region Character Combat

    public void TakeDamage(CharacterStats attacker,CharacterStats defener)
    {
        int damage =Mathf.Max(0,attacker.CurrentDamage()-defener.CurrentDefence);
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);

        if (isCritical)
        {
            defener.GetComponent<Animator>().SetTrigger("Hit");
        }
        //TODO:UPdate UI
        UpdateHealthBarOnattack?.Invoke(CurrentHealth,MaxHealth);

        //TODO:经验Update
        if (CurrentHealth <= 0)
            attacker.characterData.UpdateExp(characterData.kiilPoint);
    }

    //函数的重载
    public void TakeDamage(int damage, CharacterStats defener)
    {
        int currentDamage = Mathf.Max(0,damage-defener.CurrentDefence);
        CurrentHealth = Mathf.Max(CurrentHealth - currentDamage, 0);
        UpdateHealthBarOnattack?.Invoke(CurrentHealth, MaxHealth);

        GameManager.Instance.playStats.characterData.UpdateExp(characterData.kiilPoint);
    }

    private int CurrentDamage()
    {
        float coreDamage = UnityEngine.Random.Range(attackData.minDamage,attackData.maxDamage);

        if (isCritical)
        {
            coreDamage *= attackData.criticalMutiplier;
            //Debug.Log("暴击");
        }

        return (int)coreDamage;
    }

    #endregion

}
