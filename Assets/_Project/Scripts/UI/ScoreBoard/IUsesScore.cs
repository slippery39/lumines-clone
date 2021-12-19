using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface IUsesScore
{
    public float CurrentTime { get; set; }
    public int BlocksErased { get; set; }
    public int Score { get; set; }
    public void OnScoreAdded(int amount);
}

