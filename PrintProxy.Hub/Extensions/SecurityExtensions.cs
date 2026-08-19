using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Tokens;

namespace PrintProxy.Hub.Extensions;

public static class SecurityExtensions
{

    public static WebApplicationBuilder AddSelfSignedSslCert(this WebApplicationBuilder builder)
    {
        string certLocation = "SelfSignedCert.pfx";
        
        X509Certificate2 cert = null!;
        
        if (File.Exists(certLocation))
        { 
            cert = X509CertificateLoader.LoadCertificate(File.ReadAllBytes(certLocation));
        }
        else
        {
            #region Create Self Signed Certificate

            using RSA parent = RSA.Create(4096);
            using RSA rsa = RSA.Create(4096);
            CertificateRequest parentReq = new CertificateRequest("CN=PrintProxy Issuing Authority", parent,
                HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            parentReq.CertificateExtensions.Add(new X509BasicConstraintsExtension(true, false, 0, true));
            parentReq.CertificateExtensions.Add(new X509SubjectKeyIdentifierExtension(parentReq.PublicKey, false));

            using X509Certificate2 parentCert = parentReq.CreateSelfSigned(
                DateTimeOffset.UtcNow.AddDays(-45),
                DateTimeOffset.UtcNow.AddYears(20));

            CertificateRequest req = new CertificateRequest(
                "CN=PrintProxy",
                rsa,
                HashAlgorithmName.SHA256,
                RSASignaturePadding.Pkcs1);

            req.CertificateExtensions.Add(new X509BasicConstraintsExtension(false, false, 0, false));
            req.CertificateExtensions.Add(
                new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.NonRepudiation,
                    false));
            req.CertificateExtensions.Add(new X509EnhancedKeyUsageExtension(
                new OidCollection
                {
                    new Oid("1.3.6.1.5.5.7.3.8"),
                    new Oid("1.3.6.1.5.5.7.3.1")
                }, true));

            req.CertificateExtensions.Add(new X509SubjectKeyIdentifierExtension(req.PublicKey, false));
            
            cert = req.Create(parentCert, DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddYears(10),
                new byte[] { 1, 2, 3, 4 });
            
            cert = cert.CopyWithPrivateKey(rsa);
            
            byte[] pfxData = cert.Export(X509ContentType.Pfx);

            File.WriteAllBytes(certLocation, pfxData);

            #endregion

            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                serverOptions.ListenAnyIP(8080);
                serverOptions.ListenAnyIP(8081, listenOptions => listenOptions.UseHttps(certLocation));
            });
        }

        return builder;
    }
}
    
