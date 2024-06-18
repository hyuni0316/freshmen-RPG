using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class main_buttons : MonoBehaviour
{
    [SerializeField] GameObject ItemPopupUI;
    [SerializeField] GameObject MenuPopupUI;
    [SerializeField] GameObject StoryPopupUI;
    
    public void Onclick_Item()
    {
        ItemPopupUI.SetActive(true);
    }

    public void Onclick_Menu()
    {
        MenuPopupUI.SetActive(true);
    }
    
    public void Onclick_Story()
    {
        StoryPopupUI.SetActive(true);
    }
    
    public void Onclick_Battle()
    {
        SceneManager.LoadScene("ARCameraScene");
    }
}
