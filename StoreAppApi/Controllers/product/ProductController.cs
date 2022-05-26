using AutoMapper;
using FastestDeliveryApi.database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAppApi.Controllers.company.common;
using StoreAppApi.Controllers.product.enums;
using StoreAppApi.DTOs.company;
using StoreAppApi.DTOs.company.product;
using StoreAppApi.DTOs.product;
using StoreAppApi.DTOs.product.review;
using StoreAppApi.models.product;
using StoreAppApi.models.product.enums;
using StoreAppApi.models.product.review;
using StoreAppApi.models.user;
using StoreAppApi.models.сompany.product.enums;
using StoreAppApi.Repository.product.file;
using StoreAppApi.Repository.product.icon;
using StoreAppApi.Repository.product.image;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StoreAppApi.Controllers.product
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMapper _mapper;
        public readonly IconProductRepositoryImpl _iconProductRepositoryImpl;
        private EfModel _efModel;
        private FileProductRepository _fileProductRepository;
        private ImageProductRepository _imageProductRepository;

        public ProductController(
            IMapper mapper, EfModel model,
            IconProductRepositoryImpl iconProductRepositoryImpl,
            FileProductRepository fileProductRepository,
            ImageProductRepository imageProductRepository
            )
        {
            _imageProductRepository = imageProductRepository;
            _fileProductRepository = fileProductRepository;
            _iconProductRepositoryImpl = iconProductRepositoryImpl;
            _mapper = mapper;
            _efModel = model;
        }

        [HttpGet("icon/{productId}.jpg")]
        public async Task<ActionResult> GetProductIcon(int productId)
        {
            Product product = await _efModel.Products
                .Include(u => u.Company)
                .FirstOrDefaultAsync(u => u.Id == productId);

            if (product == null)
                return NotFound();

            byte[] file = _iconProductRepositoryImpl.GetProductIcon(
                productId, product.Title, product.Company.Title, product.Company.Id);
            if (file != null)
                return File(file, "image/jpeg");
            else
                return NotFound();
        }

        [HttpGet("{id}/Сompany")]
        public async Task<ActionResult<CompanyItemDTO>> GetProductСompany(int id)
        {
            Product product = await _efModel.Products
                .Include(u => u.Company)
                    .ThenInclude(u => u.Products)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (product == null)
                return NotFound();

            return _mapper.Map<CompanyItemDTO>(product.Company);
        }

        [Authorize(Roles = "CompanyUser")]
        [HttpDelete("{id}/image/{idImage}.jpg")]
        public async Task<ActionResult> DeleteProductImage(int id, int idImage)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
                return NotFound();

            int idUser = Convert.ToInt32(identity.FindFirst("Id").Value);

            CompanyUser user = await _efModel.CompanyUsers
                .Include(u => u.Сompany)
                .FirstOrDefaultAsync(u => u.Id == idUser);

            if (user == null)
                return NotFound();

            Product product = await _efModel.Products
                .Include(u => u.Company)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (product == null)
                return NotFound();

            if (product.Company.Id != user.Сompany.Id)
                return NotFound();

            _imageProductRepository.DeleteProductImage(
                product.Id, product.Title, product.Company.Title, idImage, product.Company.Id
                );

            return Ok();
        }

        [HttpGet("{id}/image/{idImage}.jpg")]
        public async Task<ActionResult> GetProductImage(int id, int idImage)
        {
            Product product = await _efModel.Products
                .Include(u => u.Company)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (product == null)
                return NotFound();

            byte[] file = _imageProductRepository.GetProductImage(
                product.Id, product.Title, product.Company.Title, idImage, product.Company.Id
                );
            if (file != null)
                return File(file, "image/jpeg");
            else
                return NotFound();
        }

        [Authorize(Roles = "CompanyUser")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
                return NotFound();

            int idUser = Convert.ToInt32(identity.FindFirst("Id").Value);

            CompanyUser user = await _efModel.CompanyUsers.FindAsync(idUser);

            if (user == null)
                return NotFound();

            Product product = await _efModel.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            if (product.Company.Id != user.Сompany.Id)
                return NotFound();

            _efModel.Products.Remove(product);
            await _efModel.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductItemDTO>> GetProductById(int id)
        {
            Product product = await _efModel.Products
                .Include(u => u.Video)
                .Include(u => u.Images)
                .Include(u => u.Genre)
                .Include(u => u.Reviews)
                .Include(u => u.SocialNetwork)
                .FirstOrDefaultAsync(u => u.Id == id);

            return _mapper.Map<ProductItemDTO>(product);
        }

        [Authorize(Roles = "CompanyUser")]
        [HttpPost("{id}/image")]
        public async Task<ActionResult> PostProductImage(int id, IFormFile fileImage)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
                return NotFound();

            int idUser = Convert.ToInt32(identity.FindFirst("Id").Value);

            CompanyUser user = await _efModel.CompanyUsers
                .Include(u => u.Сompany)
                .FirstOrDefaultAsync(u => u.Id == idUser);

            if (user == null)
                return NotFound();

            Product product = await _efModel.Products
                .Include(u => u.Company)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (product == null)
                return NotFound();

            if (product.Company.Id != user.Сompany.Id)
                return NotFound();

            Image imageNew = new Image
            {
                ImageUrl = "--"
            };

            product.Images.Add(imageNew);
            await _efModel.SaveChangesAsync();

            MemoryStream memoryStream = new MemoryStream();
            await fileImage.CopyToAsync(memoryStream);

            _imageProductRepository.PostProductImage(
                memoryStream.ToArray(), product.Id,
                product.Title, product.Company.Title, imageNew.Id, product.Company.Id
                );


            Image image = await _efModel.Images.FindAsync(imageNew.Id);
            image.ImageUrl = $"{Constants.BASE_URL}/Product/{product.Id}/image/{image.Id}.jpg";
            await _efModel.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "CompanyUser")]
        [HttpPost("{id}/icon")]
        public async Task<ActionResult> PostProductIcon(int id, IFormFile fileIcon)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
                return NotFound();

            int idUser = Convert.ToInt32(identity.FindFirst("Id").Value);

            CompanyUser user = await _efModel.CompanyUsers
                .Include(u => u.Сompany)
                .FirstOrDefaultAsync(u => u.Id == idUser);

            if (user == null)
                return NotFound();

            Product product = await _efModel.Products
                .Include(u => u.Company)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (product == null)
                return NotFound();

            if (product.Company.Id != user.Сompany.Id)
                return NotFound();

            MemoryStream memoryStream = new MemoryStream();
            await fileIcon.CopyToAsync(memoryStream);
            _iconProductRepositoryImpl.DeleteProductIcon(
                product.Id, product.Title, product.Company.Title, product.Company.Id);
            _iconProductRepositoryImpl.PostProductIcon(
                memoryStream.ToArray(), product.Id, product.Title,
                product.Company.Title, product.Company.Id);

            product.Icon = $"{Constants.BASE_URL}/Product/icon/{product.Id}.jpg";
            await _efModel.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "AdminUser")]
        [HttpPatch("{id}/Status")]
        public async Task<ActionResult> PatchProductStaus(int id, ProductStatus productStatus)
        {
            Product product = await _efModel.Products
                .Include(u => u.Company)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (product == null)
                return NotFound();

            product.ProductStatus = productStatus;
            _efModel.Entry(product).State = EntityState.Modified;
            await _efModel.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "CompanyUser")]
        [HttpPost]
        public async Task<ActionResult> PostProduct(ProductPostDTO productPostDTO)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
                return NotFound();

            int idUser = Convert.ToInt32(identity.FindFirst("Id").Value);

            CompanyUser user = await _efModel.CompanyUsers
                .Include(u => u.Сompany)
                .FirstOrDefaultAsync(u => u.Id == idUser);

            if (user == null)
                return NotFound();

            Product product = new Product
            {
                Company = user.Сompany,
                Title = productPostDTO.Title,
                ShortDescription = productPostDTO.ShortDescription,
                FullDescription = productPostDTO.FullDescription,
                Advertising = productPostDTO.Advertising,
                Version = productPostDTO.Version,
                Email = productPostDTO.Email,
                Phone = productPostDTO.Phone,
                Website = productPostDTO.Website,
                SocialNetwork = productPostDTO.SocialNetwork,
                AgeRating = productPostDTO.AgeRating,
                PrivacyPolicyWebUrl = productPostDTO.PrivacyPolicyWebUrl,
                DatePublication = productPostDTO.DatePublication,
                Price = productPostDTO.Price,
                ProductType = productPostDTO.ProductType,
                Genre = await _efModel.Genres.FindAsync(productPostDTO.GenreId),
                ProductStatus = ProductStatus.EXAMINATION,
                Country = productPostDTO.Country
            };

            _efModel.Products.Add(product);
            await _efModel.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<ProductDTO>> GetProduct(int pageSize, int pageNumber,
            string search, [FromQuery] List<int> genreId, [FromQuery] List<int> countryId,
             AgeRating? ageRating, Boolean? advertising,
             Boolean? free, DateTime? startDatePublication, DateTime? endDatePublication,
             float? startRating, float? endRating, ProductType? productType,
             ProductStatus? productStatus, ProductOrderBy? orderBy
            )
        {
            IQueryable<Product> products = _efModel.Products
                .Include(u => u.Video)
                .Include(u => u.Images)
                .Include(u => u.Genre)
                .Include(u => u.Reviews)
                .Include(u => u.SocialNetwork);

            if (search != null)
                products = products.Where(
                    u => u.Title.Contains(search)
                    || u.ShortDescription.Contains(search)
                    || u.FullDescription.Contains(search)
                    );

            if (genreId != null && genreId.Count > 0)
                products = products.Where(u => genreId.Contains(u.Genre.Id));

            if (countryId != null && countryId.Count > 0)
                products = products.Where(u => countryId.Contains(u.Country.Id));


            if (ageRating != null)
                products = products.Where(
                    u => u.AgeRating == ageRating);

            if (advertising != null)
                products = products.Where(
                    u => u.Advertising == advertising);

            if (free != null)
                if (free == true)
                    products = products.Where(
                        u => u.Price == null);
                else
                    products = products.Where(
                    u => u.Price != null);

            if (startDatePublication != null)
                products = products.Where(
                    u => u.DatePublication >= startDatePublication);

            if (endDatePublication != null)
                products = products.Where(
                    u => u.DatePublication <= endDatePublication);

            if (startRating != null)
                products = products.Where(
                    u => u.Rating >= startRating);

            if (endRating != null)
                products = products.Where(
                    u => u.Rating <= endRating);

            if (productType != null)
                products = products.Where(
                    u => u.ProductType == productType);

            if (productStatus != null)
                products = products.Where(
                    u => u.ProductStatus == productStatus);

            if(orderBy != null)
            {
                switch (orderBy)
                {
                    case ProductOrderBy.TITLE:
                        products = products.OrderBy(u => u.Title);
                        break;
                    case ProductOrderBy.RATING_MIN_MAX:
                        products = products.OrderBy(u => u.Rating);
                        break;
                    case ProductOrderBy.RATING_MAX_MIN:
                        products = products.OrderByDescending(u => u.Rating);
                        break;
                    case ProductOrderBy.AMOUNT_RATING_MIN_MAX:
                        products = products.OrderBy(u => u.ReviewsTotal);
                        break;
                    case ProductOrderBy.AMOUNT_RATING_MAX_MIN:
                        products = products.OrderByDescending(u => u.ReviewsTotal);
                        break;
                    case ProductOrderBy.AGE_RATING_MIN_MAX:
                        products = products.OrderBy(u => u.AgeRating);
                        break;
                    case ProductOrderBy.AGE_RATING_MAX_MIN:
                        products = products.OrderByDescending(u => u.AgeRating);
                        break;
                    case ProductOrderBy.AMOUNT_USER_DOWNLOAD_MIN_MAX:
                        products = products.OrderBy(u => u.UserDownloadTotal);
                        break;
                    case ProductOrderBy.AMOUNT_USER_DOWLOAD_MAX_MIN:
                        products = products.OrderByDescending(u => u.UserDownloadTotal);
                        break;
                    case ProductOrderBy.DATE:
                        products = products.OrderBy(u => u.DatePublication);
                        break;
                }
            }

            products = products
                .Skip(pageNumber).Take(pageSize);

            return new ProductDTO
            {
                Items = _mapper.Map<List<ProductItemDTO>>(await products.ToListAsync())
            };
        }

        [HttpGet("Country")]
        public async Task<ActionResult<CountryDTO>> GetCountry()
        {
            return new CountryDTO
            {
                Items = await _efModel.Country.ToListAsync()
            };
        }

        [HttpGet("Genre")]
        public async Task<ActionResult<GenreDTO>> GetGenre()
        {
            return new GenreDTO
            {
                Items = await _efModel.Genres.ToListAsync()
            };
        }

        [HttpGet("{id}/Review")]
        public async Task<ActionResult<ReviewDTO>> GetReview(
             int id, string search
            )
        {
            IQueryable<Product> productsIQueryable = _efModel.Products
                .Include(u => u.Reviews)
                    .ThenInclude(u => u.Product)
                        .ThenInclude(u => u.Images)
                .Include(u => u.Reviews)
                    .ThenInclude(u => u.Product)
                        .ThenInclude(u => u.Video)
                .Include(u => u.Reviews)
                    .ThenInclude(u => u.Product)
                        .ThenInclude(u => u.Genre)
                .Include(u => u.Reviews)
                    .ThenInclude(u => u.User)
                .Include(u => u.SocialNetwork);


            if (search != null)
                productsIQueryable = productsIQueryable.Where(
                    u => u.Reviews.Any(u => u.Title.Contains(search)));

            Product product = await productsIQueryable.FirstOrDefaultAsync(u => u.Id == id);

            if (product == null)
                return NotFound();

            return new ReviewDTO
            {
                Items = product.Reviews
            };
        }

        [HttpGet("{id}/file")]
        public async Task<ActionResult> GetFile(int id)
        {
            Product product = await _efModel.Products
                .Include(u => u.Company)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (product == null)
                return NotFound();

            string extension = product.FileExtension.ToString().ToLower();

            byte[] file = _fileProductRepository.GetFile(
                $".{extension}", product.Company.Title, product.Title, product.Id,
                product.Company.Id
                );

            if (file != null)
                return File(file, "multipart/form-data");
            else
                return NotFound();
        }

        [Authorize(Roles = "CompanyUser")]
        [HttpPost("{id}/File")]
        public async Task<ActionResult> PostFile(
            int id,IFormFile file)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
                return NotFound();

            int idUser = Convert.ToInt32(identity.FindFirst("Id").Value);

            CompanyUser user = await _efModel.CompanyUsers
                .Include(u => u.Сompany)
                .FirstOrDefaultAsync(u => u.Id == idUser);

            if (user == null)
                return NotFound();

            Product product = await _efModel.Products
                .Include(u => u.Company)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (product == null)
                return NotFound();

            if (product.ProductStatus == ProductStatus.BLOCKED)
                return NotFound("Product status blocked");

            if (user.Сompany.Id != product.Company.Id)
                return NotFound();

            _fileProductRepository.UploadFile(
                file, product.Company.Title, product.Title,
                id, product.Company.Id
                );

            string extension = Path.GetExtension(file.FileName);

            product.FileUrl = $"{Constants.BASE_URL}/product/{product.Id}/file.{extension}";
            switch (extension)
            {
                case ".apk" :
                    product.FileExtension = ProductExtension.APK;
                    break;

                case ".aab":
                    product.FileExtension = ProductExtension.AAB;
                    break;
                default:
                    return BadRequest();
            }
            await _efModel.SaveChangesAsync();

            return Ok();
        }

        [Authorize]
        [HttpPost("{id}/Review")]
        public async Task<ActionResult> PostReview(int id, ReviewPostDTO reviewDTO)
        {
            if (reviewDTO.Rating > 5)
                return BadRequest();

            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
                return NotFound();

            int idUser = Convert.ToInt32(identity.FindFirst("Id").Value);

            var user = await _efModel.BaseUsers.FindAsync(idUser);

            var product = await _efModel.Products.FindAsync(id);

            if (user == null || product == null)
                return NotFound();

            Review review = new Review
            {
                Title = reviewDTO.Title,
                Description = reviewDTO.Description,
                Rating = reviewDTO.Rating,
                DatePublication = reviewDTO.DatePublication,
                Product = product,
                User = user
            };

            user.Reviews.Add(review);
            product.Reviews.Add(review);
            await _efModel.SaveChangesAsync();

            return Ok();
        }
    }
}
