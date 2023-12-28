namespace Shelf
{
    /// <summary>
    /// A ShelfParser Class
    /// </summary>
    public class Parser
    {
        private Dictionary<char, string> _mutators = new();
        public Dictionary<char, string> Mutators
        {
            get => _mutators;
            set => _mutators = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Parser"/> class.  
        /// </summary>
        public Parser()
        {
            Mutators = _mutators;
        }
        
        public void Parse(char[] parsedChars, Dictionary<char, string> mutators)
        {
            
        }
    }
}
