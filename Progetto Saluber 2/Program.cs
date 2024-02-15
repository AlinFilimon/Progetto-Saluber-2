using System;
using System.Net.Http;
using System.Text.Json;
using System.Net.Mail;

class Program
{
    static void Main(string[] args)
    {
        // Imposta l'URL del web service
        string url = "https://clienti.saluber.it/api/stock/getLowAvailableItems";
        ProvaGit
        // Esegui la chiamata al web service
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                string json = response.Content.ReadAsStringAsync().Result;

                // Deserializza il JSON
                var articoli = JsonSerializer.Deserialize<Articolo[]>(json);

                // Invia la notifica email
                InviaEmail(articoli);
            }
            else
            {
                Console.WriteLine("Errore durante la chiamata al web service: " + response.StatusCode);
            }
        }
    }

    static void InviaEmail(Articolo[] articoli)
    {
        string smtpServer = "smtp.gmail.com";
        int smtpPort = 587;
        string smtpUsername = "Username";
        string smtpPassword = "Password";
        string emailMittente = "noreply@saluber.it";
        string emailDestinatario = "alinfilimon2003@gmail.com";

        // Crea il messaggio dell'email
        MailMessage messaggio = new MailMessage(emailMittente, emailDestinatario);
        messaggio.Subject = "Articoli con scorte basse";
        messaggio.Body = "Attenzione, i seguenti articoli hanno scorte basse:\n\n";
        foreach (Articolo articolo in articoli)
        {
            messaggio.Body += $" - {articolo.Nome} ({articolo.Codice}) - Scort: {articolo.Scorta}\n";
        }

        // Invio l'email
        using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
        {
            smtpClient.Credentials = new System.Net.NetworkCredential(smtpUsername, smtpPassword);
            smtpClient.EnableSsl = true;
            smtpClient.Send(messaggio);
        }

        Console.WriteLine("Email inviata correttamente!");
    }
}

public class Articolo
{
    public string Nome { get; set; }
    public string Codice { get; set; }
    public int Scorta { get; set; }
}