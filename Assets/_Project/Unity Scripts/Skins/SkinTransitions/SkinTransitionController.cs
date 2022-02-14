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
    private bool enableTransitions = false;

    [SerializeField]
    private int timeToNextSkin = 90;

    [SerializeField]
    private ForwardRendererData rendererData;
    [SerializeField]
    private string featureName;

    [SerializeField]
    private bool transitioning;

    [SerializeField] private Camera newCamera;
    [SerializeField] private Camera oldCamera;

    [SerializeField] GameUI gameUI;

    [SerializeField] private GameUI duplicateUI;

    [SerializeField] private Skin nextSkin;


    private void Start()
    {
        DefaultSetup();


        Conductor.Instance.OnLoopBegin += (ConductorInfo info) =>
        {
            if (transitioning)
            {
                EndTransition();
            }
            else
            {
                if (info.SongPositionInSeconds >=timeToNextSkin)
                {
                    if (enableTransitions)
                    {
                        StartTransition();
                    }
                    else
                    {
                        SetNewSkin();
                    }
                }
            }
        };


    }

    // Update is called once per frame
    void Update()
    {

        time = Conductor.Instance.TimeLinePosition; //We need to get the timeline position.


        if (transitioning)
        {
            UpdateTransition();
        }
    }

    private void DefaultSetup()
    {
        if (TryGetFeature(out var feature))
        {
            feature.SetActive(false);
        }
    }

    bool TryGetFeature(out ScriptableRendererFeature feature)
    {
        //This is true so it is working.
        feature = rendererData.rendererFeatures.Where((f) => f.name == featureName).FirstOrDefault();
        return feature != null;

    }


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

    private void SetNewSkin()
    {
        gameUI.SetSkin(nextSkin);
        nextSkin = Skins.GetNext(nextSkin.Name);
    }

    private void EndTransition()
    {
        transitioning = false;
        if (TryGetFeature(out var feature))
        {


            feature.SetActive(false);
            rendererData.SetDirty();
           

            var blitFeature = feature as MyBlitFeature;
            var material = blitFeature.Material;
            material.SetFloat("_Position", 0);


            //Disable the camera's with the render textures
            oldCamera.gameObject.SetActive(false);
            newCamera.gameObject.SetActive(false);



            //Update the skin to the new skin.
            //Go to the next skin
            var newNextSkin = Skins.GetNext(nextSkin.Name);

            Debug.Log("new skin in transition controller");

            Debug.Log(newNextSkin.Name);
            


            //Calls to set skin come from here
            gameUI.SetSkin(nextSkin);
            nextSkin = newNextSkin;

            //and here.
            duplicateUI.SetSkin(newNextSkin);
            //Maybe we hae to do this?
            //Disable the Duplicate UI for the new skin
            //duplicateUI.gameObject.SetActive(false);


        }
    }
}