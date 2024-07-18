using TrybeHotel.Models;
using TrybeHotel.Dto;
using TrybeHotel.Utils.interfaces;
using TrybeHotel.Exceptions;

namespace TrybeHotel.Repository
{
    public class UserRepository : IUserRepository
    {
        protected readonly ITrybeHotelContext _context;
        protected readonly IEntityUtils _entityUtils;
        public UserRepository(ITrybeHotelContext context, IEntityUtils entityUtils)
        {
            _context = context;
            _entityUtils = entityUtils;
        }
        public UserDto GetUserById(int userId)
        {
            var user = _entityUtils.VerifyUser(userId);
            var response = _entityUtils.CreateUserDto(user);
            
            return response;
        }

        public UserDto Login(LoginDto login)
        {
            var user = _context.Users.Where(u => u.Email == login.Email && u.Password == login.Password).FirstOrDefault();

            if (user == null)
            {
                throw new UnauthorizedAccessException("Incorrect e-mail or password");
            }

            var response = _entityUtils.CreateUserDto(user);

            return response;
        }
        public UserDto Add(UserDtoInsert user)
        {
            var emailAlreadyRegistered = _context.Users.Any(u => u.Email == user.Email);

            if (emailAlreadyRegistered)
            {
                throw new EmailAlreadyRegisteredException("User email already exists");
            }

            var newUser = new User()
            {
                Name = user.Name!,
                Email = user.Email!,
                Password = user.Password!,
                UserType = "client"
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            UserDto response = _entityUtils.CreateUserDto(newUser);

            return response;
        }

        public UserDto GetUserByEmail(string userEmail)
        {
            var user = _context.Users.Where(u => u.Email == userEmail).FirstOrDefault();

            if (user == null)
            {
                throw new EntityNotFoundException("User not found");
            }

            var response = _entityUtils.CreateUserDto(user);

            return response;
        }

        public IEnumerable<UserDto> GetUsers()
        {
            var users = _context.Users.Select(user => _entityUtils.CreateUserDto(user));

            return users;
        }

    }
}