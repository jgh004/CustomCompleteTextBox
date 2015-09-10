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
        /// 显示候选列表
        /// </summary>
		private ListBox box;
		private ToolStripControlHost host;

        /// <summary>
        /// 下拉控件
        /// </summary>
		private ToolStripDropDownExt drop;

		/// <summary>
		/// 获取数据集合
		/// </summary>
		public List<object> Items
		{
			get;
			set;
		}

        public object SelectedItem
        {
            get;
            set;
        }


		/// <summary>
		/// 构造函数
		/// </summary>
		public CustomCompleteTextBox()
			: base()
		{
			this.InitControl();
		}


		protected override void InitLayout()
		{
			base.InitLayout();

			this.box.Width = this.Width - 2;
		}

		protected override void OnClick( EventArgs e )
		{
			base.OnClick( e );
			this.ShowList();
		}

		protected override void OnEnter( EventArgs e )
		{
			base.OnEnter( e );
			this.ShowList();
		}

		protected override void OnLeave( EventArgs e )
		{
			base.OnLeave( e );
			this.CloseList();
		}

		protected override void OnTextChanged( EventArgs e )
		{
			base.OnTextChanged( e );

			if ( this.Items != null )
			{
				this.box.Items.Clear();

				if ( this.Text == string.Empty )
				{
					this.box.Items.AddRange( this.Items.ToArray() );
				}
				else
				{
					List<object> newList = new List<object>();

					for ( int i = 0; i < this.Items.Count; i++ )
					{
						object obj = this.Items[i];

						if ( obj != null )
						{
							if ( obj.ToString().IndexOf( this.Text, StringComparison.OrdinalIgnoreCase ) >= 0 )
							{
								newList.Add( obj );
							}
						}
					}

					this.box.Items.AddRange( newList.ToArray() );
				}
			}
		}

        
        public void ShowList()
        {
            if ( !this.drop.Visible )
            {
                this.drop.Show( this, new Point( -2, this.Height - 1 ) );
            }
        }

        public void CloseList()
        {
            this.drop.Close();
        }


        private void InitControl()
		{
			this.Items = new List<object>();

			this.box = new ListBox();
			this.box.Margin = Padding.Empty;
			this.box.BorderStyle = BorderStyle.None;
			this.box.TabStop = false;
			this.box.SelectionMode = SelectionMode.One;
			this.box.IntegralHeight = false;
			this.box.MouseMove += Box_MouseMove;
			this.box.Click += Box_Click;

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
			this.drop.Opening += Drop_Opening;
			this.drop.Closed += Drop_Closed;
			this.drop.ActiveChange += drop_ActiveChange;

			new MouseWheelRedirectorFilterHelper( this.box ).StartFilter();
			new AppClickFilterHelper( () =>
			{
				this.CloseList();
			}, this, this.drop, this.box ).StartFilter();
		}


        private void drop_ActiveChange( object sender, ActiveChangeEventArgs e )
		{
			if ( !this.drop.AutoClose && !e.Active )
			{
                this.CloseList();
			}
		}

		private void Box_Click( object sender, EventArgs e )
		{
            this.SelectedItem = this.box.SelectedItem;
			this.CloseList();
		}

		private void Drop_Opening( object sender, CancelEventArgs e )
		{
			this.OnTextChanged( EventArgs.Empty );

			if ( this.box.Items != null && this.Text != string.Empty )
			{
				if ( this.box.Items.Contains( this.Text ) )
				{
					this.box.SelectedIndex = this.box.Items.IndexOf( this.Text );
				}
			}
		}

		private void Drop_Closed( object sender, ToolStripDropDownClosedEventArgs e )
		{
			this.SelectAll();
			this.box.SelectedIndex = -1;
		}

		private void Box_MouseMove( object sender, MouseEventArgs e )
		{
			this.box.SelectedIndex = this.box.IndexFromPoint( e.Location );
		}
	}
}
