using BohaLibrary.Models;

namespace BohaLibrary
{
    public class BooksOfHouseholdAccounts
    {
        public List<string> Names { get; private set; } = [];

        /// <summary>
        /// Adds the given book name to the list of names, if it does not exist already.
        /// </summary>
        /// <param name="bookName">The name of the book to add to the list of names.</param>
        /// <exception cref="ArgumentException">Thrown, if the given book name already exists.</exception>
        public void AddBook(string bookName)
        {
            if (bookName == string.Empty)
                throw new ArgumentException("Book name must not be an empty string.");

            if (Names.Contains(bookName))
                throw new ArgumentException($"Book \"{bookName}\" already exists.");

            Names.Add(bookName);
        }

        /// <summary>
        /// Deletes the given book name from the list of names, if it exists.
        /// </summary>
        /// <param name="bookName">The name of the book to delete from the list of names.</param>
        /// <exception cref="ArgumentException">Thrown, if the given book name does not exist.</exception>
        public void DeleteBook(string bookName)
        {
            if (!Names.Remove(bookName))
                throw new ArgumentException($"Book \"{bookName}\" does not exist.");
        }

        /// <summary>
        /// Updates the list of names from the given file.
        /// </summary>
        /// <param name="path">The path where to find the file.</param>
        /// <param name="fileName">The file to update the names from. If not set, then "boha_books.json" is used.</param>
        public void LoadFromFile(string path, string fileName = "boha_books.json")
        {
            Directory.CreateDirectory(path);

            BooksOfHouseholdAccountsModel books = BohaFileHelpers.ReadFromJsonFile<BooksOfHouseholdAccountsModel>(Path.Combine(path, fileName));
            Names = [.. books.Names];
        }

        /// <summary>
        /// Saves the list of names to the given file.
        /// </summary>
        /// <param name="path">The path where to find the file.</param>
        /// <param name="fileName">The file to save the names to. If not set, then "boha_books.json" is used.</param>
        public void SaveToFile(string path, string fileName = "boha_books.json")
        {
            Directory.CreateDirectory(path);

            BohaFileHelpers.WriteToJsonFile(Path.Combine(path, fileName), new BooksOfHouseholdAccountsModel() { Names = [.. Names] });
        }
    }
}