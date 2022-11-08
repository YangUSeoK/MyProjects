using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    // 벽 때릴 때 보여줄 이미지
    public Sprite dmgSprite;
    public int hp = 4;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DamageWall (int loss)
    {
        // 맞는 이미지 보여주고
        spriteRenderer.sprite = dmgSprite;

        // 피를 깎는다.
        hp -= loss;
        if (hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }

}
