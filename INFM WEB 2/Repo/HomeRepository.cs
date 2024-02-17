using INFM_WEB_2.Data;

namespace INFM_WEB_2.Repo
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _db;
        public HomeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Category>> Categories()
        {
            return await _db.Categories.ToListAsync();
        }

        public async Task<IEnumerable<Product>> DisplayProducts(string sTerm = "", int categoryId = 0)
        {
            sTerm = sTerm.ToLower();
            IEnumerable<Product> products = await (from product in _db.Products
                                                   join category in _db.Categories
                                                   on product.Category_Id equals category.Category_Id
                                                   where string.IsNullOrWhiteSpace(sTerm) || (product != null && product.Product_Name.ToLower().StartsWith(sTerm))

                                                   select new Product
                                                   {
                                                       Product_Id = product.Product_Id,
                                                       Product_Name = product.Product_Name,
                                                       ProductDescription = product.ProductDescription,
                                                       Category_Id = product.Category_Id,
                                                       Product_Price = product.Product_Price,
                                                       Category_Name = category.CategoryName,
                                                   }
                            ).ToListAsync();
            if (categoryId > 0)
            {
                products = products.Where(async => async.Category_Id == categoryId).ToList();
            }
            return products;
        }
    }
}
