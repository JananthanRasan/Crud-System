using CrudSystem.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using ListBox = System.Windows.Forms.ListBox;

namespace CrudSystem
{
    public partial class ClassStudent : Form
    {
        Students Stu = new Students();
        public ClassStudent()
        {
            InitializeComponent();
            Subject();
            Hobby();
            Grade();
        }

        private void Subject()
        {
            DataTable dt = Stu.Subject();
            ((ListBox)chklstSubject).DataSource = dt;
            ((ListBox)chklstSubject).DisplayMember = "subject";
            ((ListBox)chklstSubject).ValueMember = "id";
        }

        private void Hobby()
        {
            DataTable dt = Stu.Hobby();
            ((ListBox)chklstHobbies).DataSource = dt;
            ((ListBox)chklstHobbies).DisplayMember = "name";
            ((ListBox)chklstHobbies).ValueMember = "id";
        }

        private void Grade()
        {
            DataTable dt = Stu.Grade();
            cboGrade.DataSource = dt;
            cboGrade.DisplayMember = "grade";
            cboGrade.ValueMember = "grade_id";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Stu.Insert(rdoMale, rdoFemale, cboGrade, chklstSubject, chklstHobbies, txtAdmissionNo, txtFirstName, txtLastName, txtAddress, txtEmail, TxtPhoneNo, txtNIC);
        }
    }
}
