using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthDefenseUpdater : MonoBehaviour
{
    public static HealthDefenseUpdater Instance;

    [Header("Здібності персонажа")]
    public CharacterAbilities warriorAbilities;
    public CharacterAbilities magicianAbilities;

    [Header("Хелсбари персонажів")]
    public Image healthBarP1;
    public Image healthBarP2;

    [Header("Лейбли для виведення хп і захисту")]
    public TextMeshProUGUI HealthP1;
    public TextMeshProUGUI HealthP1Total;
    public TextMeshProUGUI HealthP2;
    public TextMeshProUGUI HealthP2Total;
    public TextMeshProUGUI DefenseP1;
    public TextMeshProUGUI DefenseP2;

    private GameManager gm = GameManager.Instance;
    private int warriorHealth, magicianHealth, initialHP_P1, initialHP_P2, currentHP_P1, currentHP_P2;
    private float warriorDefense, magicianDefense, warriorCritChanse, magicianCritChanse;
    private float initialDef_P1, initialDef_P2, currentDef_P1, currentDef_P2;

    void Awake()
    {
        Instance = this;
    }

    public void ChangerHP1(double damageForP1)
    {
        int _damageForP1 = (int)Math.Round(damageForP1);
        currentHP_P1 -= Mathf.RoundToInt((float)_damageForP1 - _damageForP1 * currentDef_P1);
    }

    public void ChangerHP2(double damageForP2)
    {
        int _damageForP2 = (int)Math.Round(damageForP2);
        currentHP_P2 -= Mathf.RoundToInt((float)_damageForP2 - _damageForP2 * currentDef_P2);
    }

    public void ChangerDef1(float CurrentDef_P1)
    {
        currentDef_P1 = CurrentDef_P1;
    }

    public void ChangerDef2(float CurrentDef_P2)
    {
        currentDef_P2 = CurrentDef_P2;
    }

    void Start()
    {
        healthBarP1.fillAmount = 1;

        if (warriorAbilities != null && magicianAbilities != null)
        {
            warriorHealth = warriorAbilities.maxHP;
            warriorDefense = warriorAbilities.defense;
            warriorCritChanse = warriorAbilities.critChanse;
            magicianHealth = magicianAbilities.maxHP;
            magicianDefense = magicianAbilities.defense;
            magicianCritChanse = magicianAbilities.critChanse;
        }

        if (gm.selectedCharacter1 == 0)
        {
            HealthP1.text = warriorHealth.ToString();
            HealthP1Total.text = warriorHealth.ToString();
            DefenseP1.text = warriorDefense.ToString();
            currentHP_P1 = warriorHealth;
            currentDef_P1 = warriorDefense;
            initialHP_P1 = warriorHealth;
            initialDef_P1 = warriorDefense;
        }
        else
        {
            HealthP1.text = magicianHealth.ToString();
            HealthP1Total.text = magicianHealth.ToString();
            DefenseP1.text = magicianDefense.ToString();
            currentHP_P1 = magicianHealth;
            currentDef_P1 = magicianDefense;
            initialHP_P1 = magicianHealth;
            initialDef_P1 = magicianDefense;
        }

        if (gm.selectedCharacter2 == 0)
        {
            HealthP2.text = warriorHealth.ToString();
            HealthP2Total.text = warriorHealth.ToString();
            DefenseP2.text = warriorDefense.ToString();
            currentHP_P2 = warriorHealth;
            currentDef_P2 = warriorDefense;
            initialHP_P2 = warriorHealth;
            initialDef_P2 = warriorDefense;
        }
        else
        {
            HealthP2.text = magicianHealth.ToString();
            HealthP2Total.text = magicianHealth.ToString();
            DefenseP2.text = magicianDefense.ToString();
            currentHP_P2 = magicianHealth;
            currentDef_P2 = magicianDefense;
            initialHP_P2 = magicianHealth;
            initialDef_P2 = magicianDefense;
        }
    }

    private void Update()
    {
        if (currentHP_P1 > 0) HealthP1.text = currentHP_P1.ToString();
        else HealthP1.text = "0";
        if (currentHP_P2 > 0) HealthP2.text = currentHP_P2.ToString();
        else HealthP2.text = "0";

        healthBarP1.fillAmount = (float)currentHP_P1 / (float)initialHP_P1;
        healthBarP2.fillAmount = (float)currentHP_P2 / (float)initialHP_P2;

        DefenseP1.text = (currentDef_P1 * 100).ToString();
        DefenseP2.text = (currentDef_P2 * 100).ToString();
    }
}
