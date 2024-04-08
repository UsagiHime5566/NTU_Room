using UnityEngine;
using UnityEngine.Video;
using System.Collections;
using System.Collections.Generic;


namespace com.rfilkov.components
{
    public class VideoPresentationScript_v9 : MonoBehaviour
    {

        public VideoPlayer videoPlayerBack_1;
        public VideoPlayer videoPlayerBack_2;
        public VideoPlayer videoPlayerLeft;
        public VideoPlayer videoPlayerRight;

        public AudioSource audioSource1;
        public AudioSource audioSource2;

        void Start()
        {


        }


        void Update()
        {

            
        }

        


        void PlayVideo(VideoPlayer videoPlayer, VideoPlayer videoPlayer2, AudioSource audioSource)
        {
            if (videoPlayer.isPlaying)
                return;


            videoPlayer.Play();
            videoPlayer2.Play();
            audioSource.Play();
        }


        void PauseVideo(VideoPlayer videoPlayer, VideoPlayer videoPlayer2, AudioSource audioSource)
        {
            if (!videoPlayer.isPlaying)
                return;


            videoPlayer.Pause();
            videoPlayer2.Pause();
            audioSource.Pause();
        }


        void ChangeVideoSpeed(int videoNumber, float speed)
        {
            // 根據 videoNumber 改變相應的 VideoPlayer 的速度
            if (videoNumber == 1)
            {
                videoPlayerBack_1.playbackSpeed = speed;
                videoPlayerLeft.playbackSpeed = speed;
            }
            else if (videoNumber == 2)
            {
                videoPlayerBack_2.playbackSpeed = speed;
                videoPlayerRight.playbackSpeed = speed;
            }
        }


        void ClearUserInformation()
        {
            // 停止影片播放等相關的操作
            PauseVideo(videoPlayerBack_1, videoPlayerLeft, audioSource1);
            PauseVideo(videoPlayerBack_2, videoPlayerRight, audioSource2);
        }


        void ResetVideosToStart()
        {
            // 重置影片到開頭
            videoPlayerBack_1.time = 0f;
            videoPlayerBack_2.time = 0f;
            videoPlayerLeft.time = 0f;
            videoPlayerRight.time = 0f;
        }
    }
}


