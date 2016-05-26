using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD2048.View
{
    public class GameWindow
    {
        private readonly int _x;
        private readonly int _y;

        private readonly int _windowWidth;
        private readonly int _windowHeight;
        private readonly Dictionary<int, CellStyle> _cellStyles = new Dictionary<int, CellStyle>();

        public int ColWidth { get; private set; }
        public int RowHeight { get; private set; }
        public int TitleHeight { get; private set; }

        public GameWindow(int x, int y)
        {
            _x = x;
            _y = y;

            ColWidth = 8;
            RowHeight = 3;
            TitleHeight = 5;

            _windowWidth = GetWindowWidth();
            _windowHeight = GetWindowHeight();
            Console.SetWindowSize(_windowWidth, _windowHeight);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fontColor"></param>
        /// <param name="backgoundColor"></param>
        /// <returns></returns>
        public GameWindow RegisterCellStyle(int value, ConsoleColor fontColor,ConsoleColor backgoundColor)
        {
            CellStyle cellStyle;
            if (_cellStyles.TryGetValue(value, out cellStyle))
            {
                cellStyle.FontColor = fontColor;
                cellStyle.BackgroundColor = backgoundColor;              
            }
            else
            {
                _cellStyles.Add(value, new CellStyle
                {
                    FontColor = fontColor,
                    BackgroundColor = backgoundColor
                });
            }
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public GameWindow DrawTitle(string title = null)
        {
            var defaultTitle = "2048Game";
            Console.Title = defaultTitle;
            var totalHeight = 5;
            var titleImage = new string[totalHeight];
            titleImage[2] = string.IsNullOrEmpty(title) ? defaultTitle : title;
            for (var i = 0;i< titleImage.Length; i++)
            {
                var lineStr = GetBoxLineStr(totalHeight, i, titleImage[i]);
                Console.WriteLine(lineStr);
            }
            return this;
        }

        public GameWindow DrawInstraction(string instraction = null)
        {


            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public GameWindow DrawMain(int[,] image)
        {
            var mainHeight = _windowHeight - TitleHeight - 4;
            var imagePosition = new string[mainHeight];
            for(var y = 0; y < _y; y++)
            {
                var lineContent = "\t";
                for(var x = 0; x < _x; x++)
                {
                    lineContent +="|"+ image[x, y] + "|\t";
                }
                var rowY = (y + 1)*3;
                imagePosition[rowY] = lineContent;
            }
            for(var i = 0;i < imagePosition.Length; i++)
            {
                var lineStr = GetBoxLineStr(imagePosition.Length, i, imagePosition[i], s =>s);
                var lineArr = lineStr.Split('|');
                if(lineArr.Length<=1)
                {
                    Console.WriteLine(lineStr);
                    continue;
                }
                foreach(var item in lineArr)
                {
                    var cellValue = 0;
                    if(int.TryParse(item,out cellValue))
                    {
                        DrawCell(cellValue);
                    }
                    else
                    {
                        Console.Write(item);
                    }                   
                }
                Console.Write("\n");
            }
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void DrawCell(int value)
        {
            CellStyle cellStyle;
            if (_cellStyles.TryGetValue(value, out cellStyle))
            {
                Console.ForegroundColor = cellStyle.FontColor;
                Console.BackgroundColor = cellStyle.BackgroundColor;
                Console.Write(value);
                Console.ResetColor();
            }
            else
            {
                Console.Write(value);
            }
        }

        public void Refurbish(int[,] image, string title = null, string instraction = null)
        {
            Console.Clear();
            DrawTitle(title);
            DrawInstraction(instraction);
            DrawMain(image);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Show()
        {
            Console.Read();
        }

        /// <summary>
        /// 获取窗口的宽度（单位列数）
        /// </summary>
        /// <returns></returns>
        public int GetWindowWidth()
        {
            return _x * ColWidth + ColWidth + 2;
        }

        /// <summary>
        /// 获取窗口的高度（单位行数）
        /// </summary>
        /// <returns></returns>
        public int GetWindowHeight()
        {
            return _y * RowHeight + RowHeight + 5 + TitleHeight;
        }

        /// <summary>
        /// 获取容器盒子的单行字符串
        /// </summary>
        /// <param name="height">盒子总高（单位行数）</param>
        /// <param name="currentY">当前第几行</param>
        /// <param name="lineContent">行内容</param>
        /// <param name="contentLineHandleMethod">自定义行内容显示方式函数</param>
        /// <returns></returns>
        private String GetBoxLineStr(
            int height, 
            int currentY, 
            String lineContent, 
            Func<string,string> contentLineHandleMethod = null)
        {
            if(currentY == 0)
            {
                return GetBoxTopLineStr();
            }
            else if(currentY == height - 1)
            {
                return GetBoxButtonLineStr();
            }
            else
            {
                var contentStr = "";
                var spaceStr = "";
                var spaceLength = 0;
                var contentLength = 0;
                if (contentLineHandleMethod != null)
                {
                    contentStr = contentLineHandleMethod(lineContent);
                    spaceLength = string.IsNullOrEmpty(contentStr) ? (_windowWidth - 4) / 2 : 0;
                }
                else
                {                
                    if (!string.IsNullOrEmpty(lineContent))
                    {
                        contentLength = lineContent.Length * 2;
                        contentStr = lineContent
                        .Select(o => " " + o)
                        .Aggregate((a, n) => a + n);
                    }
                    spaceLength = ((_windowWidth - contentLength) - 4) / 2;
                }
                for (var i = 0; i < spaceLength; i++)
                {
                    spaceStr += " ";
                }
                return "║" + spaceStr + contentStr + spaceStr + "║";
            }
        }

        /// <summary>
        /// 获取容器盒子的顶行字符串
        /// </summary>
        /// <returns></returns>
        private String GetBoxTopLineStr()
        {
            var lineStr = "";
            for (var i = 0; i < _windowWidth; i += 2)
            {
                if (i == 0)
                {
                    lineStr += "╔";
                }
                else if (i == _windowWidth - 2)
                {
                    lineStr += "╗";
                }
                else
                {
                    lineStr += "═";
                }
            }
            return lineStr;
        }

        /// <summary>
        /// 获取容器盒子的底行字符串
        /// </summary>
        /// <returns></returns>
        private String GetBoxButtonLineStr()
        {
            var lineStr = "";
            for (var i = 0; i < _windowWidth; i += 2)
            {
                if (i == 0)
                {
                    lineStr += "╚";
                }
                else if (i == _windowWidth - 2)
                {
                    lineStr += "╝";
                }
                else
                {
                    lineStr += "═";
                }
            }
            return lineStr;
        }
    }
}
