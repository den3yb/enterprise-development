namespace AviaCompany.Domain;

public class Passenger
{
    public int Id { get; set; }
    public required string PassportNumber { get; set; }
    public required string FullName { get; set; }
    public required DateOnly BirthDate { get; set; }
    
    public List<Ticket> Ticket { get; set; } = [];
}