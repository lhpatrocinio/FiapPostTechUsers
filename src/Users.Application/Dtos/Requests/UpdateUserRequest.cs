namespace Users.Application.Dtos.Requests
{
    public class UpdateUserRequest
    {

        public int UserName { get; set; }
        public int Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
    }
}
