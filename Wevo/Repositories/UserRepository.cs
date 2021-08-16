using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wevo.Models;
using Wevo.Services;

namespace Wevo.Repositories
{
    public class UserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public dynamic Login(string email, string password)
        {
            var user = _context.Users.Where(x => x.email.ToLower() == email.ToLower() && x.password == password).FirstOrDefault();
            var token = "";

            if (user != null)
            {
                user.password = "****";
                token = TokenService.GenerateToken(user);
            }

            return new { 
                user = user, 
                token = token 
            };
        }
        public dynamic ValidateFormRequired(User user)
        {
            var error = false;
            List<string> message = new List<string>();

            if (user.name == null)
            {
                error = true;
                message.Add("name is required");
            }

            if (user.cpf == null)
            {
                error = true;
                message.Add("cpf is required");
            }

            if (user.sex == null)
            {
                error = true;
                message.Add("sex is required");
            }

            if (user.telephone == null)
            {
                error = true;
                message.Add("telephone is required");
            }

            if (user.email == null)
            {
                error = true;
                message.Add("email is required");
            }

            if (user.password == null)
            {
                error = true;
                message.Add("password is required");
            }

            if (user.password == null)
            {
                error = true;
                message.Add("birthday is required");
            }

            return new
            {
                error = error,
                message = message
            };
        }
        public dynamic ValidateFormUnique(User user)
        {
            var error = false;
            List<string> message = new List<string>();
            var email = _context.Users.Where(x => x.email.ToLower() == user.email.ToLower() && x.id != user.id).FirstOrDefault();
            if (email != null)
            {
                error = true;
                message.Add($"email '{email.email}' already registered");
            }

            var cpf = _context.Users.Where(x => x.cpf.ToLower() == user.cpf.ToLower() && x.id != user.id).FirstOrDefault();
            if (cpf != null)
            {
                error = true;
                message.Add($"cpf '{cpf.cpf}' already registered");
            }

            return new
            {
                error = error,
                message = message
            };
        }
        public dynamic ValidateRegister(User user)
        {
            var validate = ValidateFormRequired(user);
            if (validate.error == true)
            {
                return validate;
            }

            return ValidateFormUnique(user);
        }
        public async Task<User> Register(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            var saveUser = await GetById(user.id);
            saveUser.password = "****";
            return saveUser;
        }
        public async Task<User> GetById(int id)
        {
            var user = await _context.Users.FindAsync(id);

            return user;
        }
        public async Task<dynamic> updateById(int id, User user)
        {
            user.id = id;
            var validate = ValidateFormUnique(user);
            if (validate.error == true)
            {
                return validate;
            }

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var saveUser = await GetById(user.id);
            saveUser.password = "****";
            return saveUser;
        }
        public async Task<dynamic> list()
        {
            return await _context.Users.ToListAsync();
        }
        public async void delete(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
        public bool userExists(int id)
        {
            return _context.Users.Any(e => e.id == id);
        }
    }
}
