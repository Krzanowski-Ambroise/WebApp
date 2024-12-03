namespace WebApp.Models;

public class AddRoleViewModel
{
    public string UserId { get; set; }
    public string? UserName { get; set; }
    public string? SelectedRole { get; set; }
    public List<string>? AvailableRoles { get; set; }
    public List<string> UserRoles { get; set; }
}