using Game_Logic;


//This class calculates the different scorign 
public class Scorer
{
    public Scorer()
    {

    }

    public int OnTimeLineEnd(GameEventInfo gameState)
    {
        return 1;
    }

    public int OnBlockDeleted(GameEventInfo gameState)
    {
        return 1;
    }

    public int OnSoftDrop(GameEventInfo gameState)
    {
        return 1;
    }
}