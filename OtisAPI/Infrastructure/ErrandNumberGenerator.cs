namespace OtisAPI.Infrastructure;

public static class ErrandNumberGenerator
{
    private static char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
    private static Random random = new Random();

    //Generates errand number, 18 characters in total Format: yyyyMMdd-ABC012345
    public static string GenerateErrandNumber()
    {
        return $"{DateTime.UtcNow:yyyyMMdd}-{GetLetters()}{GenerateNumber()}";
    }

    //Get's thee random letters and returns that as a string
    private static string GetLetters()
    {
        return $"{alpha[random.Next(0, alpha.Length)]}{alpha[random.Next(0, alpha.Length)]}{alpha[random.Next(0, alpha.Length)]}";
    }

    //Generates random number and returns that as a string
    private static string GenerateNumber()
    {
        int number = random.Next(0, 999999);
        if (number < 10)
            return $"00000{number}";
        if (number < 100)
            return $"0000{number}";
        if (number < 1000)
            return $"000{number}";
        if (number < 10000)
            return $"00{number}";
        if (number < 100000)
            return $"0{number}";

        return $"{number}";
    }
}