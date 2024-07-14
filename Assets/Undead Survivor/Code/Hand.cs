using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool isLeft;
    public SpriteRenderer spriter;
    // 플레이어의 스프라이트렌더러 변수 선언 및 초기화
    SpriteRenderer player;
    // 오른손의 각 위치를 Vector3 형태로 저장
    Vector3 rightPos = new Vector3(0.35f, -0.15f, 0);
    Vector3 rightPosReverse = new Vector3(-0.15f, -0.15f, 0);
    // 왼손의 각 회전을 Quatanion 형태로 저장
    Quaternion leftRot = Quaternion.Euler(0, 0, -35);
    Quaternion leftRotReverse = Quaternion.Euler(0, 0, -135);

    private void Awake()
    {
        player = GetComponentsInParent<SpriteRenderer>()[1];
    }

    void LateUpdate()
    {
        bool isReverse = player.flipX;

        if (isLeft) { // 근접무기
            transform.localRotation = isReverse ? leftRotReverse : leftRot;
            spriter.flipY = isReverse; // 왼손 스프라이트는 Y축 반전
            // 왼손의 sortingOrder를 바꿔주기
            spriter.sortingOrder = isReverse ? 4: 6;
        }
        else // 원거리무기
        {
            transform.localPosition = isReverse ? rightPosReverse : rightPos;
            spriter.flipX = isReverse; // 오른손 스프라이트는 X축 반전
            // 오른손의 sortingOrder를 바꿔주기
            spriter.sortingOrder = isReverse ? 6 : 4;
        }
    }
}
