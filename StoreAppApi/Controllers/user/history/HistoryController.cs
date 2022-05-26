using AutoMapper;
using FastestDeliveryApi.database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAppApi.DTOs.user.history;
using StoreAppApi.models.product;
using StoreAppApi.models.user;
using StoreAppApi.models.user.history;
using StoreAppApi.models.user.history.enums;
using StoreAppApi.models.сompany;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StoreAppApi.Controllers.user.history
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly EfModel _efModel;
        private readonly IMapper _mapper;

        public HistoryController(
            EfModel efModel, IMapper mapper
            )
        {
            _efModel = efModel;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<HistoryDTO>> GetHistory(
            HistoryType? historyType
            )
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
                return NotFound();

            int idUser = Convert.ToInt32(identity.FindFirst("Id").Value);

            IQueryable<BaseUser> userQueryable = _efModel.BaseUsers
                .Include(u => u.Histories)
                    .ThenInclude(u => u.Product)
                .Include(u => u.Histories)
                    .ThenInclude(u => u.Event)
                .Include(u => u.Histories)
                    .ThenInclude(u => u.Сompany);

            if (historyType != null)
                userQueryable = userQueryable
                    .Where(u => u.Histories.Any(u => u.Type == historyType));

            BaseUser user = await userQueryable
                .FirstOrDefaultAsync(u => u.Id == idUser);

            if (user == null)
                return NotFound();

            return new HistoryDTO
            {
                Items = _mapper.Map<List<HistoryItemDTO>>(
                    user.Histories)
            };
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> PostHistory(HistoryPostDTO historyDTO)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
                return NotFound();

            int idUser = Convert.ToInt32(identity.FindFirst("Id").Value);

            BaseUser user = await _efModel.BaseUsers.FindAsync(idUser);

            if (user == null)
                return NotFound();

            Сompany сompany = await _efModel.Сompanies.FindAsync(historyDTO.СompanyId);

            Product product = await _efModel.Products.FindAsync(historyDTO.ProductId);

            var Event = await _efModel.Events.FindAsync(historyDTO.EventId);

            user.Histories.Add(new History
            {
                Type = historyDTO.Type,
                Date = DateTime.Now,
                Event = Event,
                Product = product,
                Сompany = сompany
            });
            await _efModel.SaveChangesAsync();

            return Ok();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteHistory(int id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
                return NotFound();

            int idUser = Convert.ToInt32(identity.FindFirst("Id").Value);

            BaseUser user = await _efModel.BaseUsers
                .Include(u => u.Histories)
                .FirstOrDefaultAsync(u => u.Id == idUser);

            if (user == null)
                return NotFound();

            if (!user.Histories.Any(u => u.Id == id))
                return NotFound();

            History history = await _efModel.Histories.FindAsync(id);

            if (history == null)
                return NotFound();

            _efModel.Histories.Remove(history);
            await _efModel.SaveChangesAsync();

            return Ok();
        }
    }
}
