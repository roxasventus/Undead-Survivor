using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Item[] items;


    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        // ��Ȱ���� ������Ʈ�� �������� ���� ���ڰ����� true�� ����
        items = GetComponentsInChildren<Item>(true);
    }

    public void Show()
    {
        Next();
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
        // ȿ���� ����� �κи��� ����Լ� ȣ��
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        // ���� �� UI�� ��Ÿ�� �� ���͸� Ű�����ϴ� �Լ�
        AudioManager.instance.EffectBgm(true);
    }

    public void Hide()
    { 
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
        // ȿ���� ����� �κи��� ����Լ� ȣ��
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        // ���� �� UI�� ������� �������ϴ� �Լ�
        AudioManager.instance.EffectBgm(false);
    }

    // ��ư�� ��� �����ִ� �Լ� �ۼ�
    public void Selected(int index)
    {
        items[index].OnClick();
    }

    void Next()
    {
        // 1. ��� ������ ��Ȱ��ȭ
        foreach (Item item in items) { 
            item.gameObject.SetActive(false);
        }
        // 2. �� �߿��� ���� 3�� ������ Ȱ��ȭ
        int[] ran = new int[3];
        while (true)
        {
            ran[0] = Random.Range(0, items.Length);
            ran[1] = Random.Range(0, items.Length);
            ran[2] = Random.Range(0, items.Length);

            if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2])
                break;

        }

        for (int index = 0; index < ran.Length; index++) {
            Item ranItem = items[ran[index]];

            // 3. ���� �������� ���� �Һ���������� ��ü
            if (ranItem.level == ranItem.data.damages.Length)
            {
                //�������� �ִ뷹���̸� �Һ� �������� ��� Ȱ��ȭ �ǵ��� �ۼ�
                items[4].gameObject.SetActive(true);
            }
            else {
                ranItem.gameObject.SetActive(true);
            }
        }

    }
}
