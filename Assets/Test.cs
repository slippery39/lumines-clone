using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Game;
using GameBoard = Assets.Game.GameBoard;

public class Test : MonoBehaviour { 

    public GameBoard grid;

    // Start is called before the first frame update
    void Start()
    {
        grid = new GameBoard(16, 10);


    }

}
