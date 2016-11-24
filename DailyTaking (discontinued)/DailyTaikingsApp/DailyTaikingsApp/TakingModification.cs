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

namespace DailyTaikingsApp
{
    public partial class TakingModification : Form
    {
        bool shouldIBeClosed = true;
        private TakingManager m_form = null;
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;
        string nameOfConnected, surnameOfConnected, IDOfConnetedUser, NameOfSeller, SurnameOfSeller, SONumber, dateOfBookingIn, description, lastEdited, lastEditedName, lastEditedSrname;
        int IndexOfTaking;
        float cashPaid, cardPaid, RefundedByCash, RefundedByCard;

        public TakingModification(string nameOfConnected, string surnameOfConnected, string IDOfConnetedUser, int IndexOfTaking, string NameOfSeller, string SurnameOfSeller, string SONumber, float cashPaid, float cardPaid, string dateOfBookingIn, string description, float RefundedByCash, float RefundedByCard, string lastEdited, string lastEditedName, string lastEditedSrname, TakingManager tm)
        {
            m_form = tm;
            server = "127.0.0.1";
            database = "DailyTakings";
            uid = "root";
            password = "";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            connection = new MySqlConnection(connectionString);
            InitializeComponent();

            this.nameOfConnected = nameOfConnected;
            this.surnameOfConnected = surnameOfConnected;
            this.IDOfConnetedUser = IDOfConnetedUser;
            this.IndexOfTaking = IndexOfTaking;

            this.NameOfSeller = NameOfSeller;
            this.SurnameOfSeller = SurnameOfSeller;
            this.SONumber = SONumber;
            this.cashPaid = cashPaid;
            this.cardPaid = cardPaid;
            this.dateOfBookingIn = dateOfBookingIn;
            this.description = description;
            this.RefundedByCash = RefundedByCash;
            this.RefundedByCard = RefundedByCard;
            this.lastEdited = lastEdited;
            this.lastEditedName = lastEditedName;
            this.lastEditedSrname = lastEditedSrname;
        }

        private void TakingModification_Load(object sender, EventArgs e)
        {
            
            textBox1.Text = NameOfSeller;
            textBox2.Text = SurnameOfSeller;
            textBox3.Text = SONumber;
            textBox4.Text = Convert.ToString(cashPaid);
            textBox5.Text = Convert.ToString(cardPaid);
            textBox6.Text = dateOfBookingIn;
            richTextBox1.Text = description;
            textBox7.Text = Convert.ToString(RefundedByCash);
            textBox8.Text = Convert.ToString(RefundedByCard);
            textBox9.Text = lastEdited;
            textBox10.Text = lastEditedName;
            textBox11.Text = lastEditedSrname;
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

        bool cashPayValid = false, cardPayValid = false, cashRefundValid = false, cardRefundValid = false;
        int howManyWrong = 0;
        string Wrong;
        private void button1_Click(object sender, EventArgs e)
        {
            Wrong =  "Wrong values entered in: ";
            if (System.Text.RegularExpressions.Regex.IsMatch(textBox4.Text, @"^(?:0\b$|[1-9][0-9]*(?:(,[0-9]{2}$)|(\b$)))"))
                cashPayValid = true;
            else
            {
                cashPayValid = false;
                howManyWrong++;
                Wrong += "cash pay";
            }


            if (System.Text.RegularExpressions.Regex.IsMatch(textBox5.Text, @"^(?:0\b$|[1-9][0-9]*(?:(,[0-9]{2}$)|(\b$)))"))
                cardPayValid = true;
            else
            {
                cardPayValid = false;
                if (cashPayValid)
                    Wrong += "card pay";
                if(!cashPayValid)
                Wrong += ", card pay";
                howManyWrong++;
            }


            if (System.Text.RegularExpressions.Regex.IsMatch(textBox7.Text, @"^(?:0\b$|[1-9][0-9]*(?:(,[0-9]{2}$)|(\b$)))"))
                cashRefundValid = true;
            else
            {

                cashRefundValid = false;
                if (cashPayValid && cardPayValid)
                    Wrong += "cash refund";
                if (!cashPayValid || !cardPayValid)
                    Wrong += ", cash refund";
                howManyWrong++;
            }


            if (System.Text.RegularExpressions.Regex.IsMatch(textBox8.Text, @"^(?:0\b$|[1-9][0-9]*(?:(,[0-9]{2}$)|(\b$)))"))
                cardRefundValid = true;
            else
            {
                cardRefundValid = false;
                if (cashPayValid && cardPayValid && cashRefundValid)
                    Wrong += "card refund";

                if (!cashPayValid || !cardPayValid || !cashRefundValid)
                    Wrong += ", card refund";
                howManyWrong++;
            }

            if (cashPayValid && cardPayValid && cashRefundValid && cardRefundValid)
            {
                UpdateSQL();
                MessageBox.Show("Updated successfully!");
                shouldIBeClosed = false;
                m_form.Select();
                this.Close();
            } 
            else
                MessageBox.Show(Wrong + ".");

            
        }

        //Update statement
        public void UpdateSQL()
        {
            string query = String.Format("UPDATE takings SET SERVICE_ORDER_NUMBER='{0}', CASH={1}, CARD={2}, SALES_DESCRIPTION = '{3}', REFUNDED_BY_CASH={4}, REFUNDED_BY_CARD={5}, LAST_EDITED=NOW(), LAST_EDITED_BY = {7} WHERE TAKINGS.ID={6}", textBox3.Text, Convert.ToSingle(textBox4.Text), Convert.ToSingle(textBox5.Text), richTextBox1.Text, Convert.ToSingle(textBox7.Text), Convert.ToSingle(textBox8.Text), IndexOfTaking, IDOfConnetedUser);

            //Open connection
            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;

                //Execute query
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        //Delete statement
        public void Delete()
        {
            string query = String.Format("DELETE FROM TAKINGS WHERE id={0}", IndexOfTaking);

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        private void TakingModification_FormClosing(object sender, FormClosingEventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult areYouSure = MessageBox.Show(
                "Are you sure you want to delete existing taking?",
                "Are you sure?",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
            if(areYouSure ==DialogResult.OK)
            {
                Delete();
                shouldIBeClosed = false;
                m_form.Select();
                this.Close();
            }
            

        }
    }
}
