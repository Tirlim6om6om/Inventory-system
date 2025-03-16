using System;
using System.IO;
using System.Text;
using UnityEngine;

public static class TokenStorage
{
    private static readonly string ResourcePath = "SecureData/ApiToken";
    private static string cachedToken = null;
    
    public static string GetToken()
    {
        if (cachedToken != null)
            return cachedToken;
            
        try
        {
            TextAsset tokenAsset = Resources.Load<TextAsset>(ResourcePath);
            if (tokenAsset != null)
            {
                // Простое обратимое преобразование для защиты токена в билде
                cachedToken = DecodeToken(tokenAsset.text);
                return cachedToken;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Ошибка при получении токена: " + e.Message);
        }
        
        Debug.LogWarning("Токен авторизации не найден!");
        return "";
    }
    
    // Простое кодирование/декодирование для базовой защиты
    private static string DecodeToken(string encodedToken)
    {
        byte[] data = Convert.FromBase64String(encodedToken);
        return Encoding.UTF8.GetString(data);
    }
    
#if UNITY_EDITOR
    // Вспомогательный метод для создания зашифрованного файла с токеном
    public static void SaveTokenToResource(string token)
    {
        string encodedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
        
        string directory = Path.Combine(Application.dataPath, "Resources/SecureData");
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        
        File.WriteAllText(Path.Combine(directory, "ApiToken.txt"), encodedToken);
        UnityEditor.AssetDatabase.Refresh();
        Debug.Log("Токен сохранен в Resources/SecureData/ApiToken.txt");
    }
#endif
}