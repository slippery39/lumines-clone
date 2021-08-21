using GameLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextBlocks : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private List<MoveableBlock> nextBlocks = new List<MoveableBlock>();

    private List<GameBlock> instantiatedBlocks = new List<GameBlock>();

    [SerializeField]
    private GameBlock gameBlockPrefab;

    private int blocksToShow = 3;

    void Start()
    {

        for (var i = 0; i < blocksToShow; i++)
        {
            var gameBlock = Instantiate(gameBlockPrefab);
            gameBlock.transform.SetParent(this.transform);
            gameBlock.transform.localPosition = new Vector3(0, i*-2.2f, 0);
            instantiatedBlocks.Add(gameBlock);
        }
    }

    // Update is called once per frame
    void Update()
    {        

        for (var i = 0; i < blocksToShow; i++)
        {
            var blockInfo = nextBlocks[i];
            instantiatedBlocks[i].SetColors(blockInfo.Data);
        }
    }
}
