using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ExtLibrary;

namespace CustomCompleteTextBox
{
    /// <summary>
    /// 带下拉列表的自定义搜索文本框
    /// </summary>
	[ToolboxItem( true )]
    [DefaultProperty( "Text" ), DefaultEvent( "Match" )]
    [ToolboxBitmap( typeof( CustomCompleteTextBox ), "Resources.ToolBox.bmp" )]
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

        /// <summary>
        /// 下拉内容宿主窗体
        /// </summary>
        private ToolStripControlHost host;

        /// <summary>
        /// 下拉控件
        /// </summary>
		private ToolStripDropDownExt drop;

        //--------------------------------------------------------------------------------

        /// <summary>
        /// 获取或设置是否自适应下拉列表的宽度, 默认为 true.
        /// </summary>
        [Browsable( true )]
        [Category( "Behavior" )]
        [Description( "是否自适应下拉列表的宽度" )]
        public bool AutoDropWidth
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置下拉列表高度, 默认100.
        /// </summary>
        [Browsable( true )]
        [Category( "Layout" )]
        [Description( "下拉列表高度" )]
        public int DropHeight
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置下拉项目的字体
        /// </summary>
        [Browsable( true )]
        [Category( "Appearance" )]
        [Description( "下拉项目的字体" )]
        public Font ItemFont
        {
            get
            {
                return this.box.Font;
            }
            set
            {
                this.box.Font = value;
            }
        }

        /// <summary>
        /// 获取或设置下拉项目的前景颜色
        /// </summary>
        [Browsable( true )]
        [Category( "Appearance" )]
        [Description( "下拉项目的前景颜色" )]
        public Color ItemForeColor
        {
            get
            {
                return this.box.ForeColor;
            }
            set
            {
                this.box.ForeColor = value;
            }
        }

