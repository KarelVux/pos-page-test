namespace Tests.Helpers;

public class DataRandomizer
{
    private static Random random = new Random();

    public string RandomString(int length = 15)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public string RandomNumber(int length = 2)
    {
        const string chars = "1234567890";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }


    public string RandomSymbol(int length = 2)
    {
        const string chars = "!#¤%&/()@£$@£$@£";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }


    public string RandomEmail(int usernameLength = 15, int domainLength = 10)
    {
        return $"{RandomString(usernameLength)}@{RandomString(domainLength)}.com";
    }

    public string RandomPassword(int stringUpperLength = 5, int stringLowerLength = 5, int numberLength = 5,
        int symbolLength = 5)
    {
        return
            $"{RandomString(stringUpperLength)}{RandomString(stringLowerLength).ToLower()}{RandomNumber(numberLength)}{RandomSymbol(symbolLength)}";
    }
}