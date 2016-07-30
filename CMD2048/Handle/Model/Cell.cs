using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD2048.Handle.Model
{
    /// <summary>
    /// 自定义网格中的单元格
    /// </summary>
    public class Cell
    {
        /// <summary>
        /// 当前数字
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// 网格x坐标
        /// </summary>
        public int IndexX { get; set; }

        /// <summary>
        /// 网格y坐标
        /// </summary>
        public int IndexY { get; set; }
    }
}
