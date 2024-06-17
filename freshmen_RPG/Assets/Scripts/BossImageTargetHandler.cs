using UnityEngine;
using UnityEngine.SceneManagement;
using Vuforia;

public class ImageTargetSceneSwitcher : MonoBehaviour
{
    private ObserverBehaviour mObserverBehaviour;

    void Start()
    {
        mObserverBehaviour = GetComponent<ObserverBehaviour>();
        if (mObserverBehaviour)
        {
            mObserverBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
        }
    }

    void OnDestroy()
    {
        if (mObserverBehaviour)
        {
            mObserverBehaviour.OnTargetStatusChanged -= OnTargetStatusChanged;
        }
    }

    private void OnTargetStatusChanged(ObserverBehaviour observer, TargetStatus targetStatus)
    {
        if (targetStatus.Status == Status.TRACKED || 
            targetStatus.Status == Status.EXTENDED_TRACKED)
        {
            // Image Target 인식됨
            Debug.Log("Image Target Found");
            // 씬 전환
            SceneManager.LoadScene("Boss3DScene");
        }
        else if (targetStatus.Status == Status.NO_POSE)
        {
            // Image Target 인식 안 됨
            Debug.Log("Image Target Lost");
        }
    }
}
