﻿using BohaLibrary;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BohaWpf.ViewModels
{
    public partial class EditCategoriesViewModel : ObservableObject
    {
        // TODO - Update categories, when entry type changed

        private readonly BookOfHouseholdAccounts _book;

        public EditCategoriesViewModel(BookOfHouseholdAccounts book)
        {
            _book = book;
            LoadAndUpdateCategories();
        }

        [ObservableProperty]
        private List<string> entryTypes = [Properties.Literals.MainView_EntryTypes_Deposit, Properties.Literals.MainView_EntryTypes_Payout];

        [ObservableProperty]
        private string choosenEntryType = Properties.Literals.MainView_EntryTypes_Deposit;

        [ObservableProperty]
        private List<string> categories = [];

        [ObservableProperty]
        private string selectedCategory = string.Empty;

        [ObservableProperty]
        private string newCategory = string.Empty;

        [RelayCommand]
        private void DeleteCategory()
        {
            if (SelectedCategory == string.Empty)
                return;

            _book.DeleteCategory(ChoosenEntryType == Properties.Literals.MainView_EntryTypes_Deposit
                                   ? BookEntryType.Deposit
                                   : BookEntryType.Payout,
                                 SelectedCategory);
            _book.SaveToFile();
            LoadAndUpdateCategories();
        }

        [RelayCommand]
        private void AddCategory()
        {
            _book.AddCategory(ChoosenEntryType == Properties.Literals.MainView_EntryTypes_Deposit
                                ? BookEntryType.Deposit
                                : BookEntryType.Payout,
                              NewCategory);
            _book.SaveToFile();
            LoadAndUpdateCategories();
        }

        private void LoadAndUpdateCategories()
        {
            _book.LoadFromFile();
            Categories = _book.Categories.Where(c => c.EntryType == (ChoosenEntryType == Properties.Literals.MainView_EntryTypes_Deposit
                                                                       ? BookEntryType.Deposit
                                                                       : BookEntryType.Payout))
                                         .Select(c => c.Name)
                                         .ToList();
        }
    }
}