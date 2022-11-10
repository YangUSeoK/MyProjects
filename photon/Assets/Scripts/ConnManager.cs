using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

                          // 접속 시 콜백메소드 사용을 위해
public class ConnManager : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        // 포톤 인스팩터 설정에 있던것들. 수동으로 설정 가능
        PhotonNetwork.GameVersion = "0.1";
        int num = Random.Range(0, 1000);
        PhotonNetwork.NickName = $"LCK 내전 가즈아";

        // 게임에 접속하면 마스터 클라이언트(호스트) 가 구성한 씬에 자동 동기화
        PhotonNetwork.AutomaticallySyncScene = true;

        // 서버 접속 함수 호출하면 자동적으로 마스터 서버에 접속 시도
        // OnConnectedToMaster 함수 자동호출(콜백)
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        print("마스터 서버 접속 성공");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        print("로비 접속 성공");
        RoomOptions ro = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 15 };

                                     // 방제 / 방옵션 / 타입
        PhotonNetwork.JoinOrCreateRoom("Test", ro, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        print("룸 입장 성공");

        // 반지름이 2인 원 안에 랜덤으로 위치 지정
        Vector2 originPos = Random.insideUnitCircle * 2f;

        // 서버에 Instantiate 
        PhotonNetwork.Instantiate("Player", new Vector3(originPos.x, 0, originPos.y), Quaternion.identity);
    }
}
