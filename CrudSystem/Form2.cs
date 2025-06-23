using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrudSystem
{
    public partial class frmView : Form
    {
        public frmView()
        {
            InitializeComponent();
            comboBox();
            //Subjects
            ((ListBox)chklstSubject).DataSource = Subject();
            ((ListBox)chklstSubject).DisplayMember = "subject";
            ((ListBox)chklstSubject).ValueMember = "id";

            //Hobbies
            ((ListBox)chklstHobbies).DataSource = Hobby();
            ((ListBox)chklstHobbies).DisplayMember = "name";
            ((ListBox)chklstHobbies).ValueMember = "id";

        }

        private DataTable Hobby()
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

                //((ListBox)chklstHobbies).DataSource = dd;
                //((ListBox)chklstHobbies).DisplayMember = "name";
                //((ListBox)chklstHobbies).ValueMember = "id";

                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();

                return dd;

            }
            catch (Exception)
            {
                return null;
            }

        }
        private DataTable Subject()
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

                

                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();

                return dd;

            }
            catch (Exception)
            {
                return null;
            }
        }

        private void comboBox()
        {
            MySqlConnection connection;
            MySqlCommand command;
            string connetionString = ConfigurationManager.AppSettings["ConnectionString"];
            string sql = "SELECT `grade` FROM `grades` ";
            connection = new MySqlConnection(connetionString);

            try
            {
                connection.Open();
                command = new MySqlCommand(sql, connection);

                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        cboGrade.Items.Add(reader.GetString(0));
                    }

                }
                reader.Close();
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection ! " + ex.Message.ToString());
            }
        }

        private void benGetData_Click(object sender, EventArgs e)
        {
            MySqlConnection connection;
            MySqlCommand command;
            string connetionString = ConfigurationManager.AppSettings["ConnectionString"];
            string sql = "SELECT s.id, s.admission_no, s.first_name, s.last_name, s.gender, g.grade, GROUP_CONCAT(DISTINCT sub.subject SEPARATOR ',') AS subjects, s.address, s.email, s.phone_no, s.nic, GROUP_CONCAT(DISTINCT h.name SEPARATOR ',') AS hobbies, s.created_at, s.updated_at FROM students s INNER JOIN grades g ON s.grade_id = g.grade_id LEFT JOIN student_subject ss ON s.id = ss.stu_id LEFT JOIN subjects sub ON ss.sub_id = sub.id LEFT JOIN hobby_student hs ON s.id = hs.stu_id LEFT JOIN hobbies h ON hs.hob_id = h.id GROUP BY s.id";
            connection = new MySqlConnection(connetionString);

            try
            {
                connection.Open();
                command = new MySqlCommand(sql, connection);
                MySqlDataReader dr = command.ExecuteReader();
                if (dr.HasRows)
                {
                    //while (dr.Read())
                    //{
                    //    MessageBox.Show("ID: " + dr["id"].ToString() + "\nAdmission No: " + dr["admission_no"].ToString() + "\nFirst Name: " + dr["first_name"].ToString() + "\nLast Name: " + dr["last_name"].ToString());
                    //}

                    DataTable dt = new DataTable();
                    dt.Load(dr);
                    dgvView.DataSource = dt;

                }

                else
                {

                    MessageBox.Show("Now rows found !");

                }

                command.Dispose();
                connection.Close();
                //MessageBox.Show(" Entry have been Viewed Successfully ! !");

            }

            catch (Exception ex)
            {

                MessageBox.Show("Can not open connection ! " + ex.Message.ToString());

            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please Select a row first !");
                return;
            }

            String id = dgvView.SelectedRows[0].Cells["id"].Value.ToString();
            MessageBox.Show("Selected ID: " + id);

            delete(id);

        }

        private static void delete(string id)
        {
            MySqlConnection connection;
            MySqlCommand command;
            string connetionString = ConfigurationManager.AppSettings["ConnectionString"];
            string sql = "DELETE FROM `students` WHERE `id` = '" + id + "' ";
            connection = new MySqlConnection(connetionString);

            try
            {
                connection.Open();
                command = new MySqlCommand(sql, connection);
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
                //MessageBox.Show(" Entry have been deleted Successfully ! !");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection ! " + ex.Message.ToString());
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please Select a row first !");
                return;
            }

            String id = dgvView.SelectedRows[0].Cells["id"].Value.ToString();
            update(id);
            //MessageBox.Show("Selected ID: " + id);

            //Validation
            //if ((!Emptycheck(txtAdmissionNo, "Enter the Admission No!")) && (!Emptycheck(txtFirstName, "Enter the First Name!")) && (!Emptycheck(txtLastName, "Enter the Last Name!")) && (!radioButtonEmptyCheck(rdoMale, rdoFemale, "Must select gender")) && (!comboEmptycheck(cboGrade, "Must select Grade")) && (!Emptycheck(txtAddress, "Enter the Address!")) && (!Emptycheck(txtEmail, "Enter the Email!")) && (!Emptycheck(TxtPhoneNo, "Enter the Phone No!")) && (!Emptycheck(txtNIC, "Enter the NIC No!")) && (!checkedListBoxEmptyCheck(chklstHobbies, "Atlest select one hobby!")) && )
            //{
            //    if (!StringValidation(txtFirstName, "Invalid First Name! Only letters are allowed.") && !StringValidation(txtLastName, "Invalid Last Name! Only letters are allowed.") && !EmailValidation(txtEmail, "Invalid Email") && !PhoneNoValidation(TxtPhoneNo, "Invalid Phone No! Only 10 numbers are allowed.") && !NICValidation(txtNIC, "Invalid NIC No!."))
            //    {
            //        update(id);
            //        clear();
            //    }
            //}
        }

        private void update(string id)
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

            //grade
            string gradeID = cboGrade.SelectedIndex.ToString();

            //Subjects
            MySqlConnection conn;
            MySqlCommand com;
            String constr = ConfigurationManager.AppSettings["ConnectionString"];
            String sql1 = $"DELETE FROM `student_subject` WHERE `stu_id` = '{id}'";
            conn = new MySqlConnection(constr);
            try
            {
                conn.Open();
                com = new MySqlCommand(sql1, conn);
                com.ExecuteNonQuery();
                com.Dispose();

                foreach (int item in chklstSubject.CheckedIndices)
                {

                    String sql2 = $"INSERT INTO `student_subject` (`sub_id`, `stu_id`) VALUES ('{item+1}','{id}')";

                    try
                    {
                        com = new MySqlCommand(sql2, conn);
                        com.ExecuteNonQuery();
                        com.Dispose();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Can not open connection ! " + ex.Message.ToString());
                    }

                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection ! " + ex.Message.ToString());
            }
            

            //hobbies
            String sql3 = $"DELETE FROM `hobby_student` WHERE `stu_id` = '{id}'";
            conn = new MySqlConnection(constr);
            try
            {
                conn.Open();
                com = new MySqlCommand(sql3, conn);
                com.ExecuteNonQuery();
                com.Dispose();

                foreach (int item in chklstHobbies.CheckedIndices)
                {

                    String sql4 = $"INSERT INTO `hobby_student` (`hob_id`, `stu_id`) VALUES ('{item+1}','{id}')";

                    try
                    {
                        com = new MySqlCommand(sql4, conn);
                        com.ExecuteNonQuery();
                        com.Dispose();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Can not open connection ! " + ex.Message.ToString());
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection ! " + ex.Message.ToString());
            }
            finally
            {
                conn.Close();
            }

            MySqlConnection connection;
            MySqlCommand command;
            String connetionString = ConfigurationManager.AppSettings["ConnectionString"];
            String sql = $"UPDATE `students` SET `admission_no`='{txtAdmissionNo.Text}',`first_name` = '{txtFirstName.Text}',`last_name` = '{txtLastName.Text}',`gender` = '{gender}',`grade_id` = '{gradeID}',`address` = '{txtAddress.Text}',`email` = '{txtEmail.Text}',`phone_no` = '{TxtPhoneNo.Text}',`nic` = '{txtNIC.Text}' WHERE `id` = '{id}' ";
            connection = new MySqlConnection(connetionString);

            try
            {
                connection.Open();
                command = new MySqlCommand(sql, connection);
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
                //MessageBox.Show(" Entry have been Updated Successfully ! !");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection ! " + ex.Message.ToString());
            }
        }

        private void btnFillData_Click(object sender, EventArgs e)
        {
            if (dgvView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please Select a row first !");
                return;
            }

            string id = dgvView.SelectedRows[0].Cells["id"].Value.ToString();
            txtID.Text = id;
            string adm = dgvView.SelectedRows[0].Cells["admission_no"].Value.ToString();
            txtAdmissionNo.Text = adm;
            string fname = dgvView.SelectedRows[0].Cells["first_name"].Value.ToString();
            txtFirstName.Text = fname;
            string lname = dgvView.SelectedRows[0].Cells["last_name"].Value.ToString();
            txtLastName.Text = lname;

            //gender
            string gender = dgvView.SelectedRows[0].Cells["gender"].Value.ToString();
            if (gender == "Male")
            {
                rdoMale.Checked = true;
            }
            else if (gender == "Female")
            {
                rdoFemale.Checked = true;
            }
            else
            {
                rdoMale.Checked = false;
                rdoFemale.Checked = false;
            }

            //grade
            string grade = dgvView.SelectedRows[0].Cells["grade"].Value.ToString();
            cboGrade.Text = grade;


            string address = dgvView.SelectedRows[0].Cells["address"].Value.ToString();
            txtAddress.Text = address;
            string email = dgvView.SelectedRows[0].Cells["email"].Value.ToString();
            txtEmail.Text = email;
            string phone = dgvView.SelectedRows[0].Cells["phone_no"].Value.ToString();
            TxtPhoneNo.Text = phone;
            string nic = dgvView.SelectedRows[0].Cells["nic"].Value.ToString();
            txtNIC.Text = nic;

            //subjects
            string subjects = dgvView.SelectedRows[0].Cells["subjects"].Value.ToString();
            string[] subjectsArray = subjects.Split(',', ' ');
            string[] sub = Subject().AsEnumerable()
                .Select(row => row.Field<string>("subject"))
                .ToArray();
            for (int x = 0; chklstSubject.Items.Count > x; x++)
            {
                chklstSubject.SetItemChecked(x, false);
            }
            foreach (var word in subjectsArray)
            {
                int j = 0;
                foreach (var i in sub)
                {
                    if (word == i)
                    {
                        chklstSubject.SetItemChecked(j, true);
                    }
                        j++;
                }

            }

            //hobbies
            string hobbies = dgvView.SelectedRows[0].Cells["hobbies"].Value.ToString();
            string[] hobbiesArray = hobbies.Split(',', ' ');

            string[] hob = Hobby().AsEnumerable()
                .Select(row => row.Field<string>("name"))
                .ToArray();
            for (int x = 0; chklstHobbies.Items.Count > x; x++)
            {
                chklstHobbies.SetItemChecked(x, false);
            }
            foreach (var word in hobbiesArray)
            {
                int j = 0;
                foreach (var i in hob)
                {
                    if (word == i)
                    {
                        chklstHobbies.SetItemChecked(j,true);
                    }
                    j++;
                }
                
            }
        }

        private void btncreate_Click(object sender, EventArgs e)
        {
            frmRegistration frm = new frmRegistration();
            frm.ShowDialog();
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

            if (!Regex.IsMatch(txt.Text, @"^([0-9]{7}[0][0-9]{4})$|(^[0-9]{9}[vVxX])$"))
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

            //cboGrade.Items
            cboGrade.SelectedIndex = -1;

            //Grade Clear
            //chkMaths.Checked = false;
            //chkScience.Checked = false;
            //chkEnglish.Checked = false;
            //chkTamil.Checked = false;
            //chkIT.Checked = false;

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

        private void btnUserEdit_Click(object sender, EventArgs e)
        {
            frmSignUp fs = new frmSignUp();
            fs.ShowDialog();
        }

        private void btnGrade_Click(object sender, EventArgs e)
        {
            frmGrade fs = new frmGrade();
            fs.ShowDialog();
        }


        private void btnShowSubject_Click(object sender, EventArgs e)
        {
            if (dgvView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please Select a row first !");
                return;
            }

            string id = dgvView.SelectedRows[0].Cells["id"].Value.ToString();

            frmSubject f3 = new frmSubject(id);
            f3.ShowDialog();
        }

        private void btnShowHobby_Click(object sender, EventArgs e)
        {
            if (dgvView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please Select a row first !");
                return;
            }

            string id = dgvView.SelectedRows[0].Cells["id"].Value.ToString();

            frmHobby f3 = new frmHobby(id);
            f3.ShowDialog();
        }
    }
}
