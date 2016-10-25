using System;

public static class AzureStorageConstants
{
    public static readonly string KeyString = "ZB7yJ/Fz4NBB1sHtbj2/k586DOfTIV/qXRuKZCtvH9oNtJJAL+RNTNM0oWeQVZ+nP5vOEJQhlgX6MRb4KUq2uw==";
    public static readonly string Account = "welfaredenmark";
    public static readonly string BlobEndPoint = "https://welfaredenmark.blob.core.windows.net/";
    public static readonly string SharedKeyAuthorizationScheme = "SharedKey";
    public static readonly byte[] Key = Convert.FromBase64String(AzureStorageConstants.KeyString);
    public static readonly string container = "vfo-recordings-staging";
}
