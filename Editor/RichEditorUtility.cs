using UnityEngine;
using UnityEditor;

public class RichEditorUtility
{
    /// <summary>
    /// Play an AudioClip one-shot inside the Editor.
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="loop"></param>
    /// <param name="startSample"></param>
    public static void EditorPlayClip(AudioClip clip, bool loop = false, int startSample = 0)
    {
        System.Reflection.Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
        System.Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
        System.Reflection.MethodInfo method = audioUtilClass.GetMethod(
            "PlayClip",
            System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public,
            null,
            new System.Type[] { typeof(AudioClip), typeof(int), typeof(bool) },
            null
        );
        method.Invoke(
            null,
            new object[] { clip, startSample, loop }
        );
    }
}
