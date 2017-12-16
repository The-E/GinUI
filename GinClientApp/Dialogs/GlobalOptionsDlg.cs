using System;
using System.Windows.Forms;
using static GinClientApp.GlobalOptions;

namespace GinClientApp.Dialogs
{
    public partial class GlobalOptionsDlg : Form
    {
        public GlobalOptions Options { get; private set; }

        public GlobalOptionsDlg(GlobalOptions options)
        {
            InitializeComponent();
            Options = options;

            if (Options.RepositoryCheckoutOption == CheckoutOption.AnnexCheckout)
                cmbCheckoutOptions.SelectedIndex = 0;

            for (var index = 0; index < cmbAutoUpdates.Items.Count; index++)
            {
                var item = (string)cmbAutoUpdates.Items[index];
                if (item.Contains(Options.RepositoryUpdateInterval.ToString()))
                {
                    cmbAutoUpdates.SelectedIndex = index;
                    break;
                }
            }
        }

        private void GlobalOptions_Load(object sender, EventArgs e)
        {

        }

        private void cmbAutoUpdates_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbAutoUpdates.SelectedIndex)
            {
                case 0:
                    Options.RepositoryUpdateInterval = 0;
                    break;
                case 1:
                    Options.RepositoryUpdateInterval = 5;
                    break;
                case 2:
                    Options.RepositoryUpdateInterval = 15;
                    break;
                case 3:
                    Options.RepositoryUpdateInterval = 30;
                    break;
                case 4:
                    Options.RepositoryUpdateInterval = 60;
                    break;
                default:
                    Options.RepositoryUpdateInterval = 0;
                    break;
            }
        }

        private void cmbCheckoutOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbCheckoutOptions.SelectedIndex)
            {
                case 0:
                    Options.RepositoryCheckoutOption = CheckoutOption.AnnexCheckout;
                    break;
                case 1:
                    Options.RepositoryCheckoutOption = CheckoutOption.FullCheckout;
                    break;
                default:
                    Options.RepositoryCheckoutOption = CheckoutOption.AnnexCheckout;
                    break;
            }
        }
    }
}
