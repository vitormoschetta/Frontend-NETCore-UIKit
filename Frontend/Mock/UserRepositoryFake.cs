using System;
using System.Collections.Generic;
using System.Linq;
using Frontend.Models;

namespace Frontend.Mock
{
    public class UserRepositoryFake
    {
        public readonly List<User> List;
        public UserRepositoryFake()
        {
            List = new List<User>();

            // admin
            var user = new User() { Id = Guid.NewGuid(), Username = "admin", Password = "1234", Role = "Admin", Active = true };
            List.Add(user);

            // Usuario comum ativo:
            user = new User() { Id = Guid.NewGuid(), Username = "usuario", Password = "1234", Role = "User", Active = true };
            List.Add(user);

            // Usuario comum inativo:
            user = new User() { Id = Guid.NewGuid(), Username = "inativo", Password = "1234", Role = "User", Active = false };
            List.Add(user);

            // Usuario cadastrado, aguardando liberação de acesso (feito pelo Admin):
            user = new User() { Id = Guid.NewGuid(), Username = "cadastrado", Password = "1234", Role = null, Active = false };
            List.Add(user);
        }


        public void Register(UserRegister model)
        {
            var user = new User()
            {           
                Id = Guid.NewGuid(),     
                Username = model.Username,
                Password = model.Password,
                Role = null,
                Active = false
            };

            List.Add(user);
        }

        public void RegisterAdmin(UserRegister model)
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Username = model.Username,
                Password = model.Password,
                Role = model.Role,
                Active = model.Active
            };

            List.Add(user);
        }

        public User Login(User model)
        {
            return List.FirstOrDefault(x => x.Username == model.Username && x.Password == model.Password);
        }

        public User ActivateFirstAccess(Guid id, string role)
        {
            var user = List.FirstOrDefault(x => x.Id == id);
            if (user != null)
            {
                user.Active = true;
                user.Role = role;
            }

            return user;
        }

        public List<User> GetInactivesFirstAccess()
        {
            return List.Where(x => x.Role == null && x.Active == false).ToList();
        }

        public List<User> GetAll()
        {
            return List
                .Where(x => x.Role != null)
                .OrderBy(x => x.Username).ToList();
        }

        public User GetById(Guid id)
        {
            return List.FirstOrDefault(x => x.Id == id);
        }

        public User GetByName(string name)
        {
            return List.FirstOrDefault(x => x.Username == name);
        }

        public void UpdateRoleActive(User model)
        {
            var user = List.FirstOrDefault(x => x.Id == model.Id);

            if (user != null)
            {
                user.Role = model.Role;
                user.Active = model.Active;
            }
        }

        public bool UpdatePasswordValid(UserUpdatePassword model)
        {
            var user = List.FirstOrDefault(x => x.Username == model.Username && x.Password == model.Password);
            if (user != null)
                return true;

            return false;
        }

        public void UpdatePassword(string newPass, User model)
        {
            var user = List.FirstOrDefault(x => x.Id == model.Id);

            if (user != null)
                user.Password = newPass;
        }


        public List<User> Search(string param)
        {
            if (param == "empty")
                param = "";

            var lista = List
                .Where(x => x.Username.Contains(param) || x.Role.Contains(param))
                .OrderBy(x => x.Username).ToList();

            return lista;
        }

        public bool Exists(UserRegister model)
        {
            var item = List.FirstOrDefault(x => x.Username == model.Username);
            if (item != null)
                return true;

            return false;
        }

        public bool ExistsUpdate(User model, Guid id)
        {
            var item = List.FirstOrDefault(x => x.Username == model.Username && x.Id != id);

            if (item != null)
                return true;

            return false;
        }




        public void Update(User model)
        {
            var item = List.FirstOrDefault(x => x.Id == model.Id);
            if (item != null)
            {
                List.Remove(item);
                List.Add(model);
            }
        }

        public void Delete(Guid id)
        {
            var item = List.FirstOrDefault(x => x.Id == id);
            if (item != null)
                List.Remove(item);
        }
    }
}