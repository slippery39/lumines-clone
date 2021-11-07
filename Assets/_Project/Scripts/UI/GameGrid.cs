using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour, ISyncToConductor
{
    // Start is called before the first frame update
    [SerializeField]
    private Renderer _gridRenderer;

    void Start()
    {
        _gridRenderer = GetComponent<Renderer>();

        if (_gridRenderer == null)
        {
            throw new System.Exception("Could not find the grid renderer on the grid game object");
        }
    }

    // Update is called once per frame
    void Update()
    {
        GridFlashEffect();
    }

    private void GridFlashEffect()
    {
        if (_gridRenderer == null)
        {
            return;
        }
        //testing the BPM setting on our material;
        _gridRenderer.material.SetFloat("_SongPositionInBeats", Conductor.Instance.SongPositionInBeats);
    }

    //This is not in use right now, but perhaps we should use this to make it easier for us to 
    public void ConductorUpdate(ConductorInfo info)
    {
        GridFlashEffect();
    }
}
