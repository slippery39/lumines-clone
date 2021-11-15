using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoypeTransition : MonoBehaviour
{

    public Material transitionMaterial;
    public float time = 0;
    // Update is called once per frame
    void Update()
    {
        time = Conductor.Instance.SongPosition; //We need to get the timeline position.
        transitionMaterial.SetFloat("_Position", Mathf.Repeat(time, 1));
    }
}