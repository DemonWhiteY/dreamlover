using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Mono.Data.Sqlite;
using System;
using System.Data.Common;
using SQLite4Unity3d;

public class DBController : MonoBehaviour
{
    private static DBController _instance;
    
    public SQLiteConnection DBConnection;

    public static DBController Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject dbController = new GameObject("DBController");
                _instance = dbController.AddComponent<DBController>();
                DontDestroyOnLoad(dbController);
                DBController.Instance.DBConnection = new SQLiteConnection(Application.streamingAssetsPath + "/Database.db", SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
            }
            return _instance;
        }
    }

   
}


