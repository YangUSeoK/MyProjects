using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{

    // 포톤매니저에 OnJoinedRoom 에서 생성하던걸 여기로 가져옴
    private void Awake()
    {
        // 20221102 룸ID 입력 만들면서 주석
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length);
        PhotonNetwork.Instantiate("Player", points[idx].position, points[idx].rotation, 0);
    }
}
