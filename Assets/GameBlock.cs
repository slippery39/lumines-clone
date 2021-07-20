using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBlock : MonoBehaviour
{

    [SerializeField]
    int blockType = 0;

    public int BlockType
    {
        get { return blockType; }
        set { blockType = value; }
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}


