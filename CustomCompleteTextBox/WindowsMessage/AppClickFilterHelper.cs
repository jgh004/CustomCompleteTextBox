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
	public class AppClickFilterHelper
	{
		/// <summary>
		/// 内部使用的过滤器
		/// </summary>
		private MessageFilter filter;

		/// <summary>
		/// 在这些控件上不执行 processFun 委托
		/// </summary>
		private List<Control> filterControls;

		/// <summary>
		/// 符合过滤条件时执行的委托
		/// </summary>
		private Action processFun;


		/// <summary>
		/// 使用参数初始化过滤器
		/// </summary>
		/// <param name="process">符合条件时执行的方法</param>
		/// <param name="controlAndChildControls">在这些控件及其子控件上不执行 process 委托</param>
		public AppClickFilterHelper(Action process, params Control[] controlAndChildControls  )
		{
			this.filter = new MessageFilter();
			this.filterControls = new List<Control>();
			this.filterControls.AddRange( controlAndChildControls );
			this.processFun = process;

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
			this.filter.AddFilterMessage( 0x0201, 0x0204, 0x0207, 0x00A1, 0x00A4, 0x00A7 );
			this.filter.FilterMessageEvent += filter_FilterMessageEvent;
		}

		/// <summary>
		/// 过滤器执行的方法
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void filter_FilterMessageEvent( object sender, MessageFilterEventArgs e )
		{
			List<IntPtr> childWindows = new List<IntPtr>();

			if ( this.filterControls != null )
			{
				foreach ( Control c in this.filterControls )
				{
					childWindows.Add( c.Handle );
					childWindows.AddRange( GetChildWindowns( c.Handle ) );
				}

				childWindows = childWindows.Distinct().ToList();
			}

			if ( !childWindows.Contains( e.CurrentMessage.HWnd ) )
			{
				if ( this.processFun != null )
				{
					this.processFun();
				}
			}
		}

		/// <summary>
		/// 返回指定句柄的所有子句柄
		/// </summary>
		/// <param name="windown"></param>
		/// <returns></returns>
		private List<IntPtr> GetChildWindowns( IntPtr windown )
		{
			List<IntPtr> result = new List<IntPtr>();

			WindowsAPI.EnumChildWindows( windown, ( hwnd, lParam ) =>
			{
				result.Add( hwnd );
				result.AddRange( GetChildWindowns( hwnd ) );
				return true;
			}, IntPtr.Zero );

			return result;
		}
	}
}
