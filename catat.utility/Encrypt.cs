namespace catat.utility;

public class Encrypt
{
    public string HashPassword(string password)
    {
        // Contoh sederhana dari fungsi hash, sebaiknya gunakan algoritma hash yang lebih aman seperti BCrypt atau Argon2
        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLowerInvariant();
            return hash;
        }
    }
}
