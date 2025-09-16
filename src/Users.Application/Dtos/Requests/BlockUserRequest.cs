using System;

namespace Users.Application.Dtos.Requests
{
    public class BlockUserRequest
    {
        public Guid Id { get; set; }
        public bool EnableBlocking { get; set; }
    }
}
