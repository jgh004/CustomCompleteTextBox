using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using System.IO;

namespace Test
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load( object sender, EventArgs e )
		{

		}

		private void button1_Click( object sender, EventArgs e )
		{
			this.comboBox1.DropDownHeight = 80;

			for ( int i = 0; i < 15; i++ )
			{
				this.comboBox1.Items.Add( "bbbbbbbb" );
				this.autoCompleteTextBox1.Items.Add( "bbbbbbbb" );
			}

			this.comboBox1.DroppedDown = true;
			this.autoCompleteTextBox1.ShowList();
		}
		
	}
}
