using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class ManagerSelect : HimeLib.SingletonMono<ManagerSelect>
{
    public TextMeshPro textLeft;
    public TextMeshPro textRight;
    public BoxCollider ColliLeft;
    public BoxCollider ColliRight;
    public List<VideoPlayer> LeftPlayer;
    public List<VideoPlayer> RightPlayer;
    public float loopTime = 0.2f;

    [SerializeField] List<int> persons = new List<int>(2);
    void Start()
    {
        persons[0] = 3;
        persons[1] = 3;

        StartCoroutine(LoopPlayer());
    }

    public void AddPersion(int index){
        persons[index] += 1;
        RefreshText();
    }
    public void ReducePersion(int index){
        persons[index] -= 1;
        RefreshText();
    }

    public void RefreshText(){
        textLeft.text = persons[0].ToString();
        textRight.text = persons[1].ToString();
    }

    IEnumerator LoopPlayer(){
        WaitForSeconds wait = new WaitForSeconds(loopTime);
        while (true)
        {
            yield return wait;

            if(persons[0] == persons[1]){
                foreach (var vp in LeftPlayer)
                {
                    vp.Pause();
                }
                foreach (var vp in RightPlayer)
                {
                    vp.Pause();
                }
                continue;
            }

            if(persons[0] > persons[1]){
                foreach (var vp in LeftPlayer)
                {
                    vp.Play();
                }
                foreach (var vp in RightPlayer)
                {
                    vp.Pause();
                }
                continue;
            }

            if(persons[0] < persons[1]){
                foreach (var vp in LeftPlayer)
                {
                    vp.Pause();
                }
                foreach (var vp in RightPlayer)
                {
                    vp.Play();
                }
                continue;
            }
        }
    }
}
