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
	[ToolboxItem( true)]
	public partial class AutoCompleteTextBox: TextBox
    {
		private ListBox view = new ListBox();

		/// <summary>
		/// 获取自动完成列表
		/// </summary>
		public ListBox DropDownList
		{
			get
			{
				return this.view;
			}
		}

		public AutoCompleteTextBox() : base()
		{
			this.InitControl();
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
			this.view.Width = this.Width;
			this.view.Font = this.Font;
			this.view.IntegralHeight = false;
		}

		private void ShowList()
		{
		}

		private void CloseList()
		{
		}
	}
}
