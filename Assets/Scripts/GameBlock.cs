using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBlock : MonoBehaviour
{

    [SerializeField]
    int blockType = 1;

    [SerializeField]
    Material material1;

    [SerializeField]
    Material material2;

    public int BlockType
    {
        get { return blockType; }
        set { blockType = value; }
    }

    private void Awake()
    {
       if(material1 == null || material2 == null)
        {
            throw new System.Exception("Make sure both materials are set for a block");
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (blockType == 0)
        {
            this.GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            this.GetComponent<MeshRenderer>().enabled = true;
        }
        if (blockType == 1)
        {
            this.GetComponent<Renderer>().material = material1;
        }
        else if (blockType == 2)
        {
            this.GetComponent<Renderer>().material = material2;
        }
        else if (blockType == 3)
        {
            this.GetComponent<Renderer>().material.color = Color.cyan;
        }
        else if (blockType == 4)
        {
            this.GetComponent<Renderer>().material.color = Color.grey;
        }
    }

}


