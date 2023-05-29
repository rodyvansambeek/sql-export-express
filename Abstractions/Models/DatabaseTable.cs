namespace Abstractions.Models;
public record DatabaseTable
{
    public required string Name { get; set; }
    public required int Rows { get; set; }
}
