using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ToDo.WebAPI.Services
{
    public abstract class ValidatableBindableBase : BindableBase, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> errors = [];
        public IEnumerable<string> AllErrors => errors.SelectMany(e => e.Value);
        public bool HasErrors => errors.Any();
        public string FirstError => AllErrors.FirstOrDefault();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return string.IsNullOrWhiteSpace(propertyName)
                ? errors.SelectMany(e => e.Value)
                : errors.TryGetValue(propertyName, out var error)
                    ? error
                    : null!;
        }

        protected override bool SetProperty<T>(ref T storage, T value,
        [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            var changed = base.SetProperty(ref storage, value, propertyName);

            if (changed)
                ValidateProperty(value, propertyName);

            return changed;
        }

        protected virtual void ValidateProperty(object value, string propertyName)
        {
            ClearErrors(propertyName);

            var context = new ValidationContext(this)
            {
                MemberName = propertyName
            };

            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            Validator.TryValidateProperty(value, context, results);

            foreach (var result in results)
                AddError(propertyName, result.ErrorMessage);
        }

        protected void AddError(string propertyName, string error)
        {
            if (!errors.ContainsKey(propertyName))
                errors[propertyName] = new();

            errors[propertyName].Add(error);
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            RaisePropertyChanged(nameof(AllErrors));
            RaisePropertyChanged(nameof(HasErrors));
            RaisePropertyChanged(nameof(FirstError));
        }

        protected void ClearErrors(string propertyName)
        {
            if (errors.Remove(propertyName))
            {
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
                RaisePropertyChanged(nameof(AllErrors));
                RaisePropertyChanged(nameof(HasErrors));
                RaisePropertyChanged(nameof(FirstError));

            }
        }

        protected bool HasErrored(string propertyName)
        {
            var result = AllErrors.Where((e) => nameof(e).Equals(propertyName));
            return result.Any();
        }
    }
}