namespace BohaLibrary.Models
{
    public class BookEntryModel
    {
        public int Id { get; set; }
        public BookEntryType EntryType { get; set; }
        public DateTime PostingDate { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string MemoText { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}