using BohaLibrary;
using FluentAssertions;

namespace BohaLibraryTests
{
    [TestClass]
    public sealed class BooksOfHousholdAccountsTests
    {
        private static readonly string _folderpath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/temp/";

        [TestMethod]
        public void TestAddEmptyStringBookAndGetArgumentException()
        {
            // Arrange
            BooksOfHouseholdAccounts books = new();

            // Act
            ArgumentException ae = Assert.ThrowsException<ArgumentException>(() => books.AddBook(""));

            // Assert
            ae.Message.Should().Be("Book name must not be an empty string.");
        }

        [TestMethod]
        public void TestAddBookTwoTimesAndGetArgumentException()
        {
            // Arrange
            BooksOfHouseholdAccounts books = new();
            books.AddBook("book1");

            // Act
            ArgumentException ae = Assert.ThrowsException<ArgumentException>(() => books.AddBook("book1"));

            // Assert
            ae.Message.Should().Be("Book \"book1\" already exists.");
        }

        [TestMethod]
        public void TestDeleteNotExistingBookAndGetArgumentException()
        {
            // Arrange
            BooksOfHouseholdAccounts books = new();
            books.AddBook("book1");
            books.AddBook("book2");

            // Act
            ArgumentException ae = Assert.ThrowsException<ArgumentException>(() => books.DeleteBook("book3"));

            // Assert
            ae.Message.Should().Be("Book \"book3\" does not exist.");
        }

        [TestMethod]
        public void TestSaveToAndLoadFromFile()
        {
            // Arrange
            BooksOfHouseholdAccounts books = new();
            books.AddBook("book1");
            books.AddBook("book2");
            books.AddBook("book3");
            books.DeleteBook("book2");
            books.SaveToFile(_folderpath);

            // Act
            BooksOfHouseholdAccounts loadedBooks = new();
            loadedBooks.LoadFromFile(_folderpath);

            // Assert
            loadedBooks.Should().BeEquivalentTo(books);
        }
    }
}
