using AutoMapper;
using FastestDeliveryApi.database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreAppApi.DTOs.company.Event;
using System.Threading.Tasks;
using StoreAppApi.models.сompany.Event;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using StoreAppApi.models.сompany.Event.enums;
using Microsoft.AspNetCore.Authorization;
using StoreAppApi.models.product;
using StoreAppApi.models.user;
using System.Security.Claims;
using System;
using StoreAppApi.DTOs.company;
using StoreAppApi.DTOs.product;
using StoreAppApi.Repository.company.Event.promo;
using System.IO;
using StoreAppApi.Controllers.company.common;
using System.Linq;
using StoreAppApi.Repository.company.Event.promoVideo;

namespace StoreAppApi.Controllers.Event
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private EfModel _efModel;
        private readonly IMapper _mapper;
        private readonly PromoImageEventRepository _promoImageEventRepository;
        private readonly PromoVideoEventRepository _promoVideoEventRepository;
        public EventController(
            EfModel efModel, IMapper mapper,
            PromoImageEventRepository promoImageEventRepository,
            PromoVideoEventRepository promoVideoEventRepository
            )
        {
            _promoVideoEventRepository = promoVideoEventRepository;
            _promoImageEventRepository = promoImageEventRepository;
            _mapper = mapper;
            _efModel = efModel;
        }

        [HttpGet]
        public async Task<ActionResult<EventDTO>> GetEvent(
            int pageNumber, int pageSize, string search, EventStatus? eventStatus,
            DateTime? startDate, DateTime? endDate
            )
        {
            IQueryable<models.сompany.Event.Event> events = _efModel.Events
                .Skip(pageNumber).Take(pageSize);

            if (search != null)
                events = events.Where(u => u.Title.Contains(search));

            if (eventStatus != null)
                events = events.Where(u => u.EventStatus == eventStatus);

            if (startDate != null)
                events = events.Where(u => u.DatePublication >= startDate);

            if (endDate != null)
                events = events.Where(u => u.DatePublication <= endDate);

            return new EventDTO
            {
                Items = _mapper.Map<List<EventItemDTO>>(await events.ToListAsync())
            };
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventItemDTO>> GetEventById(int id)
        {
            var Event = await _efModel.Events.FindAsync(id);

            if (Event == null)
                return NotFound();

            return _mapper.Map<EventItemDTO>(Event);
        }

        [HttpGet("{id}/Product")]
        public async Task<ActionResult<ProductItemDTO>> GetProduct(int id)
        {
            var Event = await _efModel.Events
                .Include(u => u.Product)
                .FirstOrDefaultAsync(u => u.Id == id);

            if(Event == null)
                return NotFound();

            return _mapper.Map<ProductItemDTO>(Event.Product);
        }

        [HttpGet("{id}/Company")]
        public async Task<ActionResult<CompanyItemDTO>> GetCompany(int id)
        {
            var Event = await _efModel.Events
                .Include(u => u.Company)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (Event == null)
                return NotFound();

            return _mapper.Map<CompanyItemDTO>(Event.Company);
        }

        [Authorize(Roles = "CompanyUser")]
        [HttpPost("{id}/Promo/Image")]
        public async Task<ActionResult> PostPromoImage(int id, IFormFile promoImage)
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

            var Event = await _efModel.Events
                .Include(u => u.Company)
                .Include(u => u.Product)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (Event == null)
                return NotFound();

            if (user.Сompany.Id != Event.Company.Id)
                return NotFound();

            MemoryStream memoryStream = new MemoryStream();
            await promoImage.CopyToAsync(memoryStream);

            _promoImageEventRepository.PostCompanyPromo(
                memoryStream.ToArray(), Event.Company.Title, Event.Company.Id,
                Event.Product.Id, Event.Product.Title, Event.Id, Event.Title
                );

            Event.PromoImageUrl = $"{Constants.BASE_URL}/Event/{Event.Id}/promo.jpg";
            await _efModel.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{id}/promo.jpg")]
        public async Task<ActionResult> GetPromoImage(int id)
        {
            var Eveent = await _efModel.Events
                .Include(u => u.Product)
                .Include(u => u.Company)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (Eveent == null)
                return NotFound();

            byte[] promoFile = _promoImageEventRepository.GetCompanyPromo(
                Eveent.Company.Title, Eveent.Company.Id, Eveent.Product.Id, Eveent.Product.Title,
                Eveent.Id, Eveent.Title
                );

            if (promoFile != null)
                return File(promoFile, "image/jpeg");
            else
                return NotFound();
        }

        [HttpOptions("{id}/Promo/Video/Size")]
        public async Task<ActionResult<string>> OptionsPromoVideo(int id)
        {
            var Eveent = await _efModel.Events
                .Include(u => u.Product)
                .Include(u => u.Company)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (Eveent == null)
                return NotFound();

            string size = _promoVideoEventRepository.GetFileVideoSize(
                Eveent.Company.Title, Eveent.Company.Id, Eveent.Product.Id,
                Eveent.Product.Title, Eveent.Id, Eveent.Title
                );

            if (size == null)
                return NotFound();
            else
                return size;
        }

        [HttpGet("{id}/promo.mp4")]
        public async Task<ActionResult> GetEventVideo(int id)
        {
            var Eveent = await _efModel.Events
                .Include(u => u.Product)
                .Include(u => u.Company)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (Eveent == null)
                return NotFound();

            byte[] fileVideo = _promoVideoEventRepository.GetFileVideo(
                Eveent.Company.Title, Eveent.Company.Id, Eveent.Product.Id,
                Eveent.Product.Title, Eveent.Id, Eveent.Title
                );

            if (fileVideo != null)
                return File(fileVideo, "video/mp4");
            else
                return NotFound();
        }

        [Authorize]
        [HttpPost("{id}/Promo/Video")]
        public async Task<ActionResult> PostVideo(int id, IFormFile promoVideo)
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

            var Event = await _efModel.Events
                .Include(u => u.Company)
                .Include(u => u.Product)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (Event == null)
                return NotFound();

            if (user.Сompany.Id != Event.Company.Id)
                return NotFound();

            _promoVideoEventRepository.UploadFileVideo(
                promoVideo, Event.Company.Title, Event.Company.Id,
                Event.Product.Id, Event.Product.Title,
                Event.Id, Event.Title
                );

            Event.PromoVideoUrl = $"{Constants.BASE_URL}/Event/{id}/promo.mp4";
            await _efModel.SaveChangesAsync();
           
            return Ok();
        }

        [HttpOptions("{id}/Promo/Image/Size")]
        public async Task<ActionResult<String>> OptionsPromoImage(int id)
        {
            var Eveent = await _efModel.Events
                .Include(u => u.Product)
                .Include(u => u.Company)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (Eveent == null)
                return NotFound();

            string size = _promoImageEventRepository.GetCompanyPromoSize(
                Eveent.Company.Title, Eveent.Company.Id, 
                Eveent.Product.Id, Eveent.Product.Title,
                Eveent.Id, Eveent.Title
                );

            if (size == null)
                return NotFound();
            else
                return size;
        }

        [Authorize(Roles = "CompanyUser")]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutEvent(int id, EventPostDTO eventDTO)
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

            var Event = await _efModel.Events.FindAsync(id);

            Product product = await _efModel.Products.FindAsync();

            if (Event == null || product == null)
                return NotFound();

            if (Event.Company.Id != user.Сompany.Id)
                return NotFound();

            Event.Title = eventDTO.Title;
            Event.ShortDescription = eventDTO.ShortDescription;
            Event.FullDescription = eventDTO.FullDescription;
            Event.WebUrl = eventDTO.WebUrl;
            Event.Product = product;

            _efModel.Entry(Event).State = EntityState.Modified;

            try
            {
                await _efModel.SaveChangesAsync();
            } catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            return Ok();
        }

        [Authorize(Roles = "CompanyUser")]
        [HttpPost]
        public async Task<ActionResult<EventItemDTO>> PostEvent(EventPostDTO eventDTO)
        {
            Product product = await _efModel.Products
                .Include(u => u.Company)
                .FirstOrDefaultAsync(u => u.Id == eventDTO.ProductId);

            if (product == null)
                return NotFound();

            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
                return NotFound();

            int idUser = Convert.ToInt32(identity.FindFirst("Id").Value);

            CompanyUser user = await _efModel.CompanyUsers
                .Include(u => u.Сompany)
                .FirstOrDefaultAsync(u => u.Id == idUser);

            if (user == null)
                return NotFound();

            if (product.Company.Id != user.Сompany.Id)
                return NotFound();

            var Event = new models.сompany.Event.Event
            {
                Title = eventDTO.Title,
                ShortDescription = eventDTO.ShortDescription,
                FullDescription = eventDTO.FullDescription,
                WebUrl = eventDTO.WebUrl,
                DatePublication = DateTime.Now,
                EventStatus = EventStatus.EXAMINATION,
                Company = product.Company,
                Product = product
            };

            product.Company.Events.Add(Event);
            product.Events.Add(Event);
            
            await _efModel.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvent), new { Event.Id },
                _mapper.Map<EventItemDTO>(Event));
        }

        [Authorize("Admin")]
        [HttpPatch("{id}/Status")]
        public async Task<ActionResult> PatchEventStatus(int id, EventStatus status)
        {
            var Event = await _efModel.Events.FindAsync(id);

            if (Event != null)
                return NotFound();

            Event.EventStatus = status;
            await _efModel.SaveChangesAsync();

            return Ok();
        }
    }
}
