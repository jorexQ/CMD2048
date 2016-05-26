using CMD2048.Handle.Model;
using CMD2048.View;
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
        private readonly GameWindow _window;
        private readonly int _x;
        private readonly int _y;
        private readonly Stack<List<int>> _histeryCellsValue = new Stack<List<int>>();
        private Event _eventCenter;

        public Core(int x, int y, GameWindow window)
        {
            _x = x;
            _y = y;
            _window = window;
            _eventCenter = new Event();
            Init();
        }

        public void Listening()
        {
            _eventCenter.Listening();
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

        public int[,] GetLastImage()
        {
            var lastCellListValue = _histeryCellsValue.Pop();
            for(var i = 0; i < _cells.Count; i++)
            {
                _cells[i].Value = lastCellListValue[i];
            }
            return GetImage();
        }

        private void Init()
        {
            InitCellList();
            SetRandom(2);
            _window.Refurbish(GetImage());
            var actionDict = new Dictionary<Direction, Action>();
            actionDict.Add(Direction.Up, GetHandleByDirection(Direction.Up));
            actionDict.Add(Direction.Down, GetHandleByDirection(Direction.Down));
            actionDict.Add(Direction.Left, GetHandleByDirection(Direction.Left));
            actionDict.Add(Direction.Right, GetHandleByDirection(Direction.Right));
            _eventCenter.RegisterHandle(actionDict,
                () => { Init(); },
                () => {
                    var lastImage = GetLastImage();
                    _window.Refurbish(lastImage);
                });
        }

        private void BackCellListValue()
        {
            var valueList = _cells.Select(o => o.Value).ToList();
            _histeryCellsValue.Push(valueList);
        }

        private Action GetHandleByDirection(Direction direction)
        {
            var cellListGroup = GetCellsListGroup(direction);
            var iterator = GetIterator(cellListGroup, direction);
            var diretionAction = new DiretionAction(cellListGroup, iterator);
            return () =>
            {
                BackCellListValue();
                if (diretionAction.Run())
                {
                    SetRandom();
                }
                _window.Refurbish(GetImage());
            };
        }

        private void InitCellList()
        {
            _cells.Clear();
            for (var y = 0; y < _y; y++)
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
            if (direction == Direction.Up || direction == Direction.Left)
            {
                return iteratorOrderAsc;
            }
            else
            {
                return iteratorOrderDesc;
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