        /// <summary>
        /// 获取或设置下拉列表的数据集合
        /// </summary>
        [Browsable( true )]
        [Category( "Data" )]
        [Description( "下拉列表的数据集合" )]
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
        [Browsable( false )]
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
        [Browsable( false )]
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
        [Browsable( true )]
        [Category( "Data" )]
        [Description( "显示的绑定属性" )]
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
        [Browsable( true )]
        [Category( "Data" )]
        [Description( "实际值的绑定属性" )]
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
        [Category( "Behavior" )]
        [Description( "当项目进行搜索匹配时引发此事件, 可在此定义匹配规则." )]
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
        }

        /// <summary>
        /// 单击文本框时
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClick( EventArgs e )
        {
            base.OnClick( e );
            this.SelectAll();
            this.Focus();
            this.DropList();
        }

        /// <summary>
        /// 文本框获得焦点时
        /// </summary>
        /// <param name="e"></param>
        protected override void OnEnter( EventArgs e )
        {
            base.OnEnter( e );
            this.SelectAll();
            this.mouseWheel.Enable = true;
            this.DropList();
        }

        /// <summary>
        /// 文本框失去焦点时
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLeave( EventArgs e )
        {
            base.OnLeave( e );
            this.mouseWheel.Enable = false;
            this.CloseList();
        }

        /// <summary>
        /// 文本框内容改变时
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTextChanged( EventArgs e )
        {
            base.OnTextChanged( e );

            this.DropList();
        }

        /// <summary>
        /// 在文本框按下并释放按键时
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyPress( KeyPressEventArgs e )
        {
            //去掉系统提示音
            if ( e.KeyChar == 13 )
            {
                e.Handled = true;
            }

            base.OnKeyPress( e );
        }

        /// <summary>
        /// 在文本框按下按键时
        /// </summary>
        /// <param name="e"></param>
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

        /// <summary>
        /// 处理针对文本框的系统消息
        /// </summary>
        /// <param name="m"></param>
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
			if ( !this.DesignMode )
			{
				List<object> objs = this.MatchAndSetListItems();
                this.SelectedItem = objs.Count > 0 ? objs[0] : null;
                this.box.SelectedItem = this.SelectedItem;

                //如无选中, 将滚动条回到最上方
                if ( this.SelectedItem == null && this.box.Items.Count > 0 )
                {
                    this.box.BeginUpdate();
                    this.box.SelectedIndex = 0;
                    this.box.ClearSelected();
                    this.box.EndUpdate();
                }

                if ( !this.drop.Visible )
                {
                    Screen screent = Screen.FromControl( this );
                    Point showPoint = new Point( 0 - (this.Size.Width - this.ClientSize.Width) / 2, this.Height - (this.Size.Height - this.ClientSize.Height) / 2 );
                    ToolStripDropDownDirection direction = this.drop.Height > screent.WorkingArea.Height - this.PointToScreen( showPoint ).Y ? ToolStripDropDownDirection.AboveRight : ToolStripDropDownDirection.BelowRight;
                    showPoint = direction == ToolStripDropDownDirection.BelowRight ? showPoint : new Point( 0 - (this.Size.Width - this.ClientSize.Width) / 2, 0 - (this.Size.Height - this.ClientSize.Height) / 2 );
                    this.drop.Show( this, showPoint, direction );
                }
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
            this.AutoDropWidth = true;
            this.DropHeight = 100;
            this.innerListBox = new ListBox();
            this.innerListBox.SelectionMode = SelectionMode.One;

            this.box = new ListBox();
            this.box.Font = this.Font;
            this.box.ForeColor = this.ForeColor;
            this.box.Height = this.DropHeight;
            this.box.Margin = Padding.Empty;
            this.box.BorderStyle = BorderStyle.None;
            this.box.SelectionMode = SelectionMode.One;
            this.box.TabStop = false;
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
            this.box.DisplayMember = this.DisplayMember;
            this.box.ValueMember = this.ValueMember;

            this.box.BeginUpdate();
            this.box.Items.Clear();
            Graphics gh = this.box.CreateGraphics();
            SizeF newSize = new SizeF();

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
                        args.MatchResult = displayText.StartsWith( this.Text ) || valueText.StartsWith( this.Text );
                        args.EqualResult = displayText == this.Text || valueText == this.Text;

                        this.OnMatch( args );

                        //部分匹配
                        if ( args.MatchResult )
                        {
                            this.box.Items.Add( obj );

                            if ( this.AutoDropWidth )
                            {
                                //测量当前项目的宽度
                                SizeF currentSize = gh.MeasureString( displayText, this.box.Font );
                                newSize = newSize.Width > currentSize.Width ? newSize : currentSize;
                            }
                        }

                        //完全相等
                        if ( args.EqualResult )
                        {
                            result.Add( obj );
                        }
                    }
                }
                
                this.box.SelectedItem = this.SelectedItem;
            }

            this.box.EndUpdate();

            //计算宽度
            newSize.Width += (this.box.Items.Count * this.box.ItemHeight > this.box.Height ? 18 : 0);
            this.box.Width = Convert.ToInt32( newSize.Width > this.Width - 2 ? newSize.Width : this.Width -2 );
            //设置列表高度, 小于 DropHeight, 收缩高度到刚好容下所有项目, 大于 DropHeight 或列表为空, 则高度为 DropHeight.
            int sumHight = this.box.Items.Count == 0 ? this.DropHeight : this.box.Items.Count * this.box.ItemHeight;
            this.box.Height = sumHight < this.DropHeight ? sumHight : this.DropHeight;

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
            if ( !e.Active )
            {
                this.CloseList();
            }
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
        /// 获取或设置 MatchText 与 Item 中的属性是否部分匹配
        /// </summary>
        public bool MatchResult
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置 MatchText 与 Item 中的属性是否完全相等 
        /// </summary>
        public bool EqualResult
        {
            get;
            set;
        }


        /// <summary>
        /// 默认构造函数
        /// </summary>
        public MatchEventArgs()
        {
            this.Item = null;
            this.MatchText = string.Empty;
            this.MatchResult = false;
            this.EqualResult = false;
        }
    }
}
