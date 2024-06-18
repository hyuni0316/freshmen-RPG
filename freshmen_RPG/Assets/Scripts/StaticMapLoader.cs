using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Cerberus_Platform_API
{
    public class StaticMapLoader : MonoBehaviour
    {
        public RawImage mapRawImage;

        [Header("맵 정보")]
        public string strBaseURL = "http://api.vworld.kr/req/image?service=image&request=getmap&key=";
        public string latitude = "";
        public string longitude = "";
        public int zoomLevel = 14;
        public int mapWidth;
        public int mapHeight;
        public string strAPIKey = "";

        private void Start()
        {
            StartCoroutine(VWorldMapLoad());
        }

        IEnumerator VWorldMapLoad()
        {
            yield return null;

            StringBuilder str = new StringBuilder();
            str.Append(strBaseURL.ToString());
            str.Append(strAPIKey.ToString());
            str.Append("&format=png");
            str.Append("&basemap=GRAPHIC");
            str.Append("&center=");
            str.Append(longitude.ToString());
            str.Append(",");
            str.Append(latitude.ToString());
            str.Append("&crs=epsg:4326");
            str.Append("&zoom=");
            str.Append(zoomLevel.ToString());
            str.Append("&size=");
            str.Append(mapWidth.ToString());
            str.Append(",");
            str.Append(mapHeight.ToString());

            string url = str.ToString();
            Debug.Log("Request URL: " + url);

            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

            // Allow insecure HTTP requests (Development Only)
            request.certificateHandler = new AcceptAllCertificatesSignedWithASpecificKeyPublicKey();

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
            }
            else if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                if (texture != null)
                {
                    mapRawImage.texture = texture;
                }
                else
                {
                    Debug.LogError("Error: Failed to load texture from response.");
                }
            }
            else
            {
                Debug.LogError("Error: Unexpected request result: " + request.result.ToString());
            }
        }
    }

    // Custom certificate handler (Development Only)
    public class AcceptAllCertificatesSignedWithASpecificKeyPublicKey : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            // Simply return true no matter what
            return true;
        }
    }
}
