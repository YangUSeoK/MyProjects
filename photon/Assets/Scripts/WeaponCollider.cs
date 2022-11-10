using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    public BoxCollider weaponCol = null;

    private void Start()
    {
        DeactivateCollider();
    }

    public void ActivateCollider()
    {
        weaponCol.enabled = true;
    }
    public void DeactivateCollider()
    {
        weaponCol.enabled = false;
    }

}
