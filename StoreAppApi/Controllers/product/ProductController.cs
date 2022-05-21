using AutoMapper;
using FastestDeliveryApi.database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAppApi.DTOs.product;
using StoreAppApi.DTOs.product.review;
using StoreAppApi.models.product;
using StoreAppApi.models.product.review;
using StoreAppApi.models.user;
using StoreAppApi.Repository.product.file;
using StoreAppApi.Repository.product.icon;
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

        public ProductController(
            IMapper mapper,EfModel model,
            IconProductRepositoryImpl iconProductRepositoryImpl,
            FileProductRepository fileProductRepository
            )
        {
            _fileProductRepository = fileProductRepository;
            _iconProductRepositoryImpl = iconProductRepositoryImpl;
            _mapper = mapper;
            _efModel = model;
        }

        [HttpGet("icon/{productId}.jpg")]
        public async Task<ActionResult> GetProductIcon(int productId)
        {
            Product product = await _efModel.Products.FindAsync(productId);

            if (product == null)
                return NotFound();

            byte[] file = _iconProductRepositoryImpl.GetProductIcon(productId, product.Title);
            if (file != null)
                return File(file, "image/jpeg");
            else
                return NotFound();
        }

        [Authorize(Roles = "CompanyUser")]
        [HttpPost]
        public async Task<ActionResult> PostProduct(ProductPostDTO productPostDTO, IFormFile fileIcon)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
                return NotFound();

            int idUser = Convert.ToInt32(identity.FindFirst("Id").Value);

            CompanyUser user = await _efModel.CompanyUsers.FindAsync(idUser);

            if (user == null)
                return NotFound();

            Product product = new Product
            {
                Company = user.Сompany,
                Title = productPostDTO.Title,
                Description = productPostDTO.Description,
                DatePublication = productPostDTO.DatePublication,
                Price = productPostDTO.Price,
                ProductType = productPostDTO.ProductType,
                Genre = await _efModel.Genres.FindAsync(productPostDTO.GenreId)
            };

            MemoryStream memoryStream = new MemoryStream();
            await fileIcon.CopyToAsync(memoryStream);
            _iconProductRepositoryImpl.DeleteProductIcon(product.Id, product.Title);
            _iconProductRepositoryImpl.PostProductIcon(memoryStream.ToArray(), product.Id, product.Title);

            _efModel.Products.Add(product);
            await _efModel.SaveChangesAsync();

            product.Icon = $"http://localhost:5000/api/product/icon/{product.Id}.jpg";
            await _efModel.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<ProductDTO>> GetProduct(
            string search, int? genreId
            )
        {
            IQueryable<Product> products = _efModel.Products
                .Include(u => u.Video)
                .Include(u => u.Images)
                .Include(u => u.Genre);


            if (search != null)
                products = products.Where(u => u.Title.Contains(search));

            if (genreId != null)
                products = products.Where(u => u.Genre.Id == genreId);

            return new ProductDTO
            {
                Items = _mapper.Map<List<ProductItemDTO>>(await products.ToListAsync())
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
                    .ThenInclude(u => u.User);


            if(search != null)
                productsIQueryable = productsIQueryable.Where(
                    u => u.Title.Contains(search) || u.Description.Contains(search)
                    );

            Product product = await productsIQueryable.FirstOrDefaultAsync(u => u.Id == id);

            if (product == null)
                return NotFound();

            return new ReviewDTO
            {
                Items = product.Reviews
            };
        }

        //[Authorize("CompanyUser")]
        [HttpPost("{id}/File")]
        public async Task<ActionResult> PostFile(int id,string extension, IFormFile formFile)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
                return NotFound();

            int idUser = Convert.ToInt32(identity.FindFirst("Id").Value);

            var user = await _efModel.CompanyUsers.FindAsync(idUser);

            if (user == null)
                return NotFound();

            Product product = await _efModel.Products
                .Include(u => u.Company)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (product == null)
                return NotFound();

          //  if (user.Сompany.Id != product.Company.Id)
           //     return NotFound();

            _fileProductRepository.UploadFile(
                formFile, extension, product.Company.Title, product.Company.Title, id
                );

            return Ok();
        }

        [Authorize]
        [HttpPost("{id}/Review")]
        public async Task<ActionResult> PostReview(int id, ReviewPostDTO reviewDTO)
        {
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
