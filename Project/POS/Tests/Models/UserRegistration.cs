using Tests.Helpers;

namespace Tests.Models;

public class UserRegistration
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;

    public UserRegistration(bool randomizeValues = true)
    {
        var dataRandomizer = new DataRandomizer();
        Email = dataRandomizer.RandomEmail();
        Password = dataRandomizer.RandomPassword();
    }
}