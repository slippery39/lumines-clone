using Game_Logic;
using System.Collections.Generic;
using UnityEngine;


//This class calculates the different scorign 
public class Scorer
{

    private int _scoreMultiplier = 1;
    public Scorer()
    {

    }

    public int OnTimeLineEnd(GameEventInfo gameState)
    {

        Debug.Log("TimeLineEnd called in the scorer");

        Dictionary<int, int> scoreMap = new Dictionary<int, int>();

        if (gameState.SquaresDeletedThisTurn <= 3)
        {
            int score = _scoreMultiplier * gameState.SquaresDeletedThisTurn * 40;
            _scoreMultiplier = 1;

            Debug.Log("Score to be added is " + score);
            return score;      
        }

        _scoreMultiplier *= 2;

        return _scoreMultiplier * gameState.SquaresDeletedThisTurn * 160;
    }

    public int OnBlockDeleted(GameEventInfo gameState)
    {
        var colorCount = new Dictionary<int, int>();
        colorCount.Add(1, 0);
        colorCount.Add(2, 0);

        int singleColorBonus = 1000;
        int allDeletedBonus = 10000;


        for (int x = 0; x < gameState.Board.GetUpperBound(0); x++)
        {
            for (int y = 0; y < gameState.Board.GetUpperBound(1); y++)
            {
                if (gameState.Board[x, y] > 0)
                {
                    colorCount[gameState.Board[x, y]]++;
                }
            }
        }

        // Single Color Bonus
        if (colorCount[1] == 0 && colorCount[2] > 0 || colorCount[2] == 0 && colorCount[1]>0)
        {

            return singleColorBonus;
        }
        //All Deleted Bonus
        if (colorCount[1] == 0 && colorCount[2]== 0)
        {
            return allDeletedBonus;
        }

        return 0;
    }

    public int OnSoftDrop(GameEventInfo gameState)
    {
        return 1;
    }
}