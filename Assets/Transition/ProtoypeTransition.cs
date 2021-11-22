using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ProtoypeTransition : MonoBehaviour
{

    public Material transitionMaterial;
    public float time = 0;

    public ForwardRendererData rendererData;

    // Update is called once per frame
    void Update()
    {
            time = (Conductor.Instance.loopPositionInBeats / 8); //We need to get the timeline position.
            transitionMaterial.SetFloat("_Position", Mathf.Repeat(time, 1));
            
            


    }
}