using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

                          // ���� �� �ݹ�޼ҵ� ����� ����
public class ConnManager : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        // ���� �ν����� ������ �ִ��͵�. �������� ���� ����
        PhotonNetwork.GameVersion = "0.1";
        int num = Random.Range(0, 1000);
        PhotonNetwork.NickName = $"LCK ���� �����";

        // ���ӿ� �����ϸ� ������ Ŭ���̾�Ʈ(ȣ��Ʈ) �� ������ ���� �ڵ� ����ȭ
        PhotonNetwork.AutomaticallySyncScene = true;

        // ���� ���� �Լ� ȣ���ϸ� �ڵ������� ������ ������ ���� �õ�
        // OnConnectedToMaster �Լ� �ڵ�ȣ��(�ݹ�)
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        print("������ ���� ���� ����");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        print("�κ� ���� ����");
        RoomOptions ro = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 15 };

                                     // ���� / ��ɼ� / Ÿ��
        PhotonNetwork.JoinOrCreateRoom("Test", ro, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        print("�� ���� ����");

        // �������� 2�� �� �ȿ� �������� ��ġ ����
        Vector2 originPos = Random.insideUnitCircle * 2f;

        // ������ Instantiate 
        PhotonNetwork.Instantiate("Player", new Vector3(originPos.x, 0, originPos.y), Quaternion.identity);
    }
}
