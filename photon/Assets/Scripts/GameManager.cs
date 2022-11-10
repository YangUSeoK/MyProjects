using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        Screen.SetResolution(960, 640, FullScreenMode.Windowed);

        // 서버 데이터 송/수신 빈도수 (초당 30회)
        PhotonNetwork.SendRate = 30;            // 보내는 빈도
        PhotonNetwork.SerializationRate = 30;   // 동기화 빈도
    }
}
