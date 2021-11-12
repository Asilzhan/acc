// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Acc;
using CommandLine;

const string dbFileName = "db.json";
List<Record> records;

Configure();

Run();

Close();

void Configure()
{ 
    // CultureInfo.CurrentCulture = new CultureInfo("ru-RU");
   if (File.Exists(dbFileName))
   {
        try
        {
            records = JsonSerializer.Deserialize<List<Record>>(File.ReadAllText(dbFileName)) ?? new List<Record>();
        }
        catch
        {
            records = new List<Record>();
        }
        
   }
   else
   {
        records = new List<Record>();
   }
}

void Run()
{
    Parser.Default.ParseArguments<AddOptions, ShowOptions>(args)
        .WithParsed<AddOptions>(Add)
        .WithParsed<ShowOptions>(Show)
        .WithNotParsed(errors => Help());
}

void Add(AddOptions options)
{
    if(options.From == options.To)
        return;

    records.Add(new Record(options.From, options.To, options.Sum, options.Message, DateTime.Now));
}

void Show(ShowOptions options)
{
    if (options.Actual)
    {
        var set = new Dictionary<(string from, string to, decimal sum), int>();
        for (var i = 0; i < records.Count; i++)
        {
            var record = records[i];
            if (record.Sum < 0)
            {
                var t = (record.From, record.To, -record.Sum);
                if (set.ContainsKey(t))
                {
                    set.Remove(t);
                    continue;
                }
            }

            set[(record.From, record.To, record.Sum)] = i;
        }

        Print(set.Select(pair => records[pair.Value]).ToList());
    }
    else
    {
        Print(records); 
    }
}

void Print(List<Record> records)
{
    Console.WriteLine($"{"Дата",-20}| {"Кредит",-10}| {"Дебет",-10}| {"Сумма",-20:C}| {"Пояснение"}");
    for (int i = 0; i < records.Count; i++)
    {
        Console.WriteLine($"{records[i].DateTime,-20:dd:MM:yyyy hh:mm:ss}| {records[i].From,-10}| {records[i].To,-10}| {records[i].Sum,-20:C}| {records[i].Description}");
    }
}

void Help()
{
    var helpText = @"
Используйте следующие команды:
acc add [--from <счет_кредита>] [--to <счет-дебета>] <сумма>    --  Добавляет новую проводку в книгу. 
                                                                    Если один из счетов не указан, используется счет по-умолчанию.
                                                                    Если не указаны оба счета, действие не запишется.

acc show [--actual]                                             --  Выводит все проводки в книге. 
                                                                    При указании --actual показывает только актуальные проводки. 
";
    Console.WriteLine(helpText);
}

void Close()
{
    try
    {
        string json = JsonSerializer.Serialize(records);
        File.WriteAllText(dbFileName, json);
    }
    catch
    {
    }
    
}

