using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // ���� ID, ������ ID, ������, ����, �ӵ�
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
        // ���� ID�� ���� ������ �и�
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime); // Vector3.back�� �ð����
                break;
            default:
                timer += Time.deltaTime;

                // speed ���� Ŀ���� �ʱ�ȭ�ϸ鼭 �߻� ���� ����
                // speed ���� ����ӵ��� �ǹ�: ���� ���� ���� �߻�
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
        // �θ� ������Ʈ�� �÷��̾�� ����
        transform.parent = player.transform;
        // ���� ��ġ�� localPosition�� �������� ����
        transform.localPosition = Vector3.zero;

        // Property Set
        // ���� ���� �Ӽ� �������� ��ũ��Ʈ�� ������Ʈ �����ͷ� �ʱ�ȭ
        id = data.itemId;
        damage = data.baseDamage * Character.Damage;
        count = data.baseCount + Character.Count;

        for (int index = 0; index < GameManager.instance.pool.prefabs.Length; index++) {
            if (data.projectile == GameManager.instance.pool.prefabs[index]) {
                prefabId = index;
                break;
            }
        }

        // ���� ID�� ���� ������ �и�
        switch (id) { 
            // ��
            case 0:
                speed = 150 * Character.WeaponSpeed; // ����� �ð����
                Batch();
                break;
            // ��ź
            default:
                speed = 0.3f * Character.WeaponRate;
                break;
        }

        // Hand Set(�� ����)
        Hand hand = player.hands[(int)data.itemType]; //(enum ������ �����ʹ� ���� ���·ε� ��� ����)
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        // ���߿� �߰��� ���⵵ ������ �� ����� ������ �޾ƾ� �Ѵ�
        // BroadcastMessage: Ư�� �Լ�ȣ���� ��� �ڽĿ��� ����ϴ� �Լ�
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    // ������ ���⸦ ��ġ�ϴ� �Լ�
    void Batch() 
    {
        for (int index = 0; index < count; index++) {

            // ������ ������Ʈ�� Transform�� ���������� ����
            Transform bullet;



            // ���� ������Ʈ�� ���� Ȱ���ϰ� ���ڶ� ���� Ǯ������ ��������
            if (index < transform.childCount)             // �ڽ��� �ڽ� ������Ʈ ���� Ȯ���� childCount �Ӽ�����
            {
                // index�� ���� childCount ���� ����� GetChild �Լ��� ��������
                bullet = transform.GetChild(index);
            }
            else 
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                // �� ������ źȯ�� �θ�� PoolManager�̴�. �� ������ źȯ�� �÷��̾ ���󰡾� �ϹǷ� �θ� �÷��̾��� �ڽ� ������Ʈ�� Weapon 0�� �ٲ�� �Ѵ�. 
                bullet.parent = transform; // parent �Ӽ��� ���� �θ� ����
            }

            // źȯ�� ��ġ�� ȸ�� �ʱ�ȭ
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            // ������ źȯ�� ������ ��ġ�� ��ġ
            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            // Translate �Լ��� �ڽ��� �������� �̵�, �̵� ������ Space.World ��������
            bullet.Translate(bullet.up * 1.5f, Space.World);


            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero); // bullet ������Ʈ �����Ͽ� �Ӽ� �ʱ�ȭ �Լ� ȣ��, -100�� ������ �����Ѵٴ� �ǹ̷� �ξ���
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
        // FromToRotation: ������ ���� �߽����� ��ǥ�� ���� ȸ���ϴ� �Լ�
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir); // bullet ������Ʈ �����Ͽ� �Ӽ� �ʱ�ȭ �Լ� ȣ��, -1�� ������ �����Ѵٴ� �ǹ̷� �ξ���
        // ȿ���� ����� �κи��� ����Լ� ȣ��
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
    }
}
