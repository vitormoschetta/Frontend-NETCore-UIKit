using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Frontend.Interfaces;
using Frontend.Models;
using Frontend.Services;

namespace Frontend.Mock
{
    public class UserServiceFake : IUserService
    {
        private readonly GetUserAuth _userAuth;
        private readonly UserRepositoryFake _repository;
        public UserServiceFake(GetUserAuth userAuth, UserRepositoryFake repository)
        {
            _userAuth = userAuth;
            _repository = repository;
        }
        private string baseUrl = "https://localhost:5001/UserAuth";

        public async Task<UserResult> Register(UserRegister modelo)
        {
            if (_repository.Exists(modelo))
                return new UserResult() { Success = false, Message = "Cliente já cadastrado. ", Object = null };

            _repository.Register(modelo);
            return new UserResult() { Success = true, Message = "Cadastrado com sucesso. ", Object = null };
        }

        public async Task<UserResult> RegisterAdmin(UserRegister modelo)
        {
            if (_repository.Exists(modelo))
                return new UserResult() { Success = false, Message = "Cliente já cadastrado. ", Object = null };

            _repository.RegisterAdmin(modelo);
            return new UserResult() { Success = true, Message = "Cadastrado com sucesso. ", Object = null };
        }

        public async Task<UserResult> Login(User modelo)
        {
            var user = _repository.Login(modelo);
            if (user == null)
                return new UserResult()
                { Success = false, Message = "Usuário inválido. ", Object = modelo };

            if (user.Active == false && user.Role == null)
                return new UserResult()
                { Success = false, Message = "Aguardando liberação do Administrador. ", Object = modelo };

            else if (user.Active == false)
                return new UserResult()
                { Success = false, Message = "Usuário inativo. ", Object = modelo };

            return new UserResult()
            { Success = true, Message = "Login efetuado. ", Object = user, Token = Guid.NewGuid().ToString() };
        }


        public async Task<List<User>> GetInactivesFirstAccess()
        {
            return _repository.GetInactivesFirstAccess();
        }

        public async Task<UserResult> ActivateFirstAccess(Guid id, string role = "User")
        {
            var user = _repository.ActivateFirstAccess(id, role);
            if (user == null)
                return new UserResult() { Success = false, Message = "Erro ", Object = null };

            return new UserResult() { Success = true, Message = "Usuário Liberado. ", Object = user };
        }

        public async Task<UserResult> Delete(Guid id)
        {
            _repository.Delete(id);
            return new UserResult() { Success = true, Message = "Excluído com Sucesso. ", Object = null };
        }


        public async Task<List<User>> GetAll()
        {
            return _repository.GetAll();
        }

        public async Task<User> GetById(Guid id)
        {
            return _repository.GetById(id);
        }

        public async Task<User> GetByName(string name)
        {
            return _repository.GetByName(name);
        }


        public async Task<UserResult> UpdateRoleActive(User user)
        {
            _repository.UpdateRoleActive(user);
            return new UserResult() { Success = true, Message = "Atualizado com Sucesso!", Object = user };
        }

        public async Task<UserResult> UpdatePassword(UserUpdatePassword userUpdatePassword)
        {
            if (!_repository.UpdatePasswordValid(userUpdatePassword))
                return new UserResult() { Success = false, Message = "Senha antiga não confere ", Object = null };

            User user = new User();
            user.Username = userUpdatePassword.Username;
            user.Id = userUpdatePassword.Id;
            user.Password = userUpdatePassword.Password;

            _repository.UpdatePassword(userUpdatePassword.NewPassword, user);
            return new UserResult() { Success = true, Message = "Senha atualizada com sucesso! ", Object = user };
        }

        public async Task<List<User>> Search(string param)
        {
            return _repository.Search(param);
        }
     
        public Task<List<User>> SearchRequestAccess(string filter)
        {
            throw new NotImplementedException();
        }
    }
}