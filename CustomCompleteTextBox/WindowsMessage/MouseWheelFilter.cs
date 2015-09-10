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
    public class MouseWheelFilter : MessageFilter
    {
        /// <summary>
        /// 获取或设置将滚轮消息发送到这些控件上
        /// </summary>
        public List<Control> SendToControls;


        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="control">将鼠标滚轮消息发送到指定的控件</param>
        public MouseWheelFilter( params Control[] control )
            : base( 0x20a )
        {
            this.SendToControls = new List<Control>();
            this.SendToControls.AddRange( control );
            this.FilterMessageEvent += MouseWheelFilter_FilterMessageEvent;
        }

        private void MouseWheelFilter_FilterMessageEvent( object sender, MessageFilterEventArgs e )
        {
            if ( this.SendToControls != null )
            {
                foreach ( Control c in this.SendToControls )
                {
                    WindowsAPI.SendMessage( c.Handle, e.CurrentMessage.Msg, e.CurrentMessage.WParam, e.CurrentMessage.LParam );
                }

                e.StopMessage = true;
            }
        }
    }
}
