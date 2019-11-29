using CallerId.Scanner.Models;

namespace CallerId.Models
{
    public class Dependency
    {
        public RichSymbol Parent { get; set; }
        public RichSymbol Child { get; set; }
    }
}