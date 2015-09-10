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
