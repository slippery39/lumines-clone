using Game_Logic;
using System;
using System.Collections.Generic;
using UnityEngine;


//This class calculates the different scorign 
public class Scorer
{

    private int _scoreMultiplier = 1;

    public event Action<int> OnScoreAdded;
    public event Action<int> OnScoreMultiplierIncrease;
    public Scorer()
    {

    }

    private void EmitScoreAdded(int score)
    {
        if (score == 0)
            return;

        OnScoreAdded?.Invoke(score);
    }

    private void EmitScoreMultiplierIncrease(int multiplier)
    {
        OnScoreMultiplierIncrease?.Invoke(multiplier);
    }

    public int OnTimeLineEnd(GameEventInfo gameState)
    {

        Debug.Log("TimeLineEnd called in the scorer");

        Dictionary<int, int> scoreMap = new Dictionary<int, int>();

        int score = 0;

        if (gameState.SquaresDeletedThisTurn <= 3)
        {
            score = _scoreMultiplier * gameState.SquaresDeletedThisTurn * 40;
            _scoreMultiplier = 1;
        }
        else
        {
            _scoreMultiplier *= 2;
            EmitScoreMultiplierIncrease(_scoreMultiplier);
            score = _scoreMultiplier * gameState.SquaresDeletedThisTurn * 160;           
        }
        EmitScoreAdded(score);
        return score;
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
            EmitScoreAdded(singleColorBonus);
            return singleColorBonus;
        }
        //All Deleted Bonus
        if (colorCount[1] == 0 && colorCount[2]== 0)
        {
            EmitScoreAdded(allDeletedBonus);
            return allDeletedBonus;
        }

        return 0;
    }

    public int OnSoftDrop(GameEventInfo gameState)
    {
        return 1;
    }
}