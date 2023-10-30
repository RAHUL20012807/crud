using System.Data;
using System.Data.SqlClient;
using ADOCRUD.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ADOCRUD.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class EmpController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EmpController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("GetAllEmp")]
        [HttpGet]

        public async Task<IActionResult> GetAllEmp()
        {
            List<EmpModel> empModels = new List<EmpModel>();
            DataTable dt= new DataTable();
            SqlConnection con= new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand cmd = new SqlCommand("select * from emoloyees", con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
            for(int i=0; i < dt.Rows.Count;i++)
            {
                EmpModel empModel = new EmpModel();
                empModel.empId = Convert.ToInt32(dt.Rows[i]["emp_id"]);
                empModel.empName = dt.Rows[i]["ename"].ToString();
                empModel.empAge = Convert.ToInt32(dt.Rows[i]["age"]);
                empModel.empAddress = dt.Rows[i]["address"].ToString();
                empModels.Add(empModel);
            }

            return Ok(empModels);
        }

        

        [Route("PostEmp")]
        [HttpPost]
        public async Task<IActionResult> PostEmp(EmpModel obj)
        {
            try
            {
                SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                SqlCommand cmd = new SqlCommand("insert into emoloyees values ('"+obj.empId+"','" + obj.empName + "','" + obj.empAge + "','" + obj.empAddress + "')", con);
                con.Open(); ;
                cmd.ExecuteNonQuery();
                con.Close();
                return Ok(obj);
            }
            catch (Exception ex) 
            {
                throw ex;
                
            }

        }



        [Route("UpdateEmp")]
        [HttpPost]
        public async Task<IActionResult> UpdateEmp(EmpModel obj)
        {

            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlConnection con1 = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            try
            {
                SqlCommand cmd1 = new SqlCommand("select * from emoloyees where emp_id='" + obj.empId + "' ", con1);
                
                con1.Open();
                
                SqlDataReader dr = cmd1.ExecuteReader();


                
                if (dr.HasRows)
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("update emoloyees set address='" + obj.empAddress + "' where emp_id='" + obj.empId + "' ", con);



                    cmd.ExecuteNonQuery();
                    
                    return Ok(obj);
                }
                else
                {
                    return NotFound("Please enter correct Id");
                }

            }
            catch (Exception ex)
            {
                throw ex;
                

            }
            finally
            {
                con.Close();
                con1.Close();
            }

        }


        [Route("DeleteEmp")]
        [HttpDelete]
        public async Task<IActionResult> DeleteEmp(long id)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlConnection con1 = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            try
            {
               
                SqlCommand cmd1 = new SqlCommand("select * from emoloyees where emp_id='" + id + "' ", con1);
                con1.Open();
                //cmd1.ExecuteNonQuery();
                SqlDataReader dr= cmd1.ExecuteReader();
                if (dr.HasRows)
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("delete from emoloyees where emp_id='" + id + "' ", con);



                
                
                cmd.ExecuteNonQuery();
                con.Close();
                    con1.Close();
                    return Ok("deleted sucessfully");
                }
                else
                {
                    return NotFound("Please enter correct Id");
                }

            }
            catch (Exception ex)
            {
                throw ex;

                
            }
            finally
            {
                con.Close() ;
                con1.Close  () ;
            }

        }



    }
}
