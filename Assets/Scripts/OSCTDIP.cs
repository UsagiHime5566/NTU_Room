using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OscJack;

public class OSCTDIP : MonoBehaviour
{
    public InputField INP_IP;
    public OscConnection connect;
    void Awake()
    {
        INP_IP.onValueChanged.AddListener(x =>
        {
            connect.host = x;
            SystemConfig.Instance.SaveData("OSCIP", x);
        });
        INP_IP.text = SystemConfig.Instance.GetData<string>("OSCIP", "192.168.1.13");
    }
}
