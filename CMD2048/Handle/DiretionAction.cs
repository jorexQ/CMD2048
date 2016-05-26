using CMD2048.Handle.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD2048.Handle
{
    public class DiretionAction
    {
        private readonly Iterator _iterator;
        private readonly List<List<Cell>> _cellsListGroup;

        private int _startIndex
        {
            get
            {
                return _iterator.StartIndex;
            }
        }

        public DiretionAction(List<List<Cell>> cellsListGroup, Iterator iterator)
        {
            _iterator = iterator;
            _cellsListGroup = cellsListGroup;
        }

        public bool Run()
        {
            var isLayoutChange = false;
            foreach (var item in _cellsListGroup)
            {
                isLayoutChange |= RunItemList(item);
            }
            return isLayoutChange;
        }

        private bool RunItemList(List<Cell> cellList)
        {
            var isLayoutChange = false;
            var currentIndex = _startIndex;
            var nextIndex = 0;
            while (Condition(currentIndex, cellList.Count))
            {
                isLayoutChange |= Align(cellList, currentIndex, out nextIndex);
                currentIndex = nextIndex;
            }
            return isLayoutChange;
        }

        private bool Align(List<Cell> cellList, int currentIndex, out int nextIndex)
        {
            var isLayoutChange = false;
            nextIndex = GetNext(currentIndex);
            var needSwapIndex = 0;
            if (cellList[currentIndex].Value == 0 
                && TryGetHasValeCellIndex(cellList, nextIndex, out needSwapIndex))
            {
                SwapValue(cellList, currentIndex, needSwapIndex);
                isLayoutChange |= true;
            }
            if(Merge(cellList, currentIndex))
            {
                isLayoutChange |= true;
                nextIndex = currentIndex;
            }
            return isLayoutChange;
        }

        private bool Merge(List<Cell> cellList, int currentIndex)
        {
            if (currentIndex == _startIndex) return false;
            var previousIndex = GetPrevious(currentIndex);
            var currentItem = cellList[currentIndex];
            var previousItem = cellList[previousIndex];
            if(currentItem.Value == previousItem.Value)
            {
                previousItem.Value = currentItem.Value + previousItem.Value;
                currentItem.Value = 0;
                return true;
            }
            return false;
        }

        private bool TryGetHasValeCellIndex(
            List<Cell> cellList,
            int currentIndex,
            out int hasValueIndex)
        {
            hasValueIndex = _startIndex;
            if (cellList[currentIndex].Value == 0)
            {
                var nextIndex = GetNext(currentIndex);
                return TryGetHasValeCellIndex(cellList, nextIndex, out hasValueIndex);
            }
            else if (currentIndex >= cellList.Count - 1)
            {
                return false;
            }
            else
            {
                hasValueIndex = currentIndex;
                return true;
            }
        }

        private void SwapValue(List<Cell> cellList, int indexA, int indexB)
        {

        }

        private bool Condition(int currentIndex, int lenght)
        {
            return _iterator.Condition(currentIndex, lenght);
        }

        private int GetNext(int currentIndex)
        {
            return _iterator.GetNext(currentIndex);
        }

        private int GetPrevious(int currentIndex)
        {
            return _iterator.GetPrevious(currentIndex);
        }
    }
}
