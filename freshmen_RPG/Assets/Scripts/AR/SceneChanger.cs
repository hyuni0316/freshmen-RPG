using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void LoadHakmoonBattleScene()
    {
        SceneManager.LoadScene("Scenes/HakmoonBattleScene");
    }
    public void LoadPoscoBattleScene()
    {
        SceneManager.LoadScene("Scenes/PoscoBattleScene");
    }
    public void LoadAsanBattleScene()
    {
        SceneManager.LoadScene("Scenes/AsanBattleScene");
    }
}
