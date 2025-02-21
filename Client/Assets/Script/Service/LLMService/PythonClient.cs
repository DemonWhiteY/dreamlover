using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class PythonClient : MonoBehaviour
{
    // 单例实例
    private static PythonClient instance;
    public static PythonClient Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PythonClient>();
                if (instance == null)
                {
                    GameObject singleton = new GameObject(typeof(PythonClient).Name);
                    instance = singleton.AddComponent<PythonClient>();
                }
            }
            return instance;
        }
    }

    public string ipAddress = "127.0.0.1"; // Python接收端的IP地址
    public int port = 12345; // Python接收端的端口号
    

    private Queue<Message> messageQueue = new Queue<Message>();

    void Awake()
    {
        // 确保单例实例的唯一性
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 保证切换场景时不销毁该对象
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        // 在销毁时清空消息队列
        messageQueue.Clear();
    }

    public void AddMessage(Message message)
    {
        messageQueue.Enqueue(message);
    }

    void Update()
    {
        // 每帧发送消息队列中的消息
        while (messageQueue.Count > 0)
        {
            Message messageToSend = messageQueue.Dequeue();
            SendMessage(messageToSend);
        }
    }

    void SendMessage(Message message)
    {
        try
        {
            using (Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                IPAddress ip = IPAddress.Parse(ipAddress);
                IPEndPoint remoteEP = new IPEndPoint(ip, port);
                clientSocket.Connect(remoteEP);
                string jsonMessage = JsonConvert.SerializeObject(message);
                byte[] byteData = Encoding.UTF8.GetBytes(jsonMessage);
                clientSocket.Send(byteData);
                Debug.Log($"Sent message: {message.command}ask:{message.ask}");

                StringBuilder responseBuilder = new StringBuilder();
                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = clientSocket.Receive(buffer)) > 0)
                {
                    string part = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    responseBuilder.Append(part);

                    if (part.EndsWith("\0"))
                    {
                        break;
                    }
                }

                string receivedMessage = responseBuilder.ToString().TrimEnd('\0');
                Debug.Log($"Received message: {receivedMessage}");

                MessageData receivemessage = JsonConvert.DeserializeObject<MessageData>(receivedMessage);
               Debug.Log($"Received data: {receivemessage.response}");

                GameObject result_object;
                Text result_text;
                
                     result_object = GameObject.Find("result");
                    result_text = result_object.GetComponent<Text>();
                

                

                switch (message.command)
                {
                    case "login":
                        if (receivemessage.emotion == 3) SceneManager.LoadScene("Loading");
                        else if (receivemessage.emotion == 2) result_text.text = "密码错误";
                        else if (receivemessage.emotion == 1) result_text.text = "不存在的账号";

                        break;
                    case "leave":
                        result_text.text = receivemessage.response;
                        EmotionController.Instance.updateEmotion(12);
                        TtsService.Instance.Tts(receivemessage.response,ModelController.Instance.character.voice);
                        break;
                    case "talk":
                        result_text.text = receivemessage.response;
                        EmotionController.Instance.updateEmotion(receivemessage.emotion);
                        var p = new HistoryMessage()
                        {
                            Charname = ModelController.Instance.character.name,
                            Q_word = message.ask,
                            A_word = receivemessage.response,
                            time = "11.11",
                        };

                        DBController.Instance.DBConnection.Insert(p);

                        TtsService.Instance.Tts(receivemessage.response, ModelController.Instance.character.voice);
                      
                        break;
                    case "meet":
                        result_text.text = receivemessage.response;
                        EmotionController.Instance.updateEmotion(11);
                        TtsService.Instance.Tts(receivemessage.response, ModelController.Instance.character.voice);
                        break;
                    default:
                        break;
                }

                clientSocket.Close();
            }
        }
        catch (SocketException e)
        {
            GameObject result_object = GameObject.Find("result");
            Text result_text = result_object.GetComponent<Text>();
            //Debug.LogError($"SocketException: {e}");
            result_text.text = ModelController.Instance.character.name+"现在没信号呢，到网络好些的地方试试吧";
            EmotionController.Instance.updateEmotion(13);
        }
        catch (System.Exception ex)
        {
            Debug.Log($"Exception: {ex}");
            GameObject result_object = GameObject.Find("result");
            Text result_text = result_object.GetComponent<Text>();
            result_text.text = ModelController.Instance.character.name + "开小差去了，等会儿再试试吧吧";
            EmotionController.Instance.updateEmotion(14);
           
        }
    }
}

    [System.Serializable]
public class MessageData
{
    public string command;
    public string response;
    public int emotion;
}








public class Message
    {
    public string command;
    public string ask{ get; set; }
}




