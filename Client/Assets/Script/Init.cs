using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
using System;

public class Init : MonoBehaviour
{
    private static Init _instance;
    public static Init Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Init>();

                if (_instance == null)
                {
                    GameObject singleton = new GameObject(typeof(Init).ToString());
                    _instance = singleton.AddComponent<Init>();
                    DontDestroyOnLoad(singleton);
                }
            }
            return _instance;
        }
    }

    public Character[] characters;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        InitCharacter();
        
       ModelController.Instance.index = DBController.Instance.DBConnection.Table<Initinfo>().FirstOrDefault().Char - 1;

        ModelController.Instance.UpdateModel(ModelController.Instance.index);
        Message msg=new Message();
        msg.ask = "你好，又见面啦";
        msg.command = "meet";
        PythonClient.Instance.AddMessage(msg);
    }

    public void InitCharacter()
    {
        int index = 0;
        var Tcharacters = DBController.Instance.DBConnection.Table<Character>();
        int characterCount = Tcharacters.Count();

        // 初始化characters数组
        characters = new Character[characterCount];
        foreach (var character in Tcharacters)
        {
            Debug.Log(character.ToString());
            characters[index] = character;
            index++;
        }
    }

    public Character CharacterInfo(int index)
    {
        return characters[index];
    }
}
