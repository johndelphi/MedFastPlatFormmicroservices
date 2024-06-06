namespace Medfast.Services.MedicationAPI.Models.Dto;

public class PasswordResetDto
{
    public string Email { get; set; }
    public string ResetCode { get; set; }
    public string NewPassword { get; set; }
}