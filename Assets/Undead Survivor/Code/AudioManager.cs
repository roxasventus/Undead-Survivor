using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
   public static AudioManager instance;

    [Header("#BGM")]
    // ������� ���õ� Ŭ��, ����, ������ҽ� ���� ����
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;
    AudioHighPassFilter bgmEffect;

    [Header("#SFX")]
    // ȿ������ ���õ� Ŭ��, ����, ������ҽ� ���� ����
    public AudioClip[] sfxClips;
    public float sfxVolume;
    // �ٷ��� ȿ������ �� �� �ֵ��� ä�� ���� ���� ����
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    public enum Sfx { Dead, Hit, LevelUp=3, Lose, Melee, Range = 7, Select, Win }

    private void Awake()
    {
        instance = this;
        Init();
    }

    void Init()
    {
        // ����� �÷��̾� �ʱ�ȭ
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform; // ������� ����ϴ� �ڽ� ������Ʈ�� ����
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        // ȿ���� �÷��̾� �ʱ�ȭ
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int index = 0; index < sfxPlayers.Length; index++) {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake=false;
            sfxPlayers[index].volume = bgmVolume;
            // ������ UI�� �� ��, ȿ������ ���Ұ� ���� �ʵ���
            sfxPlayers[index].bypassEffects = true;
        }
    }

    // ������� ����ϴ� �Լ� �ۼ�
    public void PlayBgm(bool isPlay) {
        if (isPlay) { 
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.Stop();
        }
    }

    // ����� ���͸� �Ѱ� ���� �Լ� �ۼ�
    public void EffectBgm(bool isPlay)
    {
        bgmEffect.enabled = isPlay;
    }

    // ȿ���� ��� �Լ�
    public void PlaySfx(Sfx sfx)
    {
        for (int index = 0; index < sfxPlayers.Length; index++) {
            // �� �������� �����ߴ� �÷��̾��� �ε���
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;  // ä�� ������ŭ ��ȸ�ϵ��� ä���ε��� ���� Ȱ��

            if (sfxPlayers[loopIndex].isPlaying)
                continue;

            int ranIndex = 0;
            // ȿ������ 2�� �̻��� ���� ���� �ε����� ���ϱ�
            if (sfx == Sfx.Hit || sfx == Sfx.Melee)
            {
                ranIndex = UnityEngine.Random.Range(0, 2);
            }

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + ranIndex];
            sfxPlayers[loopIndex].Play();
            // ȿ���� ����� �� ���Ŀ��� �� break�� �ݺ��� ����
            break;
        }
    }
}
