using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using TextBox = System.Windows.Forms.TextBox;

namespace CrudSystem
{
    public partial class frmRegistration : Form
    {
        //const string regex = @"^\d+$";
        public Int32 newId;
        public frmRegistration()
        {
            InitializeComponent();
            comboBox();
            Subject();
            Hobby();

        }

        private void Subject()
        {

            MySqlConnection connection;
            MySqlCommand command;
            string connetionString = ConfigurationManager.AppSettings["ConnectionString"];
            string sql = "SELECT `id`,`subject` FROM `subjects` ";
            connection = new MySqlConnection(connetionString);

            try
            {
                connection.Open();
                command = new MySqlCommand(sql, connection);

                MySqlDataReader reader = command.ExecuteReader();

                DataTable dd = new DataTable();
                dd.Load(reader);

                ((ListBox)chklstSubject).DataSource = dd;
                ((ListBox)chklstSubject).DisplayMember = "subject";
                ((ListBox)chklstSubject).ValueMember = "id";

                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection !" + ex.Message.ToString());
            }
        }

        private void Hobby()
        {
            MySqlConnection connection;
            MySqlCommand command;
            string connetionString = ConfigurationManager.AppSettings["ConnectionString"];
            string sql = "SELECT `id`,`name` FROM `hobbies` ";
            connection = new MySqlConnection(connetionString);

            try
            {
                connection.Open();
                command = new MySqlCommand(sql, connection);

                MySqlDataReader reader = command.ExecuteReader();

                DataTable dd = new DataTable();
                dd.Load(reader);

                ((ListBox)chklstHobbies).DataSource = dd;
                ((ListBox)chklstHobbies).DisplayMember = "name";
                ((ListBox)chklstHobbies).ValueMember = "id";

                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection !" + ex.Message.ToString());
            }

        }



