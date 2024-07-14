using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool isLeft;
    public SpriteRenderer spriter;
    // �÷��̾��� ��������Ʈ������ ���� ���� �� �ʱ�ȭ
    SpriteRenderer player;
    // �������� �� ��ġ�� Vector3 ���·� ����
    Vector3 rightPos = new Vector3(0.35f, -0.15f, 0);
    Vector3 rightPosReverse = new Vector3(-0.15f, -0.15f, 0);
    // �޼��� �� ȸ���� Quatanion ���·� ����
    Quaternion leftRot = Quaternion.Euler(0, 0, -35);
    Quaternion leftRotReverse = Quaternion.Euler(0, 0, -135);

    private void Awake()
    {
        player = GetComponentsInParent<SpriteRenderer>()[1];
    }

    void LateUpdate()
    {
        bool isReverse = player.flipX;

        if (isLeft) { // ��������
            transform.localRotation = isReverse ? leftRotReverse : leftRot;
            spriter.flipY = isReverse; // �޼� ��������Ʈ�� Y�� ����
            // �޼��� sortingOrder�� �ٲ��ֱ�
            spriter.sortingOrder = isReverse ? 4: 6;
        }
        else // ���Ÿ�����
        {
            transform.localPosition = isReverse ? rightPosReverse : rightPos;
            spriter.flipX = isReverse; // ������ ��������Ʈ�� X�� ����
            // �������� sortingOrder�� �ٲ��ֱ�
            spriter.sortingOrder = isReverse ? 6 : 4;
        }
    }
}
