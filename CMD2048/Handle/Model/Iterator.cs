using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD2048.Handle
{
    /// <summary>
    /// 自定义迭代器
    /// </summary>
    public class Iterator
    {
        /// <summary>
        /// 开始迭代坐标
        /// </summary>
        public int StartIndex { get; set; }

        /// <summary>
        /// 自定义下一个委托
        /// </summary>
        public Func<int, int> GetNext { get; set; }

        /// <summary>
        /// 自定义上一个委托
        /// </summary>
        public Func<int, int> GetPrevious { get; set; }

        /// <summary>
        /// 自定义迭代执行条件
        /// </summary>
        public Func<int, int, bool> Condition { get; set; }

        /// <summary>
        /// 反向迭代器
        /// </summary>
        public Iterator Reverse { get; set; }
    }
}
