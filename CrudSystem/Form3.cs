using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;
using System.Configuration;

namespace CrudSystem
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }


        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!Emptycheck(txtUserName, "User Name Missing") && !Emptycheck(txtPassword, "Password Missing"))
            {
                login();
            }

        }

        private void login()
        {
            string connetionString = null;
            MySqlConnection connection;
            MySqlCommand command;
            string sql = null;
            connetionString = ConfigurationManager.AppSettings["ConnectionString"];
            sql = "SELECT COUNT(`user_name`) AS valid,`user_type` FROM `login_user` WHERE `user_name` = '" + txtUserName.Text + "' AND `password` = '" + txtPassword.Text + "' ";
            connection = new MySqlConnection(connetionString);

            try
            {
                connection.Open();
                command = new MySqlCommand(sql, connection);

                MySqlDataReader reader = command.ExecuteReader();

                //if (reader.HasRows)
                //{

                //    while (reader.Read())
                //    {
                //        string type = reader["user_type"].ToString();
                //        if (reader["valid"].ToString() == "1")
                //        {
                //            if(type == "admin")
                //            {
                //                frmView f = new frmView();
                //                f.ShowDialog();
                //            }
                //            else
                //            {
                //                frmView2 f2 = new frmView2();
                //                f2.ShowDialog();
                //            }
                            
                //        }
                //        else
                //        {
                //            MessageBox.Show("Invalid credentials!");
                //        }
                //    }
                //    reader.Close();
                //}

    


                 command.Dispose();
                 connection.Close();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection ! " + ex.Message.ToString());
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

    }
}
