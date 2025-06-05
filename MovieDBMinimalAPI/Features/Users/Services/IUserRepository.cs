namespace MovieDBMinimalAPI.Features.Users.Services
{
    public interface IUserRepository
    {
        public Task<bool> UserExistsAsync(string userId);

        public Task<bool> UserAddAsync(string userId, string email, DateOnly dateRegisteredAt, string username);
    }
}
