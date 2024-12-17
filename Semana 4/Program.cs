using System;
using System.Diagnostics.Metrics;
using System.Threading;

class Program
{
    static Meter s_meter = new Meter("HatCo.Store");
    static Counter<int> s_hatsSold = s_meter.CreateCounter<int>("hatco.store.hats_sold");

    static void Main(string[] args)
    {
        Console.WriteLine("Press any key to exit");
        while (!Console.KeyAvailable)
        {
            // Simulando a venda de 4 itens por segundo
            Thread.Sleep(1000);
            s_hatsSold.Add(4);
            Console.WriteLine("4 hats sold");
        }
    }
}