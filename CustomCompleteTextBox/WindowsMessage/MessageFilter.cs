using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ExtLibrary
{
	/// <summary>
	/// 消息过滤事件数据
	/// </summary>
	public class MessageFilterEventArgs : EventArgs
	{
		/// <summary>
		/// 获取或设置是否停止处理当前消息
		/// </summary>
		public bool StopMessage
		{
			get;
			set;
		}

		/// <summary>
		/// 获取当前消息
		/// </summary>
		public Message CurrentMessage
		{
			get;
			internal set;
		}

		/// <summary>
		/// 默认构造函数
		/// </summary>
		public MessageFilterEventArgs()
			: base()
		{
			this.StopMessage = false;
			this.CurrentMessage = new Message();
		}
	}

	/// <summary>
	/// Windows 消息过滤类, 可过滤指定控件和消息.
	/// </summary>
	public class MessageFilter : IMessageFilter
	{
        /// <summary>
        /// 获取或设置过滤器的启用状态, 默认为 true.
        /// </summary>
        public bool Enable
        {
            get;
            set;
        }

		/// <summary>
		/// 获取或设置需要过滤的消息集合, 如果不指定, 将过滤所有消息.
		/// </summary>
		public List<int> FilterMessages
		{
			get;
			set;
		}

		/// <summary>
		/// 获取或设置过滤句柄的集合, 如果不指定, 将过滤所有句柄.
		/// </summary>
		public List<IntPtr> TargetHandles
		{
			get;
			set;
		}

		/// <summary>
		/// 当匹配到 FilterMessages 中的任意一条消息时引发此事件
		/// </summary>
		public event EventHandler<MessageFilterEventArgs> FilterMessageEvent;


		
		/// <summary>
		/// 使用目标控件与需要过滤的消息初始化过滤器
		/// </summary>
		/// <param name="controlWithChildren">只过滤发给此控件与其所有子控件的消息</param>
		/// <param name="filterMessages">只过滤给定的消息</param>
		public MessageFilter( Control controlWithChildren, params int[] filterMessages )
		{
			this.Enable = true;
			this.FilterMessages = new List<int>();
			this.TargetHandles = new List<IntPtr>();

			if ( filterMessages != null )
			{
				this.FilterMessages.AddRange( filterMessages );
			}

			if ( controlWithChildren != null )
			{
				this.TargetHandles.AddRange( WindowsAPI.GetWindows( controlWithChildren ) );
			}
		}

		/// <summary>
		/// 使用给定的消息初始化过滤器
		/// </summary>
		/// <param name="filterMessages">只过滤给定的消息</param>
		public MessageFilter( params int[] filterMessages ) : this( null, filterMessages )
		{
		}

		/// <summary>
		/// 默认构造函数, 过滤所有消息
		/// </summary>
		public MessageFilter() : this( null, null )
		{
		}



		/// <summary>
		/// 实现 IMessageFilter 接口, 当匹配到消息时引发 FilterMessageEvent 事件.
		/// </summary>
		/// <param name="m"></param>
		/// <returns></returns>
		public virtual bool PreFilterMessage( ref Message m )
		{
			bool result = false;

			if ( this.Enable )
			{
				//是否应过滤消息
				bool isOk = true;

				if ( this.FilterMessages != null && this.FilterMessages.Count > 0 )
				{
					if ( !this.FilterMessages.Contains( m.Msg ) )
					{
						isOk = false;
					}
				}

				if ( this.TargetHandles != null && this.TargetHandles.Count > 0 )
				{
					if ( !this.TargetHandles.Contains( m.HWnd ) )
					{
						isOk = false;
					}
				}

				if ( isOk )
				{
					MessageFilterEventArgs e = new MessageFilterEventArgs();
					e.CurrentMessage = m;
					e.StopMessage = result;

					this.OnFilterMessage( e );
					result = e.StopMessage;
				}
			}

			return result;
		}

		/// <summary>
		/// 添加需要过滤的消息
		/// </summary>
		/// <param name="msg"></param>
		public void AddFilterMessage( params int[] msg )
		{
			if ( this.FilterMessages != null && msg != null )
			{
				this.FilterMessages.AddRange( msg );
			}
		}

		/// <summary>
		/// 移除不需要过滤的消息
		/// </summary>
		/// <param name="msg">消息集合</param>
		public void RemoveFilterMessage( params int[] msg )
		{
			if ( this.FilterMessages != null && msg != null )
			{
				for ( int i = 0; i < msg.Length; i++ )
				{
					this.FilterMessages.Remove( msg[i] );
				}
			}
		}

		/// <summary>
		/// 添加需要过滤的句柄
		/// </summary>
		/// <param name="handle">句柄集合</param>
		public void AddTargetHandle( params IntPtr[] handle )
		{
			if ( this.TargetHandles != null && handle != null )
			{
				this.TargetHandles.AddRange( handle );
			}
		}

		/// <summary>
		/// 添加需要过滤的句柄
		/// </summary>
		/// <param name="controlWithChildren">添加控件及其所有子控件的句柄</param>
		public void AddTargetHandle( params Control[] controlWithChildren )
		{
			if ( this.TargetHandles != null && controlWithChildren != null )
			{
				this.AddTargetHandle( WindowsAPI.GetWindows( controlWithChildren ).ToArray() );
			}
		}

		/// <summary>
		/// 移除不需要过滤的句柄
		/// </summary>
		/// <param name="handle">句柄集合</param>
		public void RemoveTargetHandle( params IntPtr[] handle )
		{
			if ( this.TargetHandles != null && handle != null )
			{
				for ( int i = 0; i < handle.Length; i++ )
				{
					this.TargetHandles.Remove( handle[i] );
				}
			}
		}

		/// <summary>
		/// 移除不需要过滤的句柄
		/// </summary>
		/// <param name="controlWithChildren">移除控件及其所有子控件的句柄</param>
		public void RemoveTargetHandle( params Control[] controlWithChildren )
		{
			if ( this.TargetHandles != null && controlWithChildren != null )
			{
				this.RemoveTargetHandle( WindowsAPI.GetWindows( controlWithChildren ).ToArray() );
			}
		}


		/// <summary>
		/// 引发 FilterMessageEvent 事件
		/// </summary>
		/// <param name="e">事件参数</param>
		protected virtual void OnFilterMessage( MessageFilterEventArgs e )
		{
			if ( this.FilterMessageEvent != null )
			{
				this.FilterMessageEvent( this, e );
			}
		}
	}
}
