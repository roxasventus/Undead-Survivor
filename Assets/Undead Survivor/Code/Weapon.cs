using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // 무기 ID, 프리펩 ID, 데미지, 개수, 속도
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    float timer; 
    Player player;

    private void Awake()
    {
        player = GameManager.instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isLive)
            return;
        // 무기 ID에 따라 로직을 분리
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime); // Vector3.back이 시계방향
                break;
            default:
                timer += Time.deltaTime;

                // speed 보다 커지면 초기화하면서 발사 로직 실행
                // speed 값은 연사속도를 의미: 적을 수록 많이 발사
                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }

        // .. Test Code ..
        if (Input.GetButtonDown("Jump")) {
            LevelUp(10, 1);
        }
    }

    public void LevelUp(float damage, int count) 
    { 
        this.damage = damage * Character.Damage;
        this.count += count;

        if (id == 0)
            Batch();
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)
    {
        // Basic Set
        name = "Weapon " + data.itemId;
        // 부모 오브젝트를 플레이어로 지정
        transform.parent = player.transform;
        // 지역 위치인 localPosition을 원점으로 변경
        transform.localPosition = Vector3.zero;

        // Property Set
        // 각종 무기 속성 변수들을 스크립트블 오브젝트 데이터로 초기화
        id = data.itemId;
        damage = data.baseDamage * Character.Damage;
        count = data.baseCount + Character.Count;

        for (int index = 0; index < GameManager.instance.pool.prefabs.Length; index++) {
            if (data.projectile == GameManager.instance.pool.prefabs[index]) {
                prefabId = index;
                break;
            }
        }

        // 무기 ID에 따라 로직을 분리
        switch (id) { 
            // 삽
            case 0:
                speed = 150 * Character.WeaponSpeed; // 양수가 시계방향
                Batch();
                break;
            // 총탄
            default:
                speed = 0.3f * Character.WeaponRate;
                break;
        }

        // Hand Set(손 생성)
        Hand hand = player.hands[(int)data.itemType]; //(enum 열거형 데이터는 정수 형태로도 사용 가능)
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        // 나중에 추가된 무기도 레벨업 된 장비의 영향을 받아야 한다
        // BroadcastMessage: 특정 함수호출을 모든 자식에게 방송하는 함수
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    // 생성된 무기를 배치하는 함수
    void Batch() 
    {
        for (int index = 0; index < count; index++) {

            // 가져온 오브젝트의 Transform을 지역변수로 저장
            Transform bullet;



            // 기존 오브젝트를 먼저 활용하고 모자란 것은 풀링에서 가져오기
            if (index < transform.childCount)             // 자신의 자식 오브젝트 개수 확인은 childCount 속성에서
            {
                // index가 아직 childCount 범위 내라면 GetChild 함수로 가져오기
                bullet = transform.GetChild(index);
            }
            else 
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                // 갓 생성된 탄환의 부모는 PoolManager이다. 갓 생성된 탄환은 플레이어를 따라가야 하므로 부모를 플레이어의 자식 오브젝트인 Weapon 0로 바꿔야 한다. 
                bullet.parent = transform; // parent 속성을 통해 부모 변경
            }

            // 탄환의 위치와 회전 초기화
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            // 생성된 탄환을 적절한 위치에 배치
            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            // Translate 함수로 자신의 위쪽으로 이동, 이동 방향은 Space.World 기준으로
            bullet.Translate(bullet.up * 1.5f, Space.World);


            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero); // bullet 컴포넌트 접근하여 속성 초기화 함수 호출, -100은 무한히 관통한다는 의미로 두었다
        }
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        // FromToRotation: 지정된 축을 중심으로 목표를 향해 회전하는 함수
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir); // bullet 컴포넌트 접근하여 속성 초기화 함수 호출, -1은 무한히 관통한다는 의미로 두었다
        // 효과음 재생할 부분마다 재생함수 호출
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
    }
}
