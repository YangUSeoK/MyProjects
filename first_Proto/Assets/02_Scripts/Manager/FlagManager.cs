using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagManager : MonoBehaviour
{

    [SerializeField]
    private Flag[] mFlags = null;
    public Flag[] Flags
    {
        get
        {
            return mFlags;
        }
    }


    private void Awake()
    {
        mFlags = GetComponentsInChildren<Flag>();
    }
}
