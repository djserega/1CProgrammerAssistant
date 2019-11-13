namespace MakingCode
{
    public class Main
    {
        public string SourceText { get; set; }
        public string ResultText { get; private set; }

        public bool EnableMakingMethod { get; set; }

        public bool Making()
        {
            ResultText = string.Empty;

            if (string.IsNullOrWhiteSpace(SourceText))
                return false;

            string result = new MakingSourceText(SourceText, EnableMakingMethod).MakeText();
            
            if (string.IsNullOrWhiteSpace(result))
                return false;

            ResultText = result;

            return true;
        }
    }
}
