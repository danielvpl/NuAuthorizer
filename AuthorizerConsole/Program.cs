using Domain;
using Domain.Commands.Requests;
using Domain.Handlers;
using Domain.Interfaces.Handlers;
using Domain.Responses;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AuthorizerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var handler = serviceProvider.GetService<IRequestHandler<ExecuteOperationCommand, string>>();

            //Debug tests
            args = new string[1];
            args[0] = "operations3";

            if (args.Length == 0)
            {
                Console.WriteLine("Enter the operations file");
                return;
            }

            string FileToRead = args[0];
            string[] lines = File.ReadAllLines(FileToRead);
            List<string> commandStr = new List<string>(lines);            

            ExecuteOperationCommand execCommand = new ExecuteOperationCommand()
            {
                Commands = commandStr
            };

            Console.WriteLine("#Input");
            StringBuilder commandInput = new StringBuilder();
            foreach (var item in commandStr)
            {
                commandInput.AppendLine(item);
            }
            Console.WriteLine(commandInput.ToString());

            Console.WriteLine("#Output");
            string result = handler.Handler(execCommand);            
            Console.WriteLine(result);

            Console.WriteLine("Press a key...");
            Console.ReadKey();
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IRequestHandler<CreateAccountCommand, OperationResult>, CreateAccountHandler>();
            services.AddScoped<IRequestHandler<AuthorizeTransactionCommand, OperationResult>, AuthorizeTransactionHandler>();
            services.AddScoped<IRequestHandler<ExecuteOperationCommand, string>, ExecuteOperationHandler>();
            services.AddScoped<NotificationContext>();
        }        
    }
}

