using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RainMaker_Net;
using RaMaNetUnity;
using RaMaNet_VirusWarGameServer;

public class CNetworkManager : MonoBehaviour {

    CRaMaNetUnityService gameserver;
    string received_msg;

    public MonoBehaviour message_receiver;

    // Use this for initialization
    void Awake()
    {
        this.received_msg = "";

        // 네트워크 통신을 위해 CRaMaNetUnityService객체를 추가한다.
        this.gameserver = gameObject.AddComponent<CRaMaNetUnityService>();

        // 상태 변화(접속, 끊김등)을 통보 받을 델리게이트 설정.
        this.gameserver.appcallback_on_status_changed += on_status_changed;

        // 패킷 수신 델리게이트 설정
        this.gameserver.appcallback_on_meesage += on_message;

    }

    //void Start()
    //{
    //    connect();
    //}

    public void connect()
    {
        this.gameserver.connect("127.0.0.1", 7979);
    }

    public bool is_connected()
    {
        return this.gameserver.is_connected();
    }

    // 네트워크 상태 변경시 호출될 콜백 메소드
    void on_status_changed(NETWORK_EVENT status)
    {
        switch (status)
        {
            // 접속 성공
            case NETWORK_EVENT.connected:
                {
                    CLogManager.log("On Connected");
                    this.received_msg += "On Connected\n";

                    GameObject.Find("MainTitle").GetComponent<CMainTitle>().on_connected();

                }
                break;

            // 연결 끊김
            case NETWORK_EVENT.disconnected:
                {
                    CLogManager.log("Disconnected");
                    this.received_msg += "Disconnected\n";
                }
                break;
        }
    }

    void on_message(CPacket msg)
    {
        this.message_receiver.SendMessage("on_recv", msg);
    }

    public void send(CPacket msg)
    {
        this.gameserver.send(msg);
    }
}
