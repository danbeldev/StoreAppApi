using AutoMapper;
using FastestDeliveryApi.database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAppApi.DTOs.company;
using StoreAppApi.DTOs.product;
using StoreAppApi.models.product;
using StoreAppApi.models.user;
using StoreAppApi.models.сompany;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StoreAppApi.Controllers.company
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private EfModel _efModel;
        private readonly IMapper _mapper;

        public CompanyController(EfModel efModel, IMapper mapper)
        {
            _efModel = efModel;
            _mapper = mapper;
         }

        [HttpGet]
        public async Task<ActionResult<CompanyDTO>> GetCompany()
        {
            List<Сompany> сompany = await _efModel.Сompanies.ToListAsync();

            return new CompanyDTO
            {
                Items = _mapper.Map<List<CompanyItemDTO>>(сompany)
            };
        }

        [HttpGet("{id}/Products")]
        public async Task<ActionResult<ProductDTO>> GetCompanyProduct(int id)
        {
            Сompany company = await _efModel.Сompanies
                .Include(u => u.Products)
                    .ThenInclude(u => u.Video)
                .Include(u => u.Products)
                    .ThenInclude(u => u.Images)
                .Include(u => u.Products)
                    .ThenInclude(u => u.Genre)
                .Include(u => u.Products)
                    .ThenInclude(u => u.SocialNetwork)    
                .FirstOrDefaultAsync(u => u.Id == id);

            if (company == null)
                return NotFound();

            return new ProductDTO
            {
                Items = _mapper.Map<List<ProductItemDTO>>(company.Products)
            };
        }

        [Authorize(Roles = "BaseUser")]
        [HttpPost]
        public async Task<ActionResult> PostCompany(CompanyPostDTO companyDTO)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
                return NotFound();

            int idUser = Convert.ToInt32(identity.FindFirst("Id").Value);

            var user = await _efModel.BaseUsers.FindAsync(idUser);

            if (user == null)
                return NotFound();

            CompanyUser companyUser = new CompanyUser
            {
                Сompany = new Сompany
                {
                    DateCreating = DateTime.Now,
                    Description = companyDTO.Description,
                    Title = companyDTO.Title
                },
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Password = user.Password,
                Photo = user.Photo,
                Reviews = user.Reviews,
                ProductsDownload = user.ProductsDownload
            };

            _efModel.CompanyUsers.Add(companyUser);        
            await _efModel.SaveChangesAsync();

            return Ok();
        }
    }
}
