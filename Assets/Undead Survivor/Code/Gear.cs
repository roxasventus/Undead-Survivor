using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType type;
    public float rate;

    public void Init(ItemData data) 
    {
        // Basic Set
        name = "Gear " + data.itemId;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        // Property Set

        type = data.itemType;
        rate = data.damages[0];
        ApplyGear();
    }

    // ��� ���Ӱ� �߰��ǰų� ������ �� �� ���� ���� �Լ��� ȣ��
    public void LevelUp(float rate) 
    { 
        this.rate = rate;
        ApplyGear();
    }

    // Ÿ�Կ� ���� �����ϰ� ������ ��������ִ� �Լ� �߰�
    void ApplyGear() {
        switch (type) {
            case ItemData.ItemType.Glove:
                RateUp();
                break;
            case ItemData.ItemType.Shoe:
                SpeedUp();
                break;
        }
    }

    // �尩�� ����� ������� �ø��� �Լ� �ۼ�
    void RateUp()
    {
        // �÷��̾�� �ö󰡼� ��� Weapon�� ��������
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach (Weapon weapon in weapons)
        {
            switch (weapon.id) {
                // ���� ����
                case 0:
                    float speed = 150 * Character.WeaponSpeed;
                    weapon.speed = 150 + (150 * rate);
                    break;
                // ���Ÿ� ����
                default:
                    speed = 0.5f * Character.WeaponRate;
                    weapon.speed = speed * (1f - rate); // ���� ��� �۾����� -> �� ���� �߻��ϰ� �ȴ�
                    break;
            }
        }
    }

    // �Ź��� ����� �̵� �ӵ��� �ø��� �Լ� �ۼ�
    void SpeedUp() 
    {
        float speed = 3 * Character.Speed;
        GameManager.instance.player.speed = speed + speed * rate;
    }
}
