using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    
    public class UsersController : BaseApiController
    {
        private readonly DataContext _context;
   
        private readonly ITokenService _tokenservice;

        public UsersController(DataContext context, ITokenService tokenservice)
        {
            _tokenservice = tokenservice;
            _context = context;
            
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
           return await _context.Users.ToListAsync();
                     
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<DTOUser>> GetUser(int id)
        {
            //return _context.Users.Where(u => u.Id == id).FirstOrDefault();
            var user = await _context.Users.FindAsync(id);
             return new DTOUser{
                    un = user.UserName,
                 token = _tokenservice.CreateToken(user)
                };

                     
        }
    }
}