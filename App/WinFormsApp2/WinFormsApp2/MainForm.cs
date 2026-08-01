using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp2
{
    public partial class MainForm : Form
    {
        private Form1? ecdsaForm;
        private RSATabControl? rsaControl;

        public MainForm()
        {
            InitializeComponent();

            // 默认选中 ECDSA 标签页
            if (tabControl1 != null)
                tabControl1.SelectedIndex = 0;

            // 将 Form1 嵌入到 ECDSA 标签页
            ecdsaForm = new Form1
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill
            };
            tabEcdsa.Controls.Add(ecdsaForm);
            ecdsaForm.Show();

            // 将 RSATabControl 嵌入到 RSA 标签页
            rsaControl = new RSATabControl
            {
                Dock = DockStyle.Fill
            };
            tabRSA.Controls.Add(rsaControl);
        }
    }
}
