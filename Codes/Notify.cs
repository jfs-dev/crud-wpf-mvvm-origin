using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace crud_wpf_mvvm_origin.Codes;

public class Notify<T> : INotifyPropertyChanging, INotifyPropertyChanged, INotifyDataErrorInfo
{
    private Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();
    public event PropertyChangingEventHandler? PropertyChanging;
    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    protected virtual void OnPropertyChanging(string propertyName)
    {
        PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        ValidateProperty(propertyName);
    }

    protected virtual void OnErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }

    IEnumerable INotifyDataErrorInfo.GetErrors(string? propertyName)
    {
        if (propertyName == null)
        {
            return errors.Values.SelectMany(list => list);
        }

        if (errors.ContainsKey(propertyName))
        {
            return errors[propertyName];
        }

        return Enumerable.Empty<string>();
    }

    public bool HasErrors
    {
        get { return errors != null && errors.Count > 0; }
    }

    private void AddError(string propertyName, string error)
    {
        if (!errors.ContainsKey(propertyName))
        {
            errors[propertyName] = new List<string>();
        }

        if (!errors[propertyName].Contains(error))
        {
            errors[propertyName].Add(error);

            OnErrorsChanged(propertyName);
            OnPropertyChanged(nameof(HasErrors));
        }
    }

    private void RemoveError(string propertyName, string error)
    {
        if (errors.ContainsKey(propertyName) && errors[propertyName].Contains(error))
        {
            errors[propertyName].Remove(error);
            if (errors[propertyName].Count == 0)
            {
                errors.Remove(propertyName);
            }

            OnErrorsChanged(propertyName);
            OnPropertyChanged(nameof(HasErrors));
        }
    }

    private void ClearErrors(string propertyName)
    {
        if (errors.ContainsKey(propertyName))
        {
            errors.Remove(propertyName);
    
            OnErrorsChanged(propertyName);
            OnPropertyChanged(nameof(HasErrors));
        }
    }    

    private void ValidateProperty(string propertyName)
    {
        ClearErrors(propertyName);

        var validationContext = new ValidationContext(this, null, null) { MemberName = propertyName };
        var validationResults = new List<ValidationResult>();

        var propertyValue = GetType().GetProperty(propertyName)?.GetValue(this);
        if (propertyValue != null)

        if (!Validator.TryValidateProperty(propertyValue, validationContext, validationResults))
        {
            foreach (var validationResult in validationResults)
            {
                if (validationResult.ErrorMessage != null) AddError(propertyName, validationResult.ErrorMessage);
            }
        }
    }    

    public void ValidateAllProperties()
    {
        var properties = typeof(T).GetProperties();

        foreach (var property in properties)
        {
            ValidateProperty(property.Name);
        }
    }
}
