using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections;

namespace ExtLibrary
{
    /// <summary>
    /// 带下拉列表的自定义搜索文本框
    /// </summary>
	[ToolboxItem( true )]
    public partial class CustomCompleteTextBox : TextBox
    {
        /// <summary>
        /// 监视鼠标滚轮事件
        /// </summary>
        private MouseWheelFilter mouseWheel;

        /// <summary>
        /// 监视鼠标左,中,右键点击事件
        /// </summary>
        private AppClickFilter appClick;

        /// <summary>
        /// 内部使用,用于存储listBox数据
        /// </summary>
        private ListBox innerListBox;
        /// <summary>
        /// 显示候选列表
        /// </summary>
        private ListBox box;
        private ToolStripControlHost host;

        /// <summary>
        /// 下拉控件
        /// </summary>
		private ToolStripDropDownExt drop;

        //--------------------------------------------------------------------------------

        /// <summary>
        /// 获取或设置数据集合
        /// </summary>
        public ListBox.ObjectCollection Items
        {
            get
            {
                return this.innerListBox.Items;
            }
        }

        /// <summary>
        /// 获取或设置选择的项目
        /// </summary>
        public object SelectedItem
        {
            get
            {
                return this.innerListBox.SelectedItem;
            }
            set
            {
                this.innerListBox.SelectedItem = value;
            }
        }

        /// <summary>
        /// 获取或设置选择的值
        /// </summary>
        public object SelectedValue
        {
            get
            {
                return this.innerListBox.SelectedValue;
            }
            set
            {
                this.innerListBox.SelectedValue = value;
            }
        }

        /// <summary>
        /// 获取或设置显示的属性
        /// </summary>
        public string DisplayMember
        {
            get
            {
                return this.innerListBox.DisplayMember;
            }
            set
            {
                this.innerListBox.DisplayMember = value;
            }
        }

        /// <summary>
        /// 获取或设置值的属性
        /// </summary>
        public string ValueMember
        {
            get
            {
                return this.innerListBox.ValueMember;
            }
            set
            {
                this.innerListBox.ValueMember = value;
            }
        }

        /// <summary>
        /// 当项目进行搜索匹配时引发此事件, 可在此定义匹配规则.
        /// </summary>
        public event EventHandler<MatchEventArgs> Match;

        //--------------------------------------------------------------------------------

        /// <summary>
        /// 构造函数
        /// </summary>
        public CustomCompleteTextBox()
            : base()
        {
            this.InitControl();
        }

        //--------------------------------------------------------------------------------

        /// <summary>
        /// 初始化布局
        /// </summary>
		protected override void InitLayout()
        {
            base.InitLayout();

            this.box.Width = this.Width - 2;
        }

        protected override void OnClick( EventArgs e )
        {
            base.OnClick( e );
            this.SelectAll();
            this.Focus();
            this.MatchAndSetListItems();
            this.DropList();
        }

        protected override void OnEnter( EventArgs e )
        {
            base.OnEnter( e );
            this.SelectAll();
            this.mouseWheel.Enable = true;
            this.MatchAndSetListItems();
            this.DropList();
        }

        protected override void OnLeave( EventArgs e )
        {
            base.OnLeave( e );
            this.mouseWheel.Enable = false;
            this.CloseList();
        }

        protected override void OnTextChanged( EventArgs e )
        {
            base.OnTextChanged( e );

            List<object> objs = this.MatchAndSetListItems();
            this.SelectedItem = objs.Count == 1 ? objs[0] : null;
            this.box.SelectedItem = this.SelectedItem;

            this.DropList();
        }

        protected override void OnKeyPress( KeyPressEventArgs e )
        {
            //去掉系统提示音
            if ( e.KeyChar == 13 )
            {
                e.Handled = true;
            }

            base.OnKeyPress( e );
        }

        protected override void OnKeyDown( KeyEventArgs e )
        {
            //按上下键时不改变文本框内的光标位置
            switch ( e.KeyCode )
            {
                case Keys.Up:
                case Keys.Down:
                    e.Handled = true;
                    break;
            }

            base.OnKeyDown( e );
        }

        protected override void WndProc( ref Message m )
        {
            //上下键,回车键,将消息转发到下拉框
            if ( m.Msg == 0x100 )
            {
                switch ( m.WParam.ToInt32() )
                {
                    case 13:
                    case 38:
                    case 40:
                        if ( this.box.Visible )
                        {
                            WindowsAPI.SendMessage( this.box.Handle, m.Msg, m.WParam, m.LParam );
                        }
                        break;
                }
            }

            base.WndProc( ref m );
        }

        /// <summary>
        /// 项目匹配事件
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMatch( MatchEventArgs e )
        {
            if ( this.Match != null )
            {
                this.Match( this, e );
            }
        }

        //--------------------------------------------------------------------------------

        /// <summary>
        /// 显示下拉列表
        /// </summary>
        public void DropList()
        {
            if ( !this.drop.Visible )
            {
                Screen screent = Screen.FromControl( this );
                Point showPoint = new Point( 0 - (this.Size.Width - this.ClientSize.Width) / 2, this.Height - (this.Size.Height - this.ClientSize.Height) / 2 );
                ToolStripDropDownDirection direction = this.drop.Height > screent.WorkingArea.Height - this.PointToScreen( showPoint ).Y ? ToolStripDropDownDirection.AboveRight : ToolStripDropDownDirection.BelowRight;
                showPoint = direction == ToolStripDropDownDirection.BelowRight ? showPoint : new Point( 0 - (this.Size.Width - this.ClientSize.Width) / 2, 0 - (this.Size.Height - this.ClientSize.Height) / 2 );
                this.drop.Show( this, showPoint, direction );
            }
        }

