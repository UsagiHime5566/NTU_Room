using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walls : MonoBehaviour
{
    public static Walls instance;
    void Awake() => instance = this;
    public List<GameObject> ColapseWalls;
    public List<GameObject> SeperateWalls;

    void Start()
    {
        SetPlayMode(0);
    }

    public void SetPlayMode(int val)
    {
        if (val == 0)
        {
            foreach (var item in ColapseWalls)
            {
                item.SetActive(true);
            }
            foreach (var item in SeperateWalls)
            {
                item.SetActive(false);
            }
        }
        else
        {
            foreach (var item in ColapseWalls)
            {
                item.SetActive(false);
            }
            foreach (var item in SeperateWalls)
            {
                item.SetActive(true);
            }
        }
    }
}
