using AutoMapper;
using Users.Application.Common;
using Users.Application.Dtos.Requests;
using Users.Application.Dtos.Response;
using Users.Application.Events;
using Users.Application.Repository;
using Users.Application.Services.Interfaces;
using Users.Application.Validations.User;
using Users.Domain.Entities;
using Users.Domain.Entities.Identity;

namespace Users.Application.Services
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IUserCreatedEventHandler _userCreatedEventHandler;
        private readonly IUserEventRepository _userEventRepository;
        public UserServices(IUserRepository userRepository, IMapper mapper, IUserCreatedEventHandler userCreatedEventHandler, IUserEventRepository userEventRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userCreatedEventHandler = userCreatedEventHandler ?? throw new ArgumentNullException(nameof(userCreatedEventHandler));
            _userEventRepository = userEventRepository;
        }

        public async Task BlockUserAsync(BlockUserRequest request)
        {
            var resultValidate = new BlockUserRequestValidator().Validate(request);
            if (resultValidate.IsValid is false)
            {
                var messages = string.Concat("Message is invalid, validation errors: ", resultValidate.Errors.ConvertToString());
                throw new Exception(messages);
            }

            var data = await _userRepository.GetByIdAsync(request.Id);
            if (data is null)
            {
                throw new Exception("Usuario não encontrado.");
            }

            await _userRepository.BlockUserAsync(data, request.EnableBlocking);


            var eventType = Domain.Entities.EventUser.block;

            if (request.EnableBlocking is false)
                eventType = Domain.Entities.EventUser.active;

            //salva o evento na base
            await _userEventRepository.AddEvent(
                new Domain.Entities.UserEvent()
                {
                    Event = eventType,
                    UserId = data.Id,
                    email = data.Email,
                    CreateAt = DateTime.Now
                });
        }

        public async Task CreateAsync(CreateUserRequest request)
        {
            var persistence = _mapper.Map<UsersEntitie>(request);
            var userId = await _userRepository.CreateAsync(persistence, request.Password);

            //Publica evento
            _userCreatedEventHandler.PublishUserCreatedEvent(new Domain.Events.UserEvent()
            {
                Id = userId,
                Email = request.Email,
                Name = request.NickName,
                EventType = EventUser.create
            });

            //salva o evento na base
            await _userEventRepository.AddEvent(
                new Domain.Entities.UserEvent()
                {
                    Event = Domain.Entities.EventUser.create,
                    UserId = userId,
                    email = request.Email,
                    CreateAt = DateTime.Now
                });
        }

        public async Task DeleteAsync(DeleteUserRequest request)
        {
            var resultValidate = new DeleteUserRequestValidator().Validate(request);
            if (resultValidate.IsValid is false)
            {
                var messages = string.Concat("Message is invalid, validation errors: ", resultValidate.Errors.ConvertToString());
                throw new Exception(messages);
            }

            var data = await _userRepository.GetByIdAsync(request.Id);
            if (data is null)
            {
                throw new Exception("Usuario não encontrado.");
            }

            await _userRepository.DeleteAsync(data);

            //Publica evento
            _userCreatedEventHandler.PublishUserCreatedEvent(new Domain.Events.UserEvent()
            {
                Id = data.Id,
                Email = data.Email,
                Name = data.NickName,
                EventType = EventUser.delete
            });

            //salva o evento na base
            await _userEventRepository.AddEvent(
                new Domain.Entities.UserEvent()
                {
                    Event = Domain.Entities.EventUser.delete,
                    UserId = data.Id,
                    email = data.Email,
                    CreateAt = DateTime.Now
                });
        }

        public async Task<UserResponse> GetByEmailAsync(GetUserByEmailRequest request)
        {
            var resultValidate = new GetUserByEmailRequestValidator().Validate(request);
            if (resultValidate.IsValid is false)
            {
                var messages = string.Concat("Message is invalid, validation errors: ", resultValidate.Errors.ConvertToString());
                throw new Exception(messages);
            }

            var data = await _userRepository.GetByEmailAsync(request.Email);
            return _mapper.Map<UserResponse>(data);
        }

        public async Task<UserResponse> GetByIdAsync(GetUserByIdRequest request)
        {
            var resultValidate = new GetUserByIdRequestValidator().Validate(request);
            if (resultValidate.IsValid is false)
            {
                var messages = string.Concat("Message is invalid, validation errors: ", resultValidate.Errors.ConvertToString());
                throw new Exception(messages);
            }

            var data = await _userRepository.GetByIdAsync(request.Id);
            return _mapper.Map<UserResponse>(data);
        }

        public async Task<UserResponse> GetByNickNameAsync(GetUserByNickNameRequest request)
        {
            var resultValidate = new GetUserByNickNameRequestValidator().Validate(request);
            if (resultValidate.IsValid is false)
            {
                var messages = string.Concat("Message is invalid, validation errors: ", resultValidate.Errors.ConvertToString());
                throw new Exception(messages);
            }

            var data = await _userRepository.GetByNicknameAsync(request.NickName);
            return _mapper.Map<UserResponse>(data);
        }

        public async Task<List<UserResponse>> ListUsersNoTwoFactor()
        {
            var data = await _userRepository.ListUsersNoTwoFactor();
            List<UserResponse> result = _mapper.Map<List<UserResponse>>(data);
            return result;
        }

    }
}
