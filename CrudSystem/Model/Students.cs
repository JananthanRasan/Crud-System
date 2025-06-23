using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrudSystem.Model
{
    internal class Students
    {
        MySqlConnection connection;
        MySqlCommand command;
        MySqlCommand mySqlCommand;
        string connetionString=ConfigurationManager.AppSettings["ConnectionString"];
        string sql;
        MySqlDataReader reader;
        DataTable dd;
        string gender = "";
        int newId = 0;

        public DataTable Subject()
        {
            sql = "SELECT `id`,`subject` FROM `subjects` ";
            connection = new MySqlConnection(connetionString);

            try
            {
                connection.Open();
                command = new MySqlCommand(sql, connection);

                reader = command.ExecuteReader();

                dd = new DataTable();
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

        public DataTable Hobby()
        {
            sql = "SELECT `id`,`name` FROM `hobbies` ";
            connection = new MySqlConnection(connetionString);

            try
            {
                connection.Open();
                command = new MySqlCommand(sql, connection);

                reader = command.ExecuteReader();

                dd = new DataTable();
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

        public DataTable Grade()
        {
            sql = "SELECT `grade`,`grade_id` FROM `grades` ";
            connection = new MySqlConnection(connetionString);

            try
            {
                connection.Open();
                command = new MySqlCommand(sql, connection);

                reader = command.ExecuteReader();

                dd = new DataTable();
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

        public bool Insert(RadioButton male,RadioButton female,ComboBox grade,CheckedListBox subject,CheckedListBox hobby,TextBox adm,TextBox fname, TextBox lname, TextBox address,TextBox email, TextBox phone, TextBox nic )
        {
            //gender          
            if (male.Checked == true)
            {
                gender = male.Text;
            }
            else if (female.Checked)
            {
                gender = female.Text;
            }

            //grade_id
            String gradeID = grade.SelectedValue.ToString();

            string sql = $"INSERT INTO `students` (`admission_no`,`first_name`,`last_name`,`gender`,`address`,`email`,`phone_no`,`nic`,`grade_id`)VALUES('{adm.Text}','{fname.Text}','{lname.Text}','{gender}','{address.Text}','{email.Text}','{phone.Text}','{nic.Text}','{gradeID}')";
            connection = new MySqlConnection(connetionString);

            try
            {
                connection.Open();
                command = new MySqlCommand(sql, connection);

                command.ExecuteScalar();

                newId = Convert.ToInt32(command.LastInsertedId.ToString());

                command.Dispose();


                foreach (DataRowView item in subject.CheckedItems)
                {
                    String subID = item["id"].ToString();
                    string sql2 = $"INSERT INTO `student_subject` (`stu_id`, `sub_id`) VALUES('{ newId }', '{ subID }');";
                    mySqlCommand = new MySqlCommand(sql2, connection);
                    mySqlCommand.ExecuteNonQuery();
                    mySqlCommand.Dispose();
                }

                foreach (DataRowView item1 in hobby.CheckedItems)
                {
                    String hobID = item1["id"].ToString();
                    string sql3 = $"INSERT INTO `hobby_student` (`stu_id`, `hob_id`) VALUES('{ newId }', '{ hobID }');";
                    mySqlCommand = new MySqlCommand(sql3, connection);
                    mySqlCommand.ExecuteNonQuery();
                    mySqlCommand.Dispose();
                }

                

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Can not open connection! {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
            return true;
        }
    }
}
