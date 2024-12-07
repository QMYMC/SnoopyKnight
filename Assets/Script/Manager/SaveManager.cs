using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SavePlayerData();
            //Debug.Log("save");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadPlayerData();

            //Debug.Log("Load");
        }
    }

    public void SavePlayerData()
    {
        Save(GameManager.Instance.playStats.characterData, GameManager.Instance.playStats.characterData.name);
    }

    public void LoadPlayerData()
    {
        Load(GameManager.Instance.playStats.characterData, GameManager.Instance.playStats.characterData.name);
    }

    public void Save(Object data,string key)
    {
        var jsonData = JsonUtility.ToJson(data,true);
        PlayerPrefs.SetString(key, jsonData);

        PlayerPrefs.Save();
    }

    public void Load(Object data, string key) 
    {
        if (PlayerPrefs.HasKey(key))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
        }
    }
}
