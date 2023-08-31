using System.Net.Mail;

namespace HealthHarmony.Model;

public class User : Base.ModelBase
{
    public int Id { get; private set; }

    private string _name;
    public string Name 
    {
        get => _name;
        private set => Set(ref _name, value);
    }

    private string _sername;
    public string Sername
    {
        get => _sername;
        private set => Set(ref _sername, value);
    }

    private string _phone;
    public string Phone
    {
        get => _phone;
        private set => Set(ref _phone, value);
    }

    private string _email;
    public string Email
    {
        get => _email;
        private set => Set(ref _email, value);
    }

    [System.Text.Json.Serialization.JsonConstructor]
    public User(int id, string name, string sername, string phone, string email)
    {
        Name = name;
        Sername = sername;
        Phone = phone;
        Email = email;
        Id = id;
    }

    private User(string name, string sername, string phone, string email)
    {
        Name = name;
        Sername = sername;
        Phone = phone;
        Email = email;
    }

    public void Map(UserDataPackage dbUser)
    {
        Id = dbUser.Id;
        Name = dbUser.Name;
        Sername = dbUser.Sername;
        Phone = dbUser.Phone;
        Email = dbUser.Email;
    }

}
