using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public Scanner scanner;
    public Hand[] hands;
    public RuntimeAnimatorController[] animCon;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    // ������ �� �ѹ��� ����Ǵ� �����ֱ� Awake
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        // ó���� ��Ȱ��ȭ�� ������Ʈ�� �������� ���ؼ��� ���ڰ����� true�� �־� �Ѵ�
        hands = GetComponentsInChildren<Hand>(true);
    }

    void OnEnable()
    {
        speed *= Character.Speed;
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];    
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }

    // ���� ���� �����Ӹ��� ȣ��Ǵ� �����ֱ� FixedUpdate
    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;
        // �������ͷ� ������ �÷��̾ ����ġ�� ���� �����̴� ���� ���� �� �ִ�
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime; // Time.fixedDeltaTime: ���� ������ �ϳ��� �Һ��� �ð�
        rigid.MovePosition(rigid.position + nextVec); // rigid.position: ���� ��ġ 
    }

    // �������� ���� �Ǳ� �� ����Ǵ� �����ֱ� �Լ�
    private void LateUpdate()
    {

        // �ִϸ����Ϳ��� ������ �Ķ���� Ÿ�԰� ������ �Լ� �ۼ�
        anim.SetFloat("Speed", inputVec.magnitude); // (�Ķ���� �̸�, �ݿ��� float ��) // magnitude: ������ ������ ũ�� ��
        // ��������Ʈ ����
        if (inputVec.x != 0) {
            spriter.flipX = inputVec.x < 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive) {
            return;
        }

        GameManager.instance.health -= Time.fixedDeltaTime * 100;

        if (GameManager.instance.health < 0) { 
            for (int index=2;  index < transform.childCount; index++) 
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }
            anim.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }

}
