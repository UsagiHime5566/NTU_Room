using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosTest : MonoBehaviour
{
    public Vector2 HandPos => DemoPos();

    public Vector2 v2;
    
    void Start()
    {
        
    }

    Vector2 DemoPos(){
        return v2;
    }
}
