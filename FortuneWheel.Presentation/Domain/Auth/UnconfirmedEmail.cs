namespace OnlineAuc.Domain.Auth
{
    public class UnconfirmedEmail
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public Guid AccountId { get; set; }
        public string Code { get; set; }

    }
}
