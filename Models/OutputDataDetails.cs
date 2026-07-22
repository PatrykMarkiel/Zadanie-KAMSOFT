namespace Zadanie___KAMSOFT.Models
{
    public class OutputDataDetails
    {
        public string Status { get; set; }
        public int Count { get; set; }
        public List<Dictionary<string, object>> Data { get; set; }
    }
}
