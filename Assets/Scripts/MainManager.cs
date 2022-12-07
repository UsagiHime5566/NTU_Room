using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using SFB;

public class MainManager : MonoBehaviour
{
    public RenderTexture screen1;
    public VideoPlayer videoPlayer;

    [Header("Auto Work")]
    public bool imidiatePlay = false;

    string _path;

    void Start()
    {
        
    }

    void OnGUI() {
        var guiScale = new Vector3(Screen.width / 800.0f, Screen.height / 600.0f, 1.0f);
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, guiScale);

        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.Space(20);
        GUILayout.BeginVertical();

        // Open File Samples

        // if (GUILayout.Button("Open File")) {
        //     WriteResult(StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false));
        // }
        // GUILayout.Space(5);
        // if (GUILayout.Button("Open File Async")) {
        //     StandaloneFileBrowser.OpenFilePanelAsync("Open File", "", "", false, (string[] paths) => { WriteResult(paths); });
        // }
        // GUILayout.Space(5);
        // if (GUILayout.Button("Open File Multiple")) {
        //     WriteResult(StandaloneFileBrowser.OpenFilePanel("Open File", "", "", true));
        // }
        // GUILayout.Space(5);
        // if (GUILayout.Button("Open File Extension")) {
        //     WriteResult(StandaloneFileBrowser.OpenFilePanel("Open File", "", "txt", true));
        // }
        // GUILayout.Space(5);
        // if (GUILayout.Button("Open File Directory")) {
        //     WriteResult(StandaloneFileBrowser.OpenFilePanel("Open File", Application.dataPath, "", true));
        // }
        // GUILayout.Space(5);
        if (GUILayout.Button("Open File Filter")) {
            var extensions = new [] {
                new ExtensionFilter("Support Video Files", "mp4"),
                new ExtensionFilter("All Files", "*" ),
            };
            var result = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);
            WriteResult(result);
            if(result.Length > 0){
                Debug.Log("Success");

                SetupVideo(result[0]);
            }
        }

        GUILayout.EndVertical();
        GUILayout.Space(20);
        GUILayout.Label(_path);
        GUILayout.EndHorizontal();
    }
    public void WriteResult(string[] paths) {
        if (paths.Length == 0) {
            return;
        }

        _path = "";
        foreach (var p in paths) {
            _path += p + "\n";
        }
    }

    void SetupVideo(string filePath){
        videoPlayer.url = filePath;
        videoPlayer.Prepare();
        videoPlayer.loopPointReached += delegate {
            
        };
        videoPlayer.prepareCompleted += delegate {
            Debug.Log($"Get video size: {videoPlayer.texture.width}x{videoPlayer.texture.height}");
            
        };

        if(!imidiatePlay)
            return;
            
        videoPlayer.Play();
    }
}
