using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour
{
    Animator anim;
    // ��Ÿ�ӿ� ��������Ʈ�� �ٲٴ� ��Ʈ�ѷ�
    public RuntimeAnimatorController[] animCon;
    public float health;
    public float maxHealth;

    // �ӵ�, ��ǥ, ��������
    public float speed;
    public Rigidbody2D target;

    bool isLive;

    Rigidbody2D rigid;
    Collider2D coll;
    SpriteRenderer spriter;

    // ���� �������� �������ϱ� ���� �ʿ��� ����
    WaitForFixedUpdate wait;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        //  anim.GetCurrentAnimatorStateInfo(���̾� �ε��� ��): ���� �ִϸ��̼� ���� ������ �������� �Լ�: �˹� ����Ǵ� ���ȿ��� �������� �ʰ� �ϱ� ���� �ʿ�
        // IsName: �ش� ������ �̸��� �������� �Ͱ� ������ Ȯ���ϴ� �Լ�
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;

        // ��ġ ���� = Ÿ�� ��ġ - ���� ��ġ
        // ���� = ��ġ ������ ����ȭ
        // �÷��̾��� Ű�Է� ���� ���� �̵� = ������ ���� ���� ���� �̵�
        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        // ���� �ӵ��� �̵��� ������ ���� �ʵ��� �ӵ� ����(�÷��̾�� �浹���� �� �̲������� ���� ����)
        rigid.velocity = Vector2.zero;

    }

    private void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;
        spriter.flipX = target.position.x < 0;
    }

    // ��ũ��Ʈ�� Ȱ��ȭ �� ��, ȣ��Ǵ� �̺�Ʈ �Լ�
    // Ȱ��ȭ�� �Ǹ� ���Ͱ� �÷��̾ �����ϵ��� ����
    private void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
        health = maxHealth;
    }
    // �ʱ� �Ӽ��� �����ϴ� �Լ� �߰�
    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ��� ������ ���޾� ����Ǵ� ���� �����ϱ� ���� !isLive ���� �߰�
        if (!collision.CompareTag("Bullet") || !isLive) {
            return;
        }

        // Bullet ������Ʈ�� �����Ͽ� �������� ������ �ǰ� ����ϱ�
        health -= collision.GetComponent<Bullet>().damage;
        // StartCoroutine: �ڷ�ƾ ����
        StartCoroutine(KnockBack()); // �� �ٸ� ǥ��: StartCoroutine("KnockBack");

        // ���� ü���� �������� �ǰݰ� ������� ������ ������
        if (health > 0)
        {
            // ..Live, Hit Action
            anim.SetTrigger("Hit");// �ִϸ����Ϳ��� ������ Trigger �Ķ���� Ȱ��ȭ 
            // ȿ���� ����� �κи��� ����Լ� ȣ��
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else
        {
            // ..Die
            isLive = false;
            // ������Ʈ�� ��Ȱ��ȭ
            coll.enabled = false;
            rigid.simulated = false;
            // ���� ���Ͱ� ����ִ� �ٸ� ���͵�� �÷��̾ ������ �ʵ��� ���̾� ����
            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true);// �ִϸ����Ϳ��� ������ Bool �Ķ���� Ȱ��ȭ 
            GameManager.instance.kill++;
            GameManager.instance.GetExp();
            // ȿ���� ����� �κи��� ����Լ� ȣ��
            if (GameManager.instance.isLive)
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
        }

    }

    // �ڷ�ƾ: ���� �ֱ�� �񵿱�ó�� ����Ǵ� �Լ�(������ �ߴ� �簳 �� �� �ִ� �Լ�)
    // IEnumerator: �ڷ�ƾ���� ��ȯ�� �������̽�
    IEnumerator KnockBack() {

        // yield: �ڷ�ƾ�� ��ȯ Ű����
        // yield return�� ���� �پ��� ���� �ð��� ����
        // yield return null; // 1������ ����
        //yield return new WaitForSeconds(2f); // 2�� ����

        yield return wait;
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        // �������� ���̹Ƿ� ForceMode2D.Impulse �Ӽ� �߰�
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
}

