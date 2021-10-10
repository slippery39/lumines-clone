using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Logic
{
    public interface IGrid<T>
    {
        int Width { get; }
        int Height { get; }
        T this[int x, int y] { get; set; }
        void ForEach(Action<int, int, T> func);
        void ClearAll();
        void Delete(int x, int y);

        //Temporary while we get all the code working with this interface
        T[,] To2dArray();
    }
}
