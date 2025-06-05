using System;
using Microsoft.EntityFrameworkCore;
using MovieDBMinimalAPI.Data;
using MovieDBMinimalAPI.Features.Users.Models;
namespace MovieDBMinimalAPI.Features.Users.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly DbApplicationContext _context;

        public UserRepository(DbApplicationContext context)
        {
            _context = context;
        }

        public async Task<bool> UserExistsAsync(string userId) =>
            await _context.Users.AnyAsync(u => u.Id == userId);

        public async Task<bool> UserAddAsync(string userId, string email, DateOnly dateRegisteredAt, string username)
        {
            if (await UserExistsAsync(userId)) return false;

            _context.Users.Add(new User { Id = userId, Email = email, DateRegisteredAt = dateRegisteredAt, Username = username });
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
