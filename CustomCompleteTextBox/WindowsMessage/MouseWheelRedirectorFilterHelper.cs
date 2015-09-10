using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExtLibrary
{
	/// <summary>
	/// 过滤鼠标滚轮消息
	/// </summary>
	public class MouseWheelRedirectorFilterHelper
	{
		/// <summary>
		/// 内部过滤器
		/// </summary>
		private MessageFilter filter;

		/// <summary>
		/// 在此
		/// </summary>
		private List<Control> filterControls;


		/// <summary>
		/// 默认构造函数
		/// </summary>
		/// <param name="control">将鼠标滚轮消息发送到指定的控件</param>
		public MouseWheelRedirectorFilterHelper( params Control[] control )
		{
			this.filter = new MessageFilter();
			this.filterControls = new List<Control>();
			this.filterControls.AddRange( control );

			this.Init();
		}


		/// <summary>
		/// 添加到应该程序过滤器中开始执行
		/// </summary>
		public void StartFilter()
		{
			if ( this.filter != null )
			{
				Application.AddMessageFilter( this.filter );
			}
		}

		/// <summary>
		/// 从应用程序过滤器中移除
		/// </summary>
		public void StopFilter()
		{
			if ( this.filter != null )
			{
				Application.RemoveMessageFilter( this.filter );
			}
		}

		/// <summary>
		/// 初始化方法
		/// </summary>
		private void Init()
		{
			this.filter.AddFilterMessage( 0x20a );
			this.filter.FilterMessageEvent += filter_FilterMessageEvent;
		}

		/// <summary>
		/// 过滤器执行的方法
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void filter_FilterMessageEvent( object sender, MessageFilterEventArgs e )
		{
			if ( this.filterControls != null )
			{
				foreach ( Control c in this.filterControls )
				{
					WindowsAPI.SendMessage( c.Handle, e.CurrentMessage.Msg, e.CurrentMessage.WParam, e.CurrentMessage.LParam );
				}

				e.StopMessage = true;
			}
		}
	}
}
