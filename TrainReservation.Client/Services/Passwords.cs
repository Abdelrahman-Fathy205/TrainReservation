using System.Security.Cryptography; // cryptography library

namespace TrainReservation.Client.Services; // namespace for the services

/// <summary>Salted PBKDF2 hashes, stored as "iterations.salt.hash" (base64).</summary>
public static class Passwords // class that hashes passwords and checks them
{
    const int Iterations = 100_000; // number of hash rounds (more rounds = harder to crack)

    public static string Hash(string password) // hashes the password
    {
        byte[] salt = RandomNumberGenerator.GetBytes(16); // make a random "salt" so each hash is different
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, 32); // hash the password with PBKDF2
        return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}"; // return the result as text: rounds.salt.hash
    }

    public static bool Verify(string password, string stored) // checks that the password is correct
    {
        var parts = (stored ?? "").Split('.'); // split the stored text into its 3 parts
        if (parts.Length != 3 || !int.TryParse(parts[0], out int iterations)) return false; // if the format is wrong, reject
        try // try to decode the parts
        {
            byte[] salt = Convert.FromBase64String(parts[1]), expected = Convert.FromBase64String(parts[2]); // get the saved salt and hash
            byte[] actual = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, expected.Length); // hash the typed password with the same salt
            return CryptographicOperations.FixedTimeEquals(actual, expected); // compare both in a way that is safe from timing attacks
        }
        catch (FormatException) { return false; } // if the stored text is broken, reject
    }
}
