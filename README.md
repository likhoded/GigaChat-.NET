
![alt GigaChat.NET](Gigachat_Sber.png "GigaChat.NET")

<h4 align="center">GigaChat-.NET.</h4>
      
<p align="center">
  <a href="#installation">Установка</a> •
  <a href="#features">Возможности</a> •
  <a href="#usage">Начало работы</a> •
  <a href="#usage">Примеры</a> •
</p>

---

<table>
<tr>
<td>
  
**.NET GigaChat** - это библиотека .NET для работы с ИИ от **Сбера**, который способен вести диалог с пользователем, писать код, создавать тексты и генерировать изображения прямо в ходе диалога.

Если данный репозиторий оказался полезным для вас, не забудьте поставить ⭐
<p align="right">
<sub>(Preview)</sub>
</p>

</td>
</tr>
</table>

## Установка

[Перед началом работы рекомендуем ознакомиться с документацией по API](https://developers.sber.ru/docs/ru/gigachat/api)

Вы можете установить пакет, используя **[NuGet](https://www.nuget.org/packages/GigaChatSDK/1.0.5)** 
```bash
NuGet\Install-Package GigaChatSDK -Version 1.0.5
```

> [!NOTE]  
>Пакет поддерживает .NET Standard 2.1

## Возможности

| Возможность | Статус |
|--------|--------|
|Авторизация|✔️|
|Получение списка моделей|✔️|
|Получение ответа от модели|✔️|
|Эмбеддинги(векторное представление текста)|✔️|
|Генерация изображений|✔️|
|Поддержка контекста, из SDK|❌ |

> [!NOTE]  
>Вы можете самостоятельно реализовать поддержку контекста, используя возможности библиотеки. В будущем планируется дополнительно добавить возможность работать с контекстом нативно, из библиотеки

# Начало работы

### Иницилизация:
```cs-sharp
public static GigaChat Chat = new GigaChat("Ваши авторизационные данные", IsCommercial, IgnoreTLS, SaveImage);
```
### Получение токена:
```cs-sharp
await Chat.CreateTokenAsync());
```
### Отправка запроса к модели
Контекстозависимая отправка запроса
```cs-sharp
await Chat.CompletionsAsync(new MessageQuery(Content)).Result;
```
Контекстонезависимая отправка запроса
```cs-sharp
await Chat.CompletionsAsync("[Запрос]").Result;
```
### Создание эмбеддинга:
```cs-sharp
await Chat.EmbeddingAsync(EmbeddingRequest Request);
```
### Получение изображения по идентификатору:
Возвращает файл изображения в бинарном представлении, в формате JPG. Для включения сохранения изображения, необходимо в окнтрукторе указать SaveImage = true. По-умолчанию изображение сохраняется в директории проекта, изменение директории в SaveDirectory.
```cs-sharp
await Chat.GetImageAsByteAsync(string fileId);
```
### Получение списка моделей:
```cs-sharp
await Chat.ModelsAsync();
```
### Получение списка моделей
Используется для извлечения идентификатора изображения из сообщения. В качестве аргумента передается текст сообщения.
```cs-sharp
await Chat.GetFileId(string MessageContent);
```
## Примеры
### Пример получения строки ответа из отправленного запроса с использованием контекстозависимой перегрузки метода CompletionsAsync:
```cs-sharp
Response response = Chat.CompletionsAsync(new MessageQuery(content)).Result;
string messageTextResponse = response.choices.LastOrDefault().message.content;
```
### Пример получения строки ответа из отправленного запроса с использованием контекстонезависимой перегрузки метода CompletionsAsync:
```cs-sharp
Response response = Chat.CompletionsAsync("Расскажи о себе").Result;
string messageTextResponse = response.choices.LastOrDefault().message.content;
// content = "Я нейросетевая модель GigaChat от Сбера."
```
### Пример получения изображения из отправленного запроса:
```cs-sharp
Response response = Chat.CompletionsAsync("Нарисуй рыжего кота с зелеными глазами").Result;
string messageTextResponse = response.choices.LastOrDefault().message.content;
if (Chat.GetFileId(messageTextResponse) != null)
{
    byte[] imageBytes = await Chat.GetImageAsByteAsync(Chat.GetFileId(messageTextResponse));
    Console.WriteLine("Идентификатор изображения: " + Chat.GetFileId(messageTextResponse));
}
else
{
    await botClient.SendTextMessageAsync(chatId, response.choices.LastOrDefault().message.content);
}
```
> [!NOTE]  
> Каждый метод в качестве необязательных аргументов принимает стандартные значения из документации GigaChat API


<h4 align="center">GigaChat-.NET.</h4>
      
<p align="center">
  <a href="#installation">Installation</a> •
  <a href="#features">Features</a> •
  <a href="#usage">Usage</a> •
</p>

---

<table>
<tr>
<td>
  
**.NET GigaChat** is a library .NET for working with the service from the **Sber**, which is able to conduct a dialogue with the user, write code, create texts and generate images directly during the dialogue.


<p align="right">
<sub>(Preview)</sub>
</p>

</td>
</tr>
</table>

## Installation

##### Downloading and installing steps:
You can install the package using **[NuGet](https://www.nuget.org/packages/GigaChatSDK/1.0.5)** 

```bash
NuGet\Install-Package GigaChatSDK -Version 1.0.5
```

> [!NOTE]  
>The package supports .NET Standard 2.1

## Features

| Opportunity | Status |
|--------|--------|
|Authorization| ✔️ |
|Getting a list of models| ✔️ |
|Getting a response from the model| ✔️ |
|Embedding (vector representation of text)| ✔️ |
|Image generation| ✔️ |
|Context support, from the SDK| ❌ |

# Getting started

### Initialization:
```cs-sharp
public static GigaChat Chat = new GigaChat("Your authorization data", IsCommercial, IgnoreTLS, SaveImage);
```
### Getting a token:
```cs-sharp
await Chat.CreateTokenAsync());
```
### Sending a request to the model
Context-dependent sending of a request
```cs-sharp
await Chat.CompletionsAsync(new MessageQuery(Content)).Result;
```
Context-independent sending of a request
```cs-sharp
await Chat.CompletionsAsync("[Request]").Result;
```
### Embedding creation:
```cs-sharp
await Chat.EmbeddingAsync(EmbeddingRequest Request);
```
### Getting an image by ID:
Returns an image file in binary representation, in JPG format. To enable image saving, you must specify SaveImage = true in the window constructor. By default, the image is saved in the project directory, changing the directory to SaveDirectory.
```cs-sharp
await Chat.GetImageAsByteAsync(string fileId);
```
### Getting a list of models:
```cs-sharp
await Chat.ModelsAsync();
```
### Getting a list of models
Used to extract the image ID from the message. The text of the message is passed as an argument.
```cs-sharp
await Chat.GetFileId(string MessageContent);
```
## Examples
### Example of getting a response string from a sent request using a context-dependent overload of the CompletionsAsync method:
```cs-sharp
Response response = Chat.CompletionsAsync(new MessageQuery(content)).Result;
string messageTextResponse = response.choices.LastOrDefault().message.content;
```
### Example of getting a response string from a sent request using a context-independent overload of the CompletionsAsync method:
```cs-sharp
Response response = Chat.CompletionsAsync("Tell me about yourself").Result;
string messageTextResponse = response.choices.LastOrDefault().message.content;
// content = "I am a GigaChat neural network model from Sber."
```
### Example of getting an image from a sent request:
```cs-sharp
Response response = Chat.CompletionsAsync ("Draw a red cat with green eyes").Result;
string messageTextResponse = response.choices.LastOrDefault().message.content;
if (Chat.GetFileId(messageTextResponse) != null)
{
byte[] imageBytes = await Chat.GetImageAsByteAsync(Chat.GetFileId(messageTextResponse));
Console.WriteLine("Image ID: " + Chat.GetFileId(messageTextResponse));
}
else
{
await botClient.SendTextMessageAsync(chatId, response.choices.LastOrDefault().message.content);
}
```
> [!NOTE]
> > Each method takes standard values from the GigaChat API documentation as optional arguments
