using UnityEngine;

[CreateAssetMenu(fileName = "ServerConfig", menuName = "Config/ServerConfig")]
public class ServerConfig : ScriptableObject
{
    [SerializeField] private string serverUrl = "https://wadahub.manerai.com/api/inventory/status";
    [SerializeField] private string authToken = "";

    public string ServerUrl => serverUrl;
    public string AuthToken => authToken;
}