        /// <summary>
        /// 关闭下拉列表
        /// </summary>
        public void CloseList()
        {
            this.drop.Close();
        }

        //--------------------------------------------------------------------------------

        /// <summary>
        /// 初始化各参数
        /// </summary>
        private void InitControl()
        {
            this.innerListBox = new ListBox();
            this.innerListBox.SelectionMode = SelectionMode.One;

            this.box = new ListBox();
            this.box.Margin = Padding.Empty;
            this.box.BorderStyle = BorderStyle.None;
            this.box.TabStop = false;
            this.box.SelectionMode = SelectionMode.One;
            this.box.IntegralHeight = false;
            this.box.MouseMove += Box_MouseMove;
            this.box.Click += Box_Click;
            this.box.KeyDown += Box_KeyDown;

            this.host = new ToolStripControlHost( box );
            this.host.Margin = Padding.Empty;
            this.host.Padding = Padding.Empty;
            this.host.AutoSize = false;
            this.host.AutoToolTip = false;

            this.drop = new ToolStripDropDownExt();
            this.drop.AutoClose = false;
            this.drop.Items.Add( host );
            this.drop.Margin = Padding.Empty;
            this.drop.Padding = new Padding( 1 );
            this.drop.ShowItemToolTips = false;
            this.drop.TabStop = false;
            this.drop.Closed += Drop_Closed;
            this.drop.ActiveChange += Drop_ActiveChange;

            this.mouseWheel = new MouseWheelFilter( this.box );
            this.mouseWheel.Enable = false;
            this.appClick = new AppClickFilter( () =>
            {
                this.CloseList();
            }, this, this.drop, this.box );
            Application.AddMessageFilter( this.mouseWheel );
            Application.AddMessageFilter( this.appClick );
        }
        
        /// <summary>
        /// 设置文本框文本
        /// </summary>
        private void SetText()
        {
            this.Text = this.box.GetItemText( this.SelectedItem );
            this.SelectionStart = this.Text.Length;
        }

        /// <summary>
        /// 根据匹配规则,绑定下拉列表, 返回完全匹配项
        /// </summary>
        /// <returns>完全匹配项</returns>
        private List<object> MatchAndSetListItems()
        {
            List<object> result = new List<object>();
            this.box.Items.Clear();
            this.box.DisplayMember = this.DisplayMember;
            this.box.ValueMember = this.ValueMember;

            if ( this.Items != null )
            {
                for ( int i = 0; i < this.Items.Count; i++ )
                {
                    object obj = this.Items[i];

                    if ( obj != null )
                    {
                        object valObj = FilterItemOnProperty( obj, this.ValueMember );
                        string valueText = valObj == null ? "" : valObj.ToString();
                        string displayText = this.innerListBox.GetItemText( obj );

                        MatchEventArgs args = new MatchEventArgs();
                        args.Item = obj;
                        args.MatchText = this.Text;
                        args.MatchResult = displayText.Contains( this.Text ) || valueText.Contains( this.Text );

                        this.OnMatch( args );

                        if ( args.MatchResult )
                        {
                            this.box.Items.Add( obj );
                        }

                        if ( displayText == this.Text || valueText == this.Text )
                        {
                            result.Add( obj );
                        }
                    }
                }

                this.box.SelectedItem = this.SelectedItem;
            }

            return result;
        }

        /// <summary>
        /// 返回指定绑定的属性的值
        /// </summary>
        /// <param name="item">绑定项目</param>
        /// <param name="field">指定属性</param>
        /// <returns>属性的值</returns>
        private object FilterItemOnProperty( object item, string field )
        {
            if ( (item != null) && (field.Length > 0) )
            {
                try
                {
                    PropertyDescriptor descriptor = TypeDescriptor.GetProperties( item ).Find( field, true );

                    if ( descriptor != null )
                    {
                        item = descriptor.GetValue( item );
                    }
                }
                catch
                {
                }
            }

            return item;
        }


        /// <summary>
        /// 关闭下拉列表时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Drop_Closed( object sender, ToolStripDropDownClosedEventArgs e )
        {
            this.SetText();
            this.box.SelectedIndex = -1;
        }

        /// <summary>
        /// 下拉列表激活或失去激活状态时引发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Drop_ActiveChange( object sender, ActiveChangeEventArgs e )
        {
            this.CloseList();
        }

        /// <summary>
        /// 在 listbox 上按下按键时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Box_KeyDown( object sender, KeyEventArgs e )
        {
            switch ( e.KeyCode )
            {
                case Keys.Enter:
                    this.Box_Click( this.box, EventArgs.Empty );
                    break;
            }
        }

        /// <summary>
        /// 单击下拉选项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void Box_Click( object sender, EventArgs e )
        {
            if ( this.box.SelectedItem != null )
            {
                this.SelectedItem = this.box.SelectedItem;
            }

            this.CloseList();
        }

        /// <summary>
        /// 鼠标在选项间移动时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void Box_MouseMove( object sender, MouseEventArgs e )
        {
            int index = this.box.IndexFromPoint( e.Location );

            if ( index > -1 )
            {
                this.box.SelectedIndex = index;
            }
        }
    }

    /// <summary>
    /// 匹配事件数据
    /// </summary>
    public class MatchEventArgs : EventArgs
    {
        /// <summary>
        /// 获取当前需判断的数据项
        /// </summary>
        public object Item
        {
            get;
            internal set;
        }

        /// <summary>
        /// 获取判断的文本
        /// </summary>
        public string MatchText
        {
            get;
            internal set;
        }

        /// <summary>
        /// 获取或设置匹配结果,true,表示已匹配; false,表示未匹配.
        /// </summary>
        public bool MatchResult
        {
            get;
            set;
        }
    }
}
