﻿namespace BookStoreServer.Dtos;

public class ChangePasswordDto
{
    public int UserId { get; set; }
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
}
