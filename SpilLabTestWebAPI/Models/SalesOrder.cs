namespace SpilLabTestWebAPI.Models
{
    public class SalesOrder
    {
        public string strInvoiceNo { get; set; }
        public string strCode { get; set;}
        public string strDescription { get; set;}
        public string strNote { get; set;}
        public int qty { get; set;}
        public decimal decPrice { get; set;}
        public decimal decTax { get; set;}
        
    }
}
