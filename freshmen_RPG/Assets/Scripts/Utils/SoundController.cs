using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundController : MonoBehaviour
{
    [SerializeField] private SoundPlayer _effectPlayer;
    [SerializeField] private SoundPlayer _BGMPlayer;
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "BossBattleScene")
        {
            _BGMPlayer.Battle_BGM();
        }
        else
        {
            _BGMPlayer.Basic_BGM();
        }
    }
}
