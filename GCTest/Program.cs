using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using LikhodedDynamics.Sber.GigaChatSDK;
using LikhodedDynamics.Sber.GigaChatSDK.Models;
using System.Reflection;

class Program
{
    static ITelegramBotClient bot = new TelegramBotClient("6721597298:AAG2HMrLd9RTKaSDHwPqjZh4nwIpwht-L6E"); // Токен бота из BotFather
    public static GigaChat Chat = new GigaChat("MTJkZmRmNTEtMGMzNy00ZDViLWE1NzEtNmM0MTUzYzRkZWUwOjhhZGUwYWIzLTE2OTQtNDA5ZC04M2ZiLWEyYjZjNDcwM2VmYg==", false, true);
    public enum DATE
    {
        TODAY,
        TOMORROW,
        YESTERDAY
    };
    List<string> osk = new List<string>();
    public static async Task HandleMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
        // Only process Message updates: https://core.telegram.org/bots/api#message
        if (update.Message is not { } message)
            return;
        // Only process text messages
        if (message.Text is not { } messageText)
            return;
        var chatId = message.Chat.Id;

        Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
        List<MessageContent> content = new List<MessageContent> { new MessageContent("system","Ты продавец обуви, втоя задача продавать обувь"), new MessageContent("user", messageText) };
        Response response = Chat.CompletionsAsync(new MessageQuery(content)).Result;
        string messageTextResponse;
        if(response != null)
        {
            messageTextResponse = response.choices.LastOrDefault().message.content;
        }
        else
        {
            messageTextResponse = "Повторите запрос. Токен не авторизован.";
        }
        // Echo received message text
        await botClient.SendTextMessageAsync(
        chatId: chatId,
            text: messageTextResponse,
            cancellationToken: cancellationToken);
        /*
        if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
        {
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {

                var message = update.Message;
                String queryString = "";

                if (message.Text != null)
                {
                    if (message.Text.ToLower() == "/start")
                    {
                        // Тут можно описать какую-то логику для приветствия, или произвести
                        // инициализацию бота, если такая требуется
                        await botClient.SendTextMessageAsync(message.Chat, "Добро пожаловать, полосатый");
                        return;
                    }
                    if (message.Text.ToLower() == "/gulyat")
                    {
                        Random rnd = new System.Random();
                        int rndResult = rnd.Next(rnd.Next(0, 100));

                        if (rndResult <= 40)
                        {
                            var days = Enumerable.Range(1, 7).ToArray();
                            var times = new List<string>() { "06:00",
                                "07:00",
                                "08:00",
                                "09:00",
                                "10:00", "11:00", "12:00", "13:00", "14:00", "15:00" };


                            Random rndTime = new System.Random();
                            var randomTimeResult = rndTime.Next(0, 23);
                            Random rndDay = new System.Random();
                            int rndDayResult = rndDay.Next(1, 7);
                            string day = "";
                            switch (rndDayResult)
                            {
                                case 1:
                                    {
                                        day = "Понедельник";
                                        break;
                                    }
                                case 2:
                                    {
                                        day = "Вторник";
                                        break;
                                    }
                                case 3:
                                    {
                                        day = "Среду";
                                        break;
                                    }
                                case 4:
                                    {
                                        day = "Четверг";
                                        break;
                                    }
                                case 5:
                                    {
                                        day = "Пятницу";
                                        break;
                                    }
                                case 6:
                                    {
                                        day = "Субботу";
                                        break;
                                    }
                                case 7:
                                    {
                                        day = "Воскресенье";
                                        break;
                                    }
                            }
                            await botClient.SendTextMessageAsync(message.Chat, $"Пойдете гулять в {day} в {randomTimeResult}:00");
                            return;
                        }
                        if (rndResult > 40 && rndResult < 70)
                        {
                            await botClient.SendTextMessageAsync(message.Chat, " через час вы уже должны встретиться нахуй");
                            return;
                        }
                        if(rndResult >= 70)
                        {
                            await botClient.SendTextMessageAsync(message.Chat, "Все идут гулять, когда хотят, гриша идет нахуй");
                        }
                        //await SendReplyKeyboard(botClient, message, cancellationToken, 14);
                        return;
                    }
                    if (message.Text.ToLower() == "/delat")
                    {
                        Random rndTime = new System.Random();
                        var randomTimeResult = rndTime.Next(0, 23);
                        Random rndDay = new System.Random();
                        int rndDayResult = rndDay.Next(1, 7);
                        string day = "";
                        switch (rndDayResult)
                        {
                            case 1:
                                {
                                    day = "Гулять";
                                    break;
                                }
                            case 2:
                                {
                                    day = "Сидеть в маке";
                                    break;
                                }
                            case 3:
                                {
                                    day = "Сидеть в гранте";
                                    break;
                                }
                            case 4:
                                {
                                    day = "Проходить тест на тупого";
                                    break;
                                }
                            case 5:
                                {
                                    day = "Проходить тест на умного";
                                    break;
                                }
                            case 6:
                                {
                                    day = "Поехать есть";
                                    break;
                                }
                            case 7:
                                {
                                    day = "Ебать гришу";
                                    break;
                                }
                        }
                        await botClient.SendTextMessageAsync(message.Chat, day);
                        return;
                    }
                    if (message.Text.ToLower() == "/test")
                    {
                        Random rndTime = new System.Random();
                        var randomTimeResult = rndTime.Next(0, 23);
                        Random rndDay = new System.Random();
                        int rndDayResult = rndDay.Next(1, 7);
                        string day = "";
                        switch (rndDayResult)
                        {
                            case 1:
                                {
                                    day = "Ты тупой";
                                    break;
                                }
                            case 2:
                                {
                                    day = "Ты пизда тупой";
                                    break;
                                }
                            case 3:
                                {
                                    day = "Ты мега пизда тупой";
                                    break;
                                }
                            case 4:
                                {
                                    day = "Ты пизда супер мега тупой";
                                    break;
                                }
                            case 5:
                                {
                                    day = "Мега умный(как таджик, который снимает тест на тупого)";
                                    break;
                                }
                            case 6:
                                {
                                    day = "Почти не тупой";
                                    break;
                                }
                            case 7:
                                {
                                    day = "Гений";
                                    break;
                                }
                        }
                        await botClient.SendTextMessageAsync(message.Chat, message.From.FirstName +", твой результат теста на тупого: " + day);
                        return;
                    }
                    queryString = message.Text;// Если написать название песни текстом, вместо того,
                                               // чтобы переслать ее. Это должно тоже работать. Поэтому мы отслеживаем текст,
                                               // который присылают боту
                }

                if (message.Audio != null) //если это музыка, берем имя автора и песни
                {
                    Audio audio = message.Audio;

                    queryString = audio.Performer;
                    queryString += " - " + audio.Title;
                }
                return;
            }

        }*/
    }

    public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        // Некоторые действия
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
    }


    async static Task Main(string[] args)
    {
        Console.WriteLine($"Bot has been started: {bot.GetMeAsync().Result.FirstName}");

        var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = { },
        };

        bot.StartReceiving(
            HandleMessageAsync,
            HandleErrorAsync,
            receiverOptions,
            cancellationToken
        );
        await Task.Run(() => Chat.CreateTokenAsync());
        Console.WriteLine("TOKEN: " + Chat.Token.AccessToken);
        Console.ReadLine();
    }

    static async Task<Telegram.Bot.Types.Message> SendReplyKeyboard(ITelegramBotClient botClient, Telegram.Bot.Types.Message message, CancellationToken cancellationToken, int _boardID)
    {
        return await botClient.SendTextMessageAsync(message.Chat, "Выберите раздел");
        // _boardID - ID клавиши в меню бота.
        // Для обработки нажатия на клавишу необходимо вызвать SendReplyKeyboard(botClient, message, cancellationToken, _boardID), где _boardID - ID клавиши
        /*switch (_boardID)
        {
            // _boardID раздела "Новости"
            case 1:
                {
                    List<string> titles = await ParserController.ParserTitles(1);
                    int iter = 0;
                    foreach (var title in titles)
                    {
                        await botClient.SendTextMessageAsync(message.Chat, title);
                        iter++;
                        if (iter == 10)
                        {
                            break;
                        }
                    }
                    return await botClient.SendTextMessageAsync(message.Chat, "Выберите раздел");

                }

            default: // Вызывается при несуществующем ID
                {
                    ReplyKeyboardMarkup replyKeyboardMarkup = new(
                    new[]
                    {
                            new KeyboardButton[] { "Новости", "Спорт" },
                    })
                    {
                        ResizeKeyboard = true
                    };
                    return await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Выберите раздел",
                        replyMarkup: replyKeyboardMarkup,
                        cancellationToken: cancellationToken);
                }
        }*/
    }


    // Закрытие клавиатуры
    static async Task<Telegram.Bot.Types.Message> RemoveKeyboard(ITelegramBotClient botClient, Telegram.Bot.Types.Message message, CancellationToken cancellationToken)
    {
        return await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Removing keyboard",
            replyMarkup: new ReplyKeyboardRemove(),
            cancellationToken: cancellationToken);
    }
}
