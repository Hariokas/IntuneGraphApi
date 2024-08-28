using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Models.AppModels
{
    public class Rule
    {
        public object? AdditionalData { get; set; }
        public BackingStore BackingStore { get; set; } = default!;
        public string ODataType { get; set; } = "";
        public int? RuleType { get; set; }
    }
}
