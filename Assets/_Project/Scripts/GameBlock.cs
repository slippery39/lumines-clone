using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBlock : MonoBehaviour
{
    [SerializeField]
    GameBlockPiece topLeftPiece;
    [SerializeField]
    GameBlockPiece topRightPiece;
    [SerializeField]
    GameBlockPiece bottomLeftPiece;
    [SerializeField]
    GameBlockPiece bottomRightPiece;



    private void Awake()
    {
        if (bottomLeftPiece == null || bottomRightPiece == null || topLeftPiece == null || topRightPiece == null)
        {
            throw new System.Exception("All pieces must be defined in a GameBlock");
        }

    }

    public void SetColors(int[] colorCodes)
    {
        if (colorCodes == null)
        {
            throw new System.Exception("Null color codes found in call to SetColors for the GameBlock");
        }

        if (colorCodes.Length != 4)
        {
            throw new System.Exception("Invalid length for color codes parameter. Length was " + colorCodes.Length + ". Was expecting 4");
        }

        topLeftPiece.GetComponent<GameBlockPiece>().BlockType = (BlockTypes)colorCodes[0];
        topRightPiece.GetComponent<GameBlockPiece>().BlockType = (BlockTypes)colorCodes[1];
        bottomLeftPiece.GetComponent<GameBlockPiece>().BlockType = (BlockTypes)colorCodes[2];
        bottomRightPiece.GetComponent<GameBlockPiece>().BlockType = (BlockTypes)colorCodes[3];
        
    }

}
