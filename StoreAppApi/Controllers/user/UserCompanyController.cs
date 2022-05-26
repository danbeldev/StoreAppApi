using AutoMapper;
using FastestDeliveryApi.database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAppApi.DTOs.company;
using StoreAppApi.DTOs.company.Event;
using StoreAppApi.DTOs.product;
using StoreAppApi.models.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StoreAppApi.Controllers.user
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCompanyController : ControllerBase
    {
        private readonly EfModel _efModel;
        private readonly IMapper _mapper;

        public UserCompanyController(
            EfModel efModel, IMapper mapper
            )
        {
            _efModel = efModel;
            _mapper = mapper;
        }

        [Authorize(Roles = "UserCompany")]
        [HttpGet("/Event")]
        public async Task<ActionResult<EventDTO>> GetEvent()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
                return NotFound();

            int idUser = Convert.ToInt32(identity.FindFirst("Id").Value);

            CompanyUser companyUser = await _efModel.CompanyUsers
                .Include(u => u.Сompany)
                    .ThenInclude(u => u.Events)
                .FirstOrDefaultAsync(u => u.Id == idUser);

            if (companyUser == null)
                return NotFound();

            return new EventDTO
            {
                Items = _mapper.Map<List<EventItemDTO>>(companyUser.Сompany.Events)
            };
        }

        [Authorize(Roles = "UserCompany")]
        [HttpGet("/Company")]
        public async Task<ActionResult<CompanyItemDTO>> GetCompany()
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

            return _mapper.Map<CompanyItemDTO>(companyUser.Сompany);
        }

        [Authorize(Roles = "UserCompany")]
        [HttpGet("/Product")]
        public async Task<ActionResult<ProductDTO>> GetProductCompany()
        {

            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
                return NotFound();

            int idUser = Convert.ToInt32(identity.FindFirst("Id").Value);

            CompanyUser companyUser = await _efModel.CompanyUsers
                .Include(u => u.Сompany)
                    .ThenInclude(u => u.Products)
                .FirstOrDefaultAsync(u => u.Id == idUser);

            if (companyUser == null)
                return NotFound();

            return new ProductDTO
            {
                Items = _mapper.Map<List<ProductItemDTO>>(companyUser.Сompany.Products)
            };
        }
    }
}
