using CommandLine;

namespace Acc
{
    [Verb("add", HelpText = "Добавить новую проводку.")]
    class AddOptions
    {
        const string DefaultAccount = "def";
    
        [Option(shortName: 'f', longName: "from", Required = false, HelpText = "Счет кредита.", Default = DefaultAccount)]
        public string From { get; set; } = null!;

        [Option(shortName: 't', longName: "to", Required = false, HelpText = "Счет дебета.", Default = DefaultAccount)]
        public string To  { get; set; } = null!;

        [Option(shortName: 'm', longName: "msg", Required = false, HelpText = "Пояснение.", Default = "")]
        public string? Message { get; set; }
    
        [Value(1, Required = true, HelpText = "Сумма")]
        public decimal Sum { get; set; }
    }
}