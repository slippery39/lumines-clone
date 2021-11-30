using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SkinTransitionController : MonoBehaviour
{

    public Material transitionMaterial;
    public float time = 0;


    [SerializeField]
    private Boolean IsEnabled;

    [SerializeField]
    private ForwardRendererData rendererData;
    [SerializeField]
    private string featureName;

    [SerializeField]
    private bool transitioning;

    [SerializeField] Camera newCamera;
    [SerializeField] Camera oldCamera;

    [SerializeField] GameUI duplicateUI;

    // Update is called once per frame
    void Update()
    {
        
        time = (Conductor.Instance.TimeLinePosition); //We need to get the timeline position.

        if (ShouldStartTransition())
        {
            StartTransition();
        }
        if (transitioning)
        {
            if (ShouldEndTransition())
            {
                ResetTransition();
                EndTransition();
            }
            else
            {
                UpdateTransition();
            }
        }
    }

    bool TryGetFeature(out ScriptableRendererFeature feature)
    {
        //This is true so it is working.
        feature = rendererData.rendererFeatures.Where((f) => f.name == featureName).FirstOrDefault();
        return feature != null;

    }

    private bool ShouldStartTransition() => transitioning == false && Input.GetKeyDown(KeyCode.C);


    //Temporary Hack to make it close to the value. We can't just say <=1 because the TimeLinePosition might not ever get to exactly 1 (due to overflow).
    //Ideally we would be able to ask the Conductor if the timeline has just finished a loop in which case we would end the transition.
    private bool ShouldEndTransition() => Conductor.Instance.TimeLinePosition>=0.98;

    private void StartTransition()
    {
        if (TryGetFeature(out var feature))
        {
            feature.SetActive(true);
            transitioning = true;

            //Disable the camera's with the render textures
            oldCamera.gameObject.SetActive(true);
            newCamera.gameObject.SetActive(true);
            //Disable the Duplicate UI for the new skin

            duplicateUI.gameObject.SetActive(true);
        }
    }

    //When do we set the feature active?

    private void UpdateTransition()
    {
        if (TryGetFeature(out var feature))
        {
            var blitFeature = feature as MyBlitFeature;
            var material = blitFeature.Material;
            //What is the normalized timeline position;
            material.SetFloat("_Position", time);
        }
    }

    private void EndTransition()
    {
        if (TryGetFeature(out var feature))
        {
            feature.SetActive(false);
            rendererData.SetDirty();
            transitioning = false;

            //Disable the camera's with the render textures
            oldCamera.gameObject.SetActive(false);
            newCamera.gameObject.SetActive(false);
            //Disable the Duplicate UI for the new skin
            duplicateUI.gameObject.SetActive(false);

        }
    }

    private void ResetTransition()
    {
        if (TryGetFeature(out var feature))
        {
            feature.SetActive(false);            
            rendererData.SetDirty();
            var blitFeature = feature as MyBlitFeature;
            var material = blitFeature.Material;
            material.SetFloat("_Position", 0);
            time = 0;
            transitioning = false;


            
        }
    }
}