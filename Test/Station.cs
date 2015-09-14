using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class Station
    {
        /// <summary>
        /// 三字母缩写
        /// </summary>
        public string ThreedLetterCode
        {
            get;
            set;
        }

        /// <summary>
        /// 中文名
        /// </summary>
        public string ChineseName
        {
            get;
            set;
        }

        /// <summary>
        /// 车站编码
        /// </summary>
        public string Code
        {
            get;
            set;
        }

        /// <summary>
        /// 全拼
        /// </summary>
        public string FullPinYin
        {
            get;
            set;
        }

        /// <summary>
        /// 拼音首字母缩写
        /// </summary>
        public string SimplePinYin
        {
            get;
            set;
        }

        /// <summary>
        /// 排序号
        /// </summary>
        public int Index
        {
            get;
            set;
        }
    }
}
