using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class ServerSender : MonoBehaviour
{
    [SerializeField] private ServerConfig serverConfig;
    [SerializeField] private string serverUrl = "https://wadahub.manerai.com/api/inventory/status";
    
    // Получаем токен из зашифрованного ресурса
    private string AuthToken => TokenStorage.GetToken();
    
    // Добавьте этот метод для настройки через инспектор
    void OnValidate()
    {
        if (serverConfig != null)
        {
            serverUrl = serverConfig.ServerUrl;
        }
    }

    public void SendItemToServer(Item item, bool isAdded)
    {
        StartCoroutine(SendInventoryStatus(item, isAdded));
    }
    
    public IEnumerator SendInventoryStatus(Item item, bool isAdded)
    {
        string jsonData = $"{{\"itemId\":{item.itemId},\"event\":\"update\",\"added\":{isAdded.ToString().ToLower()}}}";
        
        using (UnityWebRequest request = new UnityWebRequest(serverUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + AuthToken);
            
            yield return request.SendWebRequest();
            
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Успешный ответ от сервера: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Ошибка при отправке запроса: " + request.error);
            }
        }
    }
}