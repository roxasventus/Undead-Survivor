using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    
    Collider2D coll;
     
    private void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area")) 
            return;

        // ���� ����(instance)�� ��� Ŭ�������� �θ� �� �ִ�
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;


        switch (transform.tag) {
            case "Ground":
                float diffX = playerPos.x - myPos.x;
                float diffY = playerPos.y - myPos.y;
                float dirX = diffX < 0 ? -1 : 1;
                float dirY = diffY < 0 ? -1 : 1;
                diffX = Mathf.Abs(diffX);
                diffY = Mathf.Abs(diffY);

                // �� ������Ʈ�� �Ÿ� ���̿���, X���� Y�ຸ�� ũ�� ���� �̵�
                if (diffX > diffY) {
                    // Translate: ������ ����ŭ ���� ��ġ���� �̵�
                    transform.Translate(Vector3.right * dirX * 40);
                }
                // �� ������Ʈ�� �Ÿ� ���̿���, Y���� X�ຸ�� ũ�� ���� �̵�
                if (diffX < diffY)
                {
                    // Translate: ������ ����ŭ ���� ��ġ���� �̵�
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;
            // �÷��̾�� ���Ͱ� �־����� �� ���� ���ġ�ϱ�
            case "Enemy":
                if (coll.enabled) {
                    Vector3 dist = playerPos - myPos;
                    Vector3 ran = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f);
                    // �÷��̾��� �̵� ���⿡ ���� ���� ���� �����ϵ��� �̵�
                    transform.Translate(ran + dist * 2);
                }
                break;
        }

    }
}
