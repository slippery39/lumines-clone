using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Logic
{
    public class Grid<T> : IGrid<T>
    {
        public T[,] _internalArr;

        public int Width { get;}
        public int Height { get;}

        private Func<int,int,T> _defaultValueFunc;


        public Grid(int width,int height,Func<int,int,T> defaultValueFunc)
        {
            Width = width;
            Height = height;
            _defaultValueFunc = defaultValueFunc;
            _internalArr = new T[width, height];

            //Initialize the default values.
            ForEach((x, y,val) =>
            {
                defaultValueFunc(x, y);
            });
        }

        public T this[int x, int y]
        {
            get { return _internalArr[x, y]; }
            set { _internalArr[x, y] = value; }
        }


        public void ClearAll()
        {
            ForEach((x, y, val) =>
            {
                Delete(x, y);
            });
        }

        public void Delete(int x, int y)
        {
            _internalArr[x, y] = _defaultValueFunc(x, y);
        }

        public void ForEach(Action<int, int, T> func)
        {
           for (int x = 0; x < Width; x++)
            {
                for (int y=0;y<Height; y++)
                {
                    func(x, y, _internalArr[x, y]);
                }
            }
        }

        public T[,] To2dArray()
        {
            var newArr = new T[Width, Height];

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    newArr[x, y] = _internalArr[x, y];
                }
            }

            return newArr;
        }
    }

}
