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
        // 호스트 씬과 동기화. 게임도중 들어와도 게임중 상황이 보임
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = version;

        // 포톤 서버와의 초당 전송횟수. 디버그용.
        print(PhotonNetwork.SendRate);

        // 포톤 서버 접속시도
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


    // 포톤서버에 접속 성공하면 호출되는 콜백 메소드
    public override void OnConnectedToMaster()
    {
        print("마스터 서버 접속 성공");
        
        // 서버 접속 후 로비 유무 확인
        print(PhotonNetwork.InLobby);
        
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    // 로비 들어가면 호출되는 콜백메소드
    public override void OnJoinedLobby()
    {
        print("로비 접속 성공");
        print(PhotonNetwork.InLobby);


        // 20221102 룸ID 입력 만들면서 주석
        // 랜덤 룸에 접속시도
        //PhotonNetwork.JoinRandomRoom();
    }

    // 랜덤접속 실패하면 호출되는 콜백메소드
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print("랜덤 룸 접속 실패");
        
        // 방 접속버튼 클릭 후 실행
    }

    // 룸 생성 성공하면 호출되는 콜백메소드
    public override void OnCreatedRoom()
    {
        print("룸 생성 완료");
        
        // CurrentRoom : 현재 접속 룸에대한 정보
        print(PhotonNetwork.CurrentRoom.Name);
    }

    // 룸 접속에 성공하면 호출되는 콜백메소드
    public override void OnJoinedRoom()
    {
        print("룸 접속 성공");
        print($"현재 접속자 수 : {PhotonNetwork.CurrentRoom.PlayerCount}");

        foreach(var player in PhotonNetwork.CurrentRoom.Players)
        {                                                       // 접속 고유번호
            print($"사용자 : {player.Value.NickName} / {player.Value.ActorNumber}");
        }

        if (PhotonNetwork.IsMasterClient)
        {
            // 씬매니저의 씬전환과 같음. 
            // 마스터 클라이언트일 경우, 룸에 입장 후 전투씬 불러오기
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

        // OnJoinRandomFailed에서 랜덤 룸 접속 실패하고 만들던걸 클릭버튼으로 만들도록 이동
        RoomOptions ro = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 20 };
        PhotonNetwork.CreateRoom("MyRoom", ro);
    }


    #endregion


}
