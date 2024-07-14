using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CreateAssetMenu: Ŀ���� �޴��� �����ϴ� �Ӽ�
[CreateAssetMenu(fileName = "Item", menuName = "Scriptble Object/ItemData")]
// ��ũ���ͺ� ������Ʈ(Scriptable Object)�� ����Ƽ���� �����ϴ� �뷮�� �����͸� �����ϰ� �����ϴµ� ����� �� �ִ� ������ �����̳�
public class ItemData : ScriptableObject
{
    public enum ItemType { Melee, Range, Glove, Shoe, Heal } // (���� ����, ���Ÿ� ����, �۷���, �Ź�, �� )

    [Header("# Main Info")]
    // �������� ���� �Ӽ����� ������ �ۼ��ϱ�
    public ItemType itemType;
    public int itemId;
    public string itemName;
    [TextArea]
    public string itemDesc;
    public Sprite itemIcon;

    [Header("# Level Data")]
    // �⺻ ������ 
    public float baseDamage;
    // �⺻ ���� ����
    public int baseCount;
    // �߰� ������ ����
    public float[] damages;
    // �߰� �Ǵ� ���� ����
    public int[] counts;

    [Header("# Weapon")]
    public GameObject projectile;
    // �� ��������Ʈ ���� �Ӽ�
    public Sprite hand;

}
