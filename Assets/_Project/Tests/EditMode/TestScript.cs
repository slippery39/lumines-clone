using System.Collections;
using System.Collections.Generic;
using GameLogic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestScript
{
    // A Test behaves as an ordinary method
    [Test]
    public void TestBlocksInFreeFallNotCountedForSquare()
    {

        var game = new Game();

        //need to make a free falling block and place it next to a regular block like below.


        /*3
         * 2    [x][x][x]
         * 1    [x][x][x]
         * 0       [x][x]
         */ //  0   1  2

        //the left most x's must not be classified as marked for deletion.

        //the square.
        game.Board[1, 0] = 1;
        game.Board[2, 0] = 1;
        game.Board[1, 1] = 1;
        game.Board[2, 1] = 1;
        game.Board[1, 2] = 1;
        game.Board[2, 2] = 1;
 
        //the free falling blocks

        game.Board[0, 1] = 1;
        game.Board[0, 2] = 1;

        game.MarkDeletions();

        if (game.Deletions[0, 1] == true || game.Deletions[0, 2] == true)
        {
            Assert.Fail("Free fallilng pieces are being marked as deletion. This should not be the case");
        }
        // Use the Assert class to test conditions
    }


    [Test]
    public void TestBlocksInFreeFallNotCountedForSquare2()
    {
        var game = new Game();

        //need to make a free falling block and place it next to a regular block like below.


        /*3
         * 2    [x][x][x]
         * 1    [x][x][x]
         * 0       [x][x]
         */ //  0   1  2

        //the left most x's must not be classified as marked for deletion.

        //make a row of same color squares at the bottom

        for (var x = 0; x < 9; x++)
        {
            for (var y = 0; y < 3; y++)
            {
                game.Board[x, y] = 2;
            }
        }

        //put a tower somewhere

        for (var y = 2; y < game.Height; y++)
        {
            game.Board[5, y] = 2;
            game.Board[6, y] = 2;
        }
        

        //the free falling blocks

        //the bug is when the free falling blocks are on the right..
        game.Board[7, 6] = 2;
        game.Board[7, 7] = 2;

        game.MarkDeletions();


        if (game.Deletions[7, 6] == true || game.Deletions[7, 7] == true)
        {
            Assert.Fail("Free fallilng pieces are being marked as deletion. This should not be the case");
        }
        // Use the Assert class to test conditions
    }


    [Test]
    public void TestBlocksNotInFreeFallCountedForSquare()
    {

        var game = new Game();

        //need to make a free falling block and place it next to a regular block like below.


        /*3
         * 2       [x][x]
         * 1    [x][x][x]
         * 0    [x][x][x]
         */ //  0   1  2

        //the left most x's must not be classified as marked for deletion.

        //the square.
        game.Board[1, 0] = 1;
        game.Board[2, 0] = 1;
        game.Board[1, 1] = 1;
        game.Board[2, 1] = 1;
        game.Board[1, 2] = 1;
        game.Board[2, 2] = 1;



        //The other blocks
        game.Board[0, 0] = 1;
        game.Board[0, 1] = 1;

        game.MarkDeletions();

        if (game.Deletions[0, 0] == false || game.Deletions[0, 1] == false)
        {
            Assert.Fail("These pieces should be marked for deletion but they are not.");
        }
        // Use the Assert class to test conditions
    }



}
