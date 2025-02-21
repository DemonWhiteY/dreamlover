using System.Collections.Generic;
using UnityEngine;
using Baidu.Aip.Speech;
using System.IO;
using System;
using UnityEngine.Networking;
using System.Collections;

public class TtsService : MonoBehaviour
{
    private static TtsService _instance;
    private static readonly object _lock = new object();
    private Tts client;
    private string APP_ID = "95538278";
    private string API_KEY = "wC75cFLaHtrSpuUNOxabWjnc";
    private string SECRET_KEY = "aLH2e3SldpCwiGjErRLUvDNuzynKFQde";
    public AudioSource audioSource;

    // 私有构造函数，防止外部实例化
    private TtsService()
    {
        client = new Baidu.Aip.Speech.Tts(API_KEY, SECRET_KEY);
        client.Timeout = 60000;  // 修改超时时间
    }

    // 公有静态方法，用于获取单例实例
    public static TtsService Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    GameObject obj = new GameObject("TtsService");
                    _instance = obj.AddComponent<TtsService>();
                    DontDestroyOnLoad(obj);
                    _instance.Initialize();
                }
                return _instance;
            }
        }
    }

    private void Initialize()
    {
        Camera c = Camera.main;
        if (c != null)
        {
            audioSource = c.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("AudioSource component not found on Camera.");
            }
        }
        else
        {
            Debug.LogError("Main Camera not found.");
        }
    }

    public void Tts(string input,int voice)
    {
        // 可选参数
        var option = new Dictionary<string, object>()
        {
            {"spd", 5}, // 语速
            {"vol", 7}, // 音量
            {"per", voice}  // 发音人，4：情感度丫丫童声
        };
        var result = client.Synthesis(input, option);

        if (result.ErrorCode == 0 && result.Data != null)  // 检查 result.Data 是否为 null
        {
            string tempFilePath = Path.Combine(Application.temporaryCachePath, "temp.mp3");
            File.WriteAllBytes(tempFilePath, result.Data);

            StartCoroutine(LoadAudio(tempFilePath));
        }
        else
        {
            Debug.LogError("Failed to synthesize speech or result.Data is null.");
        }
    }

    private IEnumerator LoadAudio(string filePath)
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSource is not initialized.");
            yield break;
        }

        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + filePath, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                if (clip != null)
                {
                    audioSource.clip = clip;
                    audioSource.Play();
                }
                else
                {
                    Debug.LogError("Failed to load AudioClip.");
                }
            }
        }
    }
}
