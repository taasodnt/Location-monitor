using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp4.MyClasses;

namespace WindowsFormsApp4
{
    public partial class AddBeaconForm : Form
    {
        String text = null;
        public AddBeaconForm(DataSourceManager dataSourceManager)
        {
            InitializeComponent();
            beaconListCB.Select();
            foreach (string beaconMac in dataSourceManager.getBeaconFromList())
            {
                beaconListCB.Items.Add(beaconMac);
            }
            beaconListCB.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            beaconListCB.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void AddBeaconForm_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
        }

        public string getBeaconMac
        {
            get
            {
                return beaconListCB.Text;
            }
        }

        private void beaconListCB_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void confirmBtn_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Close addBeaconForm");
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }
    }
}
