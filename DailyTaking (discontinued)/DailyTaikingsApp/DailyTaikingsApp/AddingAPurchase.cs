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

namespace DailyTaikingsApp
{
    public partial class AddingAPurchase : Form
    {
        private TakingManager m_form = null;
        string nameOfConnected, surnameOfConnected;
        string IDOfConnetedUser;
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;
        bool shouldIBeClosed = true;
        
       

        public AddingAPurchase(string nameOfConnected, string surnameOfConnected, string IDOfConnetedUser, TakingManager tm)
        {
            m_form = tm;
            this.nameOfConnected = nameOfConnected;
            this.surnameOfConnected = surnameOfConnected;
            this.IDOfConnetedUser = IDOfConnetedUser;
             
          
            InitializeComponent();           
        }

        private void AddingAPurchase_Load(object sender, EventArgs e)
        {
            server = "127.0.0.1";
            database = "DailyTakings";
            uid = "root";
            password = "";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            connection = new MySqlConnection(connectionString);
            textBox1.Text = nameOfConnected;
            textBox2.Text = surnameOfConnected;
            textBox5.Enabled = false;
            textBox4.Enabled = false;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

            textBox5.Enabled = false;
            textBox5.Text = "0";
            textBox4.Enabled = true;
            
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

            textBox4.Enabled = false;
            textBox4.Text = "0";
            textBox5.Enabled = true;


        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            textBox4.Text = "0";
            textBox5.Text = "0";
            textBox4.Enabled = true;
            textBox5.Enabled = true;
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox3.Text = "";
            textBox4.Text = "0";
            textBox5.Text = "0";
            richTextBox1.Text = "";
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool errorsFound = false;
            //start check of cashpay and cardpay
            float cashPaid, cardPaid;
            //if cashpaid wrongly entered
            if (textBox4.Enabled && !textBox5.Enabled)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(textBox4.Text, @"^[1-9][0-9]*(?:(,[0-9]{2}$)|(\b$))"))
                    cashPaid = Convert.ToSingle(textBox4.Text);
                else
                {
                    errorsFound = true;
                    textBox4.Text = "0";
                    MessageBox.Show("Incorrect CASH value entered. Try again.");
                }
            }
            //if cardpay wrongly entered
            if (!textBox4.Enabled && textBox5.Enabled)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(textBox5.Text, @"^[1-9][0-9]*(?:(,[0-9]{2}$)|(\b$))"))
                    cardPaid = Convert.ToSingle(textBox5.Text);
                else
                {
                    errorsFound = true;
                    textBox5.Text = "0";
                    MessageBox.Show("Incorrect CARD value entered. Try again.");
                }
            }
            // check both  
            if (textBox4.Enabled && textBox5.Enabled)
            {
                if ((!System.Text.RegularExpressions.Regex.IsMatch(textBox4.Text, @"^[1-9][0-9]*(?:(,[0-9]{2}$)|(\b$))") && System.Text.RegularExpressions.Regex.IsMatch(textBox5.Text, @"^[1-9][0-9]*(?:(,[0-9]{2}$)|(\b$))")) || ((System.Text.RegularExpressions.Regex.IsMatch(textBox4.Text, @"^[1-9][0-9]*(?:(,[0-9]{2}$)|(\b$))") && !System.Text.RegularExpressions.Regex.IsMatch(textBox5.Text, @"^[1-9][0-9]*(?:(,[0-9]{2}$)|(\b$))"))))
                {
                    //only cashpay wrongly entered
                if (System.Text.RegularExpressions.Regex.IsMatch(textBox4.Text, @"^[1-9][0-9]*(?:(,[0-9]{2}$)|(\b$))"))
                    cashPaid = Convert.ToSingle(textBox4.Text);
                else
                {
                    errorsFound = true;
                    MessageBox.Show("Incorrect CASH value entered. Try again.");
                    textBox4.Text = "0";
                    
                }
                    
                    //only cardpay wrongly entered
                if (System.Text.RegularExpressions.Regex.IsMatch(textBox5.Text, @"^[1-9][0-9]*(?:(,[0-9]{2}$)|(\b$))"))
                        cardPaid = Convert.ToSingle(textBox5.Text);
                else
                {
                    errorsFound = true;
                    MessageBox.Show("Incorrect CARD value entered. Try again.");
                    textBox5.Text = "0";
                    
                }
                    
                }
                    //both wrong entered
                if (!System.Text.RegularExpressions.Regex.IsMatch(textBox4.Text, @"^[1-9][0-9]*(?:(,[0-9]{2}$)|(\b$))") && !System.Text.RegularExpressions.Regex.IsMatch(textBox5.Text, @"^[1-9][0-9]*(?:(,[0-9]{2}$)|(\b$))"))
                {
                    errorsFound = true;
                    textBox4.Text = "0";
                    textBox5.Text = "0";
                    MessageBox.Show("Icorrect CASH and CARD value. Try again.");
                }
                
            }
            //end of check cashpay and cardpay

            //service order checking
            if (!System.Text.RegularExpressions.Regex.IsMatch(textBox3.Text, @"^[0-9]*$") && !errorsFound)
            {
                DialogResult result = MessageBox.Show(
                    String.Format("Are you sure you want to add a job with S/O number like: {0}", textBox3.Text),
                    "Are you sure?",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
                if (result == DialogResult.OK)
                {
                    Insert();
                    shouldIBeClosed = false;
                    m_form.Select();
                    this.Close();
                }
                    
            }

            if (System.Text.RegularExpressions.Regex.IsMatch(textBox3.Text, @"^[0-9]*$") && !errorsFound)
            {
                Insert();
                shouldIBeClosed = false;
                m_form.Select();
                this.Close();
            }
                 


        }

        private void AddingAPurchase_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (shouldIBeClosed)
            {
                DialogResult result = MessageBox.Show(
                        "Are you sure you want to discard entered information?",
                        "Are you sure?",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1);
                if (result != DialogResult.Yes)
                    e.Cancel = true;
            }
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

        //Insert statement
        public void Insert()
        {
            string query = String.Format("INSERT INTO takings (ID_OF_USER,SERVICE_ORDER_NUMBER, CASH, CARD, SALES_DESCRIPTION, LAST_EDITED_BY) VALUES({0}, '{1}', {2}, {3}, '{4}', {5})", Convert.ToInt16(IDOfConnetedUser), textBox3.Text, Convert.ToSingle(textBox4.Text), Convert.ToSingle(textBox5.Text), Convert.ToString(richTextBox1.Text), Convert.ToInt16(IDOfConnetedUser));

            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);


                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }

            MessageBox.Show("Data inserted successfully!");
        }

    }

    
}
