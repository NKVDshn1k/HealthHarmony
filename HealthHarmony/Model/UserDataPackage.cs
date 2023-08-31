using System.Net.Mail;

namespace HealthHarmony.Model;

public class UserDataPackage
{
    public int Id { get;  set; }
    public string Name { get; set; }
    public string Sername { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }

    public (bool isValide, string whyNot) Validate()
    {
        if (Name == null || Sername == null || Phone == null || Email == null)
            return (false, "All fields must be filled");

        if (Name.Length < 2)
            return (false, "The Name field must contain not less than two characters");

        if (Sername.Length < 2)
            return (false, "The Sername field must contain not less than two characters");

        if (Phone.Length != 11 && Phone.Length != 13)
            return (false, "The Phone field must contain 11 or 13 digits");

        MailAddress mail;
        if (!MailAddress.TryCreate(Email, out mail))
            return (false, "The Email field is filled incorrectly");

        if (mail.Address.Contains("spam"))
            return (false, "The Email field contain the \"spam\" word");

        return (true, null);
    }

    public static UserDataPackage FromUser(User user) =>
        new UserDataPackage()
        {
            Name = user.Name,
            Sername = user.Sername,
            Phone = user.Phone,
            Email = user.Email
        };
}
