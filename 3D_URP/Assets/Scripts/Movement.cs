using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;
using System;

public class Movement : MonoBehaviourPunCallbacks, IPunObservable
{
    private CharacterController controller = null;
    private Animator animator = null;

    private float moveSpeed = 10f;

    private Plane plane;
    private Ray ray;
    private Vector3 hitPoint = Vector3.zero;

    private PhotonView pv = null;
    private CinemachineVirtualCamera virtualCamera = null;

    [Header("데이터 송수신 관련 변수")]
    private Vector3 receivePos = Vector3.zero;     // 수신된 위치값
    private Quaternion receiveRot;                  // 수신된 회전값
    public float damping = 10f;  // 수신된 좌표로 이동할 때 사용할 민감도



    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        
        // 플레이어 위치 기준으로 Plane 생성
        plane = new Plane(transform.up, transform.position);

        pv = GetComponent<PhotonView>();
        virtualCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();

        virtualCamera.transform.position = transform.position - transform.forward;
        // 버츄얼카메라 타겟 설정
        if (pv.IsMine)
        {
            virtualCamera.Follow = transform;
            virtualCamera.LookAt = transform;
        }
    }

    private void Update()
    {
        if (pv.IsMine)
        {
            Move();
            Turn();
        }
        else
        {
            // 보간 안해주면 너무 뚝뚝 끊겨서 움직이는거처럼 보임.
            transform.position = Vector3.Lerp(transform.position, receivePos, Time.deltaTime * damping);
            transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, Time.deltaTime * damping);
        }
    }

    private void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        // 이동 벡터 계산
        Vector3 moveDir = (cameraForward * v) + (cameraRight * h);
        moveDir.Set(moveDir.x, 0f, moveDir.z);

        controller.SimpleMove(moveDir * moveSpeed);

        // Dot(A,B) : A와 B 내적(A, B 사이의 각도)
        float forward = Vector3.Dot(moveDir, transform.forward);
        float strafe = Vector3.Dot(moveDir, transform.right);

        animator.SetFloat("Forward", forward);
        animator.SetFloat("Strafe", strafe);
    }

    private void Turn()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float enter = 0f;

        // 클릭한 곳과 플레이어가 있는 곳의 거리 체크
        // 원래 쓰던방법이랑 동일함. 그냥 이런 방법도 있다.
        plane.Raycast(ray, out enter);
        hitPoint = ray.GetPoint(enter);

        // 플레이어가 보는 방향 설정
        Vector3 lookDir = hitPoint - transform.position;
        lookDir.y = 0f;
        transform.localRotation = Quaternion.LookRotation(lookDir);

    }

    // 움직임,회전값 동기화
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        try
        {
            if (stream.IsWriting) // 데이터를 다른유저에게 송신
            {
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
            }
            else    // 다른유저의 데이터 송신
            {
                // 보내는 순서대로 받아야 함.
                // 위치값 -> 회전값 순서로 보냈으니
                // 위치값 -> 회전값 순서로 받는다.
                receivePos = (Vector3)stream.ReceiveNext();
                receiveRot = (Quaternion)stream.ReceiveNext();
            }
        }
        catch (Exception ex)
        {
            print(ex.Message);
        }
    }
}
