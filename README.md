

<h1 align="center">
  <br>
  <a href="https://github.com/likhoded/GigaChat-.NET"><img src="https://github.com/likhoded/GigaChatSDK/blob/master/Gigachat_Sber.png?raw=true" alt=".NET GigaChat"></a>
</h1>

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
  
**.NET GigaChat** - это библиотека .NET для работы с ИИ от **Сбера**, который способен вести диалог с пользователем, писать код, создавать тексты по запросу пользователя.


<p align="right">
<sub>(Preview)</sub>
</p>

</td>
</tr>
</table>

## Установка

##### Установка:
Вы можете установить пакет, используя **[NuGet](https://www.nuget.org/packages/GigaChatSDK/)**

```bash
NuGet\Install-Package LikhodedDynamics.Sber.GigaChatSDK -Version 1.0.0
```

> [!NOTE]  
>Пакет поддерживает .NET Standard 2.1

## Возможности

| Возможность | Статус |
|--------|--------|
|Авторизация|✔️|
|Получение списка моделей|✔️|
|Получение ответа от модели|✔️|
|Поддержка контекста, из SDK|❌ |
> [!NOTE]  
>Вы можете самостоятельно реализовать поддержку контекста, используя возможности библиотеки. В будущем планируется дополнительно добавить возможность работать с контекстом нативно, из библиотеки

## Использование

Иницилизация:
```cs-sharp
public static GigaChat Chat = new GigaChat("Ваши авторизационные данные", IsCommercial, IgnoreTLS);
```
Авторизация:
```cs-sharp
await Chat.CreateTokenAsync());
```
Отправка запроса к модели
```cs-sharp
await Chat.CompletionsAsync(new MessageQuery(content)).Result;
```
Пример получения строки ответа из отправленного запроса:
```cs-sharp
Response response = Chat.CompletionsAsync(new MessageQuery(content)).Result;
string messageTextResponse = response.choices.LastOrDefault().message.content;
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
  
**.NET GigaChat** is a library .NET for working with the service from the **Sber**, which is able to interact with the user in a dialog format, write code, create texts at the user's request.


<p align="right">
<sub>(Preview)</sub>
</p>

</td>
</tr>
</table>

## Installation

##### Downloading and installing steps:
You can install the package using **[NuGet](https://)**

```bash
NuGet\Install-Package LikhodedDynamics.Sber.GigaChatSDK -Version 1.0.0
```

> [!NOTE]  
>The package supports .NET Standard 2.1

## Features

| Feature | Status |
|--------|--------|
|Authorization|✔️|
|Getting a list of models|✔️|
|Getting a response from the model|✔️|
|Support model context|✔️|

## Usage
Creating an object:
```cs-sharp
public static GigaChat Chat = new GigaChat("Authorization data", IsCommercial, IgnoreTLS);
```
Create Token
```cs-sharp
await Chat.CreateTokenAsync());
```
Send message to model
```cs-sharp
await Chat.CompletionsAsync(new MessageQuery(content)).Result;
```
> [!NOTE]
> > Each method takes standard values from the GigaChat API documentation as optional arguments
