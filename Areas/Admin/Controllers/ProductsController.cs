using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UniProject.Core.DTOs.Admin.Product;
using UniProject.Core.DTOs.Admin.Product.Delete;
using UniProject.Core.DTOs.Admin.ProductCategory;
using UniProject.Core.Security;
using UniProject.Core.Services.Interface;
using UniProject.DataLayer.Entity.Product;

namespace UniProject.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        public ProductsController(IProductService productService,
            IUserService userService)
        {
            _productService = productService;
            _userService = userService;
        }

        #region ShowProducts
        [Route("admin/ShowProducts")]
        [PermissionChecker(14)]
        public IActionResult ShowProducts(int pageId = 1, string productName = "")
        {
            ShowProductsViewModel products = _productService.GetProducts(pageId, productName);
            return View(products);
        }
        #endregion

        #region Create Product

        #region Select Category
        [PermissionChecker(1)]
        public IActionResult GetCategoryLevel2(int id)
        {
            List<SelectListItem> list = new List<SelectListItem>()
            {
                new SelectListItem(){Text = "انتخاب کنید",Value = ""}
            };
            list.AddRange(_productService.GetCategoryLevel2(id));
            return Json(new SelectList(list, "Value", "Text"));
        }

        public IActionResult GetCategoryLevel3(int id)
        {
            List<SelectListItem> list = new List<SelectListItem>()
            {
                new SelectListItem(){Text = "انتخاب کنید",Value = ""}
            };
            list.AddRange(_productService.GetCategoryLevel3(id));
            return Json(new SelectList(list, "Value", "Text"));
        }
        #endregion

        [Route("admin/createProduct")]
        [PermissionChecker(15)]
        public IActionResult CreateProduct()
        {
            //Select Category Level 1
            var categoryLevel1 = _productService.GetCategoryLevel1();
            ViewData["CategoryLevel1"] = new SelectList(categoryLevel1, "Value", "Text");

            //Select Category Level 2
            var categoryLevel2 = _productService.GetCategoryLevel2(int.Parse(categoryLevel1.First().Value));
            ViewData["CategoryLevel2"] = new SelectList(categoryLevel2, "Value", "Text");

            //Select Category Level 3
            var categoryLevel3 = _productService.GetCategoryLevel3(int.Parse(categoryLevel2.First().Value));
            ViewData["CategoryLevel3"] = new SelectList(categoryLevel3, "Value", "Text");

            //Select Seller
            var seller = _productService.GetSellers(User.Identity.Name);
            ViewData["Seller"] = new SelectList(seller, "Value", "Text");

            return View();
        }

        [Route("admin/createProduct")]
        [HttpPost]
        [PermissionChecker(15)]
        public IActionResult CreateProduct(RequestAddProductDto request, List<AddNewFeaturesDto> Features)
        {
            List<IFormFile> images = new List<IFormFile>();
            for (int i = 0; i < Request.Form.Files.Count; i++)
            {
                var file = Request.Form.Files[i];
                images.Add(file);
            }
            request.Images = images;
            request.Features = Features;

            return Json(_productService.Execute(request));
        }
        #endregion

        #region EditProduct
        [Route("admin/EditProduct/{productId}")]
        [PermissionChecker(16)]
        public IActionResult EditProduct(int productId)
        {
            var product = _productService.GetProductByProductId(productId);

            var levels = _productService.GetCategoryLevelsById(product.CategoryId);

            //Select Category Level 1
            var categoryLevel1 = _productService.GetCategoryLevel1();
            ViewData["CategoryLevel1"] = new SelectList(categoryLevel1, "Value", "Text", levels.CategoryIDLevel1);

            //Select Category Level 2
            var categoryLevel2 = _productService.GetCategoryLevel2(levels.CategoryIDLevel1.Value);
            ViewData["CategoryLevel2"] = new SelectList(categoryLevel2, "Value", "Text", levels.CategoryIDLevel2);

            //Select Category Level 3
            var categoryLevel3 = _productService.GetCategoryLevel3(levels.CategoryIDLevel2.Value);
            ViewData["CategoryLevel3"] = new SelectList(categoryLevel3, "Value", "Text", levels.CategoryIDLevel3);

            //Select Seller
            var seller = _productService.GetSellers(User.Identity.Name);
            ViewData["Seller"] = new SelectList(seller, "Value", "Text", product.SellerId);

            ViewData["Images"] = _productService.GetProductImagesByProductId(productId);
            ViewData["Features"] = _productService.GetProductFeaturesByProductId(productId);

            return View(product);
        }

        [Route("admin/EditProduct/{productId}")]
        [HttpPost]
        [PermissionChecker(16)]
        public IActionResult EditProduct(GetProductDetailsViewModel request, List<AddNewFeaturesDto> Features)
        {
            var levels = _productService.GetCategoryLevelsById(request.CategoryId);

            //Select Category Level 1
            var categoryLevel1 = _productService.GetCategoryLevel1();
            ViewData["CategoryLevel1"] = new SelectList(categoryLevel1, "Value", "Text", levels.CategoryIDLevel1);

            //Select Category Level 2
            var categoryLevel2 = _productService.GetCategoryLevel2(levels.CategoryIDLevel1.Value);
            ViewData["CategoryLevel2"] = new SelectList(categoryLevel2, "Value", "Text", levels.CategoryIDLevel2);

            //Select Category Level 3
            var categoryLevel3 = _productService.GetCategoryLevel3(levels.CategoryIDLevel2.Value);
            ViewData["CategoryLevel3"] = new SelectList(categoryLevel3, "Value", "Text", levels.CategoryIDLevel3);

            //Select Seller
            var seller = _productService.GetSellers(User.Identity.Name);
            ViewData["Seller"] = new SelectList(seller, "Value", "Text", request.SellerId);

            ViewData["Images"] = _productService.GetProductImagesByProductId(request.ProductId);
            ViewData["Features"] = _productService.GetProductFeaturesByProductId(request.ProductId);

            if (!ModelState.IsValid)
                return View(request);

            request.Features = Features;

            _productService.EditProduct(request);

            return Redirect("/admin/ShowProducts");
        }
        #endregion

        #region DeleteProduct
        [Route("admin/DeleteProduct/{productId}")]
        [PermissionChecker(17)]
        public IActionResult DeleteProduct(int productId)
        {
            var productInfo = _productService.GetProductInfoByProductId(productId);
            ViewData["Features"] = _productService.GetProductFeaturesByProductId(productId);
            ViewData["Images"] = _productService.GetProductImagesByProductId(productId);

            return View(productInfo);
        }

        [Route("admin/DeleteProduct/{productId}")]
        [HttpPost]
        [PermissionChecker(17)]
        public IActionResult DeleteProduct(int productId, ProductInfoViewModel productInfo)
        {
            ViewData["Features"] = _productService.GetProductFeaturesByProductId(productId);
            ViewData["Images"] = _productService.GetProductImagesByProductId(productId);
            if (_productService.HasInventoryOrNot(productId))
            {
                ModelState.AddModelError("Inventory", "این محصول در انبار موجود میباشد.نمیتوانید آن را حذف کنید.");
                return View(productInfo);
            }

            _productService.DeleteProductByProductId(productId);
            return Redirect("/Admin/ShowProducts");
        }
        #endregion
    }
}
