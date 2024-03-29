﻿namespace Medfast.Services.MedicationAPI.Models.Dto
{
  public class ResponseDto
  {
    public bool IsSuccess { get; set; } = true;
    public object Result { get; set; }
    public string? DisplayMessage { get; set; } = "";
    public string phoneNumber { get; set; }
    public List<string> ErrorMessages { get; set; }
  }
}
