using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Script.Serialization;
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
            List<Station> stations = new List<Station>();
            JavaScriptSerializer json = new JavaScriptSerializer();

            using ( StreamReader sr = File.OpenText( "Stations.txt" ) )
            {
                stations = json.Deserialize<List<Station>>( sr.ReadToEnd() );
            }

            this.listBox1.DisplayMember = "ChineseName";
            this.listBox1.ValueMember = "Code";
            this.listBox1.Items.AddRange( stations.ToArray() );

            this.comboBox1.DisplayMember = "ChineseName";
            this.comboBox1.ValueMember = "Code";
            this.comboBox1.Items.AddRange( stations.ToArray() );
            this.comboBox1.DropDownHeight = 80;

            this.customCompleteTextBox1.DisplayMember = "ChineseName";
            this.customCompleteTextBox1.ValueMember = "Code";
            this.customCompleteTextBox1.Items.AddRange( stations.ToArray() );
            //this.customCompleteTextBox1.Match += ( o, eve ) =>
            //{
            //    Station obj = eve.Item as Station;

            //    if ( obj.ThreedLetterCode == eve.MatchText || obj.FullPinYin == eve.MatchText 
            //        || obj.ChineseName == eve.MatchText || obj.SimplePinYin == eve.MatchText )
            //    {
            //        eve.MatchResult = true;
            //    }
            //};
		}

        private void button1_Click( object sender, EventArgs e )
		{
			this.comboBox1.DroppedDown = true;
            this.customCompleteTextBox1.DropList();
        }
		
        
	}
}
