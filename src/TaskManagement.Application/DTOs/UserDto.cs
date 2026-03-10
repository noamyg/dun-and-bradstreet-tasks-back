using AutoMapper;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.DTOs;

[AutoMap(typeof(User))]
public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
