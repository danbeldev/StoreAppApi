using AutoMapper;
using FastestDeliveryApi.database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAppApi.Controllers.company.common;
using StoreAppApi.Controllers.company.enums;
using StoreAppApi.DTOs.company;
using StoreAppApi.DTOs.company.Event;
using StoreAppApi.DTOs.product;
using StoreAppApi.models.user;
using StoreAppApi.models.сompany;
using StoreAppApi.Repository.company.banner;
using StoreAppApi.Repository.company.logo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private readonly LogoCompanyRepository _logoCompanyRepository;
        private readonly BannerRepository _bannerRepository;

        public CompanyController(
            EfModel efModel, IMapper mapper,
            LogoCompanyRepository logoCompanyRepository,
            BannerRepository bannerRepository)
        {
            _bannerRepository = bannerRepository;
            _logoCompanyRepository = logoCompanyRepository;
            _efModel = efModel;
            _mapper = mapper;
        }

        [Authorize(Roles = "CompanyUser")]
        [HttpPut]
        public async Task<ActionResult> PutCompany(CompanyPostDTO companyDTO)
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

            Сompany сompany = await _efModel.Сompanies.FindAsync(user.Сompany.Id);

            if (сompany == null)
                return NotFound();

            if (сompany.Id != user.Сompany.Id)
                return NotFound();

            сompany.Title = companyDTO.Title;
            сompany.Description = companyDTO.Description;

            _efModel.Entry(сompany).State = EntityState.Modified;

            try
            {
                await _efModel.SaveChangesAsync();
            } catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyItemDTO>> GetCompanyById(int id)
        {
            Сompany сompany = await _efModel.Сompanies
                .Include(u => u.Products)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (сompany == null)
                return NotFound();

            return _mapper.Map<CompanyItemDTO>(сompany);
        }

        [HttpOptions("{id}/Banner/Size")]
        public async Task<ActionResult<string>> OptionsCompanyBanner(int id)
        {
            Сompany сompany = await _efModel.Сompanies.FindAsync(id);

            if (сompany == null)
                return NotFound();

            string size =  _bannerRepository.GetCompanyBannerSize(
                сompany.Title, id
                );

            if (size == null)
                return NotFound();
            else
                return size;
        }

        [HttpGet("{id}/banner.jpg")]
        public async Task<ActionResult> GetCompanyBanner(int id)
        {
            Сompany сompany = await _efModel.Сompanies.FindAsync(id);

            if (сompany == null)
                return NotFound();

            byte[] file = _bannerRepository.GetCompanyBanner(
                сompany.Title, сompany.Id
                );

            if (file != null)
                return File(file, "image/jpeg");
            else
                return NotFound();
        }


        [Authorize(Roles = "CompanyUser")]
        [HttpPost("Banner")]
        public async Task<ActionResult> PostCompany(IFormFile banner)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
                return NotFound();

            int idUser = Convert.ToInt32(identity.FindFirst("Id").Value);

            CompanyUser companyUser = await _efModel.CompanyUsers
                .Include(u => u.Сompany)
                .FirstOrDefaultAsync(u => u.Id == idUser);

            if (companyUser == null)
                return NotFound();

            MemoryStream memoryStream = new MemoryStream();
            await banner.CopyToAsync(memoryStream);
            _bannerRepository.DeleteCompanyBanner(
                companyUser.Сompany.Title, companyUser.Сompany.Id
                );

            _bannerRepository.PostCompanyBanner(
                memoryStream.ToArray(),
                companyUser.Сompany.Title, companyUser.Сompany.Id
                );

            companyUser.Сompany.Banner = $"" +
                $"{Constants.BASE_URL}/Company/{companyUser.Сompany.Id}/banner.jpg";
            await _efModel.SaveChangesAsync();

            return Ok();
        }

        [HttpOptions("{id}/Logo/Size")]
        public async Task<ActionResult<string>> OptionsCompanyLogo(int id)
        {
            Сompany сompany = await _efModel.Сompanies.FindAsync(id);

            if (сompany == null)
                return NotFound();

            string size = _logoCompanyRepository.GetCompanyLogoSize(
                сompany.Title, сompany.Id
                );

            if (size == null)
                return NotFound();
            else
                return size;

        }

        [HttpGet("{id}/logo.jpg")]
        public async Task<ActionResult> GetCompanyLogo(int id)
        {
            Сompany сompany = await _efModel.Сompanies.FindAsync(id);

            if (сompany == null)
                return NotFound();

            byte[] file = _logoCompanyRepository.GetCompanyLogo(
                сompany.Title, сompany.Id
                );

            if (file != null)
                return File(file, "image/jpeg");
            else
                return NotFound();
        }

        [Authorize(Roles = "CompanyUser")]
        [HttpPost("Logo")]
        public async Task<ActionResult> PostCompanyLogo(IFormFile logo)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
                return NotFound();

            int idUser = Convert.ToInt32(identity.FindFirst("Id").Value);

            CompanyUser companyUser = await _efModel.CompanyUsers
                .Include(u => u.Сompany)
                .FirstOrDefaultAsync(u => u.Id == idUser);

            if (companyUser == null)
                return NotFound();

            MemoryStream memoryStream = new MemoryStream();
            await logo.CopyToAsync(memoryStream);
            _logoCompanyRepository.DeleteCompanyLogo(
                companyUser.Сompany.Title, companyUser.Сompany.Id
                );
            _logoCompanyRepository.PostCompanyLogo(
                memoryStream.ToArray(),
                companyUser.Сompany.Title, companyUser.Сompany.Id
                );

            companyUser.Сompany.Logo = $"" +
                $"{Constants.BASE_URL}/Company/{companyUser.Сompany.Id}/logo.jpg";
            await _efModel.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<CompanyDTO>> GetCompany(
            int pageSize, int pageNumber, string search,
            DateTime? startDateCreating, DateTime? endDateCreating,
            CompanyOrderBy? companyOrderBy
            )
        {

            IQueryable<Сompany> сompany = _efModel.Сompanies
                .Skip(pageNumber).Take(pageSize);

            if (search != null)
                сompany = сompany.Where(u => u.Title.Contains(search));

            if (startDateCreating != null)
                сompany = сompany.Where(u => u.DateCreating >= startDateCreating);

            if (endDateCreating != null)
                сompany = сompany.Where(u => u.DateCreating <= endDateCreating);

            if (companyOrderBy != null)
            {
                switch (companyOrderBy)
                {
                    case CompanyOrderBy.TITLE:
                        сompany = сompany.OrderBy(u => u.Title);
                        break;
                    case CompanyOrderBy.DATE:
                        сompany = сompany.OrderBy(u => u.DateCreating);
                        break;
                }
            }

            return new CompanyDTO
            {
                Items = _mapper.Map<List<CompanyItemDTO>>(await сompany.ToListAsync())
            };
        }

        [HttpGet("{id}/Events")]
        public async Task<ActionResult<EventDTO>> GetEvent(int id)
        {
            Сompany сompany = await _efModel.Сompanies
                .Include(u => u.Events)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (сompany == null)
                return NotFound();

            return new EventDTO
            {
                Items = _mapper.Map<List<EventItemDTO>>(сompany.Events)
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

            CompanyUser companyUser =  new CompanyUser
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
                ProductsDownload = user.ProductsDownload,
            };

            _efModel.BaseUsers.Remove(user);

            _efModel.CompanyUsers.Add(companyUser);        
            await _efModel.SaveChangesAsync();

            return Ok();
        }
    }
}
