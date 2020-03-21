using System;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using PuppeteerSharp;
using System.Timers;
using System.Diagnostics;
using Telegram.Bot;

//ParseBR.Parser.runParser().GetAwaiter().GetResult();
//await ParseBR.Parser.runParser();
//var Timer = new System.Timers.Timer(60000);
//Timer.AutoReset = true;
//Timer.Enabled = true;
//Timer.Elapsed += Parser.runParser;
//Timer.Start();

class Parser
{
    // Add telegram bot to send message if there is new appartment
    private static readonly TelegramBotClient Bot = new TelegramBotClient("877422609:AAElynKcROehBDiQJczIPVsJPxu-855QC7Y");

    string LINK;
    string linksFile;
    string newAppartmentsFile;

    // Counter of elements
    int countElements = 0;

    public Parser(string bezRealitkyLink, string linksFilePathTxt, string newItemsFileTxt)
    {
        LINK = bezRealitkyLink;
        linksFile = linksFilePathTxt;
        newAppartmentsFile = newItemsFileTxt;
    }

    public async Task saveFirstStart()
    {
        while (true)
        {
            try
            {
                await firstStart();
                break; // success!
            }
            catch
            {
                Console.WriteLine("I GOT AN ERROR INSIDE PARSER, TRYING TO REPEAT: " + DateTime.Now);
                await Bot.SendTextMessageAsync(792928784, "I GOT AN ERROR INSIDE PARSER, TRYING TO REPEAT");

                Thread.Sleep(100000);
            }
        }
    }

