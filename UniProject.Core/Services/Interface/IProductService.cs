using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniProject.Core.DTOs;
using UniProject.Core.DTOs.Admin;
using UniProject.Core.DTOs.Admin.Product;
using UniProject.Core.DTOs.Admin.Product.Delete;
using UniProject.Core.DTOs.Admin.ProductCategory;
using UniProject.DataLayer.Entity.Product;

namespace UniProject.Core.Services.Interface
{
    public interface IProductService
    {
        #region ProductCategory
        List<ProductCategory> GetAllCategories();
        ProductCategory GetCategoryById(int categoryId);
        ShowProductCategories GetCategories(int pageId, string categoryTitle = "");
        void AddProductCategory(ProductCategory category);
        CategoryInfoViewModel GetCategoryLevelsById(int categoryId);
        void DeleteCategoryById(int categotyId);
        void UpdateCategory(ProductCategory productCategory);
        bool HasChildOrNot(int categoryId);
        bool IsUniqueCategoryUrl(string url, int? parentId);
        bool IsUniqueCategoryTitle(string title, int? parentId);
        void UpdateCategory(CategoryInfoViewModel categoryInfo);
        #endregion

        #region Product
        List<SelectListItem> GetCategoryLevel1();
        List<SelectListItem> GetCategoryLevel2(int categoryId);
        List<SelectListItem> GetCategoryLevel3(int categoryId);
        List<SelectListItem> GetSellers(string username);
        //int AddProduct(RequestAddProductDto request, IFormFile mainImage);
        ShowProductsViewModel GetProducts(int pageId, string productName = "");
        ResultDto Execute(RequestAddProductDto request);
        UploadFileDto UploadFile(IFormFile file);

        ProductInfoViewModel GetProductInfoByProductId(int productId);
        List<ShowFeaturesViewModel> GetProductFeaturesByProductId(int productId);
        List<ShowImagesViewModel> GetProductImagesByProductId(int productId);
        bool HasInventoryOrNot(int productId);
        void DeleteProductByProductId(int productId);
        void UpdateProduct(Product product);
        //Product GetProductByProductId(int productId);
        GetProductDetailsViewModel GetProductByProductId(int productId);

        Tuple<List<ShowProductListItemViewModel>, int> GetProducts(int pageId = 1, string orderByType = "date",
            int startPrice = 0, int endPrice = 0, int selectedCategory = 0, string filter = ""
            , int take = 0);

        List<ShowProductListItemViewModel> GetProductsForHomePage(int selectedCategory, int take);

        Tuple<Product, CategoryInfoViewModel> GetProductForShow(int productId);

        void EditProduct(GetProductDetailsViewModel request);
        #endregion

        #region Use In Checkout
        Tuple<List<CheckoutWithSellersViewModel>, int> GetSoldProducts(string username,int pageId = 1, string filter = "", int take = 0);
        Tuple<List<ProductsOfASellerViewModel>, int, string> GetProductsOfASeller(int sellerID, int pageId = 1, string filter = "", int take = 0);
        #endregion
    }
}
