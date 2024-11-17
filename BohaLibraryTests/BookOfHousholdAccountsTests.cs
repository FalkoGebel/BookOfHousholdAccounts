using BohaLibrary;
using FluentAssertions;

namespace BohaLibraryTests
{
    [TestClass]
    public sealed class BookOfHousholdAccountsTests
    {
        private static readonly string _folderpath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/temp/";

        [DataTestMethod]
        [DataRow(BookEntryType.Deposit)]
        [DataRow(BookEntryType.Payout)]
        public void TestAddCategory(BookEntryType entryType)
        {
            // Arrange
            BookOfHousholdAccounts book = new(_folderpath, "book1");
            string categoryName = "category1";

            // Act
            book.AddCategory(entryType, categoryName);

            // Assert
            book.Categories.Count.Should().Be(1);
            book.Categories[0].Name.Should().Be(categoryName);
            book.Categories[0].EntryType.Should().Be(entryType);
        }

        [DataTestMethod]
        [DataRow(BookEntryType.Deposit)]
        [DataRow(BookEntryType.Payout)]
        public void TestAddCategoryTwoTimesAndGetArgumentException(BookEntryType entryType)
        {
            // Arrange
            BookOfHousholdAccounts book = new(_folderpath, "book1");
            string categoryName = "category1";
            book.AddCategory(entryType, categoryName);

            // Act
            ArgumentException ae = Assert.ThrowsException<ArgumentException>(() => book.AddCategory(entryType, categoryName));

            // Assert
            ae.Message.Should().Be($"{entryType} category \"{categoryName}\" already exists.");
        }


        [DataTestMethod]
        [DataRow(BookEntryType.Deposit)]
        [DataRow(BookEntryType.Payout)]
        public void TestDeleteNotExistingCategoryAndGetArgumentException(BookEntryType entryType)
        {
            // Arrange
            BookOfHousholdAccounts book = new(_folderpath, "book1");
            book.AddCategory(entryType, "category1");
            book.AddCategory(entryType, "category2");
            string categoryNameToDelete = "category3";

            // Act
            ArgumentException ae = Assert.ThrowsException<ArgumentException>(() => book.DeleteCategory(entryType, categoryNameToDelete));

            // Assert
            ae.Message.Should().Be($"{entryType} category \"{categoryNameToDelete}\" does not exist.");
        }

        [DataTestMethod]
        [DataRow(BookEntryType.Deposit)]
        [DataRow(BookEntryType.Payout)]
        public void TestAddBookEntryWithoutCategoryNameAndGetArgumentException(BookEntryType entryType)
        {
            // Arrange
            BookOfHousholdAccounts book = new(_folderpath, "book1");
            DateTime postingDate = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            // Act
            ArgumentException ae;
            if (entryType == BookEntryType.Deposit)
                ae = Assert.ThrowsException<ArgumentException>(() => book.AddDepositBookEntry("", 100, "", null));
            else
                ae = Assert.ThrowsException<ArgumentException>(() => book.AddPayoutBookEntry("", 100, "", null));

            // Assert
            ae.Message.Should().Be("Category name must not be an empty string.");
        }

        [DataTestMethod]
        [DataRow(BookEntryType.Deposit)]
        [DataRow(BookEntryType.Payout)]
        public void TestAddBookEntryWithInvalidCategoryNameAndGetArgumentException(BookEntryType entryType)
        {
            // Arrange
            BookOfHousholdAccounts book = new(_folderpath, "book1");
            DateTime postingDate = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            string categoryName = "category1";

            // Act
            ArgumentException ae;
            if (entryType == BookEntryType.Deposit)
                ae = Assert.ThrowsException<ArgumentException>(() => book.AddDepositBookEntry(categoryName, 100, "", null));
            else
                ae = Assert.ThrowsException<ArgumentException>(() => book.AddPayoutBookEntry(categoryName, 100, "", null));

            // Assert
            ae.Message.Should().Be($"{entryType} category \"{categoryName}\" does not exist.");
        }

        [DataTestMethod]
        [DataRow(BookEntryType.Deposit, 0)]
        [DataRow(BookEntryType.Payout, 0)]
        [DataRow(BookEntryType.Deposit, -1)]
        [DataRow(BookEntryType.Payout, -1)]
        [DataRow(BookEntryType.Deposit, -1000.22)]
        [DataRow(BookEntryType.Payout, -999.99)]
        [DataRow(BookEntryType.Deposit, -0.01)]
        [DataRow(BookEntryType.Payout, -0.01)]
        public void TestAddBookEntryWithInvalidAmountAndGetArgumentException(BookEntryType entryType, double amountDouble)
        {
            // Arrange
            BookOfHousholdAccounts book = new(_folderpath, "book1");
            DateTime postingDate = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            string categoryName = "category1";
            book.AddCategory(entryType, categoryName);
            decimal amount = (decimal)amountDouble;

            // Act
            ArgumentException ae;
            if (entryType == BookEntryType.Deposit)
                ae = Assert.ThrowsException<ArgumentException>(() => book.AddDepositBookEntry(categoryName, amount, "", null));
            else
                ae = Assert.ThrowsException<ArgumentException>(() => book.AddPayoutBookEntry(categoryName, amount, "", null));

            // Assert
            ae.Message.Should().Be($"Invalid amount \"{amount}\" - has to be positive.");
        }

