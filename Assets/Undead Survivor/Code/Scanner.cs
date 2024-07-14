using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    // ����, ���̾�, ��ĵ ��� �迭, ���� ����� ��ǥ�� ���� ���� ����
    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearestTarget;

    private void FixedUpdate()
    {
        // CircleCastAll: ������ ĳ��Ʈ�� ��� ��� ����� ��ȯ�ϴ� �Լ�
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer); // (ĳ���� ���� ��ġ, ���� ������, ĳ���� ����, ĳ���� ����, ��� ���̾�)
        // ���� ����� ��ǥ ������ ������Ʈ
        nearestTarget = GetNearest();
    }

    // ���� ����� ���� ã�� �Լ� �߰�
    Transform GetNearest() 
    {
        Transform result = null;
        float diff = 100;

        foreach (RaycastHit2D target in targets) 
        { 
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            // Distance(A, B): ���� A�� B�� �Ÿ��� ������ִ� �Լ�
            float curDiff = Vector3.Distance(myPos, targetPos);

            if (curDiff < diff) {
                diff = curDiff;
                result = target.transform;
            }

        }

        return result;
    }

}
