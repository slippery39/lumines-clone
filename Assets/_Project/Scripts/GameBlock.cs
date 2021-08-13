using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBlock : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameBlockPiece blockPiecePrefab;

    GameBlockPiece bottomLeftPiece;
    GameBlockPiece bottomRightPiece;
    GameBlockPiece topLeftPiece;
    GameBlockPiece topRightPiece;


    private void Awake()
    {
        if (blockPiecePrefab == null)
        {
            Debug.LogError("Could not find Block Piece Prefab for the GameBlock object");
        }

        bottomLeftPiece = Instantiate(blockPiecePrefab);
        bottomLeftPiece.transform.parent = this.transform;
        bottomLeftPiece.transform.localPosition = new Vector3(0, 0, 0);
        bottomLeftPiece.GetComponent<GameBlockPiece>().BlockType = 1;

        //use the first one as the width to use.
        var pieceSize = bottomLeftPiece.GetComponent<Collider>().bounds.size;
        //temporary fix to put have spaces. 
        var xSize = 1;
        var ySize = 1;


        bottomRightPiece = Instantiate(blockPiecePrefab);
        bottomRightPiece.transform.parent = this.transform;
        bottomRightPiece.transform.localPosition = new Vector3(xSize, 0, 0);
        bottomRightPiece.GetComponent<GameBlockPiece>().BlockType = 2;

        topLeftPiece = Instantiate(blockPiecePrefab);
        topLeftPiece.transform.parent = this.transform;
        topLeftPiece.transform.localPosition = new Vector3(0, ySize, 0);
        topLeftPiece.GetComponent<GameBlockPiece>().BlockType = 1;

        topRightPiece = Instantiate(blockPiecePrefab);
        topRightPiece.transform.parent = this.transform;
        topRightPiece.transform.localPosition = new Vector3(xSize,ySize, 0);
        topRightPiece.GetComponent<GameBlockPiece>().BlockType = 2;
    }

    public void SetColors(int[] colorCodes)
    {
        if (colorCodes == null)
        {
            throw new System.Exception("Null color codes found in call to SetColors for the GameBlock");
        }

        topLeftPiece.GetComponent<GameBlockPiece>().BlockType = colorCodes[0];
        topRightPiece.GetComponent<GameBlockPiece>().BlockType = colorCodes[1];
        bottomLeftPiece.GetComponent<GameBlockPiece>().BlockType = colorCodes[2];
        bottomRightPiece.GetComponent<GameBlockPiece>().BlockType = colorCodes[3];
        
    }

}
