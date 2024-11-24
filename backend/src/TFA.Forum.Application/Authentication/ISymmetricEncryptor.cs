namespace TFA.Forum.Application.Authentication;

public interface ISymmetricEncryptor
{
    Task<string> Encrypt(string plainText, byte[] key, CancellationToken cancellationToken);
}