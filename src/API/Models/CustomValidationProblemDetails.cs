using Microsoft.AspNetCore.Mvc;

namespace API.Models;

public class CustomValidationProblemDetails : ProblemDetails
{
  public IDictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();
}