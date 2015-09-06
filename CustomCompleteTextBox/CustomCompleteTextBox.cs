using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace ExtLibrary
{
    [ToolboxItem( true )]
    public partial class AutoCompleteTextBox : TextBox
    {
        private ListBox box;
        private ToolStripControlHost host;
        private ToolStripDropDown drop;

        /// <summary>
        /// 获取数据集合
        /// </summary>
        public List<object> Items
        {
            get;
            set;
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

            this.drop = new ToolStripDropDown();
            this.drop.AutoClose = false;
            this.drop.Items.Add( host );
            this.drop.Margin = Padding.Empty;
            this.drop.Padding = new Padding( 1 );
            this.drop.ShowItemToolTips = false;
            this.drop.TabStop = false;
            this.drop.Opening += Drop_Opening;
            this.drop.Opened += Drop_Opened;
            this.drop.Closing += Drop_Closing;
            this.drop.Closed += Drop_Closed;
            
            MouseWheelRedirector.Attach( this.box );
        }

        private void Box_Click( object sender, EventArgs e )
        {
            this.Text = this.box.SelectedItem.ToString();
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

        private void Drop_Opened( object sender, EventArgs e )
        {
        }

        private void Drop_Closing( object sender, ToolStripDropDownClosingEventArgs e )
        {
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

        public void ShowList()
        {
            //this.drop.AutoClose = false;
            this.drop.Show( this, new Point( -2, this.Height - 1 ) );
        }

        private void CloseList()
        {
            //this.drop.AutoClose = true;
            this.drop.Close();
        }
    }


    public class MouseWheelRedirector : IMessageFilter
    {
        private static MouseWheelRedirector instance = null;
        private static bool _active = false;
        public static bool Active
        {
            get
            {
                return _active;
            }
            set
            {
                if ( _active != value )
                {
                    _active = value;
                    if ( _active )
                    {
                        if ( instance == null )
                        {
                            instance = new MouseWheelRedirector();
                        }
                        Application.AddMessageFilter( instance );
                    }
                    else
                    {
                        if ( instance != null )
                        {
                            Application.RemoveMessageFilter( instance );
                        }
                    }
                }
            }
        }

        public static void Attach( Control control )
        {
            if ( !_active )
                Active = true;
            control.MouseEnter += instance.ControlMouseEnter;
            control.MouseLeave += instance.ControlMouseLeaveOrDisposed;
            control.Disposed += instance.ControlMouseLeaveOrDisposed;
        }

        public static void Detach( Control control )
        {
            if ( instance == null )
                return;
            control.MouseEnter -= instance.ControlMouseEnter;
            control.MouseLeave -= instance.ControlMouseLeaveOrDisposed;
            control.Disposed -= instance.ControlMouseLeaveOrDisposed;
            if ( object.ReferenceEquals( instance.currentControl, control ) )
                instance.currentControl = null;
        }

        private MouseWheelRedirector()
        {
        }


        private Control currentControl;
        private void ControlMouseEnter( object sender, System.EventArgs e )
        {
            var control = (Control)sender;
            if ( !control.Focused )
            {
                currentControl = control;
            }
            else
            {
                currentControl = null;
            }
        }

        private void ControlMouseLeaveOrDisposed( object sender, System.EventArgs e )
        {
            if ( object.ReferenceEquals( currentControl, sender ) )
            {
                currentControl = null;
            }
        }

        private const int WM_MOUSEWHEEL = 0x20a;

        public bool PreFilterMessage( ref Message m )
        {
            if ( currentControl != null && m.Msg == WM_MOUSEWHEEL )
            {
                SendMessage( currentControl.Handle, m.Msg, m.WParam, m.LParam );
                return true;
            }
            else
            {
                return false;
            }
        }

        [DllImport( "user32.dll", SetLastError = false )]
        private static extern IntPtr SendMessage( IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam );
    }
}
