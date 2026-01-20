using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AdZZiM.Data;
using AdZZiM.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;

[Authorize]
public class OrdersController : Controller
{
    private readonly ApplicationDbContext _context;

    public OrdersController(ApplicationDbContext context)
    {
        _context = context;
    }
    
        public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var ordersQuery = _context.Orders.AsQueryable();

        if (User.IsInRole("Admin"))
        {
                        ordersQuery = ordersQuery.Include(o => o.User);
        }
        else
        {
                        ordersQuery = ordersQuery.Where(o => o.UserId == userId);
        }

        return View(await ordersQuery.OrderByDescending(o => o.OrderDate).ToListAsync());
    }
        public IActionResult Create()
    {
                LoadProductData();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Order order, int[] ProductId, int[] Quantity)
    {
        order.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        order.OrderDate = DateTime.Now;
        order.Status = "W realizacji";
        order.ShippingMethod = "Kurier";

        ModelState.Remove("UserId");
        ModelState.Remove("User");
        ModelState.Remove("OrderItems");

        if (ModelState.IsValid && ProductId != null && ProductId.Length > 0)
        {
            decimal totalAmount = 0;

            
            for (int i = 0; i < ProductId.Length; i++)
            {
                var product = await _context.Products.FindAsync(ProductId[i]);
                if (product != null && Quantity[i] > 0)
                {
                    order.OrderItems.Add(new OrderItem
                    {
                        ProductId = ProductId[i],
                        Quantity = Quantity[i],
                        UnitPrice = product.Price
                    });

                    totalAmount += product.Price * Quantity[i];
                }
            }

            order.TotalAmount = totalAmount;

            _context.Add(order);
            await _context.SaveChangesAsync();

            TempData["OrderSuccess"] = "Zamówienie zostało złożone pomyślnie!";
            return RedirectToAction("Index", "Home");
        }

        
        LoadProductData(ProductId?.FirstOrDefault());
        return View(order);
    }

    private void LoadProductData(int? selectedProductId = null)
    {
        var products = _context.Products.Select(p => new
        {
            p.Id,
                        DisplayText = p.Name + " (" + p.Price + " PLN)",
            p.Price
        }).ToList();

                ViewBag.ProductId = new SelectList(products, "Id", "DisplayText", selectedProductId);

                ViewBag.ProductPrices = products.ToDictionary(p => p.Id, p => p.Price);
    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var order = await _context.Orders
            .Include(o => o.User)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }

            [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order != null)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

                var order = await _context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)             .FirstOrDefaultAsync(m => m.Id == id);

        if (order == null)
        {
            return NotFound();
        }

                        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!User.IsInRole("Admin") && order.UserId != currentUserId)
        {
            return Forbid();         }

        return View(order);
    }
}