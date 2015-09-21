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
using System.ComponentModel;
using System.Windows.Forms;

namespace CustomCompleteTextBox
{
    /// <summary>
    /// 继承自 ToolStripDropDown 的类
    /// </summary>
    [ToolboxItem( false )]
    public class ToolStripDropDownExt : ToolStripDropDown
	{
		/// <summary>
		/// 当控件激活或失去激活状态时发生, 发生于所有事件之前.
		/// </summary>
		public event EventHandler<ActiveChangeEventArgs> ActiveChange;

		/// <summary>
		/// 处理 windows 消息
		/// </summary>
		/// <param name="m">windows 消息</param>
		protected override void WndProc( ref Message m )
		{
			if ( m.Msg == 0x18 )
			{
				if ( m.WParam == IntPtr.Zero)
				{
					ActiveChangeEventArgs e = new ActiveChangeEventArgs();
					e.Active = false;
				}
			}

			if ( m.Msg == 0x1c|| m.Msg == 0x86 )
			{
				ActiveChangeEventArgs e = new ActiveChangeEventArgs();
				e.Active = m.WParam == IntPtr.Zero ? false : true;

				this.OnActiveChange( e );
			}

			base.WndProc( ref m );
		}

		/// <summary>
		/// 引发 ActiveChange 事件
		/// </summary>
		/// <param name="e">包含激活状态的 ActiveChangeEventArgs 参数</param>
		protected virtual void OnActiveChange( ActiveChangeEventArgs e )
		{
			if ( this.ActiveChange != null )
			{
				this.ActiveChange( this, e );
			}
		}
	}

	/// <summary>
	/// 为 ActiveChange 事件提供数据
	/// </summary>
	public class ActiveChangeEventArgs : EventArgs
	{
		/// <summary>
		/// 获取激活状态
		/// </summary>
		public bool Active
		{
			get;
			internal set;
		}
	}
}
