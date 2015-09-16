using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace CustomCompleteTextBox
{
    public class CustomCompleteTextBoxDesigner : ControlDesigner
    {
        private IComponentChangeService changeService = null;

        public override void Initialize( IComponent component )
        {
            base.Initialize( component );

            this.changeService = this.GetService( typeof( IComponentChangeService ) ) as IComponentChangeService;

            if ( this.changeService != null )
            {
                this.changeService.ComponentAdding += ChangeService_ComponentAdding;
            }
        }

        private void ChangeService_ComponentAdding( object sender, ComponentEventArgs e )
        {
            CustomCompleteTextBox box = e.Component as CustomCompleteTextBox;

            MessageBox.Show( box.Name );
            box.Enter += ( o, eve ) =>
            {
                MessageBox.Show("bbb");
                box.CloseList();
            };

            box.TextChanged += ( o, eve ) =>
            {
                box.CloseList();
            };
        }
    }
}
