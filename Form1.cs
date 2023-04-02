using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace lab1_SGBD
{
    public partial class parent : Form
    {
        //SqlConnection cs = new SqlConnection("Data Source=DESKTOP-ILTATS6\\SQLEXPRESS;Initial Catalog=Port;Integrated Security=true;TrustServerCertificate=true;");
        private readonly string connectionString = @"Server=DESKTOP-ILTATS6\SQLEXPRESS;Database=Port;Integrated Security=true;TrustServerCertificate=true;";
        private DataSet ds = new DataSet();
        private SqlDataAdapter parentDataAdapter = new SqlDataAdapter();
        private SqlDataAdapter childDataAdapter = new SqlDataAdapter();
        private BindingSource parentBindingSource = new BindingSource();
        private BindingSource childBindingSource = new BindingSource();


        public parent()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender,  EventArgs e)
        {
            try
            {
                using(SqlConnection conn = new SqlConnection(connectionString))
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
                }

            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    childDataAdapter.DeleteCommand = new SqlCommand("DELETE FROM Employees " + "WHERE EmployeeID = @id;", conn);
                    childDataAdapter.DeleteCommand.Parameters.Add("@id", SqlDbType.Int).Value = ds.Tables["Employees"].Rows[childBindingSource.Position][0];
                    childDataAdapter.DeleteCommand.ExecuteNonQuery();
                    childDataAdapter.Fill(ds);
                    conn.Close();
                }

            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
