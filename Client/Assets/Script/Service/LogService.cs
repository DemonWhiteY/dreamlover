#define MACRO_CHINAR
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Chinar可视控制台
/// </summary>
public class LogService : MonoBehaviour
{
#if MACRO_CHINAR
    struct Log
    {
        public string Message;
        public string StackTrace;
        public LogType LogType;
    }


    #region Inspector 面板属性

    [Tooltip("快捷键-开/关控制台")] public KeyCode ShortcutKey = KeyCode.BackQuote;
    [Tooltip("摇动开启控制台？")] public bool ShakeToOpen = true;
    [Tooltip("窗口打开加速度")] public float shakeAcceleration = 3f;
    [Tooltip("是否保持一定数量的日志")] public bool restrictLogCount = false;
    [Tooltip("最大日志数")] public int maxLogs = 1000;

    #endregion

    private readonly List<Log> logs = new List<Log>();
    private Log log;
    private Vector2 scrollPosition;
    private bool visible;
    public bool collapse;

    public bool showOnAwake = true;

    static readonly Dictionary<LogType, Color> logTypeColors = new Dictionary<LogType, Color>
        {
            {LogType.Assert, Color.white},
            {LogType.Error, Color.red},
            {LogType.Exception, Color.red},
            {LogType.Log, Color.white},
            {LogType.Warning, Color.yellow},
        };

    private const string ChinarWindowTitle = "Chinar-控制台";
    private const int Edge = 20;
    readonly GUIContent clearLabel = new GUIContent("清空", "清空控制台内容");
    readonly GUIContent hiddenLabel = new GUIContent("合并信息", "隐藏重复信息");

    readonly Rect titleBarRect = new Rect(0, 0, 10000, 20);
    Rect windowRect = new Rect(Edge, Edge, Screen.width - (Edge * 2), Screen.height - (Edge * 2));

    private void Awake()
    {
        if (showOnAwake)
        {
            visible = true;
        }
    }

    void OnEnable()
    {
#if UNITY_4
            Application.RegisterLogCallback(HandleLog);
#else
        Application.logMessageReceived += HandleLog;
#endif

    }


    void OnDisable()
    {
#if UNITY_4
            Application.RegisterLogCallback(null);
#else
        Application.logMessageReceived -= HandleLog;
#endif
    }


    void Update()
    {
        if (Input.GetKeyDown(ShortcutKey))
        {

        }
        if (Input.GetKeyDown(ShortcutKey)) visible = !visible;
        if (ShakeToOpen && Input.acceleration.sqrMagnitude > shakeAcceleration) visible = true;


    }


    void OnGUI()
    {
        if (!visible) return;
        windowRect = GUILayout.Window(666, windowRect, DrawConsoleWindow, ChinarWindowTitle);
    }


    void DrawConsoleWindow(int windowid)
    {
        DrawLogsList();
        DrawToolbar();
        GUI.DragWindow(titleBarRect);
    }


    void DrawLogsList()
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        for (var i = 0; i < logs.Count; i++)
        {
            if (collapse && i > 0) if (logs[i].Message != logs[i - 1].Message) continue;
            GUI.contentColor = logTypeColors[logs[i].LogType];
            GUILayout.Label(logs[i].Message);
        }
        GUILayout.EndScrollView();
        GUI.contentColor = Color.white;
    }


    void DrawToolbar()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(clearLabel))
        {
            logs.Clear();
        }

        collapse = GUILayout.Toggle(collapse, hiddenLabel, GUILayout.ExpandWidth(false));
        GUILayout.EndHorizontal();
    }


    void HandleLog(string message, string stackTrace, LogType type)
    {
        logs.Add(new Log
        {
            Message = message,
            StackTrace = stackTrace,
            LogType = type,
        });
        DeleteExcessLogs();
    }


    void DeleteExcessLogs()
    {
        if (!restrictLogCount) return;
        var amountToRemove = Mathf.Max(logs.Count - maxLogs, 0);
        print(amountToRemove);
        if (amountToRemove == 0)
        {
            return;
        }

        logs.RemoveRange(0, amountToRemove);
    }
#endif
}
