using System.Collections.Generic;

public class Role
{
    public int Id { get; set; }
    public string RoleName { get; set; }
    public List<Permission> Permissions { get; set; } = new List<Permission>();
}
