using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using LoggingAdvanced.Console;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;
using Synthesis.Bethesda;
using SynthusMaximus.Data;
using SynthusMaximus.Patchers;
using Wabbajack.Common.StatusFeed.Errors;
using Microsoft.Extensions.Hosting;
using SynthusMaximus.Data.Converters;
using SynthusMaximus.Data.Enums;
using Wabbajack.Common;
using IPatcher = SynthusMaximus.Patchers.IPatcher;

namespace SynthusMaximus
{
    public class Program
    {
        private static Action<ILoggingBuilder>? addLogger = null;
        public static Action<ILoggingBuilder>? AddLogger { get => addLogger; set => addLogger = value; }

        public static async Task<int> Main(string[] args)
        {
            return await SynthesisPipeline.Instance
                .AddPatch<ISkyrimMod, ISkyrimModGetter>(RunPatch)
                .SetTypicalOpen(GameRelease.SkyrimSE, "SynthusMaximus.esp")
                .Run(args);
        }

        public static Task RunPatch(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            var host = Host.CreateDefaultBuilder(Array.Empty<string>())
                .ConfigureAppConfiguration((hostingContext, configuration) =>
                {
                    configuration.Sources.Clear();
                    var env = hostingContext.HostingEnvironment;
                    configuration.AddJsonFile(AbsolutePath.EntryPoint.Combine("appsettings.json").ToString(), optional: false, reloadOnChange: false);
                })
                .ConfigureServices(c => ConfigureServices(c, state))
                .Build();

            var runner = host.Services.GetService<PatcherRunner>();
            runner!.RunPatchers();
            return Task.CompletedTask;
        }

        private static void ConfigureServices(IServiceCollection collection, IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            collection.AddLogging(logging =>
            {
                logging.SetMinimumLevel(LogLevel.Trace);
                logging.ClearProviders();
                if (AddLogger != null)
                    AddLogger(logging);
                else
                    logging.AddConsole();
            });
            collection.AddTransient<OverlayLoader>();
            collection.AddSingleton<DataStorage>();
            collection.AddTransient<PatcherRunner>();
            collection.AddSingleton(state);
            collection.AddSingleton<MaterialEnum>();
            collection.AddSingleton<BaseWeaponTypeEnum>();
            collection.AddSingleton<WeaponClassEnum>();

            collection.AddAllOfInterface<IPatcher>();
            collection.AddAllOfInterface<IInjectedConverter>();
            collection.AddAllOfInterface<IFormLinkJsonConverter>();
        }
    }
}
