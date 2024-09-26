
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
NuGet\Install-Package GigaChatSDK -Version 1.1.0
```

> [!NOTE]  
>Пакет поддерживает .NET Standard 2.1

[GitVerse](https://gitverse.ru/who_is_likhoded/GigaChat.NET)

## Возможности

| Возможность | Статус |
|--------|--------|
|Авторизация|✔️|
|Получение списка моделей|✔️|
|Получение ответа от модели|✔️|
|Эмбеддинги(векторное представление текста)|✔️|
|Генерация изображений|✔️|
|Работа с пользовательскими [функциями](https://developers.sber.ru/docs/ru/gigachat/api/function-calling)|✔️|


# Начало работы

### Иницилизация:
```cs-sharp
static IHttpService httpService = new HttpService(ignoreTLS);
static ITokenService tokenService = new TokenService(httpService, "secretKey", isCommercial);

static IGigaChat Chat = new GigaChat(tokenService, httpService, saveImage);
```
### Получение токена:
```cs-sharp
await Chat.CreateTokenAsync());
```
### Отправка запроса к модели
Контекстозависимая отправка запроса
```cs-sharp
MessageQuery query = new MessageQuery();
query.messages.Add(new MessageContent("role", text));
await Chat.CompletionsAsync(new MessageQuery(Content));
```
Контекстонезависимая отправка запроса
```cs-sharp
await Chat.CompletionsAsync("[Запрос]");
```
### Создание встраивания:
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
### Получение идентификатора изображения
Используется для извлечения идентификатора изображения из сообщения. В качестве аргумента передается текст сообщения.
```cs-sharp
await Chat.GetFileId(string MessageContent);
```
## Примеры
### Пример получения строки ответа из отправленного запроса с использованием контекстозависимой перегрузки метода CompletionsAsync:
```cs-sharp
var response = await gigaChatClient.CompletionsAsync(query);
Console.WriteLine(response.choices.LastOrDefault().message.content);
```
`
### Пример получения изображения из отправленного запроса:
```cs-sharp
Response response = await Chat.CompletionsAsync("Нарисуй рыжего кота с зелеными глазами");
string messageTextResponse = response.choices.LastOrDefault().message.content;
if (Chat.GetFileId(messageTextResponse) != null)
{
    byte[] imageBytes = await Chat.GetImageAsByteAsync(Chat.GetFileId(messageTextResponse));
    Console.WriteLine("Идентификатор изображения: " + Chat.GetFileId(messageTextResponse));
}
else
{
    Console.WriteLine(response.choices.LastOrDefault().message.content);
}
```
> [!NOTE]  
> Каждый метод в качестве необязательных аргументов принимает стандартные значения из документации GigaChat API