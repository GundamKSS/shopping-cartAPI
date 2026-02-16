namespace MyAPI_1.Models;

public class UserProfile
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty; // MR.Siwakorn Khongsotsap
    public string Nickname { get; set; } = string.Empty; // กันดั้ม
    public DateTime Birthday { get; set; } // 1 October 1998
    public string Personality { get; set; } = string.Empty; // ENFP-T
    public string Location { get; set; } = string.Empty; // Buriram, Thailand
}