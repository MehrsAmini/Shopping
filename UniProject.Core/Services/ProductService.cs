using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.Core.Security;
using UniProject.Core.Convertor;
using UniProject.Core.DTOs;
using UniProject.Core.DTOs.Admin;
using UniProject.Core.DTOs.Admin.Product;
using UniProject.Core.DTOs.Admin.Product.Delete;
using UniProject.Core.DTOs.Admin.ProductCategory;
using UniProject.Core.Generator;
using UniProject.Core.Services.Interface;
using UniProject.DataLayer.Context;
using UniProject.DataLayer.Entity.Order;
using UniProject.DataLayer.Entity.Product;

namespace UniProject.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly UniProjectContext _context;
        private readonly IHostingEnvironment _environment;
        public ProductService(UniProjectContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _environment = hostingEnvironment;
        }

        #region addProduct
        //public int AddProduct(RequestAddProductDto request, IFormFile mainImage)
        //{
        //    request.ImageName = "man.png";

        //    if (mainImage != null && mainImage.IsImage())
        //    {
        //        request.ImageName = NameGenerator.GenerateUniqCode() + Path.GetExtension(mainImage.FileName);
        //        string demoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Products/MainImage", request.ImageName);
        //        using (var stream = new FileStream(demoPath, FileMode.Create))
        //        {
        //            mainImage.CopyTo(stream);
        //        }
        //    }


        //    var category = _context.ProductCategories.Find(request.CategoryId);
        //    var seller = _context.Users.Find(request.SellerId);

        //    Product product = new Product()
        //    {
        //        ProductName = request.ProductName,
        //        ShortDescription = request.ShortDescription,
        //        Description = request.Description,
        //        ImageName = request.ImageName,
        //        IsActive = request.IsActive,
        //        ProductCategory = category,
        //        User = seller,
        //        Price = request.Price,
        //        Inventory = request.Inventory,
        //        CreateDate = DateTime.Now,
        //        IsDelete = false
        //    };
        //    _context.Add(product);
        //    _context.SaveChanges();

        //    return product.ProductId;


        //    #region add Images
        //    //List<ProductImages> productImages = new List<ProductImages>();
        //    //foreach (var item in request.Images)
        //    //{
        //    //    var uploadedResult = UploadFile(item);
        //    //    productImages.Add(new ProductImages
        //    //    {
        //    //        Product = product,
        //    //        Src = uploadedResult.FileNameAddress,
        //    //    });
        //    //}
        //    //_context.ProductImages.AddRange(productImages);
        //    #endregion

        //    #region add Features
        //    //List<ProductFeatures> productFeatures = new List<ProductFeatures>();
        //    //foreach (var item in request.Features)
        //    //{
        //    //    productFeatures.Add(new ProductFeatures
        //    //    {
        //    //        DisplayName = item.DisplayName,
        //    //        Value = item.Value,
        //    //        Product = product,
        //    //    });
        //    //}
        //    //_context.ProductFeatures.AddRange(productFeatures);
        //    #endregion
        //}
        #endregion

        public void AddProductCategory(ProductCategory category)
        {
            _context.Add(category);
            category.IsDelete = false;
            category.RegisterDate = DateTime.Now;
            _context.SaveChanges();
        }

        public List<ProductCategory> GetAllCategories()
        {
            return _context.ProductCategories.Include(c => c.ProductCategories).ToList();
        }

        public ShowProductCategories GetCategories(int pageId, string categoryTitle = "")
        {
            IQueryable<ProductCategory> result = _context.ProductCategories;

            if (!string.IsNullOrEmpty(categoryTitle))
                result = result.Where(u => u.CategoryTitle.Contains(categoryTitle));

            // Show Item In Page
            int take = 20;
            int skip = (pageId - 1) * take;

            ShowProductCategories list = new ShowProductCategories();

            //PageCount
            int pageCount = result.Count() % take;
            list.PageCount = result.Count() / take;
            if (pageCount > 1)
                list.PageCount++;

            list.CurrentPage = pageId;
            list.ProductCategories = result.OrderBy(u => u.RegisterDate)
                .Skip(skip).Take(take).ToList();

            return list;
        }

        #region Category Levels
        public List<SelectListItem> GetCategoryLevel1()
        {
            return _context.ProductCategories.Where(c => c.ParentId == null)
                .Select(c => new SelectListItem
                {
                    Text = c.CategoryTitle,
                    Value = c.CategoryId.ToString()
                }).ToList();
        }
        public List<SelectListItem> GetCategoryLevel2(int categoryId)
        {
            return _context.ProductCategories.Where(c => c.ParentId == categoryId)
                .Select(c => new SelectListItem
                {
                    Text = c.CategoryTitle,
                    Value = c.CategoryId.ToString()
                }).ToList();
        }

        public List<SelectListItem> GetCategoryLevel3(int categoryId)
        {
            return _context.ProductCategories.Where(c => c.ParentId == categoryId)
                .Select(c => new SelectListItem
                {
                    Text = c.CategoryTitle,
                    Value = c.CategoryId.ToString()
                }).ToList();
        }
        #endregion
        public List<SelectListItem> GetSellers(string username)
        {
            int userId = _context.Users.SingleOrDefault(u => u.UserName == username).UserId;
            List<int> roleId = _context.UserRoles.Where(u => u.UserId == userId).Select(u => u.RoleId).ToList();
            if (roleId.Contains(1))
            {
                return _context.UserRoles.Where(r => r.RoleId == 2).Include(r => r.User)
                .Select(u => new SelectListItem()
                {
                    Text = u.User.UserName,
                    Value = u.UserId.ToString()
                }).ToList();
            }
            else
            {
                return _context.UserRoles.Where(r => r.RoleId == 2 && r.UserId == userId)
                    .Include(r => r.User)
                    .Select(u => new SelectListItem()
                    {
                        Text = u.User.UserName,
                        Value = u.UserId.ToString()
                    }).ToList();
            }
        }

        public ShowProductsViewModel GetProducts(int pageId, string productName = "")
        {
            IQueryable<Product> result = _context.Products;

            if (!string.IsNullOrEmpty(productName))
                result = result.Where(u => u.ProductName.Contains(productName));

            // Show Item In Page
            int take = 10;
            int skip = (pageId - 1) * take;

            ShowProductsViewModel list = new ShowProductsViewModel();
            list.CurrentPage = pageId;

            //PageCount
            int pageCount = result.Count() % take;
            list.PageCount = result.Count() / take;
            if (pageCount > 1)
                list.PageCount++;

            list.Products = result.OrderBy(u => u.CreateDate).Include(c => c.ProductCategory).Include(c => c.User)
                .Skip(skip).Take(take).ToList();

            return list;
        }

        public CategoryInfoViewModel GetCategoryLevelsById(int categoryId)
        {
            CategoryInfoViewModel categoryInfo = new CategoryInfoViewModel();
            var category = _context.ProductCategories.SingleOrDefault(c => c.CategoryId == categoryId);

            categoryInfo.CategoryIDLevel3 = category.CategoryId;
            categoryInfo.CategoryTitleLevel3 = category.CategoryTitle;
            categoryInfo.CategoryURL = category.CategoryUrl;

            if (category.ParentId != null)
            {
                int? parentId = category.ParentId;
                var parent = _context.ProductCategories.SingleOrDefault(c => c.CategoryId == parentId);

                if (parent.ParentId != null)
                {
                    int? ParentID = parent.ParentId;
                    var Parent = _context.ProductCategories.SingleOrDefault(c => c.CategoryId == ParentID);

                    categoryInfo.CategoryTitleLevel1 = Parent.CategoryTitle;
                    categoryInfo.CategoryIDLevel1 = ParentID;

                    categoryInfo.CategoryTitleLevel2 = parent.CategoryTitle;
                    categoryInfo.CategoryIDLevel2 = parentId;
                }
                else
                {
                    categoryInfo.CategoryTitleLevel1 = parent.CategoryTitle;
                    categoryInfo.CategoryIDLevel1 = parentId;
                }
            }
            return categoryInfo;
        }

        public void DeleteCategoryById(int categotyId)
        {
            var category = _context.ProductCategories.SingleOrDefault(c => c.CategoryId == categotyId);
            category.IsDelete = true;
            UpdateCategory(category);
        }

        public void UpdateCategory(ProductCategory productCategory)
        {
            _context.Entry(productCategory).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public bool HasChildOrNot(int categoryId)
        {
            return _context.ProductCategories.Any(c => c.ParentId == categoryId);
        }

        public UploadFileDto UploadFile(IFormFile file)
        {
            if (file != null)
            {
                string folder = $@"images\ProductImages\";
                var uploadsRootFolder = Path.Combine(_environment.WebRootPath, folder);
                if (!Directory.Exists(uploadsRootFolder))
                {
                    Directory.CreateDirectory(uploadsRootFolder);
                }


                if (file == null || file.Length == 0)
                {
                    return new UploadFileDto()
                    {
                        Status = false,
                        FileNameAddress = "",
                    };
                }

                string fileName = DateTime.Now.Ticks.ToString() + file.FileName;
                var filePath = Path.Combine(uploadsRootFolder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                return new UploadFileDto()
                {
                    FileNameAddress = folder + fileName,
                    Status = true,
                };
            }
            return null;
        }
        public ResultDto Execute(RequestAddProductDto request)
        {
            try
            {
                var category = _context.ProductCategories.Find(request.CategoryId);
                var seller = _context.Users.Find(request.SellerId);

                Product product = new Product()
                {
                    ProductName = request.ProductName,
                    ShortDescription = request.ShortDescription,
                    Description = request.Description,
                    Brand = request.Brand,
                    IsActive = true,
                    Price = request.Price,
                    Inventory = request.Inventory,
                    ProductCategory = category,
                    User = seller,
                    CreateDate = DateTime.Now,
                    IsDelete = false
                };
                _context.Products.Add(product);

                List<ProductGallery> productImages = new List<ProductGallery>();
                foreach (var item in request.Images)
                {
                    var uploadedResult = UploadFile(item);
                    productImages.Add(new ProductGallery()
                    {
                        Product = product,
                        ImageName = uploadedResult.FileNameAddress,
                    });
                }

                _context.ProductGalleries.AddRange(productImages);


                List<ProductFeature> productFeatures = new List<ProductFeature>();
                foreach (var item in request.Features)
                {
                    productFeatures.Add(new ProductFeature()
                    {
                        FeatuerTitle = item.FeatuerTitle,
                        FeatureValue = item.FeatureValue,
                        Product = product,
                    });
                }
                _context.ProductFeatures.AddRange(productFeatures);

                _context.SaveChanges();
                return new ResultDto
                {
                    IsSuccess = true,
                    Message = "محصول با موفقیت به محصولات فروشگاه اضافه شد",
                };
            }
            catch (Exception ex)
            {

                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "خطا رخ داد ",
                };
            }
        }

        public ProductInfoViewModel GetProductInfoByProductId(int productId)
        {
            var product = _context.Products.SingleOrDefault(p => p.ProductId == productId);

            //Get UserName
            int sellerId = product.SellerId;
            string userName = _context.Users.SingleOrDefault(u => u.UserId == sellerId).UserName;

            //Show Product
            ProductInfoViewModel productInfo = new ProductInfoViewModel()
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ShortDescription = product.ShortDescription,
                Description = product.Description,
                Brand = product.Brand,
                Price = product.Price,
                Inventory = product.Inventory,
                IsActive = product.IsActive,
                SellerName = userName
            };

            //Get Category Title
            var categoryId = product.CategoryId;
            var category = _context.ProductCategories.SingleOrDefault(c => c.CategoryId == categoryId);

            productInfo.CategoryTitleLevel3 = category.CategoryTitle;

            if (category.ParentId != null)
            {
                int? parentId = category.ParentId;

                var parent = _context.ProductCategories.SingleOrDefault(c => c.CategoryId == parentId);

                if (parent.ParentId != null)
                {
                    int? ParentID = parent.ParentId;
                    var Parent = _context.ProductCategories.SingleOrDefault(c => c.CategoryId == ParentID);
                    productInfo.CategoryTitleLevel1 = Parent.CategoryTitle;
                    productInfo.CategoryTitleLevel2 = parent.CategoryTitle;
                }
                else
                {
                    productInfo.CategoryTitleLevel1 = parent.CategoryTitle;
                }
            }

            return productInfo;
        }

        public List<ShowFeaturesViewModel> GetProductFeaturesByProductId(int productId)
        {
            List<ShowFeaturesViewModel> features = _context.ProductFeatures
                .Where(f => f.ProductId == productId)
                .Select(f => new ShowFeaturesViewModel()
                {

                    FeatuerTitle = f.FeatuerTitle,
                    FeatureValue = f.FeatureValue
                }).ToList();

            return features;
        }

        public List<ShowImagesViewModel> GetProductImagesByProductId(int productId)
        {
            List<ShowImagesViewModel> imageInfo = _context.ProductGalleries
                .Where(i => i.ProductId == productId)
                .Select(i => new ShowImagesViewModel()
                {

                    ImageName = i.ImageName
                }).ToList();

            return imageInfo;
        }

        public void DeleteProductByProductId(int productId)
        {
            Product product = _context.Products.SingleOrDefault(c => c.ProductId == productId);
            product.IsDelete = true;
            product.IsActive = false;
            UpdateProduct(product);
        }

        public void UpdateProduct(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public bool HasInventoryOrNot(int productId)
        {
            int inventory = _context.Products.SingleOrDefault(c => c.ProductId == productId).Inventory;
            if (inventory > 0)
                return true;
            else
                return false;
        }

        public bool IsUniqueCategoryUrl(string url, int? parentId)
        {
            List<ProductCategory> CategoryLevel = _context.ProductCategories.Where(c => c.ParentId == parentId).ToList();
            foreach (var item in CategoryLevel)
            {
                if (item.CategoryUrl == url)
                    return false;
            }

            return true;
        }

        public bool IsUniqueCategoryTitle(string title, int? parentId)
        {
            List<ProductCategory> CategoryLevel = _context.ProductCategories.Where(c => c.ParentId == parentId).ToList();
            foreach (var item in CategoryLevel)
            {
                if (item.CategoryTitle == title)
                    return false;
            }

            return true;
        }

        public GetProductDetailsViewModel GetProductByProductId(int productId)
        {
            List<AddNewFeaturesDto> features = _context.ProductFeatures.Where(f => f.ProductId == productId)
                .Select(f => new AddNewFeaturesDto
                {
                    FeatuerTitle = f.FeatuerTitle,
                    FeatureValue = f.FeatureValue
                }).ToList();

            var product = _context.Products.SingleOrDefault(p => p.ProductId == productId);
            GetProductDetailsViewModel getProduct = new GetProductDetailsViewModel()
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ShortDescription = product.ShortDescription,
                Description = product.Description,
                Brand = product.Brand,
                Price = product.Price,
                Inventory = product.Inventory,
                IsActive = product.IsActive,
                CategoryId = product.CategoryId,
                SellerId = product.SellerId
            };
            getProduct.Features = features;
            return getProduct;
        }

        public void UpdateCategory(CategoryInfoViewModel categoryInfo)
        {
            var category = _context.ProductCategories.SingleOrDefault(c => c.CategoryId == categoryInfo.CategoryIDLevel3);
            category.CategoryId = categoryInfo.CategoryIDLevel3;
            category.CategoryTitle = categoryInfo.CategoryTitleLevel3;
            category.CategoryUrl = categoryInfo.CategoryURL;
            category.ParentId = categoryInfo.CategoryIDLevel1;

            _context.ProductCategories.Update(category);
            _context.SaveChanges();
        }

        public ProductCategory GetCategoryById(int categoryId)
        {
            return _context.ProductCategories.SingleOrDefault(c => c.CategoryId == categoryId);
        }

        public Tuple<List<ShowProductListItemViewModel>, int> GetProducts(int pageId = 1,
            string orderByType = "date", int startPrice = 0, int endPrice = 0,
            int selectedCategory = 0, string filter = "", int take = 0)
        {
            if (take == 0)
                take = 8;

            IQueryable<Product> result = _context.Products;

            if (!string.IsNullOrEmpty(filter))
            {
                result = result.Where(c => c.ProductName.Contains(filter));
            }
            switch (orderByType)
            {
                case "date":
                    {
                        result = result.OrderByDescending(c => c.CreateDate);
                        break;
                    }
                case "expensive":
                    {
                        result = result.OrderByDescending(c => c.Price);
                        break;
                    }
                case "cheap":
                    {
                        result = result.OrderBy(c => c.Price);
                        break;
                    }
            }

            if (startPrice > 0)
            {
                result = result.Where(c => c.Price > startPrice);
            }

            if (endPrice > 0)
            {
                result = result.Where(c => c.Price < endPrice);
            }

            if (selectedCategory != 0)
            {
                List<ProductCategory> categoriesLevel2 = _context.ProductCategories
                    .Where(c => c.ParentId == selectedCategory).ToList();

                if (categoriesLevel2.Count != 0)
                {
                    var originalResults = result.Where(x => true);
                    result = result.Where(x => false);

                    foreach (var item in categoriesLevel2)
                    {
                        List<ProductCategory> categoriesLevel3 = _context.ProductCategories
                        .Where(c => c.ParentId == item.CategoryId).ToList();

                        if (categoriesLevel3.Count != 0)
                        {
                            foreach (var item2 in categoriesLevel3)
                            {
                                result = result.Union(originalResults.Where(c => c.CategoryId == item2.CategoryId));
                            }
                        }
                        else
                        {
                            result = result.Union(originalResults.Where(c => c.CategoryId == item.CategoryId));

                        }
                    }
                }
                else
                {
                    result = result.Where(c => c.CategoryId == selectedCategory);
                }

            }

            int skip = (pageId - 1) * take;

            var query = result.Select(c => new ShowProductListItemViewModel()
            {
                ProductId = c.ProductId,
                Price = c.Price,
                ProductTitle = c.ProductName,
                ImageName = c.ProductGallery.FirstOrDefault().ImageName
            }).Skip(skip).Take(take).ToList();

            int count = result.Select(c => new ShowProductListItemViewModel()
            {
                ProductId = c.ProductId,
                Price = c.Price,
                ProductTitle = c.ProductName,
                ImageName = c.ProductGallery.FirstOrDefault().ImageName
            }).Count();


            //PageCount
            int pageCount = count % take;
            int PageCount = count / take;
            if (pageCount > 1)
                PageCount++;

            return Tuple.Create(query, PageCount);
        }

        public Tuple<Product, CategoryInfoViewModel> GetProductForShow(int productId)
        {
            var product = _context.Products.Include(c => c.ProductCategory)
                .Include(c => c.ProductFeature).Include(c => c.ProductGallery)
                .Include(c => c.User)
                .FirstOrDefault(c => c.ProductId == productId);

            return Tuple.Create(product, GetCategoryLevelsById(product.CategoryId));
        }

        public void EditProduct(GetProductDetailsViewModel request)
        {
            var product = _context.Products.SingleOrDefault(p => p.ProductId == request.ProductId);

            var features = _context.ProductFeatures.Where(f => f.ProductId == request.ProductId).ToList();
            _context.ProductFeatures.RemoveRange(features);
            _context.SaveChanges();

            product.ProductName = request.ProductName;
            product.ShortDescription = request.ShortDescription;
            product.Description = request.Description;
            product.Brand = request.Brand;
            //product.IsActive = request.IsActive;
            product.Price = request.Price;
            product.Inventory = request.Inventory;
            product.CategoryId = request.CategoryId;
            product.SellerId = request.SellerId;

            _context.Products.Update(product);

            //List<ProductGallery> productImages = new List<ProductGallery>();
            //foreach (var item in request.Images)
            //{
            //    var uploadedResult = UploadFile(item);
            //    productImages.Add(new ProductGallery()
            //    {
            //        Product = product,
            //        ImageName = uploadedResult.FileNameAddress,
            //    });
            //}

            //_context.ProductGalleries.AddRange(productImages);



            List<ProductFeature> productFeatures = new List<ProductFeature>();
            foreach (var item in request.Features)
            {
                productFeatures.Add(new ProductFeature()
                {
                    FeatuerTitle = item.FeatuerTitle,
                    FeatureValue = item.FeatureValue,
                    Product = product,
                });
            }
            _context.ProductFeatures.AddRange(productFeatures);

            _context.SaveChanges();
        }

        public Tuple<List<CheckoutWithSellersViewModel>, int> GetSoldProducts(string username, int pageId = 1, string filter = "", int take = 0)
        {
            if (take == 0)
                take = 10;

            //int userId = _context.Users.SingleOrDefault(u => u.UserName == username).UserId;
            //int roleId = _context.UserRoles.SingleOrDefault(ur => ur.UserId == userId).RoleId;

            var list = (from o in _context.Orders
                        join od in _context.OrderDetails on o.OrderId equals od.OrderId
                        join p in _context.Products on od.ProductId equals p.ProductId
                        where (o.IsFinaly == true)
                        select new
                        {
                            p.SellerId,
                            od.Price,
                            od.Count
                        }).ToList();

            var list2 = from l in list
                        group l by l.SellerId into g
                        select new CalculateSoldProducts()
                        {
                            Sum = g.Sum(x => x.Price * x.Count),
                            ID = g.Key
                        };

            int skip = (pageId - 1) * take;

            var query = list2.Select(c => new CheckoutWithSellersViewModel()
            {
                SellerId = c.ID,
                SellerName = _context.Users.SingleOrDefault(u => u.UserId == c.ID).UserName,
                ProductsValue = c.Sum,
                SiteProfit = 20,
                PayToSeller = (c.Sum) - (((c.Sum) * 20) / 100)
            }).Skip(skip).Take(take).ToList();

            int count = list2.Select(c => new CheckoutWithSellersViewModel()
            {
                SellerId = c.ID,
                SellerName = _context.Users.SingleOrDefault(u => u.UserId == c.ID).UserName,
                ProductsValue = c.Sum,
                SiteProfit = 20,
                PayToSeller = (c.Sum) - (((c.Sum) * 20) / 100)
            }).Count();

            //PageCount
            int pageCount = count % take;
            int PageCount = count / take;
            if (pageCount > 1)
                PageCount++;

            return Tuple.Create(query, PageCount);
        }

        public Tuple<List<ProductsOfASellerViewModel>, int, string> GetProductsOfASeller(int sellerID, int pageId = 1, string filter = "", int take = 0)
        {
            if (take == 0)
                take = 10;

            var list = (from o in _context.Orders
                        join od in _context.OrderDetails on o.OrderId equals od.OrderId
                        join p in _context.Products on od.ProductId equals p.ProductId
                        where (o.IsFinaly == true && p.SellerId == sellerID)
                        select new
                        {
                            p.ProductId,
                            p.ProductName,
                            od.Price,
                            od.Count,
                            o.CreateDate,
                            o.OrderId
                        }).ToList();

            int skip = (pageId - 1) * take;
            var query = list.Select(c => new ProductsOfASellerViewModel()
            {
                ProductName = c.ProductName,
                Price = c.Price,
                Count = c.Count,
                ImageName = _context.ProductGalleries.FirstOrDefault(p => p.ProductId == c.ProductId).ImageName,
                CreateDate = c.CreateDate,
                OrderID = c.OrderId,
                Sum = c.Count * c.Price
            }).Skip(skip).Take(take).ToList();

            int count = list.Select(c => new ProductsOfASellerViewModel()
            {
                ProductName = c.ProductName,
                Price = c.Price,
                Count = c.Count,
                ImageName = _context.ProductGalleries.FirstOrDefault(p => p.ProductId == c.ProductId).ImageName,
                CreateDate = c.CreateDate,
                OrderID = c.OrderId,
                Sum = c.Count * c.Price
            }).Count();

            string userName = _context.Users.SingleOrDefault(u => u.UserId == sellerID).UserName;

            //PageCount
            int pageCount = count % take;
            int PageCount = count / take;
            if (pageCount > 1)
                PageCount++;

            return Tuple.Create(query, PageCount, userName);
        }

        public List<ShowProductListItemViewModel> GetProductsForHomePage(int selectedCategory, int take)
        {
            IQueryable<Product> result = _context.Products;
            if (selectedCategory != 0)
            {
                List<ProductCategory> categoriesLevel2 = _context.ProductCategories
                    .Where(c => c.ParentId == selectedCategory).ToList();

                if (categoriesLevel2.Count != 0)
                {
                    var originalResults = result.Where(x => true);
                    result = result.Where(x => false);

                    foreach (var item in categoriesLevel2)
                    {
                        List<ProductCategory> categoriesLevel3 = _context.ProductCategories
                        .Where(c => c.ParentId == item.CategoryId).ToList();

                        if (categoriesLevel3.Count != 0)
                        {
                            foreach (var item2 in categoriesLevel3)
                            {
                                result = result.Union(originalResults.Where(c => c.CategoryId == item2.CategoryId));
                            }
                        }
                        else
                        {
                            result = result.Union(originalResults.Where(c => c.CategoryId == item.CategoryId));

                        }
                    }
                }
                else
                {
                    result = result.Where(c => c.CategoryId == selectedCategory);
                }

            }

            return result.Select(c => new ShowProductListItemViewModel()
            {
                ProductId = c.ProductId,
                Price = c.Price,
                ProductTitle = c.ProductName,
                ImageName = c.ProductGallery.FirstOrDefault().ImageName
            }).Take(take).ToList();
        }
    }
}
