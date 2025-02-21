using System.Collections;
using System.Collections.Generic;


using UnityEngine;
using UnityEngine.UI;
using ColorUtility = UnityEngine.ColorUtility;

public class SettingPanel : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider[] slider;
    public UIPolygon polygon;
    public GameObject EmotionShow;
    public Slider EmotPanel;
    public Image Fill;
    public Text EmoTest;
    public Slider AgePanel;
    public Image AgeFill;
    public Text AgeText;
    public Text Charname;
    public InputField Description;
    Color myred;
    Color myblue;
    Color myOrigen;
    public Button SaveButton;

    public GameObject[] models;

    private void Start()
    {
        
        ColorUtility.TryParseHtmlString("#E03232D9", out myred);
        ColorUtility.TryParseHtmlString("#3275E0D9", out myblue);
        ColorUtility.TryParseHtmlString("#E08532D9", out myOrigen);
        for (int i = 0; i < slider.Length; i++)
        {
            int index = i; // 创建局部变量以捕获当前索引
            slider[i].onValueChanged.AddListener(value => OnSliderValueChanged(index, value));
        }
        
        EmotPanel.onValueChanged.AddListener(OnSliderValueChanged);
        AgePanel.onValueChanged.AddListener(OnAgeValueChanged);
        SaveButton.onClick.AddListener(save);
        updateSetting();
        
        
    }

    public void updateSetting()
    {
        Debug.Log(ModelController.Instance.character.ToString());
        foreach (var model in models) model.SetActive(false);
        models[ModelController.Instance.character.modelnum-1].SetActive(true);
        Charname.text = ModelController.Instance.character.name;
        Description.text = ModelController.Instance.character.Description;
        AgePanel.value = (float)(ModelController.Instance.character.age-5)/60;
        Debug.Log(AgePanel.value);
        EmotPanel.value = ModelController.Instance.character.disposition;
       
        LayoutRebuilder.ForceRebuildLayoutImmediate(EmotionShow.GetComponent<RectTransform>());
    }

    private void OnSliderValueChanged(int index, float value)
    {
        polygon.updateDistance(index, value);
        polygon.updateDistance(index+4, 1-value);
    }

    private void OnSliderValueChanged(float value)
    {
        if(value>0&&value<0.33)
        {
            Fill.color = myblue;
            EmoTest.text = "清冷";
        }
        else if(value>=0.33&&value<0.67)
        {
            Fill.color=myOrigen;
            EmoTest.text = "温柔";
        }

        else if(value>=0.67&&value<1)
        {
            Fill.color=myred;
            EmoTest.text = "易怒";
        }
    }

    private void OnAgeValueChanged(float value)
    {
        int valuestring = (int)(value*60+5);
        string result=valuestring.ToString()+"岁";
        AgeText.text=result;
    }

    private void save()
    {
        var data = DBController.Instance.DBConnection.Table<Character>().Where(_ => _.name == ModelController.Instance.character.name).FirstOrDefault();
        //更改 Weight 
        data.disposition =EmotPanel.value;
        data.age = (int)(AgePanel.value * 60 + 5);
        data.Description = Description.text;
        

       
        DBController.Instance.DBConnection.Update(data);
        Init.Instance.InitCharacter();
        //更新数据


    }
}
