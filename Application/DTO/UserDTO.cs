using Domain.Models;

namespace Application.DTO;

public class UserDTO
{
    public Guid Id { get; set; }
    public string Names { get; set; } = string.Empty;
    public string Surnames { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime FinalDate { get; set; }
    public PeriodDateTime PeriodDateTime { get; set; } = default!;
}

