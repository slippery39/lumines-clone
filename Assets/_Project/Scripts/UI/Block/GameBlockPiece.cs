using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBlockPiece : MonoBehaviour
{

    [SerializeField]
    BlockTypes blockType = BlockTypes.Nothing;

    [SerializeField]
    private Material material1;
    public Material Material1 { get => material1; set => material1 = value; }

    [SerializeField]
    private Material material2;
    public Material Material2 { get => material2; set => material2 = value; }

    [SerializeField]
    Material deletionMaterial;
    public BlockTypes BlockType
    {
        get { return blockType; }
        set { blockType = value; }
    }

    private Renderer rendererComponent;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        if (material1 == null || material2 == null)
        {
            throw new System.Exception("Make sure both materials are set for a block");
        }

        rendererComponent = this.GetComponent<Renderer>();
        meshRenderer = this.GetComponent<MeshRenderer>();
    }


    //I'm calling this in both Update() and LateUpdate() due to some visual errors that happen.
    private void UpdateStuff()
    {
        //The ZScale is very important here, it needs to be small enough that our square blocks show above these when needed.
        //TODO - this should not be going on here?
            this.transform.localScale = new Vector3(0.9f, 0.9f, 0.01f);

        if (blockType == BlockTypes.Nothing) //nothing
        {
            meshRenderer.enabled = false;
        }
        else
        {
            meshRenderer.enabled = true;
        }

        if (blockType == BlockTypes.Color1) //color 1
        {
            rendererComponent.material = material1;
        }
        else if (blockType == BlockTypes.Color2) //color 2
        {
            rendererComponent.material = material2;
        }
        //Note that the marked Color blocks are technically are replaced by "Square" blocks now.
        //i.e. they will show above this one.
        else if (blockType == BlockTypes.Color1Marked) //color 1 marked
        {
            rendererComponent.material.color = Color.cyan;
        }
        else if (blockType == BlockTypes.Color2Marked) //color 2 marked
        {
            rendererComponent.material.color = Color.grey;
        }
        //Marked for deletion
        else if (blockType == BlockTypes.DeletionInProgress) //DeletionInProgress
        {
            //We want this to always appear above any square blocks so we specifically increase the scale to 0.2f to accomplish this.
            this.transform.localScale = new Vector3(1, 1, 0.2f);
            rendererComponent.material = deletionMaterial;
        }
    }

    void Update()
    {
        UpdateStuff();
    }

    //This fixes some UI glitches that might occur mid frame.
    void LateUpdate()
    {
        UpdateStuff();
    }

}


