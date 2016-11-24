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
    public partial class TakingManager : Form
    {
        AddingAPurchase addingAPurchaseForm;
        TakingModification TakingModificationForm;
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        string usernameOfConnectedUser, passwordOfConnectedUser, IDOfConnetedUser, nameOfConnected, surnameOfConnected;


        public TakingManager(string usernameOfConnectedUser, string passwordOfConnectedUser, string IDOfConnetedUser, string nameOfConnectedUser, string surnameOfconnectedUser)
        {
            
            InitializeComponent();
            this.usernameOfConnectedUser = usernameOfConnectedUser;
            this.passwordOfConnectedUser = passwordOfConnectedUser;
            this.IDOfConnetedUser = IDOfConnetedUser;
            this.nameOfConnected = nameOfConnectedUser;
            this.surnameOfConnected = surnameOfconnectedUser;
        }

        private void TakingManager_Load(object sender, EventArgs e)
        {
            label1.Text = String.Format("Welcome, {0}", nameOfConnected);

            server = "127.0.0.1";
            database = "DailyTakings";
            uid = "root";
            password = "";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            connection = new MySqlConnection(connectionString);
            Select();
        }

        //open connection to database
        public bool OpenConnection()
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
        public void Select()
{
    if (this.OpenConnection() == true)
    {
        MySqlDataAdapter dataAdapter = new MySqlDataAdapter("SELECT TAKINGS.ID, US.NAME AS 'SELLER NAME', US.SURNAME AS 'SELLER SURNAME', TAKINGS.SERVICE_ORDER_NUMBER AS 'S/O NUMBER', TAKINGS.CASH, TAKINGS.CARD, TAKINGS.DATESTAMP AS 'BOOK IN DATE', TAKINGS.SALES_DESCRIPTION AS DESCRIPTION, TAKINGS.REFUNDED_BY_CASH AS 'REFUNDED BY CASH', TAKINGS.REFUNDED_BY_CARD AS 'REFUNDED BY CARD', TAKINGS.LAST_EDITED AS 'LAST EDIT DATE', US1.NAME AS 'LAST EDITED NAME', US1.SURNAME AS 'LAST EDITED SURNAME' FROM TAKINGS INNER JOIN USERS US ON (TAKINGS.ID_OF_USER=US.ID) INNER JOIN USERS US1 ON (TAKINGS.LAST_EDITED_BY=US1.ID) ORDER BY TAKINGS.ID ASC", connection);
        DataSet DS = new DataSet();
        dataAdapter.Fill(DS);
        dataGridView1.DataSource = DS.Tables[0];

        //close connection
        this.CloseConnection();
    }

        //close Connection
        this.CloseConnection();
}


        bool closingPending = false;
        private void TakingManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (closingPending) return;
            if (MessageBox.Show(
                    "Are you sure you want to quit?",
                    "Are you sure?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1) == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                closingPending = true;
                Application.Exit();
            }
                
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            addingAPurchaseForm = new AddingAPurchase(nameOfConnected, surnameOfConnected, IDOfConnetedUser, this);
            addingAPurchaseForm.ShowDialog(this);
            
 
        }

        private void TakingManager_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                TakingModificationForm = new TakingModification(nameOfConnected, surnameOfConnected, IDOfConnetedUser, Convert.ToInt32(dataGridView1.SelectedCells[0].Value.ToString()), dataGridView1.SelectedCells[1].Value.ToString(), dataGridView1.SelectedCells[2].Value.ToString(), dataGridView1.SelectedCells[3].Value.ToString(), Convert.ToSingle(dataGridView1.SelectedCells[4].Value.ToString()), Convert.ToSingle(dataGridView1.SelectedCells[5].Value.ToString()), dataGridView1.SelectedCells[6].Value.ToString(), dataGridView1.SelectedCells[7].Value.ToString(), Convert.ToSingle(dataGridView1.SelectedCells[8].Value.ToString()), Convert.ToSingle(dataGridView1.SelectedCells[9].Value.ToString()), dataGridView1.SelectedCells[10].Value.ToString(), dataGridView1.SelectedCells[11].Value.ToString(), dataGridView1.SelectedCells[12].Value.ToString(), this);
                TakingModificationForm.ShowDialog(this);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Select();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        public void SelectSearch()
        {
            if (this.OpenConnection() == true)
            {
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter("SELECT TAKINGS.ID, US.NAME AS 'SELLER NAME', US.SURNAME AS 'SELLER SURNAME', TAKINGS.SERVICE_ORDER_NUMBER AS 'S/O NUMBER', TAKINGS.CASH, TAKINGS.CARD, TAKINGS.DATESTAMP AS 'BOOK IN DATE', TAKINGS.SALES_DESCRIPTION AS DESCRIPTION, TAKINGS.REFUNDED_BY_CASH AS 'REFUNDED BY CASH', TAKINGS.REFUNDED_BY_CARD AS 'REFUNDED BY CARD', TAKINGS.LAST_EDITED AS 'LAST EDIT DATE', US1.NAME AS 'LAST EDITED NAME', US1.SURNAME AS 'LAST EDITED SURNAME' FROM TAKINGS INNER JOIN USERS US ON (TAKINGS.ID_OF_USER=US.ID) INNER JOIN USERS US1 ON (TAKINGS.LAST_EDITED_BY=US1.ID) ORDER BY TAKINGS.ID ASC w", connection);
                DataSet DS = new DataSet();
                dataAdapter.Fill(DS);
                dataGridView1.DataSource = DS.Tables[0];

                //close connection
                this.CloseConnection();
            }

            //close Connection
            this.CloseConnection();
        }

    }
}
