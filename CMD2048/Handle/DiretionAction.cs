using CMD2048.Handle.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD2048.Handle
{
    /// <summary>
    /// 移动方向处理委托类
    /// </summary>
    public class DiretionAction
    {
        /// <summary>
        /// 所属迭代器
        /// </summary>
        private readonly Iterator _iterator;
        /// <summary>
        /// 迭代顺序组
        /// </summary>
        private List<List<Cell>> _cellsListGroup;

        /// <summary>
        /// 开始点
        /// </summary>
        private int _startIndex
        {
            get
            {
                return _iterator.StartIndex;
            }
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="cellsListGroup"></param>
        /// <param name="iterator"></param>
        public DiretionAction(List<List<Cell>> cellsListGroup, Iterator iterator)
        {
            _iterator = iterator;
            _cellsListGroup = cellsListGroup;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Run()
        {
            var isLayoutChange = false;
            foreach (var item in _cellsListGroup)
            {
                isLayoutChange |= RunItemList(item);
            }
            return isLayoutChange;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cellList"></param>
        /// <returns></returns>
        private bool RunItemList(List<Cell> cellList)
        {
            var isLayoutChange = false;
            var currentIndex = _startIndex;
            while (Condition(currentIndex, cellList.Count))
            {
                isLayoutChange |= Align(cellList, currentIndex);
                if (Merge(cellList, currentIndex))
                {
                    isLayoutChange |= true;
                    Align(cellList, currentIndex);
                }
                currentIndex = GetNext(currentIndex);
            }
            return isLayoutChange;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cellList"></param>
        /// <param name="currentIndex"></param>
        /// <returns></returns>
        private bool Align(List<Cell> cellList, int currentIndex)
        {
            var isLayoutChange = false;
            var nextIndex = GetNext(currentIndex);
            var needSwapIndex = 0;
            if (cellList[currentIndex].Value == 0 
                && TryGetHasValeCellIndex(cellList, nextIndex, out needSwapIndex))
            {
                SwapValue(cellList, currentIndex, needSwapIndex);
                isLayoutChange |= true;
            }
            
            return isLayoutChange;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cellList"></param>
        /// <param name="currentIndex"></param>
        /// <returns></returns>
        private bool Merge(List<Cell> cellList, int currentIndex)
        {
            if (currentIndex == _startIndex) return false;
            var previousIndex = GetPrevious(currentIndex);
            var currentItem = cellList[currentIndex];
            var previousItem = cellList[previousIndex];
            if( currentItem.Value != 0
                && previousItem.Value !=0
                && currentItem.Value == previousItem.Value)
            {
                previousItem.Value = currentItem.Value + previousItem.Value;
                currentItem.Value = 0;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cellList"></param>
        /// <param name="currentIndex"></param>
        /// <param name="hasValueIndex"></param>
        /// <returns></returns>
        private bool TryGetHasValeCellIndex(
            List<Cell> cellList,
            int currentIndex,
            out int hasValueIndex)
        {
            hasValueIndex = _startIndex;
            
            if (currentIndex > cellList.Count - 1|| currentIndex < 0)
            {
                return false;
            }
            else if (cellList[currentIndex].Value == 0)
            {
                var nextIndex = GetNext(currentIndex);
                return TryGetHasValeCellIndex(cellList, nextIndex, out hasValueIndex);
            }
            else
            {
                hasValueIndex = currentIndex;
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cellList"></param>
        /// <param name="indexA"></param>
        /// <param name="indexB"></param>
        private void SwapValue(List<Cell> cellList, int indexA, int indexB)
        {
            var indexAItem = cellList[indexA];
            var indexBItem = cellList[indexB];
            var temp = indexAItem.Value;
            indexAItem.Value = indexBItem.Value;
            indexBItem.Value = temp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentIndex"></param>
        /// <param name="lenght"></param>
        /// <returns></returns>
        private bool Condition(int currentIndex, int lenght)
        {
            return _iterator.Condition(currentIndex, lenght);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentIndex"></param>
        /// <returns></returns>
        private int GetNext(int currentIndex)
        {
            return _iterator.GetNext(currentIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentIndex"></param>
        /// <returns></returns>
        private int GetPrevious(int currentIndex)
        {
            return _iterator.GetPrevious(currentIndex);
        }
    }
}
