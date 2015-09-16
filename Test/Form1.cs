using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
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
            this.InitData();
        }

        private void button1_Click( object sender, EventArgs e )
        {
            this.InitData();
            this.comboBox1.DroppedDown = true;
            this.customCompleteTextBox1.DropList();
        }


        private void InitData()
        {
            List<Station> stations = new List<Station>();
            JavaScriptSerializer json = new JavaScriptSerializer();

            using ( StreamReader sr = File.OpenText( "Stations.txt" ) )
            {
                stations = json.Deserialize<List<Station>>( sr.ReadToEnd() );
            }
            
            this.listBox1.Items.Clear();
            this.comboBox1.Items.Clear();
            this.customCompleteTextBox1.Items.Clear();

            //Thread.Sleep( 200 );

            this.listBox1.DisplayMember = "ChineseName";
            this.listBox1.ValueMember = "Code";
            this.listBox1.Items.AddRange( stations.ToArray() );

            this.comboBox1.DisplayMember = "ChineseName";
            this.comboBox1.ValueMember = "Code";
            this.comboBox1.Items.AddRange( stations.ToArray() );
            this.comboBox1.DropDownHeight = 80;

            this.customCompleteTextBox1.DisplayMember = "ChineseName";
            this.customCompleteTextBox1.ValueMember = "Code";
            this.customCompleteTextBox1.DropHeight = 120;
            //this.customCompleteTextBox1.ItemForeColor = System.Drawing.Color.Red;
            this.customCompleteTextBox1.ItemFont = new System.Drawing.Font( "微软雅黑", 10 );
            this.customCompleteTextBox1.Items.AddRange( stations.ToArray() );
            this.customCompleteTextBox1.Match += ( o, eve ) =>
            {
                Station obj = eve.Item as Station;

                if ( obj.ThreedLetterCode.StartsWith( eve.MatchText ) || obj.ChineseName.StartsWith( eve.MatchText )
                    || obj.SimplePinYin.StartsWith( eve.MatchText ) || obj.FullPinYin.StartsWith( eve.MatchText ) )
                {
                    eve.MatchResult = true;
                }

                if ( obj.ThreedLetterCode == eve.MatchText || obj.ChineseName == eve.MatchText
                    || obj.SimplePinYin == eve.MatchText || obj.FullPinYin == eve.MatchText )
                {
                    eve.EqualResult = true;
                }
            };
        }
    }
}
