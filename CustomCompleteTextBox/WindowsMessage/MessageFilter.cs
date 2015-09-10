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
	/// Windows 消息过滤类
	/// </summary>
	public class MessageFilter : IMessageFilter
	{
        /// <summary>
        /// 获取或设置过滤器的启用状态
        /// </summary>
        public bool Enable
        {
            get;
            set;
        }

		/// <summary>
		/// 获取或设置需要过滤的消息集合
		/// </summary>
		public List<int> FilterMessages
		{
			get;
			set;
		}

		/// <summary>
		/// 当匹配到 FilterMessages 中的任意一条消息时引发此事件
		/// </summary>
		public event EventHandler<MessageFilterEventArgs> FilterMessageEvent;


		/// <summary>
		/// 默认构造函数
		/// </summary>
		public MessageFilter( params int[] filterMessages )
        {
            this.Enable = true;
			this.FilterMessages = new List<int>();

            if ( filterMessages != null )
            {
                this.FilterMessages.AddRange( filterMessages );
            }
        }



		/// <summary>
		/// 实现 IMessageFilter 接口, 当匹配到消息时引发 FilterMessageEvent 事件.
		/// </summary>
		/// <param name="m"></param>
		/// <returns></returns>
		public virtual bool PreFilterMessage( ref Message m )
		{
			bool result = false;

			if ( this.Enable && this.FilterMessages != null && FilterMessages.Contains( m.Msg ) )
			{
				MessageFilterEventArgs e = new MessageFilterEventArgs();
				e.CurrentMessage = m;
				e.StopMessage = result;

				this.OnFilterMessage( e );
				result = e.StopMessage;
			}

			return result;
		}

		/// <summary>
		/// 添加需要过滤的消息
		/// </summary>
		/// <param name="msg"></param>
		public virtual void AddFilterMessage( params int[] msg )
		{
			this.FilterMessages.AddRange( msg );
		}

		/// <summary>
		/// 引发 FilterMessageEvent 事件
		/// </summary>
		/// <param name="e">事件参数</param>
		protected virtual void OnFilterMessage( MessageFilterEventArgs e )
		{
			if ( this.Enable && this.FilterMessageEvent != null )
			{
				this.FilterMessageEvent( this, e );
			}
		}
	}
}
