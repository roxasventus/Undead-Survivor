using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // 스포너의 자식 오브젝트들(스폰 위치)의 트랜스폼을 담을 배열 변수 선언
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    // 소환 레벨 구갘을 결정하는 변수 선언
    public float levelTime;

    int level;
    float timer;

    private void Awake()
    {
        // 스포너의 자식 오브젝트들(스폰 위치)의 컴포넌트 가져오기
        // GetComponentInChildren: 자식 오브젝트 한개의 컴포넌트 가져오기
        // GetComponentsInChildren: 자신을 포함하여 여러 개의 자식 오브젝트의 컴포넌트 가져오기(배열을 반환)
        spawnPoint = GetComponentsInChildren<Transform>();
        // 최대 시간에 몬스터 데이터 크기로 나누어 자동으로 구간 시간 계산 
        levelTime = GameManager.instance.maxGameTime / spawnData.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        timer += Time.deltaTime;
        // Mathf.FloorToInt: 소수점 아래는 버리고 Int형으로 바꾸는 함수
        // Mathf.CeilToInt: 소수점 아래를 올리고 Int형으로 바꾸는 함수
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnData.Length-1);
        // level을 활용해 소환 데이터의 소환 시간 값으로 소환 타이밍을 변경
        if ( timer > spawnData[level].spawnTime) {
            timer = 0;
            Spawn();
        }

    }

    void Spawn() 
    {
       GameObject enemy = GameManager.instance.pool.Get(0); // Random.Range(0, 2): 0~1 범위
       enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position; // 자식 오브젝트에서만 선택되도록 랜덤 시작은 1부터
        // 오브젝트 풀에서 가져온 오브젝트에서 Enemy 컴포넌트로 접근
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}


// 직렬화: 개체를 저장 혹은 전송하기 위해 변환 - 직접 작성한 클래스를 선언했을 때, 유니티 엔진 내에서 변수에 접근하기 위해 필요
// 직접 작성한 클래스를 직렬화를 통해 인스펙터에서 초기화 가능
[System.Serializable]
// 소환 데이터
public class SpawnData {
    // 스프라이트 타입, 소환시간, 체력, 속도
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}