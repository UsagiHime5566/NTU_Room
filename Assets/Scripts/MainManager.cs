using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TriLibCore.SFB;

public class MainManager : MonoBehaviour
{
    public Button BTN_Wall;
    public Button BTN_Floor;
    public Button BTN_Sound;
    public Button BTN_SyncPlay;

    public Text TXT_Wall;
    public Text TXT_Floor;
    public Text TXT_Sound;
    public Text TXT_Log;

    public Toggle TG_Mute;

    public List<VideoPlayer> videoPlayers;

    [Header("Auto Work")]
    public bool imidiatePlay = false;
    string _path;

    void Start(){
        BTN_Wall.onClick.AddListener(() => {
            LoadVideo(0, TXT_Wall);
        });
        BTN_Floor.onClick.AddListener(() => {
            LoadVideo(1, TXT_Floor);
        });
        BTN_Sound.onClick.AddListener(() => {
            LoadSound(TXT_Sound);
        });

        BTN_SyncPlay.onClick.AddListener(SyncPlay);

        TG_Mute.onValueChanged.AddListener((x) => {
            SystemConfig.Instance.SaveData("Mute", x);
            MuteVideo(x);
        });

        LoadLastSetting();
    }

    void LoadVideo(int index, Text txt){
        var extensions = new[] {
            new ExtensionFilter("Support Video Files", "mp4"),
            new ExtensionFilter("All Files", "*" ),
        };
        var result = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);
        if (result != null)
        {
            var hasFiles = result.Count > 0 && result[0].HasData;

            if(hasFiles){
                string filePath = result[0].Name;

                if(txt) txt.text = filePath;
                SystemConfig.Instance.SaveData("Video"+index, filePath);
                SetupVideo(index, filePath);
                Debug.Log("Success");
            }
        }
    }
    void LoadSound(Text txt){
        var extensions = new[] {
            new ExtensionFilter("Support Sound Files", "mp3"),
            new ExtensionFilter("All Files", "*" ),
        };
        var result = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);
        if (result != null)
        {
            var hasFiles = result.Count > 0 && result[0].HasData;

            if(hasFiles){
                string filePath = result[0].Name;

                if(txt) txt.text = filePath;
                SystemConfig.Instance.SaveData("Sound", filePath);

                Debug.Log("Success");
            }
        }
    }

    void LoadLastSetting(){
        string pathWall = SystemConfig.Instance.GetData<string>("Video0", "");
        if(!string.IsNullOrEmpty(pathWall)){
            if(TXT_Wall) TXT_Wall.text = pathWall;
            SetupVideo(0, pathWall);
        }

        string pathFloor = SystemConfig.Instance.GetData<string>("Video1", "");
        if(!string.IsNullOrEmpty(pathFloor)){
            if(TXT_Floor) TXT_Floor.text = pathFloor;
            SetupVideo(1, pathFloor);
        }

        string pathSound = SystemConfig.Instance.GetData<string>("Sound", "");
        if(!string.IsNullOrEmpty(pathSound)){
            if(TXT_Sound) TXT_Sound.text = pathSound;
            
        }

        TG_Mute.isOn = SystemConfig.Instance.GetData<bool>("Mute", false);
    }

    void MuteVideo(bool val){
        foreach (var vp in videoPlayers)
        {
            vp.SetDirectAudioMute(0, val);
        }
    }

    // void OnGUI()
    // {
    //     var guiScale = new Vector3(Screen.width / 800.0f, Screen.height / 600.0f, 1.0f);
    //     GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, guiScale);

    //     GUILayout.Space(20);
    //     GUILayout.BeginHorizontal();
    //     GUILayout.Space(20);
    //     GUILayout.BeginVertical();
    //     // Open File Samples
    //     if (GUILayout.Button("Open File Video (Wall)"))
    //     {
    //         var extensions = new[] {
    //             new ExtensionFilter("Support Video Files", "mp4"),
    //             new ExtensionFilter("All Files", "*" ),
    //         };
    //         var result = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);
    //         if (result != null)
    //         {
    //             var hasFiles = result.Count > 0 && result[0].HasData;

    //             if(hasFiles){
    //                 string filePath = result[0].Name;

    //                 WriteResult(filePath);
    //                 Debug.Log("Success");
    //                 SetupVideo(0, filePath);
    //             }
    //         }
    //     }

    //     GUILayout.Space(20);

    //     if (GUILayout.Button("Open File Video (Ground)"))
    //     {
    //         var extensions = new[] {
    //             new ExtensionFilter("Support Video Files", "mp4"),
    //             new ExtensionFilter("All Files", "*" ),
    //         };
    //         var result = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);
    //         if (result != null)
    //         {
    //             var hasFiles = result.Count > 0 && result[0].HasData;

    //             if(hasFiles){
    //                 string filePath = result[0].Name;

    //                 WriteResult(filePath);
    //                 Debug.Log("Success");
    //                 SetupVideo(1, filePath);
    //             }
    //         }
    //     }

    //     GUILayout.Space(20);

    //     if (GUILayout.Button("Sync Play"))
    //     {
    //         SyncPlay();
    //     }

    //     GUILayout.Space(20);

    //     // if (GUILayout.Button("Sound File"))
    //     // {
    //     //     var extensions = new[] {
    //     //         new ExtensionFilter("Support Video Files", "mp4"),
    //     //         new ExtensionFilter("All Files", "*" ),
    //     //     };
    //     //     var result = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);
    //     //     WriteResult(result);
    //     //     if (result.Length > 0)
    //     //     {
    //     //         Debug.Log("Success");

    //     //         SetupVideo(3, result[0]);
    //     //     }
    //     // }

    //     // GUILayout.Space(20);

    //     // if (GUILayout.Button("Open File Video 4"))
    //     // {
    //     //     var extensions = new[] {
    //     //         new ExtensionFilter("Support Video Files", "mp4"),
    //     //         new ExtensionFilter("All Files", "*" ),
    //     //     };
    //     //     var result = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);
    //     //     WriteResult(result);
    //     //     if (result.Length > 0)
    //     //     {
    //     //         Debug.Log("Success");

    //     //         SetupVideo(4, result[0]);
    //     //     }
    //     // }

    //     GUILayout.EndVertical();
    //     GUILayout.Space(20);
    //     GUILayout.Label(_path);
    //     GUILayout.EndHorizontal();
    // }
    // public void WriteResult(string path)
    // {
    //     _path = path + "\n";
    // }
    // public void WriteResult(string[] paths)
    // {
    //     if (paths.Length == 0)
    //     {
    //         return;
    //     }

    //     _path = "";
    //     foreach (var p in paths)
    //     {
    //         _path += p + "\n";
    //     }
    // }

    void SetupVideo(int index, string filePath)
    {
        videoPlayers[index].url = filePath;
        videoPlayers[index].Prepare();
        videoPlayers[index].loopPointReached += delegate
        {

        };
        videoPlayers[index].prepareCompleted += delegate
        {
            Debug.Log($"Get video size: {videoPlayers[index].texture.width}x{videoPlayers[index].texture.height}");

        };

        if (!imidiatePlay)
            return;

        videoPlayers[index].Play();
    }

    void SyncPlay()
    {
        if (videoPlayers[0].isPrepared && videoPlayers[1].isPrepared)
        {
            videoPlayers[0].Play();
            videoPlayers[1].Play();

            if(TXT_Log) TXT_Log.text = "Success";
        }
        else
        {
            string log = "not all video prepared!";
            Debug.Log(log);
            if(TXT_Log) TXT_Log.text = log;
        }
    }
}
