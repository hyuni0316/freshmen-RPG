using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    private AudioClip _audioClip;
    void Start()
    {
        
    }

    public void StopBGM()
    {
        _audioSource.Stop();
    }

    public void ButtonClicked_Effect()
    {
        _audioClip = Resources.Load<AudioClip>("Sound/button_effect");
        _audioSource.PlayOneShot(_audioClip);
    }

    public void Attack_Effect()
    {
        _audioClip = Resources.Load<AudioClip>("Sound/attack_effect");
        _audioSource.PlayOneShot(_audioClip);
    }
    
    public void Damaged_Effect()
    {
        _audioClip = Resources.Load<AudioClip>("Sound/damaged_effect");
        _audioSource.PlayOneShot(_audioClip);
    }
    
    public void GameClear_Effect()
    {
        _audioClip = Resources.Load<AudioClip>("Sound/gameClear_effect");
        _audioSource.PlayOneShot(_audioClip);
    }
    
    public void GameOver_Effect()
    {
        _audioClip = Resources.Load<AudioClip>("Sound/gameOver_effect");
        _audioSource.PlayOneShot(_audioClip);
    }
    
    public void Battle_BGM()
    {
        _audioClip = Resources.Load<AudioClip>("Sound/battle_bgm");
        _audioSource.PlayOneShot(_audioClip);
    }

    public void Basic_BGM()
    {
        _audioClip = Resources.Load<AudioClip>("Sound/basic_bgm");
        _audioSource.PlayOneShot(_audioClip);
    }
}
