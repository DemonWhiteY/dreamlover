using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SQLite4Unity3d;
using System.Linq;

public class talkPanel : MonoBehaviour
{

    public GameObject talkboxLeft;
    public GameObject talkboxRight;
    public GameObject parent;
    public int Maxword=15;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void print()
    {
       Text  textLeft=talkboxLeft.GetComponentInChildren<Text>();
        Text textRight=talkboxRight.GetComponentInChildren<Text>();
        var datas = DBController.Instance.DBConnection.Table<HistoryMessage>().Where(_ => _.Charname ==ModelController.Instance.character.name); ;
        foreach(var v in datas)
        {

            textRight.text = GetNstring(v.Q_word);
            Instantiate(talkboxRight, parent.transform);
            textLeft.text = GetNstring( v.A_word);
                Instantiate(talkboxLeft,parent.transform);
            
               
            
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(parent.GetComponent<RectTransform>());
    }

    public  string GetNstring(string str)
    {
        string result = "";
        int point = 0;
        for (int i = 0; i < str.Length; i++)
        {
            if (point == Maxword)
            {
                result += '\n';
                point = 0;
            }
            result += str[i];
            point++;
        }
        return result;
    }

}
