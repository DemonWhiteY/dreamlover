using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitPanel : MonoBehaviour
{
    public void Start()
    {
        // 初始化代码（如果需要的话）
    }

    public void ExitApplication()
    {
        // 开始协程来处理等待和更新情绪的逻辑
        StartCoroutine(UpdateEmotionsWithDelay());
    }

    private IEnumerator UpdateEmotionsWithDelay()
    {

        Message msg=new Message();
        msg.ask = "我要离开了，下次再见";
        msg.command = "leave";
        PythonClient.Instance.AddMessage(msg);

        var data1 = DBController.Instance.DBConnection.Table<Initinfo>().FirstOrDefault();
        Debug.Log(data1.ToString());
        data1.Char = ModelController.Instance.character.id;

        
        data1.time = "22";

        DBController.Instance.DBConnection.Update(data1);
        Debug.Log(data1.ToString());


        // 等待3秒钟
        yield return new WaitForSeconds(4f);

        var data2 = DBController.Instance.DBConnection.Table<Initinfo>().FirstOrDefault();
        Debug.Log(data2.ToString());
        Application.Quit();
#if UNITY_EDITOR
        // 如果在编辑器中运行，可以使用以下代码来模拟退出
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
