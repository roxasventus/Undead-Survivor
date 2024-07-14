using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // �Լ��� �ƴ� �Ӽ��� �ۼ�
    public static float Speed {
        get { return GameManager.instance.playerId == 0 ? 1.1f : 1f; } // ���׿����ڸ� Ȱ���Ͽ� ĳ���Ϳ� ���� Ư��ġ ����
    }

    public static float WeaponSpeed
    {
        get { return GameManager.instance.playerId == 1 ? 1.1f : 1f; } // ���׿����ڸ� Ȱ���Ͽ� ĳ���Ϳ� ���� Ư��ġ ����
    }

    public static float WeaponRate
    {
        get { return GameManager.instance.playerId == 1 ? 0.9f : 1f; } // ���׿����ڸ� Ȱ���Ͽ� ĳ���Ϳ� ���� Ư��ġ ����
    }

    public static float Damage
    {
        get { return GameManager.instance.playerId == 2 ? 1.2f : 1f; } // ���׿����ڸ� Ȱ���Ͽ� ĳ���Ϳ� ���� Ư��ġ ����
    }

    public static int Count
    {
        get { return GameManager.instance.playerId == 3 ? 1 : 0; } // ���׿����ڸ� Ȱ���Ͽ� ĳ���Ϳ� ���� Ư��ġ ����
    }
}
