namespace BohaLibrary.Models
{
    internal class BookOfHouseholdAccountsModel
    {
        public string Name { get; set; } = string.Empty;
        public CategoryModel[] Categories { get; set; } = [];
        public BookEntryModel[] Entries { get; set; } = [];
    }
}