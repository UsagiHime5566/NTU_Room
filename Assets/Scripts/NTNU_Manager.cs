using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TriLibCore.SFB;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class NTNU_Manager : MonoBehaviour
{
    public static NTNU_Manager instance;
    void Awake()
    {
        instance = this;
    }

    public Button BTN_Wall;
    public Button BTN_Floor;
    public Button BTN_WallBack;
    public Button BTN_Sound;
    public Button BTN_SyncPlay;
    public Button BTN_ClearSound;

    public Text TXT_Wall;
    public Text TXT_Floor;
    public Text TXT_WallBack;
    public Text TXT_Sound;
    public Text TXT_Log;

    public Toggle TG_Mute;

    public List<VideoPlayer> videoPlayers;
    public AudioSource audioSource;

    [Header("Auto Work")]
    public bool imidiatePlay = false;
    int PlayMode = 0;
    string _path;

    void Start()
    {
        BTN_Wall.onClick.AddListener(() =>
        {
            LoadVideo(0, TXT_Wall);
            //Debug.Log("click event");
        });
        BTN_Floor.onClick.AddListener(() =>
        {
            LoadVideo(1, TXT_Floor);
        });
        BTN_WallBack.onClick.AddListener(() =>
        {
            LoadVideo(2, TXT_WallBack);
        });
        BTN_Sound.onClick.AddListener(() =>
        {
            LoadSound(TXT_Sound);
        });

        BTN_ClearSound.onClick.AddListener(() =>
        {
            audioSource.clip = null;
            SystemConfig.Instance.SaveData("SoundNU", "");
            TXT_Sound.text = "null";
        });

        BTN_SyncPlay.onClick.AddListener(SyncPlay);

        TG_Mute.onValueChanged.AddListener((x) =>
        {
            SystemConfig.Instance.SaveData("MuteNU", x);
            MuteVideo(x);
        });

        LoadLastSetting();
    }

    void LoadVideo(int index, Text txt)
    {   
        var extensions = new[] {
            new ExtensionFilter("Support Video Files", "mp4"),
            new ExtensionFilter("All Files", "*" ),
        };
        var result = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);
        if (result != null)
        {
            var hasFiles = result.Count > 0 && result[0].HasData;

            if (hasFiles)
            {
                string filePath = result[0].Name;

                if (txt) txt.text = filePath;
                SystemConfig.Instance.SaveData("VideoNU" + index, filePath);
                SetupVideo(index, filePath);
                Debug.Log("Success");
            }
        }
    }
    void LoadSound(Text txt)
    {
        var extensions = new[] {
            new ExtensionFilter("Support Sound Files", "mp3"),
            new ExtensionFilter("All Files", "*" ),
        };
        var result = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);
        if (result != null)
        {
            var hasFiles = result.Count > 0 && result[0].HasData;

            if (hasFiles)
            {
                string filePath = result[0].Name;

                if (txt) txt.text = filePath;
                SystemConfig.Instance.SaveData("SoundNU", filePath);
                StartCoroutine(GetAudioClip(filePath));
                Debug.Log("Success");
            }
        }
    }

    void LoadLastSetting()
    {
        string path = SystemConfig.Instance.GetData<string>("VideoNU0", "");
        if (!string.IsNullOrEmpty(path))
        {
            if (TXT_Wall) TXT_Wall.text = path;
            SetupVideo(0, path);
        }

        path = SystemConfig.Instance.GetData<string>("VideoNU1", "");
        if (!string.IsNullOrEmpty(path))
        {
            if (TXT_Floor) TXT_Floor.text = path;
            SetupVideo(1, path);
        }

        path = SystemConfig.Instance.GetData<string>("VideoNU2", "");
        if (!string.IsNullOrEmpty(path))
        {
            if (TXT_WallBack) TXT_WallBack.text = path;
            SetupVideo(2, path);
        }

        path = SystemConfig.Instance.GetData<string>("SoundNU", "");
        if (!string.IsNullOrEmpty(path))
        {
            if (TXT_Sound) TXT_Sound.text = path;
            StartCoroutine(GetAudioClip(path));
        }

        TG_Mute.isOn = SystemConfig.Instance.GetData<bool>("MuteNU", false);
    }

    void MuteVideo(bool val)
    {
        foreach (var vp in videoPlayers)
        {
            vp.SetDirectAudioMute(0, val);
        }
    }

    // private IEnumerator ConvertFilesToAudioClip(string songName)
    // {
    //     string url = string.Format("file://{0}", songName);
    //     // WWW www = new WWW(url);
    //     // yield return www;
    //     // songs.Add(www.GetAudioClip(false,false));

    //     using (UnityWebRequest web = UnityWebRequestMultimedia.GetAudioClip(url))
    //     {
    //         yield return web.SendWebRequest();
    //         if(!web.isNetworkError && !web.isHttpError)
    //         {
    //             var clip = DownloadHandlerAudioClip.GetContent(web);
    //             if(clip != null)
    //             {
    //                 songs.Add(clip);
    //             }
    //         }
    //     }
    // }

    IEnumerator GetAudioClip(string filePath)
    {
        string url = string.Format("file://{0}", filePath);
        UnityWebRequest webRequest = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG);

        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(webRequest.error);
        }
        else
        {
            AudioClip clip = DownloadHandlerAudioClip.GetContent(webRequest);
            clip.name = "tempAudio";
            //audioClips.Add(clip);
            audioSource.clip = clip;
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

    public void SetPlayMode(int val)
    {
        PlayMode = val;
        Walls.instance.SetPlayMode(val);
    }

    async void SyncPlay()
    {
        if (PlayMode == 0)
        {
            if (videoPlayers[0].isPrepared && videoPlayers[1].isPrepared && videoPlayers[2].isPrepared)
            {
                videoPlayers[0].Stop();
                videoPlayers[1].Stop();
                videoPlayers[2].Stop();
                audioSource.Stop();

                await Task.Delay(500);
                if (this == null) return;

                videoPlayers[0].Play();
                videoPlayers[1].Play();
                videoPlayers[2].Play();

                if (audioSource.clip != null)
                    audioSource.Play();

                if (TXT_Log) TXT_Log.text = "Success";
            }
            else
            {
                string log = "not all video prepared!";
                Debug.Log(log);
                if (TXT_Log) TXT_Log.text = log;
            }
        }
        // else
        // {
        //     if (videoPlayers[1].isPrepared && videoPlayersWalls[0].isPrepared && videoPlayersWalls[1].isPrepared && videoPlayersWalls[2].isPrepared)
        //     {
        //         videoPlayers[0].Stop();
        //         videoPlayers[1].Stop();
        //         videoPlayersWalls[0].Stop();
        //         videoPlayersWalls[1].Stop();
        //         videoPlayersWalls[2].Stop();
        //         audioSource.Stop();

        //         await Task.Delay(500);
        //         if (this == null) return;

        //         videoPlayers[1].Play();
        //         videoPlayersWalls[0].Play();
        //         videoPlayersWalls[1].Play();
        //         videoPlayersWalls[2].Play();

        //         if (audioSource.clip != null)
        //             audioSource.Play();

        //         if (TXT_Log) TXT_Log.text = "Success";
        //     }
        //     else
        //     {
        //         string log = "not all video prepared!";
        //         Debug.Log(log);
        //         if (TXT_Log) TXT_Log.text = log;
        //     }
        // }
    }
}
