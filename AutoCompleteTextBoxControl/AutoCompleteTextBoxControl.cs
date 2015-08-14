using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtLibrary
{
	[ToolboxBitmap( typeof( ComboBox ) )]
	[ToolboxItem( true )]
	public partial class AutoCompleteTextBox : TextBox
	{
		private ListBox box;
		private ToolStripControlHost host;
		private ToolStripDropDown drop;

		/// <summary>
		/// 获取数据集合
		/// </summary>
		public ListBox.ObjectCollection Items
		{
			get
			{
				return this.box.Items;
			}
		}

		/// <summary>
		/// 构造函数
		/// </summary>
		public AutoCompleteTextBox() : base()
		{
			this.InitControl();
		}


		protected override void InitLayout()
		{
			base.InitLayout();

			this.box.Width = this.Width - 2;
		}

		protected override void OnGotFocus( EventArgs e )
		{
			base.OnGotFocus( e );

			this.ShowList();
		}

		protected override void OnLostFocus( EventArgs e )
		{
			base.OnLostFocus( e );

			this.CloseList();
		}


		private void InitControl()
		{
			this.box = new ListBox();
			this.box.Margin = Padding.Empty;
			this.box.BorderStyle = BorderStyle.None;
			this.box.TabStop = false;
			this.box.SelectionMode = SelectionMode.One;
			this.box.IntegralHeight = false;
            this.box.MouseMove += Box_MouseMove;

			this.host = new ToolStripControlHost( box );
			this.host.Margin = Padding.Empty;
			this.host.Padding = Padding.Empty;
			this.host.AutoSize = false;
			this.host.MouseEnter += Host_MouseEnter;

			this.drop = new ToolStripDropDown();
			this.drop.Items.Add( host );
			this.drop.Margin = Padding.Empty;
			this.drop.Padding = new Padding(1);
			this.drop.ShowItemToolTips = false;
			this.drop.TabStop = false;
			this.drop.Closed += Drop_Closed;
			this.drop.VisibleChanged += Drop_VisibleChanged;
		}

		private void Drop_VisibleChanged( object sender, EventArgs e )
		{
			if ( !this.drop.Visible )
			{
				this.box.SelectedIndex = -1;
			}
		}

		private void Drop_Closed( object sender, ToolStripDropDownClosedEventArgs e )
		{
			this.box.SelectedIndex = -1;
		}

		private void Host_MouseEnter( object sender, EventArgs e )
		{
			//MessageBox.Show("host");
		}

		private void Box_MouseMove( object sender, MouseEventArgs e )
		{
			this.box.SelectedIndex = this.box.IndexFromPoint( e.Location );
		}

		public void ShowList()
		{
			this.drop.Show( this, new Point(-2,this.Height-1)  );
			this.box.Select();
		}

		private void CloseList()
		{
			//this.drop.Close();
		}
	}
}
