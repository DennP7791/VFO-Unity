using UnityEngine;
using System;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Security.Cryptography;
using System.Globalization;
using System.IO;
using System.Net;
using UnityEngine.Networking;
using System.Threading;


public class AzureManager : MonoBehaviour
{
    public IEnumerator GetBlob(string blockBlobReference)
    {
        string requestMethod = "GET";
        String urlPath = string.Format("{0}/{1}", AzureStorageConstants.container, blockBlobReference);
        String msVersion = "2009-09-19";
        String dateInRfc1123Format = DateTime.UtcNow.ToString("R", CultureInfo.InvariantCulture);
        String canonicalizedHeaders = String.Format("x-ms-date:{0}\nx-ms-version:{1}", dateInRfc1123Format, msVersion);
        String canonicalizedResource = String.Format("/{0}/{1}", AzureStorageConstants.Account, urlPath);
        String stringToSign = String.Format("{0}\n\n\n\n\n\n\n\n\n\n\n\n{1}\n{2}", requestMethod, canonicalizedHeaders, canonicalizedResource);
        String authorizationHeader = CreateAuthorizationHeader(stringToSign);

        Uri uri = new Uri(AzureStorageConstants.BlobEndPoint + urlPath);
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        using (UnityWebRequest webRequest = new UnityWebRequest())
        {
            webRequest.SetRequestHeader("x-ms-date", dateInRfc1123Format);
            webRequest.SetRequestHeader("x-ms-version", msVersion);
            webRequest.SetRequestHeader("Authorization", authorizationHeader);
            webRequest.SetRequestHeader("Accept", "application/json");

            DownloadHandler downloadHandler = new DownloadHandlerBuffer();
            webRequest.downloadHandler = downloadHandler;

            webRequest.method = requestMethod;
            webRequest.url = uri.ToString();

            webRequest.Send();

            while (!webRequest.isDone)
            {
                ProgressBar = webRequest.downloadProgress;
                yield return null;
            }
            if (webRequest.isDone && webRequest.error == null)
            {
                Debug.Log(Application.persistentDataPath);

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                File.WriteAllBytes(Application.persistentDataPath + "/video.ogv", webRequest.downloadHandler.data);
#endif
#if UNITY_ANDROID || UNITY_IOS
                File.WriteAllBytes(Application.persistentDataPath + "/video.mp4", webRequest.downloadHandler.data);
#endif

                //Hardcoded value to tell the event that this method is done and that the file has been written to the device.
                ProgressBar = 2;
            }
        }

    }

    public IEnumerator PutBlob(string filePath, string blockBlobReference)
    {
        String httpMethod = UnityWebRequest.kHttpVerbPUT;

        Byte[] blobContent = File.ReadAllBytes(filePath);

        Int32 blobLength = blobContent.Length;

        const String blobType = "BlockBlob";

        String urlPath = String.Format("{0}/{1}", AzureStorageConstants.container, blockBlobReference);
        String msVersion = "2009-09-19";
        //String msVersion = "2015-02-21";
        String dateInRfc1123Format = DateTime.UtcNow.AddHours(1).ToString("R", CultureInfo.InvariantCulture);
        //String dateInRfc1123Format = DateTime.Now.ToString("R", CultureInfo.InvariantCulture);

        String canonicalizedHeaders = String.Format("x-ms-blob-type:{0}\nx-ms-date:{1}\nx-ms-version:{2}", blobType, dateInRfc1123Format, msVersion);
        String canonicalizedResource = String.Format("/{0}/{1}", AzureStorageConstants.Account, urlPath);
        String stringToSign = String.Format("{0}\n\n\n{1}\n\n\n\n\n\n\n\n\n{2}\n{3}", httpMethod, blobLength, canonicalizedHeaders, canonicalizedResource);
        String authorizationHeader = CreateAuthorizationHeader(stringToSign);

        Uri uri = new Uri(AzureStorageConstants.BlobEndPoint + urlPath);

        Debug.Log("PutBlob [HANS]: Does file Exist: " + File.Exists(filePath));
        Debug.Log("PutBlob [HANS]: File length: " + blobContent.Length);
        Debug.Log("PutBlob [HANS]: " + uri.ToString());

        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

#if UNITY_IOS
        using (UnityWebRequest webRequest = new UnityWebRequest())
        {
            webRequest.SetRequestHeader("x-ms-blob-type", blobType);
            webRequest.SetRequestHeader("x-ms-date", dateInRfc1123Format);
            webRequest.SetRequestHeader("x-ms-version", msVersion);
            webRequest.SetRequestHeader("Authorization", authorizationHeader);

            UploadHandler uploadHandler = new UploadHandlerRaw(blobContent);
            //uploadHandler.contentType = "application/octet-stream";
            webRequest.uploadHandler = uploadHandler;
            webRequest.url = uri.ToString();
            webRequest.method = httpMethod;

            webRequest.Send();

            while (!webRequest.isDone)
            {
                ProgressBar = webRequest.uploadProgress;
                Debug.Log("Progress: " + ProgressBar);
                if (webRequest.isError)
                {
                    Debug.Log("PutBlob [HANS]: " + webRequest.error);
                    break;
                }

                yield return new WaitForSeconds(1f);
            }


            if (webRequest.isDone)
            {
                ProgressBar = 2;
            }

        }

#else
    using (UnityWebRequest webRequest = new UnityWebRequest())
        {
            webRequest.SetRequestHeader("x-ms-blob-type", blobType);
            webRequest.SetRequestHeader("x-ms-date", dateInRfc1123Format);
            webRequest.SetRequestHeader("x-ms-version", msVersion);
            webRequest.SetRequestHeader("Authorization", authorizationHeader);

            UploadHandler uploadHandler = new UploadHandlerRaw(blobContent);
            //uploadHandler.contentType = "application/octet-stream";
            webRequest.uploadHandler = uploadHandler;
            webRequest.url = uri.ToString();
            webRequest.method = httpMethod;
            webRequest.Send();


            while (!webRequest.isDone)
            {
                ProgressBar = webRequest.uploadProgress;
                if (webRequest.isError)
                {
                    break;
                }
                yield return null;
            }
        }
        
#endif
    }
    //public IEnumerator PutBlob(string filePath, string blockBlobReference)
    //{
    //    String httpMethod = "PUT";

