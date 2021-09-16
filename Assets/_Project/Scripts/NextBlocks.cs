using GameLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextBlocks : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    public List<MoveableBlock> nextBlocks = new List<MoveableBlock>();

    [SerializeField]
    private List<GameBlock> instantiatedBlocks = new List<GameBlock>();


    // Update is called once per frame

    void Update()
    {        

        for (var i = 0; i < nextBlocks.Count; i++)
        {
            var blockInfo = nextBlocks[i];
            instantiatedBlocks[i].SetColors(blockInfo.Data);
        }
    }
}
