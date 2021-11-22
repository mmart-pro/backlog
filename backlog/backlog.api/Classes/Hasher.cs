using System.Security.Cryptography;
using System.Text;

namespace backlog.api.Classes;

/// <summary>
/// Класс для вычисления хешей
/// </summary>
public static class Hasher
{
    /// <summary>
    /// Вычисляет хеш для строки (например, для пароля)
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static byte[] GetHash(string str)
    {
        return MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(str));
    }

    /// <summary>
    /// Возвращает хеш, преобразованный в 16-ричную систему, в виде строки
    /// </summary>
    /// <param name="hash"></param>
    /// <returns></returns>
    public static string HashToHex(byte[] hash)
    {
        return hash.Aggregate(string.Empty, (current, b) => current + b.ToString("X2"));
    }
}
