using BohaLibrary.Models;

namespace BohaLibrary
{
    internal class BooksOfHouseholdAccounts
    {
        public List<string> Names { get; set; } = [];

        public void AddBook(string bookName) => Names.Add(bookName);

        public void DeleteBook(string bookName) => Names.Remove(bookName);

        public bool BookExists(string bookName) => Names.Contains(bookName);

        public void LoadFromFile(string path, string fileName = "boha_books.json")
        {
            BooksOfHouseholdAccountsModel books = BohaFileHelpers.ReadFromJsonFile<BooksOfHouseholdAccountsModel>(Path.Combine(path, fileName));
            Names = [.. books.Names];
        }
    }
}