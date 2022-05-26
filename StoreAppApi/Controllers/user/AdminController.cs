using AutoMapper;
using FastestDeliveryApi.database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreAppApi.models.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreAppApi.Controllers.user
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly EfModel _efModel;
        private readonly IMapper _mapper;

        public AdminController(
            EfModel efModel, IMapper mapper
            )
        {
            _efModel = efModel;
            _mapper = mapper;
        }

        [Authorize(Roles = "AdminUser")]
        [HttpPost]
        public async Task<ActionResult> PostAdmin(int idBaseUser)
        {
            BaseUser baseUser = await _efModel.BaseUsers.FindAsync(idBaseUser);

            if (baseUser == null)
                return NotFound();

            _efModel.BaseUsers.Remove(baseUser);
            _efModel.AdminUsers.Add(_mapper.Map<AdminUser>(baseUser));
            await _efModel.SaveChangesAsync();

            return Ok();
        }
    }
}
