
namespace ApiAggregation.Domain.Models
{
    public class AggregatedItem
    {
        public string Source { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string Url { get; set; }
    }
}
