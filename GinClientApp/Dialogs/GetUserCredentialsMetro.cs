using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GinClientApp.Dialogs
{
    public partial class GetUserCredentialsMetro : MetroFramework.Forms.MetroForm
    {
        public GetUserCredentialsMetro()
        {
            InitializeComponent();
            metroLabel1.TabStop = false;
            metroLabel2.TabStop = false;
        }
    }
}
