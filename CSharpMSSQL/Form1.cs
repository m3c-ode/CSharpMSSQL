using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpMSSQL
{
    //At the base level for connecting DB to front end, we need to have something that connects and something that brings forth the records in ADO it was ConncetionOBJ and RecordSet Object
    //
    public partial class Form1 : Form
    {
        System.Data.SqlClient.SqlConnection _ConnObj;//we create the connection object to connect front end to the back end
        System.Data.SqlClient.SqlDataAdapter _DataAdapObj;//this helps us populate the dataset  
        DataSet _DataSet;//this is like our record set
        int _recordCount;
        int _currentRow;
        public Form1()

        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            _ConnObj = new System.Data.SqlClient.SqlConnection();// we create a new connection object
            _ConnObj.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\m3\source\repos\ITDCanada\C#\CSharpMSSQL\CSharpMSSQL\ClubMemebers.mdf;Integrated Security=True";//we use the location of our table as the connection string
            _ConnObj.Open();
            String SqlStr = "SELECT * FROM MemberList";//makes our SQL statment
            //the following two lines are always used to connect to our table  
            System.Data.SqlClient.SqlConnection SC = new System.Data.SqlClient.SqlConnection(_ConnObj.ConnectionString);
            _DataAdapObj = new System.Data.SqlClient.SqlDataAdapter(SqlStr, _ConnObj);
            _DataSet = new DataSet();
            _DataAdapObj.Fill(_DataSet, "MemberList");
            _recordCount = _DataSet.Tables["MemberList"].Rows.Count;// tells us how many records we have starting at 0
        }
        public void ShowRecord(int ThisRow)
        {
            //displays our row field in our textBoxes
            textBox1.Text = _DataSet.Tables["MemberList"].Rows[ThisRow].Field<int>("MemberID").ToString();
            textBox2.Text = _DataSet.Tables["MemberList"].Rows[ThisRow].Field<String>("FirstName").ToString();
            textBox3.Text = _DataSet.Tables["MemberList"].Rows[ThisRow].Field<String>("LastName").ToString();
            textBox4.Text = _DataSet.Tables["MemberList"].Rows[ThisRow].Field<DateTime>("DOB").ToString();
            textBox5.Text = _DataSet.Tables["MemberList"].Rows[ThisRow].Field<String>("Rank").ToString();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //goes to the first record and shows it
            _currentRow = 0;
            ShowRecord(_currentRow);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //increases our row number by 1
            _currentRow++;
            if (_currentRow > _recordCount - 1)
            {
                MessageBox.Show("End Of File");
                _currentRow--;
            }
            ShowRecord(_currentRow);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            _currentRow--;
            if (_currentRow < 0)
            {
                MessageBox.Show("Beginning Of File");
                _currentRow++;
            }
            ShowRecord(_currentRow);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _currentRow = _recordCount - 1;
            ShowRecord(_currentRow);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            System.Data.DataRow[] FoundRows;
            String StrToFind;

            if (textBox1.Text == "")
            {
                System.Windows.Forms.MessageBox.Show("Please Enter A Member ID To Find");
                return;
            }
            StrToFind = "MemberID =" + textBox1.Text;
            FoundRows = _DataSet.Tables["MemberList"].Select(StrToFind);
            int _rowIndex;
            if (FoundRows.Length == 0)
            {
                MessageBox.Show("Record Not Found");

            }
            else
            {
                _rowIndex = _DataSet.Tables["MemberList"].Rows.IndexOf(FoundRows[0]);
                _currentRow = _rowIndex;
                ShowRecord(_currentRow);
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            ClearBoxes();
        }
        private void ClearBoxes()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            System.Data.DataRow[] _foundRows;
            String StrToFind;

            if (textBox1.Text == "")
            {
                MessageBox.Show("Please Enter in a member ID to Add");
            }
            StrToFind = "MemberID = " + textBox1.Text;
            _foundRows = _DataSet.Tables["MemberList"].Select(StrToFind);
            if (_foundRows.Length == 0)
            {
                System.Data.DataRow NewRow = _DataSet.Tables["MemberList"].NewRow();
                System.Data.SqlClient.SqlCommandBuilder CommBuilder = new System.Data.SqlClient.SqlCommandBuilder(_DataAdapObj);
                NewRow.SetField<int>("MemberID", Convert.ToInt32(textBox1.Text));
                NewRow.SetField<String>("FirstName", textBox2.Text);
                NewRow.SetField<String>("LastName", textBox3.Text);
                NewRow.SetField<DateTime>("DOB", Convert.ToDateTime(textBox4.Text));
                NewRow.SetField<String>("Rank", textBox5.Text);
                _DataSet.Tables["MemberList"].Rows.Add(NewRow);
                _DataAdapObj.Update(_DataSet, "MemberList");
                _recordCount += 1;
                MessageBox.Show("Add Successful");
            }
            else
            {
                MessageBox.Show("Duplicate Record, Please Try Again");
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {

            System.Data.DataRow[] _foundRows;
            String StrToFind;
            int _rowIndex;
            if (textBox1.Text == "")
            {
                MessageBox.Show("Please Enter in a member ID to Add");
            }
            StrToFind = "MemberID = " + textBox1.Text;
            _foundRows = _DataSet.Tables["MemberList"].Select(StrToFind);
            System.Data.SqlClient.SqlCommandBuilder CommBuilder = new System.Data.SqlClient.SqlCommandBuilder(_DataAdapObj);
            if (_foundRows.Length == 0)
            {
                MessageBox.Show("Record Not Found, Try Agian");
            }
            else
            {
                _rowIndex = _DataSet.Tables["MemberList"].Rows.IndexOf(_foundRows[0]);
                _DataSet.Tables["MemberList"].Rows[_rowIndex].SetField<String>("FirstName", textBox2.Text);
                _DataSet.Tables["MemberList"].Rows[_rowIndex].SetField<String>("LastName", textBox3.Text);
                _DataSet.Tables["MemberList"].Rows[_rowIndex].SetField<String>("DOB", textBox4.Text);
                _DataSet.Tables["MemberList"].Rows[_rowIndex].SetField<String>("Rank", textBox5.Text);
                MessageBox.Show("Update Successful!");

            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            DataRow[] _foundRows;
            String StrToFind;
            int _rowIndex;
            if (textBox1.Text == "")
            {
                MessageBox.Show("Enter an ID, Please Try Again");

            }
            StrToFind = "MemberID = " + textBox1.Text;
            _foundRows = _DataSet.Tables["MemberList"].Select(StrToFind);
            if (_foundRows.Length == 0)
            {
                MessageBox.Show("Record Not Found");
            }
            else
            {
                int result;
                _rowIndex = _DataSet.Tables["MemberList"].Rows.IndexOf(_foundRows[0]);

                result = Convert.ToInt32(MessageBox.Show("Are you sure you want to Delete?", "Deleting Record", MessageBoxButtons.YesNo));

                if (result == 6)
                {
                    _DataSet.Tables["MemberList"].Rows[_rowIndex].Delete();


                    ClearBoxes();
                    MessageBox.Show("Delete Successful");
                    _recordCount -= 1;


                }
            }
        }
    }
}
