using System;
using System.Text;
using System.Windows.Forms;

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
            for ( int i = 'a'; i <= 'z'; i++ )
            {
                TestObject obj = new TestObject();
                obj.id = (char)i;
                obj.Name = Convert.ToString( (char)i );

                this.comboBox1.Items.Add( obj );
                this.customCompleteTextBox1.Items.Add( obj );
                this.listBox1.Items.Add( obj );
            }

            this.comboBox1.DisplayMember = "idAndName";
            this.comboBox1.ValueMember = "id";
            this.comboBox1.DropDownHeight = 80;
            this.customCompleteTextBox1.DisplayMember = "idAndName";
            this.customCompleteTextBox1.ValueMember = "id";
            this.customCompleteTextBox1.Match += ( o, eve ) =>
            {
                TestObject obj = eve.Item as TestObject;

                if ( Convert.ToString( (char)(obj.id)) == eve.MatchText )
                {
                    eve.MatchResult = true;
                }
            };
		}

        private void button1_Click( object sender, EventArgs e )
		{
			this.comboBox1.DroppedDown = true;
            this.customCompleteTextBox1.DropList();
        }
		
        
	}


    public class TestObject
    {
        public int id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string idAndName
        {
            get
            {
                return this.id.ToString() + "-" + this.Name;
            }
        }
    }
}
