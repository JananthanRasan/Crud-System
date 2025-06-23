using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrudSystem
{
    public partial class Form8 : Form
    {
        public Form8()
        {
            InitializeComponent();
        }

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmView frm = new frmView();
            frm.MdiParent = this;
            frm.Show();
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRegistration frm = new frmRegistration();
            frm.MdiParent = this;
            frm.Show();
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin frm = new frmLogin();
            frm.MdiParent = this;
            frm.Show();
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSignUp frm = new frmSignUp();
            frm.MdiParent = this;
            frm.Show();
        }

        private void view2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmView2 frm = new frmView2();
            frm.MdiParent = this;
            frm.Show();
        }

        private void gradeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmGrade frm = new frmGrade();
            frm.MdiParent = this;
            frm.Show();
        }

    }
}
