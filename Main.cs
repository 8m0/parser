using System;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;
using System.Diagnostics;
using Telegram.Bot;

namespace StartParse
{
    class StartParseBR
    {
        static readonly TelegramBotClient Bot = new TelegramBotClient("877422609:AAElynKcROehBDiQJczIPVsJPxu-855QC7Y");

        private static void bot_mess(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            if(e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
            {
                Console.WriteLine(e.Message.Text);
                Bot.SendTextMessageAsync(e.Message.Chat.Id, "zasd: " + e.Message.Chat.Id);
            }
        }

        static public void Main()
        {
            // In case to get ID of channels etc..
            //Bot.OnMessage += bot_mess;
            //Bot.StartReceiving();
            //Console.ReadLine();
            //Bot.StopReceiving();

            string BezRealitkyLink = "https://www.bezrealitky.cz/vyhledat?offerType=pronajem&estateType=byt&disposition=&ownership=&construction=&equipped=&balcony=&order=timeOrder_desc&boundary=%5B%5B%7B%22lat%22%3A50.171436864513%2C%22lng%22%3A14.506905276796942%7D%2C%7B%22lat%22%3A50.154133576294%2C%22lng%22%3A14.599004629591036%7D%2C%7B%22lat%22%3A50.14524430128%2C%22lng%22%3A14.58773054712799%7D%2C%7B%22lat%22%3A50.129307131988%2C%22lng%22%3A14.60087568578706%7D%2C%7B%22lat%22%3A50.122604734575%2C%22lng%22%3A14.659116306376973%7D%2C%7B%22lat%22%3A50.106512499343%2C%22lng%22%3A14.657434650206028%7D%2C%7B%22lat%22%3A50.090685542974%2C%22lng%22%3A14.705099547441932%7D%2C%7B%22lat%22%3A50.072175921973%2C%22lng%22%3A14.700004206235008%7D%2C%7B%22lat%22%3A50.056898491904%2C%22lng%22%3A14.640206899053055%7D%2C%7B%22lat%22%3A50.038528576841%2C%22lng%22%3A14.666852728301023%7D%2C%7B%22lat%22%3A50.030955909657%2C%22lng%22%3A14.656128752460972%7D%2C%7B%22lat%22%3A50.013435368522%2C%22lng%22%3A14.66854956530301%7D%2C%7B%22lat%22%3A49.99444182116%2C%22lng%22%3A14.640153080292066%7D%2C%7B%22lat%22%3A50.010839032542%2C%22lng%22%3A14.527474219359988%7D%2C%7B%22lat%22%3A49.970771602447%2C%22lng%22%3A14.46224174052395%7D%2C%7B%22lat%22%3A49.970669964027%2C%22lng%22%3A14.400648545303966%7D%2C%7B%22lat%22%3A49.941901176098%2C%22lng%22%3A14.395563234671044%7D%2C%7B%22lat%22%3A49.948384148423%2C%22lng%22%3A14.337635637038034%7D%2C%7B%22lat%22%3A49.958376114735%2C%22lng%22%3A14.324977842107955%7D%2C%7B%22lat%22%3A49.9676286223%2C%22lng%22%3A14.34491711110104%7D%2C%7B%22lat%22%3A49.971859099005%2C%22lng%22%3A14.326815050839059%7D%2C%7B%22lat%22%3A49.990608728081%2C%22lng%22%3A14.342731259186962%7D%2C%7B%22lat%22%3A50.002211140429%2C%22lng%22%3A14.29483886971002%7D%2C%7B%22lat%22%3A50.023596577558%2C%22lng%22%3A14.315872285282012%7D%2C%7B%22lat%22%3A50.058309376419%2C%22lng%22%3A14.248086830069042%7D%2C%7B%22lat%22%3A50.073179111%2C%22lng%22%3A14.290193274400963%7D%2C%7B%22lat%22%3A50.102973823639%2C%22lng%22%3A14.224439442359994%7D%2C%7B%22lat%22%3A50.130060800171%2C%22lng%22%3A14.302396419107936%7D%2C%7B%22lat%22%3A50.116019827009%2C%22lng%22%3A14.360785349547996%7D%2C%7B%22lat%22%3A50.148005694843%2C%22lng%22%3A14.365662825877052%7D%2C%7B%22lat%22%3A50.14142969454%2C%22lng%22%3A14.394903042943952%7D%2C%7B%22lat%22%3A50.171436864513%2C%22lng%22%3A14.506905276796942%7D%2C%7B%22lat%22%3A50.171436864513%2C%22lng%22%3A14.506905276796942%7D%5D%5D&hasDrawnBoundary=1&mapBounds=%5B%5B%7B%22lat%22%3A50.289447077141126%2C%22lng%22%3A14.68724263943227%7D%2C%7B%22lat%22%3A50.289447077141126%2C%22lng%22%3A14.087801111111958%7D%2C%7B%22lat%22%3A50.039169221047985%2C%22lng%22%3A14.087801111111958%7D%2C%7B%22lat%22%3A50.039169221047985%2C%22lng%22%3A14.68724263943227%7D%2C%7B%22lat%22%3A50.289447077141126%2C%22lng%22%3A14.68724263943227%7D%5D%5D&center=%7B%22lat%22%3A50.16447196305031%2C%22lng%22%3A14.387521875272125%7D&zoom=11&locationInput=praha&limit=15";

            string linksFile;
            string newAppartments;

            Console.WriteLine("Set path to the files(just copy).");
            Console.Write("Links: ");
            linksFile = Console.ReadLine();
            Console.Write("Appartments: ");
            newAppartments = Console.ReadLine();

            // Path to a file with apartmenst links
            if (linksFile == "skip")
                linksFile = "D:\\parser\\pppppaaaaaaaaar\\BezRealitkyAppartmentLinks.txt";

            if (newAppartments == "skip")
                newAppartments = "D:\\parser\\pppppaaaaaaaaar\\NewAppartments.txt";

            while(true)
            {
                try
                {
                    Parser bezRealitkyParser = new Parser(BezRealitkyLink, linksFile, newAppartments);

                    // First engage to get everything
                    bezRealitkyParser.saveFirstStart().GetAwaiter().GetResult();

                    TimerPBR timer = new TimerPBR(30, bezRealitkyParser.runParser);
                    timer.StartTimer();
                }
                catch
                {
                    Console.WriteLine("Got Error inside Main(); Repeat in 10 mins.");

                    Thread.Sleep(100000);
                }
            }

            //Console.ReadLine();
        }
    }
}
    