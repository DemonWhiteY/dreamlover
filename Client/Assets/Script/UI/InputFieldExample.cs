using UnityEngine;
using UnityEngine.UI;

public class InputFieldExample : MonoBehaviour
{
    // 引用InputField组件
    public InputField inputField;

    void Start()
    {
        // 订阅InputField的OnEndEdit事件，当用户结束输入时触发
        inputField.onEndEdit.AddListener(OnInputEndEdit);
    }

    // 当用户结束输入时调用的方法
    void OnInputEndEdit(string inputText)
    {
        // 输出输入的文本到控制台
        Debug.Log("输入的文本为：" + inputText);
        Message msg = new Message();
        msg.ask=inputText;
        msg.command = "talk";
        PythonClient.Instance.AddMessage(msg);

        // 在这里可以将输入的文本用于其他操作，比如保存到变量或处理逻辑
        // 例如：
        // GameManager.Instance.HandleInput(inputText);
    }


    

    
}
