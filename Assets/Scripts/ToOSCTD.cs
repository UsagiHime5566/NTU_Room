using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToOSCTD : MonoBehaviour
{
    public Transform HeadTrans;
    public Transform RightHandTrans;

    public float x_AreaMax = 5.25f;
    public float x_AreaMin = -4.5f;
    public float y_AreaMax = 2f;
    public float y_AreaMin = 0;

    // public float x_amp = 1.0f;
    // public float y_amp = 1.0f;
    // public float y_delta = 0.25f;

    public Vector2 GetHandPos => HandPos();

    Vector2 HandPos(){
        var x = Map(x_AreaMin, x_AreaMax, RightHandTrans.position.x);
        var y = Map(y_AreaMin, y_AreaMax, RightHandTrans.position.y);

        //x = x * x_amp;
        //y = (y - y_delta)*y_amp;

        x = Mathf.Lerp(1, -1, x);
        y = Mathf.Lerp(-0.5f, 0.5f, y);

        return new Vector2(x, y);
    }

    float Map(float a, float b, float input){
        Mathf.Clamp(input, a, b);
        return (input - a)/(b - a);
    }
}
