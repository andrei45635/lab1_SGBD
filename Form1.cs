using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace lab1_SGBD
{
    public partial class parent : Form
    {
        private static readonly string connectionString = @"Server=DESKTOP-P0J5H9P;Database=Port;Integrated Security=true;TrustServerCertificate=true;";
        private static readonly string selectEmpQuery = "SELECT * FROM Employees;";
        private static readonly DataSet ds = new DataSet();
        private static readonly SqlDataAdapter parentDataAdapter = new SqlDataAdapter();
        private static readonly SqlDataAdapter childDataAdapter = new SqlDataAdapter();
        private static readonly BindingSource parentBindingSource = new BindingSource();
        private static readonly BindingSource childBindingSource = new BindingSource();


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
                    String parentQuery = "SELECT * FROM Managers";
                    String childQuery = "SELECT * FROM Employees";
                    parentDataAdapter.SelectCommand = new SqlCommand(parentQuery, conn);
                    childDataAdapter.SelectCommand = new SqlCommand(childQuery, conn);
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
                    string deleteQuery = "DELETE FROM Employees WHERE EmployeeID = @id;";
                    childDataAdapter.SelectCommand = new SqlCommand(selectEmpQuery, conn);
                    childDataAdapter.DeleteCommand = new SqlCommand(deleteQuery, conn);
                    childDataAdapter.DeleteCommand.Parameters.Add("@id", SqlDbType.Int).Value = dataGridViewChild.Rows[dataGridViewChild.SelectedRows[0].Index].Cells["EmployeeID"].Value.ToString();
                    conn.Open();
                    ds.Tables["Employees"].Rows[0].Delete();
                    int rowsAffected = childDataAdapter.Update(ds, "Employees");
                    
                    if(rowsAffected > 0)
                    {
                        MessageBox.Show("Deleted successfully!");
                        ds.Tables["Employees"].Clear();
                        childDataAdapter.Fill(ds, "Employees");
                    }
                    else
                    {
                        MessageBox.Show("Error");
                    }
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
                    string updateQuery = "UPDATE Employees SET EmployeeSalary=@salary, EmployeeBonus=@bonus, EmployeeName=@name WHERE EmployeeID=@id;";
                    childDataAdapter.UpdateCommand = new SqlCommand(updateQuery, conn);
                    childDataAdapter.UpdateCommand.Parameters.Add("@salary", SqlDbType.Float).Value = Convert.ToDouble(textBox1.Text.ToString());
                    childDataAdapter.UpdateCommand.Parameters.Add("@bonus", SqlDbType.Float).Value = Convert.ToDouble(textBox2.Text.ToString());
                    childDataAdapter.UpdateCommand.Parameters.Add("@name", SqlDbType.VarChar).Value = textBox5.Text;
                    childDataAdapter.UpdateCommand.Parameters.Add("@id", SqlDbType.Int).Value = dataGridViewChild.Rows[dataGridViewChild.SelectedRows[0].Index].Cells["EmployeeID"].Value.ToString();

                    conn.Open();
                    
                    int rows = childDataAdapter.UpdateCommand.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Updated!");
                    }
                    conn.Close();

                    conn.Open();
                    childDataAdapter.SelectCommand = new SqlCommand(selectEmpQuery, conn);
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
                    string insertQuery = "INSERT INTO Employees(EmployeeSalary, EmployeeBonus, EmployeeAge, ManagerID, EmployeeName) VALUES (@salary, @bonus, @age, @mID, @name);";
                    childDataAdapter.InsertCommand = new SqlCommand(insertQuery, conn);
                    childDataAdapter.InsertCommand.Parameters.Add("@salary", SqlDbType.Float).Value = Convert.ToDouble(textBox1.Text.ToString());
                    childDataAdapter.InsertCommand.Parameters.Add("@bonus", SqlDbType.Float).Value = Convert.ToDouble(textBox2.Text.ToString());
                    childDataAdapter.InsertCommand.Parameters.Add("@age", SqlDbType.Int).Value = Convert.ToInt64(textBox3.Text.ToString());
                    childDataAdapter.InsertCommand.Parameters.Add("@mID", SqlDbType.Int).Value = Convert.ToInt64(textBox4.Text.ToString());
                    childDataAdapter.InsertCommand.Parameters.Add("@name", SqlDbType.VarChar).Value = textBox5.Text;
                    conn.Open();
                    childDataAdapter.InsertCommand.ExecuteNonQuery();
                    conn.Close();

                    conn.Open();
                    childDataAdapter.SelectCommand = new SqlCommand(selectEmpQuery, conn);
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
    }
}
