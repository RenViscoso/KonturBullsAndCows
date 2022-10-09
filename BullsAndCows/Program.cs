using System;
using System.Collections.Generic;

namespace BullsAndCows
{
    class Program
    {


        // Метод для окончания программы
        protected static void GoodBye()
        {
            Console.WriteLine("Программа завершила свою работу. Нажмите клавишу Enter для продолжения...");
            Console.ReadLine();
        }


        // Главный метод программы
        public static void Main(string[] args)
        {
            // Аргументы для запуска программы
            List<string> help_args = new List<string>(){ "-h", "-help" };
            List<string> debug_args = new List<string>() { "-d", "-debug" };
            List<string> length_args = new List<string>() { "-l", "-length" };

            // Флаги для особенностей работы игры
            bool debug = false;
            int number_length = 4;

            // Проверка аргументов
            int args_number = args.Length;
            for (int index = 0; index < args_number; index += 1)
            {
                string arg = args[index].ToLower();

                // Аргумент "помощь"
                if (help_args.Contains(arg))
                {
                    Console.WriteLine($"----Commands----\n-{help_args[0]} or {help_args[1]} — show help\n{debug_args[0]} or {debug_args[1]} — start in debug mode\n{length_args[0]} <number> or {length_args[1]} <number> — change game number length");
                    GoodBye();
                    return;
                }
                // Аргумент "отладка"
                else if (debug_args.Contains(arg))
                {
                    debug = true;
                }
                // Аргумент "длина числа"
                else if (length_args.Contains(arg))
                {
                    index += 1;
                    try
                    {
                        number_length = int.Parse(args[index]);
                    }
                    catch (Exception err)
                    {
                        Console.WriteLine($"Bad length argument: {err.Message}");
                        GoodBye();
                        return;
                    }
                }
                // Ошибка в аргументах
                else
                {
                    Console.WriteLine($"Bad argument: '{arg}'\nTry to use -help for more information");
                    GoodBye();
                    return;
                }
            }

            // Запуск игры
            BullsAndCowsGame game = new BullsAndCowsGame(debug, number_length);
            game.Start();
            
            // Окончание программы
            GoodBye();
        }
    }
}
