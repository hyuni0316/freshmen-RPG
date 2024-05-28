using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadBattleScene : MonoBehaviour
{
    public void Load_BattleScene()
    {
        SceneManager.LoadScene("Scenes/BattleScene");
    }
}
