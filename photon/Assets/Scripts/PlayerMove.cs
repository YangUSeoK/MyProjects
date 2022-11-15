using System.Collections.Generic;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


// 네트워크 동기화 : Pun
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
        // 내 오브젝트일 때 카메라 활성화
        cameraRig.SetActive(photonView.IsMine);

        // 로비 접속할 때 설정한 닉네임을 텍스트에 표시
        txt_playerName.text = photonView.Owner.NickName;

        // 내꺼면 초록색, 아니면 빨간색
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
        // 내꺼일때만 컨트롤
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
            // 내꺼가 아닐때는 서버에서 수신한 정보를 토대로 위치설정
            // 데이터 송수신 비율과 로컬 Update 차이로 인한 지연율을 줄이기 위해 Lerp 사용
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
        if (stream.IsWriting) // 데이터 송신
        {
            // 위치, 회전값, 애니메이션 스피드 전송
            stream.SendNext(transform.position);
            stream.SendNext(myCharacter.rotation);
            stream.SendNext(anim.GetFloat("Speed"));
        }
        else if (stream.IsReading) // 데이터 수신
        {
            setPos = (Vector3)stream.ReceiveNext();
            setRot = (Quaternion)stream.ReceiveNext();
            dir_speed = (float)stream.ReceiveNext();
        }
    }
}
