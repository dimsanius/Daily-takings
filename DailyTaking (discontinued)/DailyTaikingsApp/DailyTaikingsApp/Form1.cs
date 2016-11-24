using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace DailyTaikingsApp
{


    public  partial class Form1 : Form
    {

        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            server = "127.0.0.1";
            database = "DailyTakings";
            uid = "root";
            password = "";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            connection = new MySqlConnection(connectionString);

        }


        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        //Select statement
        public void Select(string username, string password)
        {
             string query = String.Format("SELECT USERNAME, PASSWORD, ID, NAME, SURNAME FROM USERS WHERE USERNAME = '{0}' AND PASSWORD = '{1}'", username, password);


    //Open connection
    if (this.OpenConnection() == true)
    {
        //Create Command
        MySqlCommand cmd = new MySqlCommand(query, connection);
        //Create a data reader and Execute the command
        MySqlDataReader dataReader = cmd.ExecuteReader();
        
        try
        {
            string enteredUsername, enteredPassword, UsersID, UsersName, UsersSurname;
            //Read the data and store them in the list
            dataReader.Read();
            enteredUsername = dataReader[0].ToString();
            enteredPassword = dataReader[1].ToString();
            UsersID = dataReader[2].ToString();
            UsersName = dataReader[3].ToString();
            UsersSurname = dataReader[4].ToString();
            if (enteredUsername == textBox1.Text && enteredPassword == textBox2.Text)
            {

                MessageBox.Show("Success");
                TakingManager TakingForm = new TakingManager(textBox1.Text, textBox2.Text, UsersID, UsersName, UsersSurname);
                TakingForm.Show();
                this.Hide();
            }

            else
                MessageBox.Show("Fail");
        }
        catch
        {
            if (!dataReader.HasRows)
                MessageBox.Show("No user found");
            // code of error if needed
            // MessageBox.Show(ex.ToString());
        }
        //close Data Reader
        dataReader.Close();

        //close Connection
        this.CloseConnection();
    }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Select(textBox1.Text, textBox2.Text);

        }
        public void CloseApp()
        {
            this.Close();
        }


    }

    
}
