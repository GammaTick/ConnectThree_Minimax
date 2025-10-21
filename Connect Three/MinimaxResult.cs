using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect_Three
{
    public struct MinimaxResult
    {
        public int Score;
        public int Column;

        public MinimaxResult(int score, int column)
        {
            Score = score;
            Column = column;
        }
    }
}
