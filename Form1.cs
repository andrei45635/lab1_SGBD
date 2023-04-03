using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace lab1_SGBD
{
    public partial class parent : Form
    {
        string connectionString = @"Server=DESKTOP-ILTATS6\SQLEXPRESS;Database=Port;Integrated Security=true;TrustServerCertificate=true;";
        //private readonly string connectionString = "Data Source=DESKTOP-ILTATS6\\SQLEXPRESS;Initial Catalog=Port;Integrated Security=True;TrustServerCertificate=true;";
        DataSet ds = new DataSet();
        SqlDataAdapter parentDataAdapter = new SqlDataAdapter();
        SqlDataAdapter childDataAdapter = new SqlDataAdapter();
        BindingSource parentBindingSource = new BindingSource();
        BindingSource childBindingSource = new BindingSource();


        public parent()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    //Managers - Employees (1 - m)
                    parentDataAdapter.SelectCommand = new SqlCommand("SELECT * FROM " + "Managers;", conn);
                    childDataAdapter.SelectCommand = new SqlCommand("SELECT * FROM " + "Employees", conn);
                    parentDataAdapter.Fill(ds, "Managers");
                    childDataAdapter.Fill(ds, "Employees");

                    DataColumn parentColumn = ds.Tables["Managers"].Columns["ManagerID"];
                    DataColumn childColumn = ds.Tables["Employees"].Columns["ManagerID"];
                    DataRelation relation = new DataRelation("FK_Employees_Managers", parentColumn, childColumn);
                    ds.Relations.Add(relation);

                    parentBindingSource.DataSource = ds.Tables["Managers"];
                    dataGridViewParent.DataSource = parentBindingSource;
                    childBindingSource.DataSource = parentBindingSource;
                    childBindingSource.DataMember = "FK_Employees_Managers";
                    dataGridViewChild.DataSource = childBindingSource;
                    conn.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Port"].ConnectionString.ToString()))
                {
                    childDataAdapter.DeleteCommand = new SqlCommand("DELETE FROM Employees " + "WHERE EmployeeID = @id;", conn);
                    childDataAdapter.DeleteCommand.Parameters.Add("@id", SqlDbType.Int).Value = ds.Tables["Employees"].Rows[childBindingSource.Position][0];

                    conn.Open();
                    childDataAdapter.DeleteCommand.ExecuteNonQuery();
                    conn.Close();

                    conn.Open();
                    childDataAdapter.SelectCommand = new SqlCommand("SELECT * FROM Employees", conn);
                    ds.Tables["Employees"].Clear();
                    childDataAdapter.Fill(ds, "Employees");
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Port"].ConnectionString))
                {
                    childDataAdapter.UpdateCommand = new SqlCommand("UPDATE Employees " + "SET EmployeeSalary=@salary, EmployeeBonus=@bonus, EmployeeName=@name" + " WHERE EmployeeID=@id;", conn);
                    childDataAdapter.UpdateCommand.Parameters.Add("@salary", SqlDbType.Float).Value = Convert.ToDouble(textBox1.Text.ToString());
                    childDataAdapter.UpdateCommand.Parameters.Add("@bonus", SqlDbType.Float).Value = Convert.ToDouble(textBox2.Text.ToString());
                    childDataAdapter.UpdateCommand.Parameters.Add("@name", SqlDbType.VarChar).Value = textBox5.Text;
                    childDataAdapter.UpdateCommand.Parameters.Add("@id", SqlDbType.Int).Value = ds.Tables["Employees"].Rows[childBindingSource.Position][0];
                    
                    conn.Open();
                    int rows = childDataAdapter.UpdateCommand.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Updated!");
                    }
                    conn.Close();

                    conn.Open();
                    childDataAdapter.SelectCommand = new SqlCommand("SELECT * FROM Employees", conn);
                    ds.Tables["Employees"].Clear();
                    childDataAdapter.Fill(ds, "Employees");
                    conn.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Port"].ConnectionString))
                {
                    childDataAdapter.InsertCommand = new SqlCommand("INSERT INTO Employees(EmployeeSalary, EmployeeBonus, EmployeeAge, ManagerID, EmployeeName)" + " VALUES (@salary, @bonus, @age, @mID, @name);", conn);
                    childDataAdapter.InsertCommand.Parameters.Add("@salary", SqlDbType.Float).Value = Convert.ToDouble(textBox1.Text.ToString());
                    childDataAdapter.InsertCommand.Parameters.Add("@bonus", SqlDbType.Float).Value = Convert.ToDouble(textBox2.Text.ToString());
                    childDataAdapter.InsertCommand.Parameters.Add("@age", SqlDbType.Int).Value = Convert.ToInt64(textBox3.Text.ToString());
                    childDataAdapter.InsertCommand.Parameters.Add("@mID", SqlDbType.Int).Value = Convert.ToInt64(textBox4.Text.ToString());

                    childDataAdapter.InsertCommand.Parameters.Add("@name", SqlDbType.VarChar).Value = textBox5.Text;
                    conn.Open();
                    childDataAdapter.InsertCommand.ExecuteNonQuery();
                    conn.Close();

                    conn.Open();
                    childDataAdapter.SelectCommand = new SqlCommand("SELECT * FROM Employees", conn);
                    ds.Tables["Employees"].Clear();
                    childDataAdapter.Fill(ds, "Employees");
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    //parentDataAdapter.SelectCommand = new SqlCommand("SELECT * FROM " + "Managers;", conn);
                    childDataAdapter.SelectCommand = new SqlCommand("SELECT * FROM " + "Employees", conn);
                    //parentDataAdapter.Fill(ds, "Managers");
                    ds.Clear();
                    childDataAdapter.Fill(ds, "Employees");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
