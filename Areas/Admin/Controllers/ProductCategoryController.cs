using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniProject.Core.DTOs.Admin;
using UniProject.Core.DTOs.Admin.ProductCategory;
using UniProject.Core.Security;
using UniProject.Core.Services.Interface;
using UniProject.DataLayer.Entity.Product;

namespace UniProject.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductCategoryController : Controller
    {
        private readonly IProductService _productService;
        public ProductCategoryController(IProductService productService)
        {
            _productService = productService;
        }

        #region ShowCategories
        [Route("Admin/ProductCategory")]
        [PermissionChecker(10)]
        public IActionResult ShowCategories(int pageId = 1, string CategoryTitle = "")
        {
            ShowProductCategories category = _productService.GetCategories(pageId, CategoryTitle);
            return View(category);
        }
        #endregion

        #region Create Category
        [Route("Admin/CreateCategory")]
        [PermissionChecker(11)]
        public IActionResult CreateCategory()
        {
            //Select Category Level 1
            var category = _productService.GetCategoryLevel1();
            ViewData["Category"] = new SelectList(category, "Value", "Text");

            //Select Category Level 2
            //var categoryLevel2 = _productService.GetCategoryLevel2(int.Parse(category.First().Value));
            //ViewData["SubCategory"] = new SelectList(categoryLevel2, "Value", "Text");

            return View();
        }

        [Route("Admin/CreateCategory")]
        [HttpPost]
        [PermissionChecker(11)]
        public IActionResult CreateCategory(ProductCategory productCategory)
        {
            var category = _productService.GetCategoryLevel1();
            ViewData["Category"] = new SelectList(category, "Value", "Text");

            if (!ModelState.IsValid)
                return View(productCategory);

            if (!_productService.IsUniqueCategoryTitle(productCategory.CategoryTitle,productCategory.ParentId))
            {
                ModelState.AddModelError("CategoryTitle", "نام دسته بندی منحصر به فرد نیست");
                return View(productCategory);
            }

            if (!_productService.IsUniqueCategoryUrl(productCategory.CategoryUrl, productCategory.ParentId))
            {
                ModelState.AddModelError("CategoryUrl", "Url دسته بندی منحصر به فرد نیست");
                return View(productCategory);
            }

            _productService.AddProductCategory(productCategory);
            return Redirect("/admin/ProductCategory");
        }
        #endregion

        #region Edit Category
        [Route("Admin/EditCategory/{categoryId}")]
        [PermissionChecker(12)]
        public IActionResult EditCategory(int categoryId)
        {
            var categoryInfo = _productService.GetCategoryLevelsById(categoryId);

            //Select Category Level 1
            var category = _productService.GetCategoryLevel1();
            ViewData["Category"] = new SelectList(category, "Value", "Text", categoryInfo.CategoryIDLevel1);

            //Select Category Level 2
            var categoryLevel2 = _productService.GetCategoryLevel2(int.Parse(category.First().Value));
            ViewData["SubCategory"] = new SelectList(categoryLevel2, "Value", "Text", categoryInfo.CategoryIDLevel2);

            return View(categoryInfo);
        }
        [Route("Admin/EditCategory/{categoryId}")]
        [HttpPost]
        [PermissionChecker(12)]
        public IActionResult EditCategory(CategoryInfoViewModel categoryinfo)
        {
            if (!ModelState.IsValid)
                return View(categoryinfo);

            var category = _productService.GetCategoryById(categoryinfo.CategoryIDLevel3);
            if (categoryinfo.CategoryTitleLevel3 != category.CategoryTitle)
            {
                if (!_productService.IsUniqueCategoryTitle(categoryinfo.CategoryTitleLevel3, categoryinfo.CategoryIDLevel1))
                {
                    ModelState.AddModelError("CategoryTitleLevel3", "نام دسته بندی منحصر به فرد نیست");
                    return View(categoryinfo);
                }
            }
            if (categoryinfo.CategoryURL != category.CategoryUrl)
            {
                if (!_productService.IsUniqueCategoryUrl(categoryinfo.CategoryURL, categoryinfo.CategoryIDLevel1))
                {
                    ModelState.AddModelError("CategoryURL", "Url دسته بندی منحصر به فرد نیست");
                    return View(categoryinfo);
                }
            }

            _productService.UpdateCategory(categoryinfo);

            return Redirect("/admin/ProductCategory");
        }
        #endregion

        #region Delete Category
        [Route("Admin/DeleteCategory/{categoryId}")]
        [PermissionChecker(13)]
        public IActionResult DeleteCategory(int categoryId)
        {
            var categoryInfo = _productService.GetCategoryLevelsById(categoryId);
            return View(categoryInfo);
        }

        [Route("Admin/DeleteCategory/{categoryId}")]
        [HttpPost]
        [PermissionChecker(13)]
        public IActionResult DeleteCategory(int categoryId, CategoryInfoViewModel categoryInfo)
        {
            if(_productService.HasChildOrNot(categoryId))
            {
                ModelState.AddModelError("CategoryTitleLevel3", "این دسته بندی دارای دسته بندی فرزند است. نمیتوانید آن را حذف کنید.");
                return View(categoryInfo);
            }

            _productService.DeleteCategoryById(categoryId);
            return Redirect("/Admin/ProductCategory");
        }
        #endregion
    }
}
