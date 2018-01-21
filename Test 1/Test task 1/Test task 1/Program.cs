/*
Контекст: для реализации определённого алгоритма машинного обучения вам нужно найти минимальное число в массиве чисел.Массив чисел – это 10 000 000 элементов типа double.
Постановка задачи:
1)  Решите задачу традиционным(однопоточным) способом.Массив данных сгенерируйте случайным образом.Замерьте производительность традиционного способа.
2)  Напишите код, который решает задачу поиска минимального элемента про помощи потоков(Thread). Исходный массив разбивается на равные по длине части для поиска.К-во частей = к - во процессорных ядер на машине. Получили ли вы выгоду в производительности в сравнение с однопоточным способом?
3)  Выясните опытным путём, начиная с какой длины массива выгоднее использовать многопоточную версию(очевидно, что для «маленьких» массивов с потоками можно не заморачиваться)
*/


using System;

namespace Test_task_1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var threadCount = 4;

            var math = new Math(threadCount, 100000000);

            TimeSpan searchTimeExecutionSync;
            TimeSpan searchTimeExecutionAsync;
            try
            {
                searchTimeExecutionSync = math.GetMinimumValueSearchTimeSync();
            }
            catch
            {
                throw new Exception("something wrong with sync search, dude!");
            }

            try
            {
                searchTimeExecutionAsync = math.GetMinimumValueSearchTimeAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($@"something wrong with async search, dude! {ex}");
            }

            Console.WriteLine($@"sync search time is: {searchTimeExecutionSync}");
            Console.WriteLine($@"min value is {math.MinimumValueSync}");
            Console.WriteLine("================================================");
            Console.WriteLine($@"sync search time is: {searchTimeExecutionAsync}");
            Console.WriteLine($@"min value is {math.MinimumValueAsync}");

            Console.ReadKey();
        }
    }
}