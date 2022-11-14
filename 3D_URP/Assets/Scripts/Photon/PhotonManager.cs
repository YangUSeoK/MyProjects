using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;


public class PhotonManager : MonoBehaviourPunCallbacks
{
    readonly private string version = "1.0";
    private string nickName = "Yang";

    public TMP_InputField userID = null;
    public TMP_InputField roomID = null;

    private void Awake()
    {
        // ȣ��Ʈ ���� ����ȭ. ���ӵ��� ���͵� ������ ��Ȳ�� ����
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = version;

        // ���� �������� �ʴ� ����Ƚ��. ����׿�.
        print(PhotonNetwork.SendRate);

        // ���� ���� ���ӽõ�
        PhotonNetwork.ConnectUsingSettings();
    }

   

    void SetUserID()
    {
        if (string.IsNullOrEmpty(userID.text))
        {
            nickName = $"USER_{Random.Range(1,21) : 00}";
        }
        else
        {
            nickName = userID.text;
        }
        PlayerPrefs.SetString("USER_ID",nickName);
        PhotonNetwork.NickName = nickName;
    }

    string SetRoomID()
    {
        if (string.IsNullOrEmpty(roomID.text))
        {
            roomID.text = $"ROOM{Random.Range(1,101):000}";
        }
        return roomID.text;
    }


    // ���漭���� ���� �����ϸ� ȣ��Ǵ� �ݹ� �޼ҵ�
    public override void OnConnectedToMaster()
    {
        print("������ ���� ���� ����");
        
        // ���� ���� �� �κ� ���� Ȯ��
        print(PhotonNetwork.InLobby);
        
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    // �κ� ���� ȣ��Ǵ� �ݹ�޼ҵ�
    public override void OnJoinedLobby()
    {
        print("�κ� ���� ����");
        print(PhotonNetwork.InLobby);


        // 20221102 ��ID �Է� ����鼭 �ּ�
        // ���� �뿡 ���ӽõ�
        //PhotonNetwork.JoinRandomRoom();
    }

    // �������� �����ϸ� ȣ��Ǵ� �ݹ�޼ҵ�
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print("���� �� ���� ����");
        
        // �� ���ӹ�ư Ŭ�� �� ����
    }

    // �� ���� �����ϸ� ȣ��Ǵ� �ݹ�޼ҵ�
    public override void OnCreatedRoom()
    {
        print("�� ���� �Ϸ�");
        
        // CurrentRoom : ���� ���� �뿡���� ����
        print(PhotonNetwork.CurrentRoom.Name);
    }

    // �� ���ӿ� �����ϸ� ȣ��Ǵ� �ݹ�޼ҵ�
    public override void OnJoinedRoom()
    {
        print("�� ���� ����");
        print($"���� ������ �� : {PhotonNetwork.CurrentRoom.PlayerCount}");

        foreach(var player in PhotonNetwork.CurrentRoom.Players)
        {                                                       // ���� ������ȣ
            print($"����� : {player.Value.NickName} / {player.Value.ActorNumber}");
        }

        if (PhotonNetwork.IsMasterClient)
        {
            // ���Ŵ����� ����ȯ�� ����. 
            // ������ Ŭ���̾�Ʈ�� ���, �뿡 ���� �� ������ �ҷ�����
            PhotonNetwork.LoadLevel("Standard");
        }
    }

    #region UI_BUTTON_EVENT

    public void OnLoginClick()
    {
        SetUserID();
        PhotonNetwork.JoinRandomRoom();
    }
    public void OnMakeRoomClick()
    {
        SetUserID();

        // OnJoinRandomFailed���� ���� �� ���� �����ϰ� ������� Ŭ����ư���� ���鵵�� �̵�
        RoomOptions ro = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 20 };
        PhotonNetwork.CreateRoom("MyRoom", ro);
    }


    #endregion


}
