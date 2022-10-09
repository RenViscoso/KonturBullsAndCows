using System;
using System.Collections.Generic;

namespace BullsAndCows
{
    class BullsAndCowsGame
    {
        // Переменная для режима отладки
        protected bool debug;
        // Переменная для определения, закончилась ли игра
        protected bool number_guessed;
        // Длина числа
        protected int number_length;
        // Хранилище всех введённых и выведенных строках
        protected List<string> log;
        // Генератор рандомных чисел
        protected Random random;
        // Загаданное число
        protected string hidden_number;
        // Команды для программы
        protected string help_command = "help";
        protected string log_command = "log";
        protected string exit_command = "exit";


        // Конструктор для смены параметров
        public BullsAndCowsGame(bool enable_debug, int desired_number_length)
        {
            debug = enable_debug;
            number_length = desired_number_length;
        }


        // Метод, который выводит строку в консоль и заносит её в лог
        protected void PrintAndLog(string input)
        {
            log.Add(input);
            Console.WriteLine(input);
        }


        // Метод для вывода лога в консоль
        protected void LogOutput()
        {
            Console.WriteLine("---LOG---START---");
            int log_size = log.Count;
            for (int index = 0; index < log_size; index += 1)
            {
                Console.WriteLine(log[index]);
            }
            Console.WriteLine("----LOG---END----");
        }


        // Метод для генерации случайного 4-значного числа без повторяющихся цифр
        protected string GetRandomNumber()
        {
            // Создание генератора случайных чисел
            if (random == null)
            {
                random = new Random();
            }

            // Создание числа по цифрам
            string result = "";
            bool done = false;
            int index = 0;
            while (!done)
            {
                string digit = random.Next(0, 10).ToString();

                if (!result.Contains(digit))
                {
                    // Первая цифра не может быть нулём
                    if (index == 0 && digit == "0")
                    {
                        continue;
                    }
                    result += digit;
                    index += 1;
                }

                if (index == number_length)
                {
                    done = true;
                }
            }

            return result;
        }


        // Метод для проверки количества цифр в числе
        protected bool CheckCorrectLenght(string input)
        {
            int lentgh = input.Length;
            if (lentgh != number_length)
            {
                return false;
            }

            return true;
        }


        // Метод для проверки числа на различие цифр
        protected bool CheckDifferentDigits(string input)
        {
            // Берём число и проверяем, встречается ли оно ещё раз
            for (int index = 0; index < number_length; index += 1)
            {
                char digit = input[index];
                if (input.IndexOf(digit, index + 1) != -1)
                {
                    return false;
                }
            }

            return true;
        }


        // Метод для вычисления количества быков и коров
        protected string GetBullsAndCows(string input)
        {
            int cows = 0;
            int bulls = 0;

            // Проверка каждой цифры числа
            for (int index = 0; index < number_length; index += 1)
            {
                char input_digit = input[index];

                // Проверка на наличие в числе
                if (!hidden_number.Contains(input_digit))
                {
                    continue;
                }

                // Проверка на совпадение индексов
                int hidden_digit_index = hidden_number.IndexOf(input_digit);
                if (index != hidden_digit_index)
                {
                    cows += 1;
                }
                else
                {
                    bulls += 1;
                }
            }

            string result;

            // Если все числа быки — значит, число угадано
            if (bulls == number_length)
            {
                number_guessed = true;
                result = "Число угадано!";
            }
            else
            {
                result = $"{bulls} быков, {cows} коров";
            }

            return result;
        }


        // Основной метод класса игры
        public void Start()
        {
            log = new List<string>();
            hidden_number = GetRandomNumber();

            // Строки, использующиеся по несколько раз:
            string help_line = $@"Ваша задача: угадать число, которое загадала программа. Для этого Вы должны ввести {number_length}-значное число БЕЗ повторяющихся цифр.
Программа даст Вам подсказку, указав количество 'коров' — цифр Вашего числа, которые есть загаданном числе, и 'быков' — цифр, которые есть в загаданом числе И находятся на том же месте.
Для получения данной подсказки введите команду '{help_command}', для вывода истории запросов и ответов введите команду '{log_command}', для выхода из игры введите команду '{exit_command}' (все команды пишутся без кавычек).";
            string please_line = $"Пожалуйста, введите число или команды '{help_command}', '{log_command}' или '{exit_command}' (без кавычек): ";

            Console.WriteLine("Добро пожаловать в игру 'Быки и Коровы'!");
            Console.WriteLine(help_line);

            // Вывод загаданного числа для отладки
            if (debug)
            {
                PrintAndLog($"Загаданное число: {hidden_number}");
            }

            Console.WriteLine(please_line);

            while (!number_guessed)
            {
                string input = Console.ReadLine();
                input = input.ToLower();

                // Проверка на команды
                if (input == help_command)
                {
                    Console.WriteLine(help_line);
                    Console.WriteLine(please_line);
                    continue;
                }
                else if (input == log_command)
                {
                    LogOutput();
                    Console.WriteLine(please_line);
                    continue;
                }
                else if (input == exit_command)
                {
                    Console.WriteLine($"Загаданное число было: {hidden_number}\nУдачи в следующий раз!");
                    number_guessed = true;
                    continue;
                }
                // Проверка на число и на количество цифр
                else if (int.TryParse(input, out _) && CheckCorrectLenght(input))
                {
                    // Проверка на разные цифры
                    if (!CheckDifferentDigits(input))
                    {
                        Console.WriteLine("В Вашем числе повторяются цифры!");
                        continue;
                    }

                    // Вычисление количества "коров" и "быков"
                    log.Add(input);
                    string result = GetBullsAndCows(input);
                    PrintAndLog(result);
                    continue;
                }

                // Ошибка, если ввод не подошёл
                Console.WriteLine($"Ваш ввод не является {number_length}-значным числом или командой!");
            }

            // Повторный вывод загаданного числа для отладки
            if (debug)
            {
                Console.WriteLine($"Загаданное число для сравнения: {hidden_number}");
            }

            // Очистка лога на всякий случай
            log.Clear();
        }
    }
}
