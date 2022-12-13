using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using SFB;

public class MainManager : MonoBehaviour
{
    public List<VideoPlayer> videoPlayers;

    [Header("Auto Work")]
    public bool imidiatePlay = false;
    string _path;

    void OnGUI() {
        var guiScale = new Vector3(Screen.width / 800.0f, Screen.height / 600.0f, 1.0f);
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, guiScale);

        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.Space(20);
        GUILayout.BeginVertical();
        // Open File Samples
        if (GUILayout.Button("Open File Video 0")) {
            var extensions = new [] {
                new ExtensionFilter("Support Video Files", "mp4"),
                new ExtensionFilter("All Files", "*" ),
            };
            var result = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);
            WriteResult(result);
            if(result.Length > 0){
                Debug.Log("Success");
                SetupVideo(0, result[0]);
            }
        }

        GUILayout.Space(20);

        if (GUILayout.Button("Open File Video 1")) {
            var extensions = new [] {
                new ExtensionFilter("Support Video Files", "mp4"),
                new ExtensionFilter("All Files", "*" ),
            };
            var result = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);
            WriteResult(result);
            if(result.Length > 0){
                Debug.Log("Success");

                SetupVideo(1, result[0]);
            }
        }

        GUILayout.Space(20);

        if (GUILayout.Button("Open File Video 2")) {
            var extensions = new [] {
                new ExtensionFilter("Support Video Files", "mp4"),
                new ExtensionFilter("All Files", "*" ),
            };
            var result = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);
            WriteResult(result);
            if(result.Length > 0){
                Debug.Log("Success");

                SetupVideo(2, result[0]);
            }
        }

        GUILayout.Space(20);

        if (GUILayout.Button("Open File Video 3")) {
            var extensions = new [] {
                new ExtensionFilter("Support Video Files", "mp4"),
                new ExtensionFilter("All Files", "*" ),
            };
            var result = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);
            WriteResult(result);
            if(result.Length > 0){
                Debug.Log("Success");

                SetupVideo(3, result[0]);
            }
        }

        GUILayout.Space(20);

        if (GUILayout.Button("Open File Video 4")) {
            var extensions = new [] {
                new ExtensionFilter("Support Video Files", "mp4"),
                new ExtensionFilter("All Files", "*" ),
            };
            var result = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);
            WriteResult(result);
            if(result.Length > 0){
                Debug.Log("Success");

                SetupVideo(4, result[0]);
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

    void SetupVideo(int index, string filePath){
        videoPlayers[index].url = filePath;
        videoPlayers[index].Prepare();
        videoPlayers[index].loopPointReached += delegate {
            
        };
        videoPlayers[index].prepareCompleted += delegate {
            Debug.Log($"Get video size: {videoPlayers[index].texture.width}x{videoPlayers[index].texture.height}");
            
        };

        if(!imidiatePlay)
            return;
            
        videoPlayers[index].Play();
    }
}