        [DataTestMethod]
        [DataRow("category1", "", 100, BookEntryType.Deposit)]
        [DataRow("category1", "memo1", 55.32, BookEntryType.Deposit)]
        [DataRow("category1", "memo1", 55, BookEntryType.Payout)]
        [DataRow("category1", "memo1", 55.32, BookEntryType.Payout)]
        public void TestAddBookEntryWithoutPostingDate(string categoryName, string memo, double amountDouble, BookEntryType entryType)
        {
            // Arrange
            BookOfHousholdAccounts book = new(_folderpath, "book1");
            DateTime today = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            decimal amount = (decimal)amountDouble;
            book.AddCategory(entryType, categoryName);

            // Act
            if (entryType == BookEntryType.Deposit)
                book.AddDepositBookEntry(categoryName, amount, memo, null);
            else
                book.AddPayoutBookEntry(categoryName, amount, memo, null);

            // Assert
            book.Entries.Count.Should().Be(1);
            book.Entries[0].Id.Should().Be(1);
            book.Entries[0].EntryType.Should().Be(entryType);
            book.Entries[0].PostingDate.Should().Be(today);
            book.Entries[0].CategoryName.Should().Be(categoryName);
            book.Entries[0].MemoText.Should().Be(memo);
            book.Entries[0].Amount.Should().Be(amount);
        }

        [DataTestMethod]
        [DataRow("category1", "", 100, BookEntryType.Deposit, "11.01.24")]
        [DataRow("category1", "memo1", 55.32, BookEntryType.Deposit, "28.02.15")]
        [DataRow("category1", "memo1", 55, BookEntryType.Payout, "01.07.19")]
        [DataRow("category1", "memo1", 55.32, BookEntryType.Payout, "12.12.12")]
        public void TestAddBookEntryWithPostingDate(string categoryName, string memo, double amountDouble, BookEntryType entryType, string postingDateString)
        {
            // Arrange
            BookOfHousholdAccounts book = new(_folderpath, "book1");
            DateTime postingDateParsed = DateTime.ParseExact(postingDateString, "dd.mm.yy", null);
            DateTime postingDate = new(postingDateParsed.Year, postingDateParsed.Month, postingDateParsed.Day);
            decimal amount = (decimal)amountDouble;
            book.AddCategory(entryType, categoryName);

            // Act
            if (entryType == BookEntryType.Deposit)
                book.AddDepositBookEntry(categoryName, amount, memo, postingDate);
            else
                book.AddPayoutBookEntry(categoryName, amount, memo, postingDate);

            // Assert
            book.Entries.Count.Should().Be(1);
            book.Entries[0].Id.Should().Be(1);
            book.Entries[0].EntryType.Should().Be(entryType);
            book.Entries[0].PostingDate.Should().Be(postingDate);
            book.Entries[0].CategoryName.Should().Be(categoryName);
            book.Entries[0].MemoText.Should().Be(memo);
            book.Entries[0].Amount.Should().Be(amount);
        }

        [TestMethod]
        public void TestDeleteBookEntryWithInvalidId()
        {
            // Arrange
            BookOfHousholdAccounts book = new(_folderpath, "book1");

            // Act
            ArgumentException ae;
            ae = Assert.ThrowsException<ArgumentException>(() => book.DeleteBookEntry(1));

            // Assert
            ae.Message.Should().Be($"Book entry with id \"{1}\" does not exist.");
        }

        [DataTestMethod]
        [DataRow(BookEntryType.Deposit)]
        [DataRow(BookEntryType.Payout)]
        public void TestDeleteBookEntry(BookEntryType entryType)
        {
            // Arrange
            BookOfHousholdAccounts book = new(_folderpath, "book1");
            string categoryName = "category1";
            decimal amount = 11.11M;
            string memo = "memo1";

            book.AddCategory(entryType, categoryName);
            if (entryType == BookEntryType.Deposit)
                book.AddDepositBookEntry(categoryName, amount, memo, null);
            else
                book.AddPayoutBookEntry(categoryName, amount, memo, null);

            // Act
            book.DeleteBookEntry(1);

            // Assert
            book.Entries.Count.Should().Be(0);
        }

        //[TestMethod]
        //public void TestSaveToAndLoadFromFile()
        //{
        //    BooksOfHouseholdAccounts books = new();
        //    books.AddBook("book1");
        //    books.AddBook("book2");
        //    books.AddBook("book3");
        //    books.DeleteBook("book2");
        //    books.SaveToFile(_folderpath);

        //    BooksOfHouseholdAccounts loadedBooks = new();
        //    loadedBooks.LoadFromFile(_folderpath);

        //    loadedBooks.Should().BeEquivalentTo(books);
        //}
    }
}
