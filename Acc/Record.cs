using System;

namespace Acc
{
    record Record(string From, string To, decimal Sum, string? Description, DateTime DateTime);
}