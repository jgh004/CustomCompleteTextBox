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
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ExtLibrary
{
	public static class WindowsAPI
	{
		/// <summary>
		/// 枚举指定窗口的子窗口时的数据
		/// </summary>
		/// <param name="hwnd">子窗口句柄</param>
		/// <param name="lParam">自定义参数</param>
		/// <returns>返回true,继续枚举;返回false,停止枚举.</returns>
		public delegate bool EnumWindownCallback( IntPtr hwnd, IntPtr lParam );

        /// <summary>
		/// 返回指定句柄的所有子句柄
		/// </summary>
		/// <param name="windown">父窗口句柄</param>
		/// <returns>所有子窗口</returns>
		public static List<IntPtr> GetChildrenWindowns( IntPtr windown )
        {
            List<IntPtr> result = new List<IntPtr>();

            WindowsAPI.EnumChildWindows( windown, ( hwnd, lParam ) =>
            {
                result.Add( hwnd );
                result.AddRange( GetChildrenWindowns( hwnd ) );
                return true;
            }, IntPtr.Zero );

            return result;
        }

        /// <summary>
        /// 返回指定控件的所有句柄
        /// </summary>
        /// <param name="control">控件集合</param>
        /// <returns>所有句柄,包括自身的.</returns>
        public static List<IntPtr> GetWindows( params Control[] control )
        {
            List<IntPtr> result = new List<IntPtr>();

            if ( control != null )
            {
                foreach ( Control c in control )
                {
                    result.Add( c.Handle );
                    result.AddRange( GetChildrenWindowns( c.Handle ) );
                }
            }

            return result.Distinct().ToList();
        }


        //------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 调用 windows api 发送消息到指定的窗口句柄
        /// </summary>
        /// <param name="hWnd">目标窗口句柄</param>
        /// <param name="msg">windows消息</param>
        /// <param name="wParam">通常是一个与消息有关的常量值，也可能是窗口或控件的句柄</param>
        /// <param name="lParam">通常是一个指向内存中数据的指针</param>
        /// <returns></returns>
        [DllImport( "user32.dll", CharSet = CharSet.Auto )]
		public static extern IntPtr SendMessage( IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam );

		/// <summary>
		/// 枚举所有顶级窗口
		/// </summary>
		/// <param name="lpEnumFunc">找到顶级窗口后调用的函数</param>
		/// <param name="lParam">自定义参数</param>
		/// <returns></returns>
		[DllImport( "user32.dll" )]
		public static extern int EnumWindows( EnumWindownCallback lpEnumFunc, IntPtr lParam );

		/// <summary>
		/// 从给定的父窗口枚举所有子窗口
		/// </summary>
		/// <param name="hwndParent">父窗口句柄</param>
		/// <param name="lpEnumFunc">找到子窗口后调用的函数</param>
		/// <param name="lParam">自定义参数</param>
		/// <returns></returns>
		[DllImport( "user32.dll", ExactSpelling = true )]
		public static extern bool EnumChildWindows( IntPtr hwndParent, EnumWindownCallback lpEnumFunc, IntPtr lParam );
	}
}
