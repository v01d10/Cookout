using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class SendEmail : MonoBehaviour {
    public string fromAddress;
    public string smtpServer;
    public string smtpPassword;

    private MailMessage mail = new MailMessage();

    void SendEmails() {
        mail.From = new MailAddress("GIVE_YOUR_EMAIL_HERE");
        mail.To.Add("GIVE_YOUR_DESTINATION_HERE");

        SmtpClient smtpServer = new SmtpClient("GIVE_SMTP_INFO_HERE");
        smtpServer.Port = 587;//GIVE CORRECT PORT HERE
        mail.Subject = "WHATEVER_YOU_WANT_TEXT";
        mail.Body = "WHATEVER_YOU_WANT_TEXT";
        smtpServer.Credentials = new System.Net.NetworkCredential("GIVE_SMTP_INFO_HERE", "GIVE_YOUR_EMAIL_PASSWORD_HERE") as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
        delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
        smtpServer.Send(mail);
        //smtpServer.SendAsync(mail)
        Debug.Log("success");
    }
}
