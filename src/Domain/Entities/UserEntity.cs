using System;
using System.Data;
using System.Xml.Linq;

namespace Domain.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public int Roles { get; set; }
}
