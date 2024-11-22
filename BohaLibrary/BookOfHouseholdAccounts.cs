using BohaLibrary.Models;

namespace BohaLibrary
{
    public class BookOfHouseholdAccounts(string path, string bookName)
    {
        public string FilePath { get; private set; } = Path.Combine(path, bookName + ".json");
        public string Name { get; private set; } = bookName;
        public List<Category> Categories { get; private set; } = [];
        public List<BookEntryModel> Entries { get; private set; } = [];

        /// <summary>
        /// Adds a book entry to the list of entries, if the given parameters are valid.
        /// </summary>
        /// <param name="entryType">The entry type of this entry (Deposit or Payout).</param>
        /// <param name="categoryName">The name of the category of this entry.</param>
        /// <param name="amount">The amount for this entry. Has to be a positive value.</param>
        /// <param name="memo">A optional memo text for this entry.</param>
        /// <param name="postingDate">The date of this entry.</param>
        /// <exception cref="ArgumentException">Thrown, if category name is missing or invalid or if amount is invalid.</exception>
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

        /// <summary>
        /// Adds a category to the list of categories, if the given parameters are valid.
        /// </summary>
        /// <param name="bookEntryType">The entry type this category is valid for (Deposit or Payout).</param>
        /// <param name="categoryName">The name of the category.</param>
        /// <exception cref="ArgumentException">Thrown, if the category already exists.</exception>
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

        /// <summary>
        /// Deletes the book entry identified by the given id.
        /// </summary>
        /// <param name="id">The id of the entry to delete.</param>
        /// <exception cref="ArgumentException">Thrown, if there is no entry with the given id.</exception>
        public void DeleteBookEntry(int id)
        {
            BookEntryModel? entry = Entries.FirstOrDefault(e => e.Id == id)
                ?? throw new ArgumentException($"Book entry with id \"{id}\" does not exist.");

            Entries.Remove(entry);
        }

        /// <summary>
        /// Adds the given category from the list of categories, if the given parameters are valid.
        /// </summary>
        /// <param name="bookEntryType">The entry type this category is valid for (Deposit or Payout).</param>
        /// <param name="categoryName">The name of the category.</param>
        /// <exception cref="ArgumentException">Thrown, if the category does not exist.</exception>
        public void DeleteCategory(BookEntryType bookEntryType, string categoryName)
        {
            Category categoryToDelete = new() { EntryType = bookEntryType, Name = categoryName };

            if (!Categories.Remove(categoryToDelete))
                throw new ArgumentException($"{bookEntryType} category \"{categoryName}\" does not exist.");
        }

        /// <summary>
        /// Updates the book from the corresponding file.
        /// </summary>
        public void LoadFromFile()
        {
            BookOfHouseholdAccountsModel bookModel = BohaFileHelpers.ReadFromJsonFile<BookOfHouseholdAccountsModel>(FilePath);
            Name = bookModel.Name;
            Categories = bookModel.Categories.Select(c => new Category { EntryType = c.EntryType, Name = c.Name }).ToList();
            Entries = [.. bookModel.Entries];
        }

        /// <summary>
        /// Saves the book to the corresponding file.
        /// </summary>
        public void SaveToFile()
        {
            BookOfHouseholdAccountsModel bookModel = new()
            {
                Name = Name,
                Categories = Categories.Select(c => new CategoryModel { EntryType = c.EntryType, Name = c.Name }).ToArray(),
                Entries = [.. Entries]
            };

            BohaFileHelpers.WriteToJsonFile(FilePath, bookModel);
        }

        /// <summary>
        /// Deletes the corresponding file.
        /// </summary>
        public void DeleteFile() => BohaFileHelpers.DeleteFile(FilePath);
    }
}