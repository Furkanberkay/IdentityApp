using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace IdentityApp.ViewModels
{
    public class EditViewModel
    {
        public string Username { get; set; } = string.Empty;
        public string? Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        public string? PassWord { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(PassWord), ErrorMessage = "Parolanız Eşleşmiyor")]
        public string? ConfirmPassWord { get; set; }
        
        public IList<string>? SelectedRoles { get; set; }
    }
}