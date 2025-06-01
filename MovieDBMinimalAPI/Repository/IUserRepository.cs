namespace MovieDBMinimalAPI.Repository
{
    public interface IUserRepository
    {
        public Task<bool> UserExistsAsync(string userId);

        public Task<bool> UserAddAsync(string userId, string email);
    }
}
