using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBlockPiece : MonoBehaviour
{

    public bool PartOfSquare { get; set; }

    [SerializeField]
    BlockTypes blockType = BlockTypes.Nothing;

    [SerializeField]
    Material material1;

    [SerializeField]
    Material material2;

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

        if (!PartOfSquare)
        {
            this.transform.localScale = new Vector3(0.95f, 0.95f, 0.1f);
        }
        else
        {
            this.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        }

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
            //hack to make the scale normal
            this.transform.localScale = new Vector3(1, 1, 0.1f);
            rendererComponent.material = deletionMaterial;
        }
    }

    void Update()
    {
        UpdateStuff();
    }

    void LateUpdate()
    {
        UpdateStuff();
    }

}


