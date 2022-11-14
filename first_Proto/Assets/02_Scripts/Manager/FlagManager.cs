using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagManager : MonoBehaviour
{

    [SerializeField]
    private Flag[] m_Flags = null;
    public Flag[] Flags
    {
        get
        {
            return m_Flags;
        }
    }


    private void Awake()
    {
        m_Flags = GetComponentsInChildren<Flag>();
    }
}
