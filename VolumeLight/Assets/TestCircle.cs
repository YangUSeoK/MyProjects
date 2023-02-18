using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class TestCircle : MonoBehaviour
{
    [SerializeField] private GameObject m_Light;
    Vector3 m_Pos = Vector3.zero;

   
    Vector3 origCircleVert = Vector3.zero;
    Vector3 newCircleVert = Vector3.zero;



    private void Update()
    {
        this.transform.up = m_Light.transform.forward;
        this.transform.position = m_Light.transform.position + m_Light.transform.forward;
        this.transform.localScale = new Vector3(m_Light.GetComponentInChildren<SpotLightMeshBuilder>().radius* 2f,
            0.0001f,
            m_Light.GetComponentInChildren<SpotLightMeshBuilder>().radius * 2f);
    }

}
