using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // �������� �ڽ� ������Ʈ��(���� ��ġ)�� Ʈ�������� ���� �迭 ���� ����
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;
    // ��ȯ ���� ���J�� �����ϴ� ���� ����
    public float levelTime;

    int level;
    float timer;

    private void Awake()
    {
        // �������� �ڽ� ������Ʈ��(���� ��ġ)�� ������Ʈ ��������
        // GetComponentInChildren: �ڽ� ������Ʈ �Ѱ��� ������Ʈ ��������
        // GetComponentsInChildren: �ڽ��� �����Ͽ� ���� ���� �ڽ� ������Ʈ�� ������Ʈ ��������(�迭�� ��ȯ)
        spawnPoint = GetComponentsInChildren<Transform>();
        // �ִ� �ð��� ���� ������ ũ��� ������ �ڵ����� ���� �ð� ��� 
        levelTime = GameManager.instance.maxGameTime / spawnData.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        timer += Time.deltaTime;
        // Mathf.FloorToInt: �Ҽ��� �Ʒ��� ������ Int������ �ٲٴ� �Լ�
        // Mathf.CeilToInt: �Ҽ��� �Ʒ��� �ø��� Int������ �ٲٴ� �Լ�
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnData.Length-1);
        // level�� Ȱ���� ��ȯ �������� ��ȯ �ð� ������ ��ȯ Ÿ�̹��� ����
        if ( timer > spawnData[level].spawnTime) {
            timer = 0;
            Spawn();
        }

    }

    void Spawn() 
    {
       GameObject enemy = GameManager.instance.pool.Get(0); // Random.Range(0, 2): 0~1 ����
       enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position; // �ڽ� ������Ʈ������ ���õǵ��� ���� ������ 1����
        // ������Ʈ Ǯ���� ������ ������Ʈ���� Enemy ������Ʈ�� ����
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}


// ����ȭ: ��ü�� ���� Ȥ�� �����ϱ� ���� ��ȯ - ���� �ۼ��� Ŭ������ �������� ��, ����Ƽ ���� ������ ������ �����ϱ� ���� �ʿ�
// ���� �ۼ��� Ŭ������ ����ȭ�� ���� �ν����Ϳ��� �ʱ�ȭ ����
[System.Serializable]
// ��ȯ ������
public class SpawnData {
    // ��������Ʈ Ÿ��, ��ȯ�ð�, ü��, �ӵ�
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}