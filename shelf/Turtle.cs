using SysIO = System.IO;

namespace Shelf
{
    public class Turtle
    {
        private bool _isOn;

        public bool IsOn
        {
            get => _isOn;
            set
            {
                if (!FinishedJob)
                {
                    throw new Exception("How can one be ON after finishing their day?");
                }
                else
                {
                    _isOn = value;
                }
            }
        }

        public bool FinishedJob { get; set; }

        protected static string DocumentDataRead(File aFile)
        {
            string docText = "";

            if (SysIO.File.Exists($"{aFile}"))
            {
                try
                {
                    var reader = new StreamReader($"{aFile}");

                    while (!reader.EndOfStream)
                    {
                        docText += reader.ReadLine();
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }
            }
            else
            {
                throw new Exception("NO FILE?");
            }

            return docText;
        }
    }
}