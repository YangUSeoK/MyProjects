using System.Collections.Generic;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


// ��Ʈ��ũ ����ȭ : Pun
public class PlayerMove : MonoBehaviourPun, IPunObservable
{
    public float moveSpeed = 3f;
    public float rotSpeed = 200f;
    public GameObject cameraRig = null;
    public Transform myCharacter = null;
    public Animator anim = null;

    private Vector3 setPos = Vector3.zero;  
    private Quaternion setRot;
    float dir_speed = 0f;

    public Text txt_playerName = null;


    private void Start()
    {
        // �� ������Ʈ�� �� ī�޶� Ȱ��ȭ
        cameraRig.SetActive(photonView.IsMine);

        // �κ� ������ �� ������ �г����� �ؽ�Ʈ�� ǥ��
        txt_playerName.text = photonView.Owner.NickName;

        // ������ �ʷϻ�, �ƴϸ� ������
        if (photonView.IsMine)
        {
            txt_playerName.color = Color.green;
        }
        else
        {
            txt_playerName.color = Color.red;
        }
    }

    private void Update()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        // �����϶��� ��Ʈ��
        if (photonView.IsMine)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            Vector3 dir = new Vector3(h, 0, v);
            dir.Normalize();

            dir = cameraRig.transform.TransformDirection(dir);
            transform.position += dir * moveSpeed * Time.deltaTime;


            //myCharacter.rotation = Quaternion.LookRotation(dir);
            anim.SetFloat("Speed", dir.sqrMagnitude);
        }
        else
        {
            // ������ �ƴҶ��� �������� ������ ������ ���� ��ġ����
            // ������ �ۼ��� ������ ���� Update ���̷� ���� �������� ���̱� ���� Lerp ���
            transform.position = Vector3.Lerp(transform.position, setPos,Time.deltaTime * 20);
            myCharacter.rotation = Quaternion.Lerp(transform.rotation,setRot,Time.deltaTime * 20);
            anim.SetFloat("Speed", dir_speed);
        }

    }

    private void Rotate()
    {
        if (photonView.IsMine)
        {
            float y = Input.GetAxis("Mouse X");
            cameraRig.transform.eulerAngles += new Vector3(0f, y, 0f) * rotSpeed * Time.deltaTime;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) // ������ �۽�
        {
            // ��ġ, ȸ����, �ִϸ��̼� ���ǵ� ����
            stream.SendNext(transform.position);
            stream.SendNext(myCharacter.rotation);
            stream.SendNext(anim.GetFloat("Speed"));
        }
        else if (stream.IsReading) // ������ ����
        {
            setPos = (Vector3)stream.ReceiveNext();
            setRot = (Quaternion)stream.ReceiveNext();
            dir_speed = (float)stream.ReceiveNext();
        }
    }
}
