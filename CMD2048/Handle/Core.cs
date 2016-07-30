using CMD2048.Handle.Model;
using CMD2048.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD2048.Handle
{
    /// <summary>
    /// 2048逻辑核心
    /// </summary>
    public class Core
    {
        /// <summary>
        /// 网格单元集合
        /// </summary>
        public List<Cell> _cells = new List<Cell>();
        /// <summary>
        /// 游戏界面
        /// </summary>
        private readonly GameWindow _window;
        /// <summary>
        /// 网格x轴个数
        /// </summary>
        private readonly int _x;
        /// <summary>
        /// 网格y轴个数
        /// </summary>
        private readonly int _y;
        /// <summary>
        /// 游戏动作记录，记录当前游戏状态图
        /// </summary>
        private readonly Stack<List<int>> _histeryCellsValue = new Stack<List<int>>();
        /// <summary>
        /// 游戏事件中心
        /// </summary>
        private Event _eventCenter;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="window"></param>
        public Core(int x, int y, GameWindow window)
        {
            _x = x;
            _y = y;
            _window = window;
            _eventCenter = new Event();
            Init();
        }

        /// <summary>
        /// 事件监听开始
        /// </summary>
        public void Listening()
        {
            _eventCenter.Listening();
        }

        /// <summary>
        /// 随机点生成器
        /// </summary>
        /// <param name="count">生成几个（默认一个）</param>
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

        /// <summary>
        /// 获取当前游戏状态图
        /// </summary>
        /// <returns></returns>
        public int[,] GetImage()
        {
            var image = new int[_x, _y];
            foreach(var cell in _cells)
            {
                image[cell.IndexX, cell.IndexY] = cell.Value;
            }
            return image;
        }

        /// <summary>
        /// 获取游戏记录的最后一张状态图
        /// </summary>
        /// <returns></returns>
        public int[,] GetLastImage()
        {
            if (_histeryCellsValue.Count == 0) return null;
            var lastCellListValue = _histeryCellsValue.Pop();
            for(var i = 0; i < _cells.Count; i++)
            {
                _cells[i].Value = lastCellListValue[i];
            }
            return GetImage();
        }

        /// <summary>
        /// 初始化游戏
        /// </summary>
        private void Init()
        {
            _histeryCellsValue.Clear();
            InitCellList();
            SetRandom(2);
            _window.StatusMsg = "Game Starting...";
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
                    _window.StatusMsg = "Game Starting...";
                    _window.Refurbish(lastImage);
                });
        }

        /// <summary>
        /// 回退上一个游戏状态图
        /// </summary>
        private void BackCellListValue()
        {
            var valueList = _cells.Select(o => o.Value).ToList();
            if (_histeryCellsValue.Count == 0)
            {
                _histeryCellsValue.Push(valueList);
            }
            else
            {
                var lastCellValues = _histeryCellsValue.Peek();
                var isSame = true;
                for(var i = 0; i < lastCellValues.Count; i++)
                {
                    isSame &= lastCellValues[i] == valueList[i];
                }
                if (!isSame)
                {
                    _histeryCellsValue.Push(valueList);
                }
            }                 
        }

        /// <summary>
        /// 游戏移动的处理委托工厂
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
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
                if (IsGameOver())
                {
                    _window.StatusMsg = "Game Over (press Q restart) ";
                    _window.Refurbish(GetImage());
                }
            };
        }

        /// <summary>
        /// 游戏是否结束
        /// </summary>
        /// <returns></returns>
        private bool IsGameOver()
        {
            var hasEmptyValue = false;
            var hasNearSameValue = false;
            for(var i = 0; i < _cells.Count; i++)
            {
                var item = _cells[i];
                hasEmptyValue |= item.Value == 0;

                if (item.IndexX != _x - 1)
                {
                    var nearRightIndex = i + 1;
                    hasNearSameValue |= item.Value == _cells[nearRightIndex].Value;
                }
                if (item.IndexY != 0)
                {
                    var nearUpIndex = i - _y;
                    hasNearSameValue |= item.Value == _cells[nearUpIndex].Value;
                }
            }
            return !(hasEmptyValue || hasNearSameValue);
        }

        /// <summary>
        /// 初始化网格单元格
        /// </summary>
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

        /// <summary>
        /// 迭代器生成工厂
        /// </summary>
        /// <param name="cellListGroup"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 根据移动方向，生成迭代器，迭代分组顺序
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
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
