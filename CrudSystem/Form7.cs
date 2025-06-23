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
    public partial class frmSubject : Form
    {
        public string id { get; set; }
        public frmSubject(string id)
        {
            InitializeComponent();
            this.id = id;
            subjectCount();
            
        }

        private void frmSubject_Load(object sender, EventArgs e)
        {
            comboBox();
            gridview();
        }

        private void comboBox()
        {
            string connetionString = null;
            MySqlConnection connection;
            MySqlCommand command;
            string sql = null;
            connetionString = ConfigurationManager.AppSettings["ConnectionString"];
            sql = "SELECT `subject`,`id` FROM `subjects` WHERE `id` NOT IN (SELECT `sub_id` FROM `student_subject` WHERE `stu_id` = '"+id+"') ";
            connection = new MySqlConnection(connetionString);

            try
            {
                connection.Open();
                command = new MySqlCommand(sql, connection);

                MySqlDataReader reader = command.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Load(reader);

                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "subject";
                comboBox1.ValueMember = "id";

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
            sql = "SELECT `students`.`first_name`,`sub_id`,`subjects`.`subject` FROM `subjects` INNER JOIN `student_subject` ON `subjects`.`id`=`student_subject`.`sub_id` INNER JOIN `students` ON `students`.`id` = `student_subject`.`stu_id` WHERE `stu_id`=" + id + " ";
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
                    dataGridView1.DataSource = dt;

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
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please Select a row first !");
                return;
            }

            String sub_id = dataGridView1.SelectedRows[0].Cells["sub_id"].Value.ToString();

            string connetionString = null;
            MySqlConnection connection;
            MySqlCommand command;
            string sql = null;
            connetionString = ConfigurationManager.AppSettings["ConnectionString"];
            sql = "DELETE FROM `student_subject` WHERE `stu_id` = '"+id+"' AND `sub_id` = '"+sub_id+"';";
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
            subjectCount();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            String subID = comboBox1.SelectedValue.ToString();

            string connetionString = null;
            MySqlConnection connection;
            MySqlCommand command;
            string sql = null;
            connetionString = ConfigurationManager.AppSettings["ConnectionString"];
            sql = "INSERT INTO `student_subject` (`stu_id`, `sub_id`) VALUES ('"+ id +"', '"+subID+"')";
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
            subjectCount();
        }
        private void subjectCount() 
        {
            string connetionString = null;
            MySqlConnection connection;
            MySqlCommand command;
            string sql = null;
            connetionString = ConfigurationManager.AppSettings["ConnectionString"];
            sql = "SELECT COUNT(`sub_id`) FROM `student_subject` WHERE `stu_id` = '"+ id +"' ";
            connection = new MySqlConnection(connetionString);

            try
            {
                connection.Open();
                command = new MySqlCommand(sql, connection);
                //command.ExecuteNonQuery();
                object result = command.ExecuteScalar();
                if(result != null)
                {
                    string count = Convert.ToString(result);
                    lblSubjectCount.Text = "Total Subjects: " + count;
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
