using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;

namespace INFM_WEB_2.Repo
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartRepository(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor,
            UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<int> AddItem(int productId, int qty)
        {
            // cart - saved
            // cartDetail -> error
            string userId = GetUserId();
            using var transaction = _db.Database.BeginTransaction();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not Logged-In");
                var cart = await GetCart(userId);
                if (cart is null)
                {
                    cart = new ShoppingCart
                    {
                        UserId = userId
                    };
                    _db.ShoppingCarts.Add(cart);
                }
                _db.SaveChanges();
                // cart details section
                var cartItem = _db.CartDetails
                    .FirstOrDefault(a => a.ShoppingCart_Id == cart.ShoppingCart_Id && a.Product_Id == productId);
                if (cartItem != null)
                {
                    cartItem.Quantity += qty;
                }
                else
                {
                    var product = _db.Products.Find(productId);
                    cartItem = new CartDetail
                    {
                        Product_Id = productId,
                        ShoppingCart_Id = cart.ShoppingCart_Id,
                        Quantity = qty,
                        UnitPrice = product.Product_Price
                    };
                    _db.CartDetails.Add(cartItem);
                }
                _db.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }

        public async Task<int> RemoveItem(int productId)
        {
            // cart - saved
            // cartDetail -> error
            string userId = GetUserId();
            //using var transaction = _db.Database.BeginTransaction();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not Logged-In");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new Exception("Cart is Empty");
                // cart details section
                var cartItem = _db.CartDetails
                                  .FirstOrDefault(a => a.ShoppingCart_Id == cart.ShoppingCart_Id && a.Product_Id == productId);
                if (cartItem is null)
                    throw new Exception("No Items in Cart");
                else if (cartItem.Quantity == 1)
                    _db.CartDetails.Remove(cartItem);
                else
                    cartItem.Quantity = cartItem.Quantity - 1;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }

        public async Task<ShoppingCart> GetUserCart()
        {
            var userId = GetUserId();
            if (userId == null)
                throw new Exception("Invalid userId");
            var shoppingCart = await _db.ShoppingCarts
                .Include(a => a.CartDetails)
                .ThenInclude(a => a.Products)
                .ThenInclude(a => a.Category)
                .Where(a => a.UserId == userId).FirstOrDefaultAsync();
            return shoppingCart;
        }
        public async Task<ShoppingCart> GetCart(string userId)
        {
            var cart = await _db.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;
        }

        public async Task<int> GetCartItemCount(string userId = "")
        {
            if (!string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }
            var data = await (from cart in _db.ShoppingCarts
                              join cartDetail in _db.CartDetails
                              on cart.ShoppingCart_Id equals cartDetail.ShoppingCart_Id
                              select new { cartDetail.Id }
                        ).ToListAsync();
            return data.Count;
        }

        public async Task DoCheck()
        {
            using var transaction = _db.Database.BeginTransaction();
            try
            {
                // move data -> order and orderDetail 
                // order, orderDetails
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not logged-In");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new Exception("Invalid Cart");
                var cartDetail = _db.CartDetails
                    .Where(a => a.Id == cart.ShoppingCart_Id).ToList();
                if (cartDetail.Count == 0)
                    throw new Exception("Cart is Empty");
                var order = new Order
                {
                    UserId = userId,
                    CreatedDate = DateTime.Now,
                    OrderStatusId = 1 //Pending 
                };
                _db.Orders.Add(order);
                _db.SaveChanges();
                foreach(var item in cartDetail)
                {
                    var orderDetail = new OrderDetail
                    {
                        Product_Id = item.Product_Id,
                        OrderId = order.Order_Id,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }
    }
}