    //    Byte[] blobContent = File.ReadAllBytes(filePath);

    //    Int32 blobLength = blobContent.Length;

    //    const String blobType = "BlockBlob";

    //    String urlPath = String.Format("{0}/{1}", AzureStorageConstants.container, blockBlobReference);
    //    String msVersion = "2009-09-19";
    //    //String msVersion = "2015-02-21";
    //    String dateInRfc1123Format = DateTime.UtcNow.AddHours(1).ToString("R", CultureInfo.InvariantCulture);

    //    String canonicalizedHeaders = String.Format("x-ms-blob-type:{0}\nx-ms-date:{1}\nx-ms-version:{2}", blobType, dateInRfc1123Format, msVersion);
    //    String canonicalizedResource = String.Format("/{0}/{1}", AzureStorageConstants.Account, urlPath);
    //    String stringToSign = String.Format("{0}\n\n\n{1}\n\n\n\n\n\n\n\n\n{2}\n{3}", httpMethod, blobLength, canonicalizedHeaders, canonicalizedResource);
    //    String authorizationHeader = CreateAuthorizationHeader(stringToSign);

    //    Uri uri = new Uri(AzureStorageConstants.BlobEndPoint + urlPath);
    //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
    //    request.Method = httpMethod;
    //    request.Headers.Add("x-ms-blob-type", blobType);
    //    request.Headers.Add("x-ms-date", dateInRfc1123Format);
    //    request.Headers.Add("x-ms-version", msVersion);
    //    request.Headers.Add("Authorization", authorizationHeader);

    //    ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

    //    using (Stream requestStream = request.BeginGetRequestStream())
    //    {
    //        requestStream.Write(blobContent, 0, blobLength);
    //        yield return requestStream;
    //    }

    //    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
    //    {
    //        String ETag = response.Headers["ETag"];
    //    }
    //}

    private static void CopyStream(Stream input, Stream output)
    {
        byte[] buffer = new byte[32768];
        int read;
        while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
        {
            output.Write(buffer, 0, read);
        }
    }

    private static String CreateAuthorizationHeader(String canonicalizedString)
    {
        if (String.IsNullOrEmpty(canonicalizedString))
        {
            throw new ArgumentNullException("canonicalizedString");
        }

        String signature = CreateHmacSignature(canonicalizedString, AzureStorageConstants.Key);
        String authorizationHeader = String.Format(CultureInfo.InvariantCulture, "{0} {1}:{2}", AzureStorageConstants.SharedKeyAuthorizationScheme, AzureStorageConstants.Account, signature);

        return authorizationHeader;
    }

    private static String CreateHmacSignature(String unsignedString, Byte[] key)
    {
        if (String.IsNullOrEmpty(unsignedString))
        {
            throw new ArgumentNullException("unsignedString");
        }

        if (key == null)
        {
            throw new ArgumentNullException("key");
        }

        Byte[] dataToHmac = System.Text.Encoding.UTF8.GetBytes(unsignedString);
        using (HMACSHA256 hmacSha256 = new HMACSHA256(key))
        {
            return Convert.ToBase64String(hmacSha256.ComputeHash(dataToHmac));
        }
    }

    public static bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        bool isOk = true;
        // If there are errors in the certificate chain, look at each error to determine the cause.
        if (sslPolicyErrors != SslPolicyErrors.None)
        {
            for (int i = 0; i < chain.ChainStatus.Length; i++)
            {
                if (chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown)
                {
                    chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                    chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                    chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                    chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                    bool chainIsValid = chain.Build((X509Certificate2)certificate);
                    if (!chainIsValid)
                    {
                        isOk = false;
                    }
                }
            }
        }
        return isOk;
    }

    #region Progress Event
    private float _progress;
    public event EventHandler<ProgressEventArgs> ProgressChanged;

    public float ProgressBar
    {
        get { return _progress; }
        set
        {
            _progress = value;
            ProgressEventArgs e = new ProgressEventArgs { Progress = value };
            OnProgressChanged(e);
        }
    }

    public class ProgressEventArgs : EventArgs
    {
        public float Progress { get; set; }
    }

    protected virtual void OnProgressChanged(ProgressEventArgs e)
    {
        if (ProgressChanged != null)
            ProgressChanged(this, e);
    }
    #endregion
}
