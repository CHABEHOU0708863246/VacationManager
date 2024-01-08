
using System.Net.Mail;
using System.Net;

namespace VacationManager.Domain.Tools
{
    public static class EmailTool
    {
        // Configuration de l'envoi d'email (à adapter à votre configuration SMTP)
        private const string SmtpServer = "smtp.example.com";
        private const int SmtpPort = 587; // Port SMTP (peut varier selon le fournisseur)
        private const string SmtpUsername = "your_username";
        private const string SmtpPassword = "your_password";

        public static void SendResetPasswordEmail(string email, string resetToken)
        {
            try
            {
                // Création de l'objet MailMessage
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("from@example.com"); // Adresse expéditeur
                mail.To.Add(email); // Adresse destinataire
                mail.Subject = "Réinitialisation de mot de passe"; // Objet du mail
                mail.Body = $"Bonjour,\n\nVous avez demandé une réinitialisation de mot de passe. Veuillez utiliser ce token pour réinitialiser votre mot de passe : {resetToken}"; // Corps du mail

                // Configuration du client SMTP
                SmtpClient smtpClient = new SmtpClient(SmtpServer, SmtpPort);
                smtpClient.EnableSsl = true; // Utilisation du SSL
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(SmtpUsername, SmtpPassword);

                // Envoi du mail
                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                // Gestion des exceptions liées à l'envoi d'email
                Console.WriteLine($"Erreur lors de l'envoi de l'email : {ex.Message}");
                throw;
            }
        }
    }

}
