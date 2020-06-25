namespace FoodOrderSystem.Domain.Adapters.Template
{
    public class EmailData
    {
        public string Subject { get; set; }
        
        public string TextPart { get; set; }
        
        public string HtmlPart { get; set; }
    }
}