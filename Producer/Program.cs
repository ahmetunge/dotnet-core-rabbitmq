using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Core.DependencyResolvers;
using Core.Extensions;
using Core.Queue;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Producer
{
    class Program
    {
        static void Main(string[] args)
        {


            try
            {
                var serviceProvider = StartServices().BuildServiceProvider();

                var queueService = serviceProvider.GetService<IQueueService>();

                List<object> dataList = new List<object>();

                var data = File.ReadAllText("./Assets/data.json");

                dataList = JsonConvert.DeserializeObject<List<object>>(data);


                queueService.AddQueue(dataList, "Helloworld");

            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }



            // for (int i = 0; i < 10; i++)
            // {
            //     Console.Write("Send message : ");
            //     string message = Console.ReadLine();

            //     List<Message> messages = new List<Message>(){
            //             new Message{Subject="Subject 1",Body=message}
            //       };

            //     queueService.AddQueue(messages, "Helloworld");

            //     Console.WriteLine("Hello World!");
            // }

        }


        public static IServiceCollection StartServices()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddDependencyResolvers(new ICoreModule[]{
                new CoreModule(),
                new LoggingModule()
            });

            return services;
        }
    }


}
