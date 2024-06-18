using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebCamDisplay : MonoBehaviour
{
    private WebCamTexture webcamTexture;

    void Start()
    {
        // 웹캠 목록을 가져옴
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length > 0)
        {
            // 첫 번째 웹캠을 사용
            string webcamName = devices[0].name;
            Debug.Log("Using webcam: " + webcamName);

            // 웹캠 텍스처 생성
            webcamTexture = new WebCamTexture(webcamName);

            // 현재 오브젝트의 메인 머티리얼의 메인 텍스처를 웹캠 텍스처로 설정
            Renderer renderer = GetComponent<Renderer>();
            renderer.material.mainTexture = webcamTexture;

            // 웹캠 텍스처 시작
            webcamTexture.Play();

            // 웹캠 텍스처의 상태를 디버그 로그에 출력
            Debug.Log("Webcam is playing: " + webcamTexture.isPlaying);
            Debug.Log("Webcam resolution: " + webcamTexture.width + "x" + webcamTexture.height);
        }
        else
        {
            Debug.LogError("No webcam found");
        }
    }

    void Update()
    {
        if (webcamTexture != null && webcamTexture.isPlaying)
        {
            if (webcamTexture.width < 100)
            {
                Debug.LogWarning("Webcam is initializing, width is less than 100.");
            }
            else
            {
                Debug.Log("Webcam is running, resolution: " + webcamTexture.width + "x" + webcamTexture.height);
            }
        }
    }

    void OnDestroy()
    {
        // 웹캠 텍스처 정지
        if (webcamTexture != null)
        {
            webcamTexture.Stop();
        }
    }
}
