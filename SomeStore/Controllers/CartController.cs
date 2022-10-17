using Microsoft.AspNetCore.Mvc;
using SomeStore.Models;

namespace SomeStore.Controllers
{
	public class CartController : Controller
	{
		private ApplicationContext _db;

		public CartController(ApplicationContext db)
		{
			_db = db;
		}

		public IActionResult CartPage()
		{
			var cart = _db.CartItems
				.Where(x => x.UserName == User.Identity.Name)
				.Select(x => x.Product);

			return View(cart);
		}

		public IActionResult AddToCart(int id)
		{
            var product = _db.Products.FirstOrDefault(x => x.Id == id);

			if (product != null) 
			{
                _db.CartItems.Add(new CartItem 
				{ ProductId = id,
				  UserName = User.Identity.Name});
                _db.SaveChanges();
			}

            return RedirectToAction("Shop", "Home");
		}

		public async Task<IActionResult> DeleteFromCart(int id)
		{ 
			_db.CartItems.Remove(
				_db.CartItems
				.FirstOrDefault(x => x.ProductId == id));

			_db.SaveChanges();

            return RedirectToAction("CartPage");
        }
	}
}
