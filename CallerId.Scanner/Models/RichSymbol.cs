namespace CallerId.Scanner.Models
{
    public class RichSymbol
    {
        public DocumentCoordinates DocumentData { get; set; }
        public string ContainingClass { get; set; }
        public string ContainingInterface { get; set; }
        public string ContainingNamespace { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
