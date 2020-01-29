using CrudDemo1.PlainSQL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CrudDemo1.PlainSQL
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        static string ConnectionString = "Data source=DESKTOP-R9QOUTA\\SQLEXPRESS; database= dbName; integrated security=SSPI";


        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {

            PopulateGrid();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region DatabaseMethods

        public List<Employee> GetAll_SqlDataAdapter()
        {
            List<Employee> lsEmployee = new List<Employee>();
            DataTable dt;

            try
            {
                SqlConnection con = new SqlConnection(ConnectionString);

                dt = new DataTable();

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("getAllEmployees_sp", con);
                sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                sqlDataAdapter.Fill(dt);

                if (dt != null)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        lsEmployee.Add(new Employee()
                        {
                            EmployeeID = Convert.ToInt64(item["EmployeeID"]),
                            Age = Convert.ToInt32(item["Age"]),
                            Name = Convert.ToString(item["Name"]),
                            Position = Convert.ToString(item["Position"]),
                            Salary = Convert.ToInt32(item["Salary"]),
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                var errMsg = ex.Message;
                throw;
            }

            return lsEmployee;

        }

        public List<Employee> GetAll_DataReader()
        {
            List<Employee> lsEmployee = new List<Employee>();

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {

                con.Open();
                SqlCommand sqlCommand = new SqlCommand("getAllEmployees_sp", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    DataTable dataTable = new DataTable();

                    dataTable.Columns.Add("EmployeeID");
                    dataTable.Columns.Add("Name");
                    dataTable.Columns.Add("Position");
                    dataTable.Columns.Add("Age");
                    dataTable.Columns.Add("Salary");

                    while (sqlDataReader.Read())
                    {
                        DataRow dataRow = dataTable.NewRow();

                        dataRow["EmployeeID"] = sqlDataReader["EmployeeID"];
                        dataRow["Name"] = sqlDataReader["Name"];
                        dataRow["Position"] = sqlDataReader["Position"];
                        dataRow["Age"] = sqlDataReader["Age"];
                        dataRow["Salary"] = sqlDataReader["Salary"];

                        dataTable.Rows.Add(dataRow);

                        
                    }

                    if (dataTable != null)
                    {
                        foreach (DataRow item in dataTable.Rows)
                        {
                            lsEmployee.Add(new Employee()
                            {
                                EmployeeID = Convert.ToInt64(item["EmployeeID"]),
                                Age = Convert.ToInt32(item["Age"]),
                                Name = Convert.ToString(item["Name"]),
                                Position = Convert.ToString(item["Position"]),
                                Salary = Convert.ToInt32(item["Salary"]),
                            });
                        }
                    }

                }

            }

            return lsEmployee;
        }
        #endregion

        #region GridMethods

        public void PopulateGrid()
        {
            List<Employee> lsEmployee = new List<Employee>();
            lsEmployee = GetAll_DataReader();


            gvEmployee.DataSource = lsEmployee;
            gvEmployee.DataBind();
        }
        #endregion

        #region Helper Methods
        public Employee FormToEntity()
        {
            Employee employee = new Employee();


            try
            {
                if ((!string.IsNullOrEmpty(txtName.Text)) && (!string.IsNullOrEmpty(txtPosition.Text)) && (!string.IsNullOrEmpty(txtAge.Text)) && (!string.IsNullOrEmpty(txtSalary.Text)))

                {

                    employee.Age = Convert.ToInt32(txtName.Text);
                    employee.Name = txtName.Text;
                    employee.Position = (txtPosition.Text);
                    employee.Salary = Convert.ToInt32(txtSalary.Text);
                }

            }
            catch (Exception ex)
            {

                var errMsg = ex.Message;
            }


            return employee;
        }
        #endregion

    }

}





//CREATE TABLE[dbo].[Employee]
//(

//   [EmployeeID][int] IDENTITY(1,1) NOT NULL,

//  [Name] [varchar] (50) NULL,
//	[Position] [varchar] (50) NULL,
//	[Age] [int] NULL,
//	[Salary] [int] NULL,
// CONSTRAINT[PK_Employee] PRIMARY KEY CLUSTERED
//(
//   [EmployeeID] ASC
//)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
//) ON[PRIMARY]

//CREATE PROCEDURE getAllEmployees_sp
//AS
//BEGIN
//   SELECT* FROM  dbo.Employee
//END;

//public class Employee
//{
//    public long EmployeeID { get; set; }
//    public string Name { get; set; }

//    public string Position { get; set; }

//    public int Age { get; set; }

//    public int Salary { get; set; }
//}