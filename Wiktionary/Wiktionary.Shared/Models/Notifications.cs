using System;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Networking.PushNotifications;
using Windows.UI.Popups;
using Microsoft.WindowsAzure.Messaging;

//Ce fichier permet d'utiliser le système de notification

namespace Wiktionary.Models
{
    public class Notifications
    {
        public void StartListening()
        {
            string hubName = "wiktionaryhub";
            string  endPoint = "Endpoint=sb://wiktionary.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=sWROQRh/lq7vChsrKn/6Lfb3VqRU+dyGI0tmbsvhiCI=";

            NotificationHub hub = new NotificationHub(hubName, endPoint);

            // Execution du register en asynchrone
            Task.Run(async () =>
            {
                var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
                await hub.RegisterNativeAsync(channel.Uri);
            });
        }

        //Permet d'afficher en popup une notification (mot + définition)
        public async void AfficherNotification(string mot)
        {
            string definition;

            Webservices ws = new Webservices();

            string response = await ws.GetDefinition(mot);

            if (!response.Equals(""))
            {
                JsonObject jsonObject = JsonObject.Parse(response);

                definition = jsonObject.GetNamedString("Definition");

                MessageDialog msgDialog = new MessageDialog("Le mot ajouté est " + mot + " : " + definition, "Information");
                await msgDialog.ShowAsync();
            }  
        }
    }
}
