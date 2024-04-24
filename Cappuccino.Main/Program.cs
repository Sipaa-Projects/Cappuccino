using System.IO.Pipes;

namespace Cappuccino.Main
{
    internal static class Program
    {
        static void Info(string cmp, string msg) => Console.WriteLine($"INFO > {cmp}: {msg}");
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Console.WriteLine(@" ~         _   _           ____                                 _                                              ");
            Console.WriteLine(@" ~        | |_| |_  ___   / ___|__ _ _ __  _ __  _   _  ___ ___(_)_ __   ___        __        __   __          ");
            Console.WriteLine(@".---.     |  _| ' \/ -_) | |   / _` | '_ \| '_ \| | | |/ __/ __| | '_ \ / _ \   ___/ ___ ___ / /__/ /____  ___ ");
            Console.WriteLine(@"`---'=.    \__|_||_\___| | |__| (_| | |_) | |_) | |_| | (_| (__| | | | | (_) | / _  / -_(_-</  '_/ __/ _ \/ _ \");
            Console.WriteLine(@"|VK | |                   \____\__,_| .__/| .__/ \__,_|\___\___|_|_| |_|\___/  \_,_/\__/___/_/\_\\__/\___/ .__/");
            Console.WriteLine(@"|   |='                             |_|   |_|                                                           /_/    ");
            Console.WriteLine(@"`---'     Copyright (C) 2024-present Sipaa Projects. We aren't responsible of ANY DAMAGES");
            
            Info("Cappuccino.Main", "Shell loaded, now entering the loop.");
            
            while (true)
            {

            }
        }
    }
}