﻿using AutoMapper;
using FastestDeliveryApi.database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StoreAppApi.Auth;
using StoreAppApi.Controllers.company.common;
using StoreAppApi.DTOs.product;
using StoreAppApi.DTOs.product.review;
using StoreAppApi.DTOs.user;
using StoreAppApi.models.user;
using StoreAppApi.Repository;
using StoreAppApi.Repository.image;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StoreAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private EfModel _efModel;
        private readonly IMapper _mapper;
        private readonly ImageUserRepository _imageUserRepository;

        public UserController(
            EfModel model, ImageUserRepository imageUserRepository, IMapper mapper
            )
        {
            _mapper = mapper;
            _imageUserRepository = imageUserRepository;
            _efModel = model;
        }

        [Authorize]
        [HttpGet("Product/Review")]
        public async Task<ActionResult<ReviewDTO>> GetUserReview()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
                return NotFound();

            int idUser = Convert.ToInt32(identity.FindFirst("Id").Value);

            BaseUser user = await _efModel.BaseUsers
                .Include(u => u.Reviews)
                .FirstOrDefaultAsync(u => u.Id == idUser);

            if (user == null || user.Reviews == null)
                return NotFound();

            return new ReviewDTO
            {
                Items = user.Reviews
            };
        }

        [Authorize]
        [HttpGet("/Product/Download")]
        public async Task<ActionResult<ProductDTO>> GetProductDowlaund()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
                return NotFound();

            int idUser = Convert.ToInt32(identity.FindFirst("Id").Value);

            BaseUser user = await _efModel.BaseUsers
                .Include(u => u.ProductsDownload)
                .FirstOrDefaultAsync(u => u.Id == idUser);

            if (user == null)
                return NotFound();

            return new ProductDTO
            {
                Items = _mapper.Map<List<ProductItemDTO>>(user.ProductsDownload)
            };
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult> PutUser(UserUpdateDTO userDTO)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
                return NotFound();

            int idUser = Convert.ToInt32(identity.FindFirst("Id").Value);

            BaseUser user = await _efModel.BaseUsers.FindAsync(idUser);

            if (user == null)
                return NotFound();

            user.Username = userDTO.Username;
            user.Email = userDTO.Email;

            _efModel.Entry(user).State = EntityState.Modified;

            try
            {
                await _efModel.SaveChangesAsync();
            } catch (Exception ex)
            {
                return NotFound(ex);
            }

            return Ok();
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<BaseUserDTO>> GetBaseUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
                return NotFound();

            int idUser = Convert.ToInt32(identity.FindFirst("Id").Value);

            BaseUser user = await _efModel.BaseUsers.FindAsync(idUser);

            if (user == null)
                return NotFound();

            return _mapper.Map<BaseUserDTO>(user);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseUserDTO>> GetBaseUserById(int id)
        {
            BaseUser user = await _efModel.BaseUsers.FindAsync(id);

            if (user == null)
                return NotFound();

            return _mapper.Map<BaseUserDTO>(user);
        }

        [HttpOptions("{id}/Image/Size")]
        public ActionResult<string> OptionsUserImage(int id)
        {
            string size = _imageUserRepository.GetUserImageSize(id);

            if (size == null)
                return NotFound();
            else
                return size;
        }

        [HttpGet("user_{id}.jpg")]
        public ActionResult GetUserPhoto(int id)
        {
            byte[] file = _imageUserRepository.GetUserImage(id);

           if (file != null)
                return File(file, "image/jpeg");
            else
                return NotFound();

        }

        [Authorize]
        [HttpPost("Photo")]
        public async Task<ActionResult> PostUserImage(IFormFile file)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
                return NotFound();

            int idUser = Convert.ToInt32(identity.FindFirst("Id").Value);

            BaseUser user = await _efModel.BaseUsers.FindAsync(idUser);

            if (user == null)
                return NotFound();

            user.Photo = $"{Constants.BASE_URL}/user/user_" + idUser + ".jpg";

            MemoryStream memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            _imageUserRepository.DeleteUserImage(idUser);
            _imageUserRepository.PostUserImage(memoryStream.ToArray(), idUser);

            await _efModel.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("Registration")]
        public async Task<ActionResult<RegistrationResultDTO>> Registration(RegistrationDTO registrationDTO)
        {
            if (_efModel.BaseUsers.Any(u => u.Email == registrationDTO.Email))
                return new RegistrationResultDTO
                {
                    Error = "Пользователь с таким email уже существует"
                };

            if (registrationDTO.Email.Length < 6 || registrationDTO.Password.Length < 6)
                return new RegistrationResultDTO
                {
                    Error = "" +
                    "Email должен состоять из 8 или больше символов \n" +
                    "Password должен состоять из 8 или больше символов \n" +
                   "FIO должен состоять из 6 или больше символов"
                };

            if (!registrationDTO.Email.Contains(".") || !registrationDTO.Email.Contains("@"))
                return new RegistrationResultDTO
                {
                    Error = "" +
                    "Некорректно введен Email"
                };

            _efModel.BaseUsers.Add(new BaseUser
            {
                Username = registrationDTO.Username,
                Email = registrationDTO.Email,
                Password = registrationDTO.Password
            });
            await _efModel.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("Authorization")]
        public ActionResult<object> Token(AuthorizationDTO authorization)
        {
            var indentity = GetIdentity(authorization.Email, authorization.Password);

            if (indentity == null)
            {
                return BadRequest();
            }

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    audience: TokenBaseOptions.AUDIENCE,
                    issuer: TokenBaseOptions.ISSUER,
                    notBefore: now,
                    claims: indentity.Claims,
                    expires: now.Add(TimeSpan.FromDays(TokenBaseOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(TokenBaseOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = indentity.Name,
                role = indentity.FindFirst(ClaimsIdentity.DefaultRoleClaimType).Value
            };

            return response;
        }

        [NonAction]
        public ClaimsIdentity GetIdentity(string email, string password)
        {
            BaseUser user = _efModel.BaseUsers.FirstOrDefault(x => x.Email == email && x.Password == password);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Username),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString()),
                    new Claim("Id", user.Id.ToString())
                };

                ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType);

                return claimsIdentity;
            }
            return null;
        }
    }
}
