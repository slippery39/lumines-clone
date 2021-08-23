using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBlockPiece : MonoBehaviour
{

    [SerializeField]
    int blockType = 1;

    [SerializeField]
    Material material1;

    [SerializeField]
    Material material2;

    [SerializeField]
    Material deletionMaterial;
    public int BlockType
    {
        get { return blockType; }
        set { blockType = value; }
    }


    private Renderer rendererComponent;
    private MeshRenderer meshRenderer;



    private void Awake()
    {
       if(material1 == null || material2 == null)
        {
            throw new System.Exception("Make sure both materials are set for a block");
        }

        rendererComponent = this.GetComponent<Renderer>();
        meshRenderer = this.GetComponent<MeshRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localScale = new Vector3(0.95f, 0.95f, 0.1f);

        if (blockType == 0)
        {
            meshRenderer.enabled = false;
        }
        else
        {
            meshRenderer.enabled = true;
        }

        if (blockType == 1)
        {
            rendererComponent.material = material1;
        }
        else if (blockType == 2)
        {
            rendererComponent.material = material2;
        }
        else if (blockType == 3)
        {
            rendererComponent.material.color = Color.cyan;
        }
        else if (blockType == 4)
        {
            rendererComponent.material.color = Color.grey;
        }
        //Marked for deletion
        else if (blockType == 5)
        {
            //hack to make the scale normal
            this.transform.localScale = new Vector3(1, 1, 0.1f);
            rendererComponent.material = deletionMaterial;
        }
    }

}


