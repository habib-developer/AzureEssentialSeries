namespace Ep2.Models
{
    public record Student(
        string id,
        string name,
        string email,
        List<string> hobbies
        );
}
