namespace TFA.Forum.Application.Authentication;

internal interface ISymmetricDecryptor
{
    Task<string> Decrypt(string encryptedText, byte[] key, CancellationToken cancellationToken);
}