using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using SpilLabTestWebAPI.Models;

namespace SpilLabTestWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesOrderController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public SalesOrderController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public JsonResult Post(SalesOrder salesOrder)
        {
            decimal exclamount = salesOrder.decPrice * salesOrder.qty;
            decimal taxAmount = exclamount * (salesOrder.decTax / 100);
            decimal inclAmount = exclamount + taxAmount;
            string query = @"
                           insert into dbo.Order_Item
                           (InvoiceNo,Code,Description,Note,Qty,Price,Tax,ExclAmount,TaxAmount,InclAmount)
                    values (@strInvoiceNo,@strCode,@strDescription,@strNote,@qty,@decPrice,@decTax,@exclAmount,@taxAmount,
                            @inclAmount)
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@strInvoiceNo", salesOrder.strInvoiceNo);
                    myCommand.Parameters.AddWithValue("@strCode", salesOrder.strCode);
                    myCommand.Parameters.AddWithValue("@strDescription", salesOrder.strDescription);
                    myCommand.Parameters.AddWithValue("@strNote", salesOrder.strNote);
                    myCommand.Parameters.AddWithValue("@qty", salesOrder.qty);
                    myCommand.Parameters.AddWithValue("@decPrice", salesOrder.decPrice);
                    myCommand.Parameters.AddWithValue("@decTax", salesOrder.decTax);
                    myCommand.Parameters.AddWithValue("@exclAmount", exclamount);
                    myCommand.Parameters.AddWithValue("@taxAmount", taxAmount);
                    myCommand.Parameters.AddWithValue("@inclAmount", inclAmount);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Sales Orde Added Successfully");
        }
    }
}
