using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        Screen.SetResolution(960, 640, FullScreenMode.Windowed);

        // ���� ������ ��/���� �󵵼� (�ʴ� 30ȸ)
        PhotonNetwork.SendRate = 30;            // ������ ��
        PhotonNetwork.SerializationRate = 30;   // ����ȭ ��
    }
}
