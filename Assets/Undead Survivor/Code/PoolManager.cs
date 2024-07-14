using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // .. ��������� ������ ����
    public GameObject[] prefabs; // ��������� ������ �迭 ���� ����

    // .. Ǯ ����� �ϴ� ����Ʈ��
    List<GameObject>[] pools;

    // ������� ����Ʈ�� 1�� 1 �����̴� 

    private void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }

    }

    // ���� ������Ʈ�� ��ȯ�ϴ� �Լ� ����
    public GameObject Get(int index) 
    {
        GameObject select = null;

        // ... ������ Ǯ�� ��Ȱ��ȭ �Ǿ� �ִ� ���� ������Ʈ ����
        // foreach: �迭, ����Ʈ�� �����͸� ���������� �����ϴ� �ݹڹ�
        foreach (GameObject item in pools[index]) {
            // ... �߰��ϸ� select ������ �Ҵ�
            // activeSelf: ������Ʈ�� ��Ȱ��ȭ���� Ȯ��
            if (!item.activeSelf) {
                select = item;
                select.SetActive(true);
                break;
            }
        }
        // ... �� ã������?
        if (!select) {
            // ... ���Ӱ� �����ϰ� select ������ �Ҵ�
            // Instantiate: ���� ������Ʈ�� �����Ͽ� ��鿡 �����ϴ� �Լ�
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);

        }
        return select;
    }
}
