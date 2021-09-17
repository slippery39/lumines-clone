using DG.Tweening;
using GameLogic;
using System.Collections;
using System.Collections.Generic;
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


    // Update is called once per frame

    void Update()
    {        
        if (Input.GetKeyDown(KeyCode.F))
        {
            AnimateUp();
        }
    }

    //Used when the user places a block.
    void AnimateUp()
    {
        var animSeq = DOTween.Sequence();
        blocksContainer.transform.DOMove(blocksContainer.transform.position + new Vector3(0, 2.5f, 0),0.1f);
    }

    public void SetNextBlocks(List<MoveableBlock> nextBlocks)
    {

        for (var i = 0; i < nextBlocks.Count; i++)
        {
            var blockInfo = nextBlocks[i];
            instantiatedBlocks[i].SetColors(blockInfo.Data);
        }
    }

}