        private void comboBox()
        {
            MySqlConnection connection;
            MySqlCommand command;
            string connetionString = ConfigurationManager.AppSettings["ConnectionString"];
            string sql = "SELECT `grade`,`grade_id` FROM `grades` ";
            connection = new MySqlConnection(connetionString);

            try
            {
                connection.Open();
                command = new MySqlCommand(sql, connection);

                MySqlDataReader reader = command.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Load(reader);

                cboGrade.DataSource = dt;
                cboGrade.DisplayMember = "grade";
                cboGrade.ValueMember = "grade_id";

                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection !" + ex.Message.ToString());
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {

            //Validation
            //if ((!Emptycheck(txtAdmissionNo, "Enter the Admission No!")) && (!Emptycheck(txtFirstName, "Enter the First Name!")) && (!Emptycheck(txtLastName, "Enter the Last Name!")) && (!radioButtonEmptyCheck(rdoMale, rdoFemale, "Must select gender")) && (!comboEmptycheck(cboGrade, "Must select Grade")) && (!Emptycheck(txtAddress, "Enter the Address!")) && (!Emptycheck(txtEmail, "Enter the Email!")) && (!Emptycheck(TxtPhoneNo, "Enter the Phone No!")) && (!Emptycheck(txtNIC, "Enter the NIC No!")) && (!checkedListBoxEmptyCheck(chklstSubject,"Atleast Select one Subject!")) && (!checkedListBoxEmptyCheck(chklstHobbies, "Atlest select one hobby!")))
            //{
            //    if (!StringValidation(txtFirstName, "Invalid First Name! Only letters are allowed.") && !StringValidation(txtLastName, "Invalid Last Name! Only letters are allowed.") && !EmailValidation(txtEmail, "Invalid Email") && !PhoneNoValidation(TxtPhoneNo, "Invalid Phone No! Only 10 numbers are allowed.") && !NICValidation(txtNIC, "Invalid NIC No!."))
            //    {
            //        bool flowControl = create();
            //        if (!flowControl)
            //        {
            //            return;
            //        }
            //    }

            //}

            create();

        }

        private bool create()
        {
            //gender
            string gender = "";
            if (rdoMale.Checked == true)
            {
                gender = rdoMale.Text;
            }
            else if (rdoFemale.Checked)
            {
                gender = rdoFemale.Text;
            }

            //grade_id
            String gradeID = cboGrade.SelectedIndex.ToString();

            MySqlConnection connection;
            MySqlCommand command;
            MySqlCommand mySqlCommand;
            string connetionString = ConfigurationManager.AppSettings["ConnectionString"];
            string sql = $"INSERT INTO `students` (`admission_no`,`first_name`,`last_name`,`gender`,`address`,`email`,`phone_no`,`nic`,`grade_id`)VALUES('{ txtAdmissionNo.Text }','{ txtFirstName.Text }','{ txtLastName.Text }','{ gender }','{ txtAddress.Text }','{ txtEmail.Text }','{ TxtPhoneNo.Text }','{ txtNIC.Text }','{ gradeID }')";
            connection = new MySqlConnection(connetionString);

            try
            {
                connection.Open();
                command = new MySqlCommand(sql, connection);

                //command.ExecuteNonQuery();
                command.ExecuteScalar();

                newId = Convert.ToInt32(command.LastInsertedId.ToString());
                MessageBox.Show("Save data with ID:"+ newId.ToString());

                command.Dispose();

                
                foreach(DataRowView item in chklstSubject.CheckedItems)
                {
                    String subID = item["id"].ToString();
                    string sql2 = "INSERT INTO `student_subject` (`stu_id`, `sub_id`) VALUES('{ newId }', '{ subID }');";
                    mySqlCommand = new MySqlCommand(sql2, connection);
                    mySqlCommand.ExecuteNonQuery();
                    mySqlCommand.Dispose();
                }

                foreach (DataRowView item1 in chklstHobbies.CheckedItems)
                {
                    String hobID = item1["id"].ToString();
                    string sql3 = "INSERT INTO `hobby_student` (`stu_id`, `hob_id`) VALUES('{ newId }', '{ hobID }');";
                    mySqlCommand = new MySqlCommand(sql3, connection);
                    mySqlCommand.ExecuteNonQuery();
                    mySqlCommand.Dispose();
                }

                connection.Close();

                DialogResult result = MessageBox.Show("Do you want to add more students", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    clear();
                }
                else
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Can not open connection! { ex.Message}");
            }



            return true;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clear();

        }

        private void clear()
        {
            txtAdmissionNo.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();

            //Gender Clear
            rdoMale.Checked = false;
            rdoFemale.Checked = false;

            //Grade clear
            cboGrade.SelectedIndex = -1;

            //Subject Clear
            foreach (int j in chklstSubject.CheckedIndices)
            {
                chklstSubject.SetItemChecked(j, false);
            }

            txtAddress.Clear();
            txtEmail.Clear();
            TxtPhoneNo.Clear();
            txtNIC.Clear();

            //Hobbies Clear
            foreach (int i in chklstHobbies.CheckedIndices)
            {
                chklstHobbies.SetItemChecked(i, false);
            }


        }
        Boolean StringValidation(TextBox txt, String msg)
        {
            if (!Regex.IsMatch(txt.Text, @"^[A-Za-z]+$"))
            {
                MessageBox.Show(msg, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt.Focus();
                return true;
            }
            else
            {
                return false;
            }

        }

        Boolean Emptycheck(TextBox txt, String msg)
        {
            if (String.IsNullOrWhiteSpace(txt.Text))
            {
                MessageBox.Show(msg, "EmptyCheck", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt.Focus();
                return true;
            }
            else
            {
                return false;
            }


        }

        Boolean comboEmptycheck(ComboBox cbo, String msg)
        {
            if (cbo.SelectedIndex == -1)
            {
                MessageBox.Show(msg, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbo.Focus();
                return true;
            }
            else
            {
                return false;
            }

        }

        Boolean checkBoxEmptyCheck(CheckBox chk1, CheckBox chk2, CheckBox chk3, CheckBox chk4, CheckBox chk5, String msg)
        {
            if (!(chk1.Checked || chk2.Checked || chk3.Checked || chk4.Checked || chk5.Checked))
            {
                MessageBox.Show(msg, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }
            else
            {
                return false;
            }
        }

        Boolean checkedListBoxEmptyCheck(CheckedListBox clb, String msg)
        {
            if (clb.CheckedItems.Count == 0)
            {
                MessageBox.Show(msg, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }
            else
            {
                return false;
            }
        }

        Boolean radioButtonEmptyCheck(RadioButton rdo1, RadioButton rdo2, String msg)
        {
            if (!(rdo1.Checked || rdo2.Checked))
            {
                MessageBox.Show(msg, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }
            else
            {
                return false;
            }
        }
        Boolean NICValidation(TextBox txt, String msg)
        {
            
            if (!Regex.IsMatch(txt.Text, @"^([0-9]{7}[0]{1}[0-9]{4})$|(^[0-9]{9}[vVxX])$"))
            {
                MessageBox.Show(msg, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt.Focus();
                return true;
            }
            else
            {
                return false;
            }
        }
        Boolean EmailValidation(TextBox txt, String msg)
        {
            // Assuming Email is in standard format
            if (!Regex.IsMatch(txt.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show(msg, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt.Focus();
                return true;
            }
            else
            {
                return false;
            }
        }

        Boolean PhoneNoValidation(TextBox txt, String msg)
        {
            // Assuming Phone No is a 10 digit number
            if (!Regex.IsMatch(txt.Text, @"^\d{10}$"))
            {
                MessageBox.Show(msg, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt.Focus();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
