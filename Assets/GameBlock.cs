using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBlock : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameBlockPiece blockPiecePrefab;

    GameBlockPiece topLeftPiece;
    GameBlockPiece topRightPiece;
    GameBlockPiece bottomLeftPiece;
    GameBlockPiece bottomRightPiece;


    private void Awake()
    {
        if (blockPiecePrefab == null)
        {
            Debug.LogError("Could not find Block Piece Prefab for the GameBlock object");
        }

        topLeftPiece = Instantiate(blockPiecePrefab);
        topLeftPiece.transform.parent = this.transform;
        topLeftPiece.transform.localPosition = new Vector3(0, 0, 0);
        topLeftPiece.GetComponent<GameBlockPiece>().BlockType = 1;

        //use the first one as the width to use.
        var pieceSize = topLeftPiece.GetComponent<Collider>().bounds.size;


        topRightPiece = Instantiate(blockPiecePrefab);
        topRightPiece.transform.parent = this.transform;
        topRightPiece.transform.localPosition = new Vector3(pieceSize.x, 0, 0);
        topRightPiece.GetComponent<GameBlockPiece>().BlockType = 2;

        bottomLeftPiece = Instantiate(blockPiecePrefab);
        bottomLeftPiece.transform.parent = this.transform;
        bottomLeftPiece.transform.localPosition = new Vector3(0, pieceSize.y, 0);
        bottomLeftPiece.GetComponent<GameBlockPiece>().BlockType = 1;

        bottomRightPiece = Instantiate(blockPiecePrefab);
        bottomRightPiece.transform.parent = this.transform;
        bottomRightPiece.transform.localPosition = new Vector3(pieceSize.x, pieceSize.y, 0);
        bottomRightPiece.GetComponent<GameBlockPiece>().BlockType = 2;
    }

}
