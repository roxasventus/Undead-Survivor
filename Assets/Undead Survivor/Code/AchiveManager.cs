using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchiveManager : MonoBehaviour
{
    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;
    public GameObject uiNotice;

    // 업적 데이터
    enum Achive { UnlockPotato, UnlockBean }
    // 업적 데이터들을 저장해둘 배열 선언 및 초기화
    Achive[] achives;
    // WaitForSeconds는 Time.timeScale이 반영된 시간을 기다리고 WaitForSecondsRealtime은 현실 시간을 기다린다.
    // 즉 게임속 흘러가는 시간에 맞춰 기다려야 하는 것이 있으면 WaitForSeconds를 쓰고 그렇지 않으면 WaitForSecondsRealtime를 쓰면 된다.
    WaitForSecondsRealtime wait;

    void Awake()
    {
        // Enum.GetValues: 주어진 열거형의 데이터를 모두 가져오는 함수
        achives = (Achive[])Enum.GetValues(typeof(Achive));
        wait = new WaitForSecondsRealtime(5);
        // HasKey 함수로 데이터 유무 체크 후 초기화 실행
        if (!PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }
    }
    // 저장 데이터 초기화 함수
    void Init()
    {
        // PlaayerPrefs: 간단한 저장 기능을 제공하는 유니티 제공 클래스
        PlayerPrefs.SetInt("MyData", 1); // SetInt 함수를 사용하여 key와 연결된 int형 데이터를 저장

        foreach (Achive achive in achives) {
            PlayerPrefs.SetInt(achive.ToString(), 0);
        }
    }

    void Start()
    {
        UnlockCharacter();
    }

    // 캐릭터 버튼 해금을 위한 함수
    void UnlockCharacter() {
        for (int index = 0; index < lockCharacter.Length; index++) { 
            // 잠금 버튼 배열을 순회하면서 인덱스에 해당하는 업적 이름 가져오기
            string achiveName = achives[index].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;
            lockCharacter[index].SetActive(!isUnlock);
            unlockCharacter[index].SetActive(isUnlock);
        }
    }
    // 모든 업적 확인
    void LateUpdate()
    {
        foreach (Achive achive in achives) {
            CheckAchive(achive);
        }
    }
    // 업적 달성을 위한 함수
    void CheckAchive(Achive achive) {
        bool isAchive = false;

        switch (achive) {
            case Achive.UnlockPotato:
                isAchive = GameManager.instance.kill >= 10;
                break;
            case Achive.UnlockBean:
                isAchive = GameManager.instance.gameTime == GameManager.instance.maxGameTime;
                break;

        }
        // 해당 업적이 처음 달성했다는 조건
        if (isAchive && PlayerPrefs.GetInt(achive.ToString())==0)
        {
            PlayerPrefs.SetInt(achive.ToString(), 1);

            for (int index = 0; index < uiNotice.transform.childCount; index++)
            {
                // 알림 창의 자식 오브젝트를 순회하면서 순번이 맞으면 활성화
                bool isActive = index == (int)achive;
                uiNotice.transform.GetChild(index).gameObject.SetActive(isActive);
            }

            StartCoroutine(NoticeRoutine());
        }
    }
    // 알림 창을 활성화했다가 일정 시간 이후 비활성화하는 코루틴 생성
    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);
        // 효과음 재생할 부분마다 재생함수 호출
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);

        yield return wait;

        uiNotice.SetActive(false);
    }

}
