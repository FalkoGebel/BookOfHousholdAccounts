using BohaLibrary;
using FluentAssertions;

namespace BohaLibraryTests
{
    [TestClass]
    public sealed class BooksOfHousholdAccountsTests
    {
        private static readonly string _folderpath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/temp/";

        [TestMethod]
        public void TestAddBookTwoTimesAndGetArgumentException()
        {
            BooksOfHouseholdAccounts books = new();
            books.AddBook("book1");
            ArgumentException ae = Assert.ThrowsException<ArgumentException>(() => books.AddBook("book1"));
            ae.Message.Should().Be("Book \"book1\" already exists.");
        }

        [TestMethod]
        public void TestDeleteNotExistingBookAndGetArgumentException()
        {
            BooksOfHouseholdAccounts books = new();
            books.AddBook("book1");
            books.AddBook("book2");
            ArgumentException ae = Assert.ThrowsException<ArgumentException>(() => books.DeleteBook("book3"));
            ae.Message.Should().Be("Book \"book3\" does not exist.");
        }

        [TestMethod]
        public void TestSaveToAndLoadFromFile()
        {
            BooksOfHouseholdAccounts books = new();
            books.AddBook("book1");
            books.AddBook("book2");
            books.AddBook("book3");
            books.DeleteBook("book2");
            books.SaveToFile(_folderpath);

            BooksOfHouseholdAccounts loadedBooks = new();
            loadedBooks.LoadFromFile(_folderpath);

            loadedBooks.Should().BeEquivalentTo(books);
        }
    }
}