    private async Task firstStart()
    {
        using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions()))
        {
            Console.WriteLine("Starting process \n");

            // Send to the status channel
            await Bot.SendTextMessageAsync(792928784, "Starting process...");

            var page = await browser.NewPageAsync();
            await page.GoToAsync(LINK);
            await page.WaitForTimeoutAsync(15000);

            await page.WaitForSelectorAsync("#search-content button.btn-icon").EvaluateFunctionAsync("e => e.click()");
            var articlesForNow = await page.QuerySelectorAllAsync("#search-content article");

            // Counter in case of short checks
            //int count = 0;

            countElements = 0;

            while (await page.QuerySelectorAsync("#search-content button.btn-icon") != null)
            {
                //count++;
                articlesForNow = await page.QuerySelectorAllAsync("#search-content article");

                // Right counter
                //if (countElements < articlesForNow.Length)
                //{
                  //  countElements = articlesForNow.Length;
                    //Console.WriteLine("Items proceed: " + countElements);
                //}

                await page.WaitForSelectorAsync("#search-content button.btn-icon").EvaluateFunctionAsync("e => e.click()");

                // Wait for 5 seconds every time to prevent reset
                await page.WaitForTimeoutAsync(5000);
            }

            articlesForNow = await page.QuerySelectorAllAsync("#search-content article");

            Console.WriteLine("Scanning: [" + articlesForNow.Length + "] items...");

            // Send to the status channel
            await Bot.SendTextMessageAsync(792928784, "Scanning: [" + articlesForNow.Length + "] items...");

            // Get all existing links to an array
            string[] existingLinks = File.ReadAllLines(linksFile);

            // Counter for old links
            int oldLinks = 0;

            // Counter for new links
            int newLinks = 0;

            using (StreamWriter writer = new StreamWriter(linksFile, true))
            {
                for (int i = 0; i < articlesForNow.Length; i++)
                {
                    string articleLink = await articlesForNow[i].QuerySelectorAsync(".product__link").EvaluateFunctionAsync<string>("e => e.href");

                    // Match flag
                    bool foundMatch = false;

                    for (int j = 0; j < existingLinks.Length; j++)
                    {
                        if (existingLinks[j] == articleLink)
                        {
                            oldLinks++;
                            foundMatch = true;
                            break;
                        }
                    }

                    // If no matches = new link, add to a file
                    if (foundMatch == false)
                    {
                        newLinks++;

                        //Console.WriteLine("New Appartment added.");

                        // Write a new link into file, to compare it next times
                        writer.WriteLine(articleLink);

                        // Make a string out of a new appartment and get it into a txt file
                        string newAppartmentHeader = await articlesForNow[i].QuerySelectorAsync(".product__link strong").EvaluateFunctionAsync<string>("e => e.innerText");
                        string newAppartmentCost = await articlesForNow[i].QuerySelectorAsync(".product__value").EvaluateFunctionAsync<string>("e => e.innerHTML");
                        string newAppartmentLink = await articlesForNow[i].QuerySelectorAsync(".product__link").EvaluateFunctionAsync<string>("e => e.href");

                        // Send new appratment to a telegram group
                        //await Bot.SendTextMessageAsync(-277455680, newAppartmentHeader + "\n" + newAppartmentCost + "\n" + newAppartmentLink);
                        await Bot.SendTextMessageAsync("@norealityprague", newAppartmentHeader + "\n" + newAppartmentCost + "\n" + newAppartmentLink);

                        // Add new appartment into file
                        using (StreamWriter newAppWr = new StreamWriter(newAppartmentsFile, true))
                        {
                            newAppWr.WriteLine(newAppartmentHeader);
                            newAppWr.WriteLine(newAppartmentCost);
                            newAppWr.WriteLine(newAppartmentLink);
                            newAppWr.WriteLine(new String('=', 200));
                            newAppWr.WriteLine();
                        }
                    }

                }
            }

            // Get current date and time
            var dateTime = DateTime.Now;

            Console.WriteLine("\t|Old links: " + oldLinks + "|     ---==---     |" + "New Appartments: " + newLinks + "|");
            Console.WriteLine();
            Console.WriteLine("Log date: " + dateTime);
            Console.WriteLine(new String('=', 50));

            // If there is typical lag, than repeat the method
            if (oldLinks == 0 && newLinks == 0)
                await firstStart();

            // Mesage to a status channel
            //await Bot.SendTextMessageAsync(-277455680, "|Old Appartments: " + oldLinks + "|\n|" + "New Appartments: " + newLinks + "|"); 
            await Bot.SendTextMessageAsync(792928784, "|Old Appartments: " + oldLinks + "|\n|" + "New Appartments: " + newLinks + "|");

            // Close oppened page
            await page.CloseAsync();

            // Kill Chromium Procesess
            foreach (var process in Process.GetProcessesByName("Chromium"))
            {
                process.Kill();
            }
        }
    }

    public async void runParser(object sender, ElapsedEventArgs e)
    {
        using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions()))
        {
            Console.WriteLine("Starting process \n");

            // Send to the status channel
            await Bot.SendTextMessageAsync(792928784, "Starting process...");

            var page = await browser.NewPageAsync();
            await page.GoToAsync(LINK);
            await page.WaitForTimeoutAsync(15000);

            await page.WaitForSelectorAsync("#search-content button.btn-icon").EvaluateFunctionAsync("e => e.click()");
            var articlesForNow = await page.QuerySelectorAllAsync("#search-content article");

            // Counter in case of short checks
            //int count = 0;

            countElements = 0;

            while (await page.QuerySelectorAsync("#search-content button.btn-icon") != null)
            {
                //count++;
                articlesForNow = await page.QuerySelectorAllAsync("#search-content article");

                // Right counter
                //if (countElements < articlesForNow.Length)
                //{
                 //   countElements = articlesForNow.Length;
                   // Console.WriteLine("Items proceed: " + countElements);
                //}

                await page.WaitForSelectorAsync("#search-content button.btn-icon").EvaluateFunctionAsync("e => e.click()");

                // Wait for 5 seconds every time to prevent reset
                await page.WaitForTimeoutAsync(5000);
            }

            articlesForNow = await page.QuerySelectorAllAsync("#search-content article");

            Console.WriteLine("Scanning: [" + articlesForNow.Length + "] items...");

            // Send to the status channel
            await Bot.SendTextMessageAsync(792928784, "Scanning: [" + articlesForNow.Length + "] items...");

            // Get all existing links to an array
            string[] existingLinks = File.ReadAllLines(linksFile);

            // Counter for old links
            int oldLinks = 0;

            // Counter for new links
            int newLinks = 0;

            using (StreamWriter writer = new StreamWriter(linksFile, true))
            {
                for (int i = 0; i < articlesForNow.Length; i++)
                {
                    string articleLink = await articlesForNow[i].QuerySelectorAsync(".product__link").EvaluateFunctionAsync<string>("e => e.href");

                    // Match flag
                    bool foundMatch = false;

                    for (int j = 0; j < existingLinks.Length; j++)
                    {
                        if (existingLinks[j] == articleLink)
                        {
                            oldLinks++;
                            foundMatch = true;
                            break;
                        }
                    }

                    // If no matches = new link, add to a file
                    if (foundMatch == false)
                    {
                        newLinks++;

                        //Console.WriteLine("New Appartment added.");

                        // Write a new link into file, to compare it next times
                        writer.WriteLine(articleLink);

                        // Make a string out of a new appartment and get it into a txt file
                        string newAppartmentHeader = await articlesForNow[i].QuerySelectorAsync(".product__link strong").EvaluateFunctionAsync<string>("e => e.innerText");
                        string newAppartmentCost = await articlesForNow[i].QuerySelectorAsync(".product__value").EvaluateFunctionAsync<string>("e => e.innerHTML");
                        string newAppartmentLink = await articlesForNow[i].QuerySelectorAsync(".product__link").EvaluateFunctionAsync<string>("e => e.href");

                        // Send new appratment to a telegram group
                        //await Bot.SendTextMessageAsync(-277455680, newAppartmentHeader + "\n" + newAppartmentCost + "\n" + newAppartmentLink); 
                        await Bot.SendTextMessageAsync("@norealityprague", newAppartmentHeader + "\n" + newAppartmentCost + "\n" + newAppartmentLink);

                        // Add new appartment into file
                        using (StreamWriter newAppWr = new StreamWriter(newAppartmentsFile, true))
                        {
                            newAppWr.WriteLine(newAppartmentHeader);
                            newAppWr.WriteLine(newAppartmentCost);
                            newAppWr.WriteLine(newAppartmentLink);
                            newAppWr.WriteLine(new String('=', 200));
                            newAppWr.WriteLine();
                        }
                    }

                }
            }

            // Get current date and time
            var dateTime = DateTime.Now;

            Console.WriteLine("\t|Old links: " + oldLinks + "|     ---==---     |" + "New Appartments: " + newLinks + "|");
            Console.WriteLine();
            Console.WriteLine("Log date: " + dateTime);
            Console.WriteLine(new String('=', 50));

            // If there is typical lag, than repeat the method
            if (oldLinks == 0 && newLinks == 0)
                await firstStart();

            // Mesage to a bot
            await Bot.SendTextMessageAsync(792928784, "|Old Appartments: " + oldLinks + "|\n|" + "New Appartments: " + newLinks + "|");

            // Close oppened page
            await page.CloseAsync();

            // Kill Chromium Procesess
            foreach(var process in Process.GetProcessesByName("Chromium"))
            {
                process.Kill();
            }
        }
    }
}

