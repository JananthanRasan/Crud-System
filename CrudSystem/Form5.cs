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
    public partial class frmView2 : Form
    {
        public frmView2()
        {
            InitializeComponent();
            string connetionString = null;
            MySqlConnection connection;
            MySqlCommand command;
            string sql = null;
            connetionString = ConfigurationManager.AppSettings["ConnectionString"];
            sql = "SELECT s.id, s.admission_no, s.first_name, s.last_name, s.gender, g.grade, GROUP_CONCAT(DISTINCT sub.subject SEPARATOR ', ') AS subjects, s.address, s.email, s.phone_no, s.nic, GROUP_CONCAT(DISTINCT h.name SEPARATOR ', ') AS hobbies, s.created_at, s.updated_at FROM students s INNER JOIN grades g ON s.grade_id = g.grade_id LEFT JOIN student_subject ss ON s.id = ss.stu_id LEFT JOIN subjects sub ON ss.sub_id = sub.id LEFT JOIN hobby_student hs ON s.id = hs.stu_id LEFT JOIN hobbies h ON hs.hob_id = h.id GROUP BY s.id";
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
                    dgvGetData.DataSource = dt;


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
    }
}
