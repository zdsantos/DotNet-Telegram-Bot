using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace DotNetTelegramBot.Services
{
    public class TelegramService
    {
        public TelegramService(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("Telegram bot token not provided!");

            Client = new TelegramBotClient(token);

            Client.SetMyCommandsAsync(new List<BotCommand>()
            {
                new BotCommand()
                {
                        Command = "teste",
                        Description = "Comando de teste"
                }
            }).Wait();
        }

        public async Task EchoAsync(Update update)
        {
            if (update.Type != UpdateType.Message)
                return;

            var message = update.Message;

            //await GetUserProfilePhotos(message);

            switch (message.Type)
            {
                case MessageType.Text:
                    switch (message.Text)
                    {
                        case "imagem":
                            var photoInput = new Telegram.Bot.Types.InputFiles.InputOnlineFile("https://cdn.shortpixel.ai/client/q_glossy,ret_img/https://chatbotmaker.io/wp-content/uploads/Ativo-3.png");
                            await Client.SendPhotoAsync(message.Chat.Id, photoInput);
                            break;

                        case "contato":
                            await Client.SendContactAsync(message.Chat.Id, "55 85 99999-9999", "Fulano", "de Tal", replyToMessageId: message.MessageId);
                            break;

                        case "digitando":
                            await Client.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
                            await Task.Delay(2000);

                            await Client.SendTextMessageAsync(message.Chat.Id, message.Text);
                            break;

                        case "teclado":
                            var customKeyboard = new ReplyKeyboardMarkup()
                            {
                                OneTimeKeyboard = true,
                                Keyboard = new List<List<KeyboardButton>>()
                                {
                                    new List<KeyboardButton>()
                                    {
                                        new KeyboardButton("Botão 1"),
                                        new KeyboardButton("Botão 2")
                                    },
                                    new List<KeyboardButton>()
                                    {
                                        new KeyboardButton("Botão 3")
                                    },
                                }
                            };

                            await Client.SendTextMessageAsync(message.Chat.Id, "teclado", replyMarkup: customKeyboard);
                            break;

                        default:
                            await Client.SendTextMessageAsync(message.Chat.Id, message.Text);
                            break;
                    }
                    break;

                case MessageType.Photo:
                    var fileId = message.Photo.LastOrDefault()?.FileId;
                    var file = await Client.GetFileAsync(fileId);

                    var filename = file.FileId + "." + file.FilePath.Split('.').Last();
                    using (var saveImageStream = System.IO.File.Open(filename, FileMode.Create))
                    {
                        await Client.DownloadFileAsync(file.FilePath, saveImageStream);
                    }

                    await Client.SendTextMessageAsync(message.Chat.Id, "Vlw pela foto!");
                    break;

                case MessageType.GroupCreated:
                    await Client.SendTextMessageAsync(message.Chat.Id, "E aí galerinha do mal!");
                    break;
            }
        }

        public async Task GetUserProfilePhotos(Message message)
        {
            var photos = await Client.GetUserProfilePhotosAsync(message.From.Id);

            var fileId = photos.Photos.LastOrDefault().LastOrDefault().FileId;
            var file = await Client.GetFileAsync(fileId);

            var filename = file.FileId + "." + file.FilePath.Split('.').Last();
            using (var saveImageStream = System.IO.File.Open(filename, FileMode.Create))
            {
                await Client.DownloadFileAsync(file.FilePath, saveImageStream);
            }
        }

        public async Task SetWebHook()
        {
            await Client.SetWebhookAsync("https://telegrambot.conveyor.cloud/api/webhook/");
        }

        public TelegramBotClient Client;
    }
}
