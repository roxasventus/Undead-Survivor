using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // ������, ���� ����
    public float damage;
    public int per;

    Rigidbody2D rigid;

    public void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        // ������ -1(����)���� ū �Ϳ� ���ؼ��� �ӵ� ����
        if (per >= 0) {
            // �ӷ��� �����־� �Ѿ��� ���ư��� �ӵ� ������Ű��
            rigid.velocity = dir * 15f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || per == -100) 
        {
            return;
        }
        // ���� ���� �ϳ��� �پ��鼭 -1�� �Ǹ� ��Ȱ��ȭ
        per--;

        if (per < 0) { 
            rigid.velocity = Vector3.zero;
            gameObject.SetActive(false);
        }
    }
    // ����ü ����
    private void OnTriggerExit2D(Collider2D collision)
    {
        // ���� ����� ���� ����
        if (!collision.CompareTag("Area") || per == -100)
            return;
        // ���Ÿ� �����϶�
        gameObject.SetActive(false);
    }

}
