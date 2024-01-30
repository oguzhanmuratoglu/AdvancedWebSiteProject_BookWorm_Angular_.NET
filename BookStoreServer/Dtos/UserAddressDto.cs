namespace BookStoreServer.Dtos;

public class UserAddressDto
{
    public int UserId { get; set; }
    public string AddressType { get; set; }
    public string ContactName { get; set; }
    public string ContactEmail { get; set; }
    public string ContactPhoneNumber { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string ZipCode { get; set; }
    public string AddressDescription { get; set; }
}
