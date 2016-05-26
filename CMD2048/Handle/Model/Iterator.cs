using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD2048.Handle
{
    public class Iterator
    {
        public int StartIndex { get; set; }

        public Func<int, int> GetNext { get; set; }

        public Func<int, int> GetPrevious { get; set; }

        public Func<int, int, bool> Condition { get; set; }

        public Iterator Reverse { get; set; }
    }
}
