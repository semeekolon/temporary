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
        static int RecordId;


        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {

            PopulateGrid();
            //var foo = GetEmployee(2);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Employee employee = FormToEntity();
            AddEmployee(employee);

            PopulateGrid();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        protected void btnModify_Click(object sender, EventArgs e)
        {
            var employee = FormToEntity();
            ModifyEmployee(RecordId, employee);
            PopulateGrid();
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


                SqlCommand sqlCommand = new SqlCommand("getAllEmployees_sp", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                con.Open();

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

        public Employee GetEmployee(int Id)
        {
            Employee employee = new Employee();

            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {

                    SqlCommand sqlCommand = new SqlCommand("GetEmployee_sp", con);
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    con.Open();

                    sqlCommand.Parameters.AddWithValue("@EmployeeID", Id);

                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())

                        {
                            DataTable dataTable = new DataTable();

                            dataTable.Columns.Add("EmployeeID");
                            dataTable.Columns.Add("Name");
                            dataTable.Columns.Add("Position");
                            dataTable.Columns.Add("Age");
                            dataTable.Columns.Add("Salary");

                            DataRow dataRow = dataTable.NewRow();

                            dataRow["EmployeeID"] = sqlDataReader["EmployeeID"];
                            dataRow["Name"] = sqlDataReader["Name"];
                            dataRow["Position"] = sqlDataReader["Position"];
                            dataRow["Age"] = sqlDataReader["Age"];
                            dataRow["Salary"] = sqlDataReader["Salary"];

                            dataTable.Rows.Add(dataRow);

                            if (dataTable.Rows.Count == 1)
                            {
                                foreach (DataRow item in dataTable.Rows)
                                {

                                    employee.EmployeeID = Convert.ToInt64(item["EmployeeID"]);
                                    employee.Age = Convert.ToInt32(item["Age"]);
                                    employee.Name = Convert.ToString(item["Name"]);
                                    employee.Position = Convert.ToString(item["Position"]);
                                    employee.Salary = Convert.ToInt32(item["Salary"]);

                                }
                            }

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                var ErrMsg = ex.Message;
            }

            return employee;
        }


        public void AddEmployee(Employee employee)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {

                SqlCommand sqlCommand = new SqlCommand("AddEmployee_sp", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@Name", employee.Name);
                sqlCommand.Parameters.AddWithValue("@Position", employee.Position);
                sqlCommand.Parameters.AddWithValue("@Age", employee.Age);
                sqlCommand.Parameters.AddWithValue("@Salary", employee.Salary);

                con.Open();

                try
                {
                    int RowsAffected = sqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    var errMsg = ex.Message;
                }

            }
        }

        public void ModifyEmployee(int Id, Employee employee)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {

                SqlCommand sqlCommand = new SqlCommand("UpdateEmployee_sp", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@Name", employee.Name);
                sqlCommand.Parameters.AddWithValue("@Position", employee.Position);
                sqlCommand.Parameters.AddWithValue("@Age", employee.Age);
                sqlCommand.Parameters.AddWithValue("@Salary", employee.Salary); 
                sqlCommand.Parameters.AddWithValue("@EmployeeID", Id); 


                con.Open();

                try
                {
                    int RowsAffected = sqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    var errMsg = ex.Message;
                }


            }
        }
        #endregion

        #region GridEvents       

        protected void gvEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            RecordId = Convert.ToInt32(gvEmployee.SelectedRow.Cells[0].Text);

            var employee = GetEmployee(RecordId);

            EntityToForm(employee);

            btnSave.Enabled = false;

        }

        protected void gvEmployee_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvEmployee, "Select$" + e.Row.RowIndex);
                e.Row.ToolTip = "Click to View Record";
            }

        }

        #endregion

        #region Helper Methods

        public void PopulateGrid()
        {
            List<Employee> lsEmployee = new List<Employee>();
            lsEmployee = GetAll_DataReader();


            gvEmployee.DataSource = lsEmployee;
            gvEmployee.DataBind();
        }

        public Employee FormToEntity()
        {
            Employee employee = new Employee();


            try
            {
                if ((!string.IsNullOrEmpty(txtName.Text)) && (!string.IsNullOrEmpty(txtPosition.Text)) && (!string.IsNullOrEmpty(txtAge.Text)) && (!string.IsNullOrEmpty(txtSalary.Text)))

                {

                    employee.Age = Convert.ToInt32(txtAge.Text);
                    employee.Name = txtName.Text;
                    employee.Position = txtPosition.Text;
                    employee.Salary = Convert.ToInt32(txtSalary.Text);
                }

            }
            catch (Exception ex)
            {

                var errMsg = ex.Message;
            }


            return employee;
        }

        public Employee EntityToForm(Employee employee)
        {

            try
            {
                txtAge.Text = employee.Age.ToString();
                txtName.Text = employee.Name;
                txtPosition.Text = employee.Position;
                txtSalary.Text = employee.Salary.ToString();
            }
            catch (Exception ex)
            {
                var errMsg = ex.Message;
            }

            return employee;
        }

        public void ClearForm()
        {
            txtAge.Text = string.Empty;
            txtName.Text = string.Empty;
            txtPosition.Text = string.Empty;
            txtSalary.Text = string.Empty;

            btnSave.Enabled = true;
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

//CREATE PROCEDURE AddEmployee_sp
//    @Name       NVARCHAR(50),
//	@Position NVARCHAR(50),
//	@Age INT,
//    @Salary     INT
//AS
//BEGIN
//    INSERT INTO[dbo].[Employee]
//VALUES(
//       @Name,
//       @Position,
//       @Age,
//       @Salary )
//END

//EXECUTE AddEmployee_sp @Name = 'Emp1', @Position = 'Tester', @Age = 25, @Salary = 1200

//CREATE PROCEDURE UpdateEmployee_sp
//    @EmployeeID INT,
//	@Name NVARCHAR(50),
//	@Position NVARCHAR(50),
//	@Age INT,
//    @Salary     INT
//AS
//BEGIN
//    UPDATE[dbo].[Employee]
//SET
//   Name = @Name,
//   Position = @Position,
//   Age = @Age,
//   Salary = @Salary

//    WHERE EmployeeID = @EmployeeID
//END

//EXECUTE UpdateEmployee_sp @EmployeeID = 4, @Name = 'Name4', @Position = 'Position4', @Age = 21, @Salary = 1000;


//CREATE PROCEDURE GetEmployee_sp
//(
//    @EmployeeID INT
//)
//AS
//BEGIN

//    SELECT*
//    FROM[dbo].[Employee]
//WHERE EmployeeID = @EmployeeID
//END

//EXECUTE GetEmployee_sp @EmployeeID = 3;
