using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChosePanel : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] model;
    public Button buttonLeft;
    public Button buttonRight;
    public Button buttonsubmit;
    public Text Charname;
    public Text Description;
    private int index=1;

    private void Start()
    {
        buttonLeft.onClick.AddListener(changeLeft);
        buttonRight.onClick.AddListener(changeRight);
        buttonsubmit.onClick.AddListener(submit);
        model[index].SetActive(true);
        Charname.text = Init.Instance.CharacterInfo(index).name;
        Description.text = Init.Instance.CharacterInfo(index).Description;
        ;    }


    

    private void changeLeft()
    {
        if (index > 0)
        { 
        model[index].SetActive(false);
        model[index - 1].SetActive(true);
        index--;
    }
        else if(index==0)
        { model[index].SetActive(false);
            model[model.Length-1].SetActive(true);
            index = model.Length-1;
        }

        Charname.text = Init.Instance.CharacterInfo(index).name;
        Description.text = Init.Instance.CharacterInfo(index).Description;
    }

    private void changeRight()
    {
        if (index < model.Length - 1)
        {
            model[index].SetActive(false);
            model[index + 1].SetActive(true);
            index++;
        }
        else if(index == model.Length - 1)
        { model[index].SetActive(false);
            model[0].SetActive(true);
        index = 0;}

        Charname.text = Init.Instance.CharacterInfo(index).name;
        Description.text = Init.Instance.CharacterInfo(index).Description;
    }

    private void submit()
    {
        ModelController.Instance.UpdateModel(index);
        var data1 = DBController.Instance.DBConnection.Table<Initinfo>().FirstOrDefault();
        data1.Char = ModelController.Instance.character.id;


        data1.time = "22";

        DBController.Instance.DBConnection.Update(data1);
        Debug.Log(data1.ToString());
        Message msg=new Message();
        msg.ask = ModelController.Instance.character.name;
        msg.command = "update";
        PythonClient.Instance.AddMessage(msg);

        Message msg2=new Message();
        msg2.ask = "你好哦，又见面了";
            msg2.command = "meet";
        PythonClient.Instance.AddMessage(msg2);
    }
}
