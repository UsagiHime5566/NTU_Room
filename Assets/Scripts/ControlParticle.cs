using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlParticle : MonoBehaviour
{
    public Toggle TG_Particle;
    public List<GameObject> particles;
    void Start()
    {
        TG_Particle.onValueChanged.AddListener((x) =>
        {
            foreach (var item in particles)
            {
                item.SetActive(x);
            }
        });
    }
}
