using CommandLine;

namespace Acc
{
    [Verb("show", HelpText = "Выводит все проводки в книге.")]
    class ShowOptions
    {
        [Option(longName: "actual", Required = false, HelpText = "Показать только актуальные значения.", Default = false)]
        public bool Actual { get; set; }
    }
}