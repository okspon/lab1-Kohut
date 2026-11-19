using System;
using IndependentWork7;

var cache = new Cache<string, string>(3);

Console.WriteLine("Додаємо 3 елементи (ємність кешу = 3):");
cache.Add("user_1", "Олександр");
cache.Add("user_2", "Марія");
cache.Add("user_3", "Іван");

Console.WriteLine($"Кількість елементів: {cache.Count}");
Console.WriteLine($"Отримуємо user_1: {cache.Get("user_1")}");

Console.WriteLine("\nДодаємо 4-й елемент (user_4). Найстаріший (user_1) має видалитися:");
cache.Add("user_4", "Олена");

Console.WriteLine($"Кількість елементів: {cache.Count}");
Console.WriteLine($"Чи є user_1 у кеші? {cache.ContainsKey("user_1")}");
Console.WriteLine($"Чи є user_4 у кеші? {cache.ContainsKey("user_4")}");

try
{
    Console.WriteLine(cache.Get("user_1"));
}
catch (KeyNotFoundException ex)
{
    Console.WriteLine($"Очікувана помилка: {ex.Message}");
}