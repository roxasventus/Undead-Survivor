using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour
{
    Animator anim;
    // 런타임에 스프라이트를 바꾸는 컨트롤러
    public RuntimeAnimatorController[] animCon;
    public float health;
    public float maxHealth;

    // 속도, 목표, 생존여부
    public float speed;
    public Rigidbody2D target;

    bool isLive;

    Rigidbody2D rigid;
    Collider2D coll;
    SpriteRenderer spriter;

    // 물리 프레임을 딜레이하기 위해 필요한 변수
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

        //  anim.GetCurrentAnimatorStateInfo(레이어 인덱스 값): 현재 애니메이션 상태 정보를 가져오는 함수: 넉백 적용되는 동안에는 움직이지 않게 하기 위해 필요
        // IsName: 해당 상태의 이름이 지저오딘 것과 같은지 확인하는 함수
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;

        // 위치 차이 = 타겟 위치 - 나의 위치
        // 방향 = 위치 차이의 정규화
        // 플레이어의 키입력 값을 더한 이동 = 몬스터의 방향 값을 더한 이동
        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        // 물리 속도가 이동에 영향을 주지 않도록 속도 제거(플레이어와 충돌했을 때 미끄러지는 것을 방지)
        rigid.velocity = Vector2.zero;

    }

    private void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;
        spriter.flipX = target.position.x < 0;
    }

    // 스크립트가 활성화 될 때, 호출되는 이벤트 함수
    // 활성화가 되면 몬스터가 플레이어를 추적하도록 설정
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
    // 초기 속성을 적용하는 함수 추가
    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 사망 로직이 연달아 실행되는 것을 방지하기 위해 !isLive 조건 추가
        if (!collision.CompareTag("Bullet") || !isLive) {
            return;
        }

        // Bullet 컴포넌트로 접근하여 데미지를 가져와 피격 계산하기
        health -= collision.GetComponent<Bullet>().damage;
        // StartCoroutine: 코루틴 시작
        StartCoroutine(KnockBack()); // 또 다른 표현: StartCoroutine("KnockBack");

        // 남은 체력을 조건으로 피격과 사망으로 로직을 나누기
        if (health > 0)
        {
            // ..Live, Hit Action
            anim.SetTrigger("Hit");// 애니메이터에서 설정한 Trigger 파라메터 활성화 
            // 효과음 재생할 부분마다 재생함수 호출
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else
        {
            // ..Die
            isLive = false;
            // 컴포넌트의 비활성화
            coll.enabled = false;
            rigid.simulated = false;
            // 죽은 몬스터가 살아있는 다른 몬스터들과 플레이어를 가리지 않도록 레이어 수정
            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true);// 애니메이터에서 설정한 Bool 파라메터 활성화 
            GameManager.instance.kill++;
            GameManager.instance.GetExp();
            // 효과음 재생할 부분마다 재생함수 호출
            if (GameManager.instance.isLive)
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
        }

    }

    // 코루틴: 생명 주기와 비동기처럼 실행되는 함수(실행을 중단 재개 할 수 있는 함수)
    // IEnumerator: 코루틴만의 반환형 인터페이스
    IEnumerator KnockBack() {

        // yield: 코루틴의 반환 키워드
        // yield return을 통해 다양한 쉬는 시간을 지정
        // yield return null; // 1프레임 쉬기
        //yield return new WaitForSeconds(2f); // 2초 쉬기

        yield return wait;
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        // 순간적인 힘이므로 ForceMode2D.Impulse 속성 추가
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
}

