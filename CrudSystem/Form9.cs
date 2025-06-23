using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace CrudSystem
{
    public partial class frmHobby : Form
    {
        public string id { get; set; }
        public frmHobby(string id)
        {
            InitializeComponent();
            this.id = id;
        }

        private void frmHobby_Load(object sender, EventArgs e)
        {
            comboBox();
            gridview();
            HobbyCount();
        }

        private void comboBox()
        {
            string connetionString = null;
            MySqlConnection connection;
            MySqlCommand command;
            string sql = null;
            connetionString = ConfigurationManager.AppSettings["ConnectionString"];
            sql = "SELECT `name`,`id` FROM `Hobbies` WHERE `id` NOT IN (SELECT `hob_id` FROM `hobby_student` WHERE `stu_id` = '"+id+"')";
            connection = new MySqlConnection(connetionString);

            try
            {
                connection.Open();
                command = new MySqlCommand(sql, connection);

                MySqlDataReader reader = command.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Load(reader);

                cboHobbies.DataSource = dt;
                cboHobbies.DisplayMember = "name";
                cboHobbies.ValueMember = "id";

                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection ! " + ex.Message.ToString());
            }
        }

        private void gridview()
        {
            string connetionString = null;
            MySqlConnection connection;
            MySqlCommand command;
            string sql = null;
            connetionString = ConfigurationManager.AppSettings["ConnectionString"];
            sql = "SELECT `students`.`first_name`,`hob_id`,`hobbies`.`name` FROM `hobbies` INNER JOIN `hobby_student` ON `hobbies`.`id`=`hobby_student`.`hob_id` INNER JOIN `students` ON `students`.`id` = `hobby_student`.`stu_id` WHERE `stu_id`=" + id + " ";
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
                    dgvStudentHobby.DataSource = dt;

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
            if (dgvStudentHobby.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please Select a row first !");
                return;
            }

            String hob_id = dgvStudentHobby.SelectedRows[0].Cells["hob_id"].Value.ToString();

            string connetionString = null;
            MySqlConnection connection;
            MySqlCommand command;
            string sql = null;
            connetionString = ConfigurationManager.AppSettings["ConnectionString"];
            sql = "DELETE FROM `hobby_student` WHERE `stu_id` = '" + id + "' AND `hob_id` = '" + hob_id + "';";
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
            gridview();
            HobbyCount();
            comboBox();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            String hobID = cboHobbies.SelectedValue.ToString();

            string connetionString = null;
            MySqlConnection connection;
            MySqlCommand command;
            string sql = null;
            connetionString = ConfigurationManager.AppSettings["ConnectionString"];
            sql = "INSERT INTO `hobby_student` (`stu_id`, `hob_id`) VALUES ('" + id + "', '" + hobID + "')";
            connection = new MySqlConnection(connetionString);

            try
            {
                connection.Open();
                command = new MySqlCommand(sql, connection);
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection ! " + ex.Message.ToString());
            }
            gridview();
            HobbyCount();
            comboBox();
        }

        private void HobbyCount()
        {
            string connetionString = null;
            MySqlConnection connection;
            MySqlCommand command;
            string sql = null;
            connetionString = ConfigurationManager.AppSettings["ConnectionString"];
            sql = "SELECT COUNT(`hob_id`) FROM `hobby_student` WHERE `stu_id` = '" + id + "' ";
            connection = new MySqlConnection(connetionString);

            try
            {
                connection.Open();
                command = new MySqlCommand(sql, connection);
                //command.ExecuteNonQuery();
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    string count = Convert.ToString(result);
                    lblHobbyCount.Text = "Total Hobbies: " + count;
                }
                command.Dispose();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection ! " + ex.Message.ToString());
            }
        }
    }
}
