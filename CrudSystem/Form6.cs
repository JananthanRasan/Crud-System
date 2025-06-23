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
    public partial class frmGrade : Form
    {
        public frmGrade()
        {
            InitializeComponent();
        }

        private void btnFillData_Click(object sender, EventArgs e)
        {
            if (dgvgrade.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please Select a row first !");
                return;
            }

            string id = dgvgrade.SelectedRows[0].Cells["grade"].Value.ToString();
            txtGrade.Text = id;
            btnUpdate.Visible = true;
            btnCreate.Visible = false;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            string connetionString = null;
            MySqlConnection connection;
            MySqlCommand command;
            string sql = null;
            connetionString = ConfigurationManager.AppSettings["ConnectionString"];
            sql = "INSERT INTO `grade` (`grade`)VALUES('" + txtGrade.Text + "')";
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
            txtGrade.Clear();
            MessageBox.Show("New Grade Added!");
            getData();
        }

        private void btnGetData_Click(object sender, EventArgs e)
        {
            getData();
        }

        private void getData()
        {
            string connetionString = null;
            MySqlConnection connection;
            MySqlCommand command;
            string sql = null;
            connetionString = ConfigurationManager.AppSettings["ConnectionString"];
            sql = "SELECT `grade_id`,`grade` FROM `grade`";
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
                    dgvgrade.DataSource = dt;

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
            if (dgvgrade.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please Select a row first !");
                return;
            }

            String grade_id = dgvgrade.SelectedRows[0].Cells["grade_id"].Value.ToString();
            MessageBox.Show("Selected ID: " + grade_id);

            string connetionString = null;
            MySqlConnection connection;
            MySqlCommand command;
            string sql = null;
            connetionString = ConfigurationManager.AppSettings["ConnectionString"];
            sql = "DELETE FROM `grade` WHERE `id` = '" + grade_id + "' ";
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
            if (dgvgrade.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please Select a row first !");
                return;
            }

            String id = dgvgrade.SelectedRows[0].Cells["grade_id"].Value.ToString();

            string connetionString = null;
            MySqlConnection connection;
            MySqlCommand command;
            string sql = null;
            connetionString = ConfigurationManager.AppSettings["ConnectionString"];
            sql = "UPDATE `grade` SET `grade` = '"+ txtGrade.Text +"' WHERE `grade_id` = '"+ id +"'";
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
            txtGrade.Clear();
            MessageBox.Show("Grade Altered!");
            btnUpdate.Visible = false;
            btnCreate.Visible = true;
            getData();
        }
    }
}
