using Microsoft.AspNetCore.Mvc;  
using MyAPI_1.Models;        
using Microsoft.EntityFrameworkCore; 
using System.Collections.Generic; 
using System.Linq;
using MyAPI_1.Data; 


[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context; 

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

//    [HttpPost("checkout")]
//     public IActionResult Checkout([FromBody] List<CheckoutItem> cartItems)
// {
//     // 1. ตรวจสอบว่ามีการส่งข้อมูลมาไหม
//     if (cartItems == null || cartItems.Count == 0)
//     {
//         return BadRequest("ตะกร้าสินค้าว่างเปล่า");
//     }

//     using (var transaction = _context.Database.BeginTransaction())
//     {
//         try
//         {
//             // 2. วนลูปตรวจสอบสต็อกสินค้าทุกชิ้นก่อน (ยังไม่ตัดจริง)
//             foreach (var item in cartItems)
//             {
//                 var product = _context.Products.FirstOrDefault(p => p.ProductID == item.ProductID);
//                 if (product == null)
//                 {
//                     return BadRequest($"ไม่พบสินค้า ID: {item.ProductID}");
//                 }
//                 if (product.ProductStock < item.Quantity)
//                 {
//                     return BadRequest($"สินค้า '{product.ProductName}' มีของไม่พอ (เหลือ {product.ProductStock})");
//                 }
//             }

//             // 3. เมื่อผ่านการตรวจสอบทุกชิ้นแล้ว ค่อยวนลูปตัดสต็อกจริง
//             foreach (var item in cartItems)
//             {
//                 var product = _context.Products.First(p => p.ProductID == item.ProductID);
//                 product.ProductStock -= item.Quantity; // ตัดสต็อก
//             }

//             // 4. บันทึกลง Database
//             _context.SaveChanges();
//             transaction.Commit(); // ยืนยันการทำงานทั้งหมด

//             return Ok(new { message = "ชำระเงินสำเร็จ ตัดสต็อกเรียบร้อยแล้ว" });
//         }
//         catch (Exception ex)
//         {
//             transaction.Rollback(); // ถ้ามีอะไรผิดพลาด ให้ย้อนกลับข้อมูลทั้งหมด (ไม่ตัดมั่ว)
//             return StatusCode(500, $"เกิดข้อผิดพลาดภายในระบบ: {ex.Message}");
//         }
//     }
// }


[HttpPost("pickup")]
public IActionResult Pickup([FromBody] PickupRequest request)
{
    // ตรวจสอบข้อมูลเบื้องต้น
    if (request == null || request.Items == null || !request.Items.Any())
    {
        return BadRequest("ข้อมูลไม่ถูกต้อง หรือไม่มีรายการสินค้า");
    }

    using (var transaction = _context.Database.BeginTransaction())
    {
        try
        {
            foreach (var item in request.Items)
            {
                var product = _context.Products.FirstOrDefault(p => p.ProductID == item.ProductID);
                if (product == null) continue; 

                switch (request.ActionType.ToLower())
                {
                    case "update":
                
                        if (item.Quantity > 0 && product.ProductStock < item.Quantity)
                        {
                            throw new Exception($"สินค้า '{product.ProductName}' มีของไม่พอ (เหลือ {product.ProductStock})");
                        }
                        product.ProductStock -= item.Quantity;

                        break;

                    case "clear_all":
                        product.ProductStock += item.Quantity;
                    break;

                    case "clear_category":
                        product.ProductStock += item.Quantity;
                        break;

                }
            }

            _context.SaveChanges();
            transaction.Commit();

            return Ok(new { message = "อัปเดตสต็อกสำเร็จ" });
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return StatusCode(500, $"เกิดข้อผิดพลาด: {ex.Message}");
        }
    }
}
[HttpGet("products")]
public IActionResult GetProducts()
{
    var products = _context.Products.ToList();
    return Ok(products);
}
}