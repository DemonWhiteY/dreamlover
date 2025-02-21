using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelController : MonoBehaviour
{
    public static ModelController Instance { get; set; }

    public GameObject[] Models;

    public Character character;
    public  int index;

    // 在对象初始化时调用
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        
        character=new Character();
        Models[index].SetActive(true);
        character = Init.Instance.CharacterInfo(index);
       
    }

    public void UpdateModel(int newindex)
    {
        if (newindex >= 0 && newindex < Models.Length)
        {
            Models[index].SetActive(false);
            Models[newindex].SetActive(true);
            index = newindex;
            character = Init.Instance.CharacterInfo(newindex);
           
                     
        }
        else
        {
            Debug.LogError("Invalid index: " + newindex);
        }
    }
}
