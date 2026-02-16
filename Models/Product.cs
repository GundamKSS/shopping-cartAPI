namespace MyAPI_1.Models;

public class Product
{
    public int ProductID { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public double ProductPrice { get; set; }
    public string ProductDes { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime DT { get; set; } 
    public string ProductCategory { get; set; } = string.Empty;
    public string ProductImage { get; set; } = string.Empty;
    public int ProductStock { get; set; }

}