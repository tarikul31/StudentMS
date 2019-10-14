using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Windows.Forms;
using StudentMS.Model;
using System.Data.SqlClient;

namespace StudentMS
{
    public partial class StudentForm : Form
    {
        public StudentForm()
        {
            InitializeComponent();
        }
        List<StudentModel> studentList = new List<StudentModel>();
        private Boolean isListBoxEmpty = true;
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=DESKTOP-LNF6V3A\SQLEXPRESS;Initial Catalog=StudentDb;Integrated Security=True");
        private int ID;

        private void StudentForm_Load(object sender, EventArgs e)
        {
            getStudentInfo();
        }

        private void getStudentInfo()
        {
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM StudentInfo", sqlConnection);
            DataTable dtTable = new DataTable();
            sqlConnection.Open();
            SqlDataReader dtReader = sqlCommand.ExecuteReader();
            dtTable.Load(dtReader);
            sqlConnection.Close();
            studentGridView.DataSource = dtTable;
           
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (isValidForm())
            {
                StudentModel students = new StudentModel();
                students.Name = nameTextBox.Text;
                students.Department = departmentTextBoz.Text;
                students.Semester = semesterTextBox.Text;
                students.Contact = contactTextBox.Text;
                students.Email = emailTextBox.Text;
                studentList.Add(students);

                String query = "INSERT INTO StudentInfo (Name,Department,Semester,Contact,Email) VALUES (@name,@department,@semester,@contact,@email)";
                foreach (StudentModel studentModel in studentList)
                {
                    SqlCommand insertCommand = new SqlCommand(query, sqlConnection);
                    insertCommand.CommandType = CommandType.Text;
                    insertCommand.Parameters.AddWithValue("@name", studentModel.Name);
                    insertCommand.Parameters.AddWithValue("@department", studentModel.Department);
                    insertCommand.Parameters.AddWithValue("@semester", studentModel.Semester);
                    insertCommand.Parameters.AddWithValue("@contact", studentModel.Contact);
                    insertCommand.Parameters.AddWithValue("@email", studentModel.Email);

                    sqlConnection.Open();
                    int result = insertCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                    if (result < 0)
                    {
                        MessageBox.Show("Data Insert Failed","Failed",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                    MessageBox.Show("New Student Added", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    getStudentInfo();
                    showData();
                    resetForm();
                }
                countStudentLabel.Text = studentList.Count().ToString();

            }

        }

        private Boolean isValidForm()
        {
            if (nameTextBox.Text == String.Empty)
            {
                MessageBox.Show("Name is required ", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (departmentTextBoz.Text == String.Empty)
            {
                MessageBox.Show("Department is required ", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (semesterTextBox.Text == String.Empty)
            {
                MessageBox.Show("Semester is required ", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (contactTextBox.Text == String.Empty)
            {
                MessageBox.Show("Contact is required ", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (emailTextBox.Text == String.Empty)
            {
                MessageBox.Show("Email is required ", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;

        }
        private void clListBox()
        {
            studentListBox.Items.Clear();
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            resetForm();
            clListBox();
            
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            if (ID > 0)
            {
                string query = "UPDATE StudentInfo SET  Name=@name, Department=@department, Semester=@semester, Contact=@contact, Email=@email WHERE ID = @ID";
                SqlCommand updateCommand = new SqlCommand(query, sqlConnection);
                updateCommand.CommandType = CommandType.Text;
                updateCommand.Parameters.AddWithValue("@name", nameTextBox.Text);
                updateCommand.Parameters.AddWithValue("@department", departmentTextBoz.Text);
                updateCommand.Parameters.AddWithValue("@semester", semesterTextBox.Text);
                updateCommand.Parameters.AddWithValue("@contact", contactTextBox.Text);
                updateCommand.Parameters.AddWithValue("@email", emailTextBox.Text);
                updateCommand.Parameters.AddWithValue("@ID", this.ID);

                sqlConnection.Open();
                updateCommand.ExecuteNonQuery();
                sqlConnection.Close(); 
                MessageBox.Show("Succesfully Updates", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                getStudentInfo();
                resetForm();
            }
            else
            {
                MessageBox.Show("Failed to Update, Select a Information", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (ID > 0)
            {
                string query = "DELETE StudentInfo WHERE ID = @ID";
                SqlCommand updateCommand = new SqlCommand(query, sqlConnection);
                updateCommand.CommandType = CommandType.Text;
                updateCommand.Parameters.AddWithValue("@ID", this.ID);

                sqlConnection.Open();
                updateCommand.ExecuteNonQuery();
                sqlConnection.Close();
                MessageBox.Show("Succesfully Deleted", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                getStudentInfo();
                resetForm();
            }
            else
            {
                MessageBox.Show("Failed to Delete, Select a Information", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void studentGridView_Click(object sender, EventArgs e)
        {
            ID = Convert.ToInt32(studentGridView.SelectedRows[0].Cells[0].Value);
            nameTextBox.Text = studentGridView.SelectedRows[0].Cells[1].Value.ToString();
            departmentTextBoz.Text = studentGridView.SelectedRows[0].Cells[2].Value.ToString();
            semesterTextBox.Text = studentGridView.SelectedRows[0].Cells[3].Value.ToString();
            contactTextBox.Text = studentGridView.SelectedRows[0].Cells[4].Value.ToString();
            emailTextBox.Text = studentGridView.SelectedRows[0].Cells[5].Value.ToString();

        }

        private void showData()
        {
            foreach (StudentModel studentModel in studentList)
            {
                studentListBox.Items.Add(studentModel.Name + " \t " + studentModel.Semester + "\t" + studentModel.Department + "\t" +
                                              studentModel.Contact + "\t" + studentModel.Email);
            }

        }

        private void resetForm()
        {
            nameTextBox.Clear();
            departmentTextBoz.Clear();
            contactTextBox.Clear();
            emailTextBox.Clear();
            semesterTextBox.Clear();
            nameTextBox.Focus();
            ID = 0;

        }
    }

}
