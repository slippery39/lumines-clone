using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ILuminesGameUpdateable
{
    //This method is for anything that needs to be updated while the game is running.
    //Most game logic for example should go into here.
    void LuminesGameUpdate();
}

