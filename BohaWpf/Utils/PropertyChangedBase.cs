using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BohaWpf.Utils;

/// <summary>
/// Taken from https://robbelroot.de/blog/der-ultimative-inotifypropertychanged-guide/#PropertyChangedBase-Klasse
/// </summary>
public abstract class PropertyChangedBase : INotifyPropertyChanged
{
    protected void NotifyOfPropertyChange([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}