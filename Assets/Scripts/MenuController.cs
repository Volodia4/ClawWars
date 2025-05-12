using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

public class MenuController : MonoBehaviour
{
    [Header("Посилання на селектор")]
    public Selector selector;

    [Header("Панель налаштувань")]
    public GameObject settingsPanel;

    [Header("Панель для введення нової клавіші дії")]
    public GameObject rebindPanel;

    [Header("Кнопки переприв’язки")]
    public Button[] rebindButtons;

    [Header("Нікнейми гравців")]
    public TMP_InputField nicknameP1Input;
    public TMP_InputField nicknameP2Input;

    private AudioManager am;
    private KeyCode[] keys;
    private bool rebinding, isExitingGame;

    private string ConfigPath => Path.Combine(Application.persistentDataPath, "keybindings.json");

    [Serializable]
    private class KeyConfig
    {
        public string[] keys;
    }

    private void Awake()
    {
        am = AudioManager.Instance;

        Time.timeScale = 1f;
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("NicknameP1"))
            nicknameP1Input.text = PlayerPrefs.GetString("NicknameP1");
        if (PlayerPrefs.HasKey("NicknameP2"))
            nicknameP2Input.text = PlayerPrefs.GetString("NicknameP2");

        try
        {
            var json = File.ReadAllText(ConfigPath);
            var data = JsonUtility.FromJson<KeyConfig>(json);
            keys = new KeyCode[data.keys.Length];
            for (int i = 0; i < data.keys.Length; i++)
                keys[i] = (KeyCode)Enum.Parse(typeof(KeyCode), data.keys[i]);
        }
        catch
        {
            Debug.LogWarning("Не вдалось розпарсити");
            InitDefaultKeys();
        }

        UpdateAllLabels();
    }

    public void SaveNickname()
    {
        PlayerPrefs.SetString("NicknameP1", nicknameP1Input.text);
        PlayerPrefs.SetString("NicknameP2", nicknameP2Input.text);
        PlayerPrefs.Save();
    }

    private void InitDefaultKeys()
    {
        keys = new KeyCode[]
        {
            KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D,
            KeyCode.F, KeyCode.G, KeyCode.H,
            KeyCode.Keypad8, KeyCode.Keypad2, KeyCode.Keypad4, KeyCode.Keypad6,
            KeyCode.Keypad7, KeyCode.Keypad9, KeyCode.Keypad5
        };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Settings();
    }

    public void StartRebind(int index)
    {
        am.PlaySFX("KeyChange");

        if (rebinding) return;
        StartCoroutine(WaitForKeyAndBind(index));
    }

    private IEnumerator WaitForKeyAndBind(int index)
    {
        rebinding = true;
        rebindPanel.SetActive(true);

        KeyCode newKey = KeyCode.None;
        while (newKey == KeyCode.None)
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode kc in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(kc))
                    {
                        newKey = kc;
                        break;
                    }
                }
            }
            yield return null;
        }

        keys[index] = newKey;
        UpdateAllLabels();

        SaveToJson();

        rebindPanel.SetActive(false);
        rebinding = false;
    }

    private void UpdateAllLabels()
    {
        for (int i = 0; i < rebindButtons.Length; i++)
        {
            var label = rebindButtons[i].transform.Find("TextKey").GetComponent<TextMeshProUGUI>();
            label.text = keys[i].ToString();
        }
    }

    private void SaveToJson()
    {
        var data = new KeyConfig();
        data.keys = new string[keys.Length];
        for (int i = 0; i < keys.Length; i++) data.keys[i] = keys[i].ToString();

        try
        {
            var json = JsonUtility.ToJson(data, true);
            File.WriteAllText(ConfigPath, json);
        }
        catch (Exception e)
        {
            Debug.LogError("Не вдалось зберегти в keybindings.json: " + e.Message);
        }
    }

    public void DefaultSettings()
    {
        am.PlaySFX("Click");

        InitDefaultKeys();
        SaveToJson();
        UpdateAllLabels();
    }

    public void Settings()
    {
        am.PlaySFX("Click");

        if (settingsPanel.activeSelf) settingsPanel.SetActive(false);
        else settingsPanel.SetActive(true);
    }

    public void PlayGame()
    {
        GameManager.Instance.selectedCharacter1 = selector.charIndex1;
        GameManager.Instance.selectedCharacter2 = selector.charIndex2;
        GameManager.Instance.selectedArena = selector.arenaIndex;
        GameManager.Instance.keys = keys;
        SaveNickname();
        SceneManager.LoadScene("BattleScene");
    }

    public void QuitGame()
    {
        if (isExitingGame) return;
        am.PlaySFX("ExitGame");

        StartCoroutine(QuitAfterSound());
    }

    private IEnumerator QuitAfterSound()
    {
        isExitingGame = true;
        yield return new WaitForSeconds(1.2f);

        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
