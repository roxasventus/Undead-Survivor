using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    // RectTransform�� transform�� �ٸ��� ������ ��������� ��
    RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        // ����! ���� ��ǥ�� ��ũ�� ��ǥ�� ���� �ٸ��� ������ ���� ��ǥ ���� �÷��̾� ��ġ�� �ٷ� ����ϸ� �ȵȴ� -> (rect.position = GameManager.instance.player.transform.position) ����ϸ� �ȵ�
        // WorldToScreenPoint: ���� ���� ������Ʈ ��ġ�� ��ũ�� ��ǥ�� ��ȯ
        rect.position = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position);
    }
}
