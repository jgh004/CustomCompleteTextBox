/*  CustomCompleteTextBox
    Copyright (C) <2015>  <ITnmg>

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.

	ITnmg' blog: http://blog.itnmg.net
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExtLibrary
{
    /// <summary>
    /// 在当前程序界面内点击鼠标左,中,右键时过滤
    /// </summary>
    public class AppClickFilter : MessageFilter
    {
        /// <summary>
        /// 在这些控件上不执行 ProcessFun 方法
        /// </summary>
        public List<Control> ExcludeControls;

        /// <summary>
        /// 符合过滤条件时执行的方法
        /// </summary>
        public Action ProcessFun;


        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="processFun">符合条件时执行的方法</param>
        public AppClickFilter( Action processFun )
            : this( processFun, null )
        {
        }

        /// <summary>
        /// 使用参数初始化过滤器
        /// </summary>
        /// <param name="processFun">符合条件时执行的方法</param>
        /// <param name="controlAndChildControls">在这些控件及其子控件上不执行 processFun 方法</param>
        public AppClickFilter( Action processFun, params Control[] excludeControls )
            : base( 0x0201, 0x0204, 0x0207, 0x00A1, 0x00A4, 0x00A7 )
        {
            this.ExcludeControls = new List<Control>();
            this.ExcludeControls.AddRange( excludeControls );
            this.ProcessFun = processFun;
            this.FilterMessageEvent += AppClickFilter_FilterMessageEvent;
        }

        private void AppClickFilter_FilterMessageEvent( object sender, MessageFilterEventArgs e )
        {
            if ( this.ExcludeControls != null && this.ProcessFun != null )
            {
                List<IntPtr> childWindows = WindowsAPI.GetWindows( this.ExcludeControls.ToArray() );

                if ( !childWindows.Contains( e.CurrentMessage.HWnd ) )
                {
                    this.ProcessFun();
                }
            }
        }
    }
}
