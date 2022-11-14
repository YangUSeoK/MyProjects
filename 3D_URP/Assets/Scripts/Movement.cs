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

    [Header("������ �ۼ��� ���� ����")]
    private Vector3 receivePos = Vector3.zero;     // ���ŵ� ��ġ��
    private Quaternion receiveRot;                  // ���ŵ� ȸ����
    public float damping = 10f;  // ���ŵ� ��ǥ�� �̵��� �� ����� �ΰ���



    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        
        // �÷��̾� ��ġ �������� Plane ����
        plane = new Plane(transform.up, transform.position);

        pv = GetComponent<PhotonView>();
        virtualCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();

        virtualCamera.transform.position = transform.position - transform.forward;
        // �����ī�޶� Ÿ�� ����
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
            // ���� �����ָ� �ʹ� �Ҷ� ���ܼ� �����̴°�ó�� ����.
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

        // �̵� ���� ���
        Vector3 moveDir = (cameraForward * v) + (cameraRight * h);
        moveDir.Set(moveDir.x, 0f, moveDir.z);

        controller.SimpleMove(moveDir * moveSpeed);

        // Dot(A,B) : A�� B ����(A, B ������ ����)
        float forward = Vector3.Dot(moveDir, transform.forward);
        float strafe = Vector3.Dot(moveDir, transform.right);

        animator.SetFloat("Forward", forward);
        animator.SetFloat("Strafe", strafe);
    }

    private void Turn()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float enter = 0f;

        // Ŭ���� ���� �÷��̾ �ִ� ���� �Ÿ� üũ
        // ���� ��������̶� ������. �׳� �̷� ����� �ִ�.
        plane.Raycast(ray, out enter);
        hitPoint = ray.GetPoint(enter);

        // �÷��̾ ���� ���� ����
        Vector3 lookDir = hitPoint - transform.position;
        lookDir.y = 0f;
        transform.localRotation = Quaternion.LookRotation(lookDir);

    }

    // ������,ȸ���� ����ȭ
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        try
        {
            if (stream.IsWriting) // �����͸� �ٸ��������� �۽�
            {
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
            }
            else    // �ٸ������� ������ �۽�
            {
                // ������ ������� �޾ƾ� ��.
                // ��ġ�� -> ȸ���� ������ ��������
                // ��ġ�� -> ȸ���� ������ �޴´�.
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
