using BohaLibrary.Models;

namespace BohaLibrary
{
    public class BookOfHousholdAccounts(string path, string bookName)
    {
        public string FilePath { get; private set; } = Path.Combine(path, bookName + ".json");
        public List<Category> Categories { get; private set; } = [];
        public List<BookEntryModel> Entries { get; private set; } = [];

        public void AddBookEntry(BookEntryType entryType, string categoryName, decimal amount, string memo, DateTime? postingDate)
        {
            if (categoryName == string.Empty)
                throw new ArgumentException("Category name must not be an empty string.");

            if (amount <= 0)
                throw new ArgumentException($"Invalid amount \"{amount}\" - has to be positive.");

            if (!Categories.Any(c => c.Equals(new() { EntryType = entryType, Name = categoryName })))
                throw new ArgumentException($"{entryType} category \"{categoryName}\" does not exist.");

            DateTime postingDateForEntry;
            if (postingDate == null)
                postingDateForEntry = DateTime.Now;
            else
                postingDateForEntry = (DateTime)postingDate;
            postingDateForEntry = new DateTime(postingDateForEntry.Year, postingDateForEntry.Month, postingDateForEntry.Day);

            int id = Entries.OrderBy(e => e.Id).Select(e => e.Id).LastOrDefault() + 1;

            Entries.Add(new BookEntryModel()
            {
                Id = id,
                EntryType = entryType,
                CategoryName = categoryName,
                Amount = amount,
                MemoText = memo,
                PostingDate = postingDateForEntry
            });
        }

        public void AddCategory(BookEntryType bookEntryType, string categoryName)
        {
            Category newCategory = new() { EntryType = bookEntryType, Name = categoryName };

            if (Categories.Any(c => c.Equals(newCategory)))
                throw new ArgumentException($"{bookEntryType} category \"{categoryName}\" already exists.");

            Categories.Add(newCategory);
        }

        public void AddDepositBookEntry(string categoryName, decimal amount, string memo, DateTime? postingDate)
            => AddBookEntry(BookEntryType.Deposit, categoryName, amount, memo, postingDate);

        public void AddPayoutBookEntry(string categoryName, decimal amount, string memo, DateTime? postingDate)
            => AddBookEntry(BookEntryType.Payout, categoryName, amount, memo, postingDate);

        public void DeleteBookEntry(int id)
        {
            BookEntryModel? entry = Entries.FirstOrDefault(e => e.Id == id)
                ?? throw new ArgumentException($"Book entry with id \"{id}\" does not exist.");

            Entries.Remove(entry);
        }

        public void DeleteCategory(BookEntryType bookEntryType, string categoryName)
        {
            Category categoryToDelete = new() { EntryType = bookEntryType, Name = categoryName };

            if (!Categories.Remove(categoryToDelete))
                throw new ArgumentException($"{bookEntryType} category \"{categoryName}\" does not exist.");
        }
    }
}