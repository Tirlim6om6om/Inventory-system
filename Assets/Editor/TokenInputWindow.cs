#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;


public class TokenInputWindow : EditorWindow
{
    private string token = "";
    
    [MenuItem("Tools/Security/Save API Token")]
    public static void ShowWindow()
    {
        var window = GetWindow<TokenInputWindow>("Ввод токена API");
        window.minSize = new Vector2(450, 120);
        window.Show();
    }
    
    void OnGUI()
    {
        EditorGUILayout.LabelField("Введите токен API (будет сохранен в зашифрованном виде):", EditorStyles.wordWrappedLabel);
        EditorGUILayout.Space(5);
        
        token = EditorGUILayout.TextField(token);
        
        EditorGUILayout.Space(10);
        
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        
        if (GUILayout.Button("Сохранить", GUILayout.Width(120)))
        {
            if (!string.IsNullOrEmpty(token))
            {
                TokenStorage.SaveTokenToResource(token);
                Close();
                EditorUtility.DisplayDialog("Успех", "Токен успешно сохранен", "OK");
            }
            else
            {
                EditorUtility.DisplayDialog("Ошибка", "Токен не может быть пустым", "OK");
            }
        }
        
        if (GUILayout.Button("Отмена", GUILayout.Width(120)))
        {
            Close();
        }
        
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }
}
#endif