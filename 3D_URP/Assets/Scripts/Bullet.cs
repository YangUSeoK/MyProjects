using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject effect = null;

    private void Start()
    {                             // ������ǥ ����
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 1000f);
        Destroy(gameObject, 3f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var contact = collision.GetContact(0);
        
        // ����Ʈ�� ������缭 ����
        var obj = Instantiate(effect, contact.point, Quaternion.LookRotation(-contact.normal));
        
        Destroy(obj, 2f);
        Destroy(gameObject);
    }



}
