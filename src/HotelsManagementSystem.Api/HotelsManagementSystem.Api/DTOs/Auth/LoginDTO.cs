using HotelsManagementSystem.Api.Constants;
using System.ComponentModel.DataAnnotations;

namespace HotelsManagementSystem.Api.DTOs.Auth
{
    public class LoginDTO
    {
        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        public string Password { get; set; } = string.Empty;
    }
}
