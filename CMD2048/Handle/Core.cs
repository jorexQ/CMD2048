using CMD2048.Handle.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD2048.Handle
{
    public class Core
    {
        public List<Cell> _cells = new List<Cell>();

        private readonly int _x;
        private readonly int _y;

        public Core(int x, int y)
        {
            _x = x;
            _y = y;
            GetCellList();
            SetRandom(2);
        }

        public Action GetHandleByDirection(Direction direction)
        {
            var cellListGroup = GetCellsListGroup(direction);
            var iterator = GetIterator(cellListGroup, direction);
            var diretionAction = new DiretionAction(cellListGroup, iterator);
            return () =>
            {
                if (diretionAction.Run())
                {
                    SetRandom();
                }
            };
        }

        public void SetRandom(int count = 1)
        {           
            var random = new Random();
            for (var i = 0; i < count; i++)
            {
                var emptyList = _cells.Where(o => o.Value == 0).ToList();
                var randomPosition = random.Next(0, emptyList.Count);
                var randomValue = random.Next(0, 6) == 4 ? 4 : 2;
                emptyList[randomPosition].Value = randomValue;
            }
        }

        public int[,] GetImage()
        {
            var image = new int[_x, _y];
            foreach(var cell in _cells)
            {
                image[cell.IndexX, cell.IndexY] = cell.Value;
            }
            return image;
        }

        private void GetCellList()
        {
            for(var y = 0; y < _y; y++)
            {
                for (var x= 0; x < _x; x++)
                {
                    _cells.Add(new Cell
                    {
                        IndexX = x,
                        IndexY = y
                    });
                }
            }
        }

        private Iterator GetIterator(List<List<Cell>> cellListGroup, Direction direction)
        {
            var iteratorOrderAsc = new Iterator
            {
                StartIndex = 0,
                GetNext = i => ++i,
                GetPrevious = i => --i,
                Condition = (i, length) => i < length
            };
            var iteratorOrderDesc = new Iterator
            {
                StartIndex = cellListGroup.Count - 1,
                GetNext = i => --i,
                GetPrevious = i => ++i,
                Condition = (i, length) => i >= 0,
                Reverse = iteratorOrderAsc
            };
            iteratorOrderAsc.Reverse = iteratorOrderDesc;
            if (direction == Direction.Up || direction == Direction.Right)
            {
                return iteratorOrderDesc;
            }
            else
            {
                return iteratorOrderAsc;
            }
        }

        private List<List<Cell>> GetCellsListGroup(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                case Direction.Down:
                    return _cells
                        .GroupBy(i => i.IndexX)
                        .Select(i => i.AsEnumerable()
                            .OrderBy(o => o.IndexY)
                            .ToList())
                        .ToList();
                case Direction.Left:
                case Direction.Right:
                    return _cells
                        .GroupBy(i => i.IndexY)
                        .Select(i => i.AsEnumerable()
                            .OrderBy(o => o.IndexX)
                            .ToList())
                        .ToList();
                default:
                    return new List<List<Cell>>();
            }
        }
    }
}
