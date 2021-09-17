using DG.Tweening;
using GameLogic;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NextBlocks : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private GameBlock extraBlock;

    [SerializeField]
    private List<GameBlock> instantiatedBlocks = new List<GameBlock>();

    [SerializeField]
    private GameObject blocksContainer;

    [SerializeField]
    private List<MoveableBlock> nextBlocksTemp = new List<MoveableBlock>();

    private List<Vector3> initialPositions;
    private Vector3 initialExtraBlockPos;
    private float distanceBetweenBlocks;
    private Vector3 initialContainerPosition;


    // Update is called once per frame

    void Start()
    {
        initialPositions = instantiatedBlocks.Select(bl => bl.transform.position).ToList();

        if (extraBlock == null)
        {
            throw new System.Exception("Extra block is not instantiated in our next blocks component");
        }

        initialExtraBlockPos = extraBlock.transform.position;
        distanceBetweenBlocks = (instantiatedBlocks[1].transform.position - instantiatedBlocks[2].transform.position).y;
        initialContainerPosition = blocksContainer.transform.position;
    }

    void Update()
    {        
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Key down f");
            StartCoroutine("AnimateUp");
            Debug.Log("animate up done?");
        }
    }

    //Used when the user places a block.
    IEnumerator ScrollUpAnimation()
    {
        var animSeq = DOTween.Sequence();
        animSeq.Append(blocksContainer.transform.DOMove(blocksContainer.transform.position + new Vector3(0, distanceBetweenBlocks, 0),0.1f));
        yield return animSeq.WaitForCompletion();

        ResetContainerPosition();
        SetNextBlocks(nextBlocksTemp);
        //Reset all the things.
    }


    public void UpdateNextBlocks(List<MoveableBlock> nextBlocks)
    {
        //Don't update right away. 
        //Set the extra block to the last of the next blocks.

        if (nextBlocks.Count<3)
        {
            throw new System.Exception("Next Blocks should always have at least 3 blocks upcoming");
        }

        extraBlock.SetColors(nextBlocks[2].Data);
        nextBlocksTemp = nextBlocks.ToList();
        StartCoroutine("AnimateUp");
    }



    public void SetNextBlocks(List<MoveableBlock> nextBlocks)
    {

        for (var i = 0; i < nextBlocks.Count; i++)
        {
            var blockInfo = nextBlocks[i];
            instantiatedBlocks[i].SetColors(blockInfo.Data);
        }
    }

    private void ResetContainerPosition()
    {
        blocksContainer.transform.position = initialContainerPosition;
    }

    private void ResetBlockPositions()
    {

        for (var i = 0; i < instantiatedBlocks.Count; i++)
        {
            var block = instantiatedBlocks[i];
            block.transform.position = initialPositions[i];
        }
    }

  

}
