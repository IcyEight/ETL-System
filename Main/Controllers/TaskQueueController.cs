using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Main.Data;
using Main.ViewModels;
using Main.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Main.Controllers
{
    public class TaskQueueController : Controller
    {
        private readonly BamsDbContext _dbcontext;

        public TaskQueueController(BamsDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public ViewResult Index()
        {
            ViewBag.Title = "Task Queue";
            TaskQueueViewModel vm = new TaskQueueViewModel();
            vm.TaskQueue = GetTasksList();
            return View(vm);
        }

        public List<TaskQueueDisplayModel> GetTasksList()
        {
            DataTable table = new DataTable();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString =
                "Data Source=(localdb)\\MSSQLLocalDB;" +
                "Initial Catalog=Bams;" +
                "Integrated Security=SSPI;";
                conn.Open();
                // Creates a SQL command
                using (var command = new SqlCommand("SELECT * FROM v_GetTasksForGrid", conn))
                {
                    // Loads the query results into the table
                    table.Load(command.ExecuteReader());
                }
            }

            List<TaskQueueDisplayModel> clslist = new List<TaskQueueDisplayModel>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                TaskQueueDisplayModel cls = new TaskQueueDisplayModel();
                cls.AssetId = Convert.ToInt32(table.Rows[i]["AssetId"]);
                cls.Name = table.Rows[i]["Name"].ToString();
                cls.alertMessage = table.Rows[i]["alertMessage"].ToString();
                cls.resolvedBy = table.Rows[i]["resolvedBy"].ToString();
                cls.dateComplete = (table.Rows[i]["dateComplete"] == System.DBNull.Value) ? null : (DateTime?)table.Rows[i]["dateComplete"];
                cls.isComplete = Convert.ToBoolean(table.Rows[i]["isComplete"]) == true ? "complete" : "incomplete";
                clslist.Add(cls);
            }
            return clslist;
        }

        public JsonResult GetTasksWebView()
        {
            DataTable table = new DataTable();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString =
                "Data Source=(localdb)\\MSSQLLocalDB;" +
                "Initial Catalog=Bams;" +
                "Integrated Security=SSPI;";
                conn.Open();
                // Creates a SQL command
                using (var command = new SqlCommand("SELECT * FROM v_GetTasksForGrid", conn))
                {
                    // Loads the query results into the table
                    table.Load(command.ExecuteReader());
                }
            }

            List<TaskQueueDisplayModel> clslist = new List<TaskQueueDisplayModel>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                TaskQueueDisplayModel cls = new TaskQueueDisplayModel();
                cls.AssetId = Convert.ToInt32(table.Rows[i]["AssetId"]);
                cls.Name = table.Rows[i]["Name"].ToString();
                cls.alertMessage = table.Rows[i]["alertMessage"].ToString();
                cls.resolvedBy = table.Rows[i]["resolvedBy"].ToString();
                cls.dateComplete = (table.Rows[i]["dateComplete"] == System.DBNull.Value) ? null : (DateTime?)table.Rows[i]["dateComplete"];
                cls.isComplete = Convert.ToBoolean(table.Rows[i]["isComplete"]) == true ? "complete" : "incomplete";
                clslist.Add(cls);
            }
            return Json(clslist);
        }

        // for getting assets in the task queue without refreshing the repo to initial assets
        public JsonResult GetUpdatedTasks()
        {
            return null;
        }

        // mark task from the task queue as complete
        public void MarkTaskAsComplete(int assetId)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString =
                "Data Source=(localdb)\\MSSQLLocalDB;" +
                "Initial Catalog=Bams;" +
                "Integrated Security=SSPI;";
                conn.Open();
                // Creates a SQL command
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "MarkTaskComplete";

                    SqlParameter param = new SqlParameter("@assetId", assetId);
                    param.Direction = ParameterDirection.Input;
                    param.DbType = DbType.String;
                    cmd.Parameters.Add(param);

                    // temporarily passing my name for dummy data until we get user auth working
                    SqlParameter param2 = new SqlParameter("@resolvedBy", "Cashel, Bridget");
                    param2.Direction = ParameterDirection.Input;
                    param2.DbType = DbType.String;
                    cmd.Parameters.Add(param2);

                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }
    }
}