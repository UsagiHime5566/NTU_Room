using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliEnterEvent : MonoBehaviour
{
    public int index;
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player"){
            ManagerSelect.instance.AddPersion(index);
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Player"){
            ManagerSelect.instance.ReducePersion(index);
        }
    }
}
