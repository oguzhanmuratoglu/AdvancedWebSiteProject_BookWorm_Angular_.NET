namespace BookStoreServer.Dtos;

public class LoginRequestDto
{
    public string UserNameOrEmail { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; }
}
