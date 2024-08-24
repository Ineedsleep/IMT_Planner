using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MVVM
{
    /// <inheritdoc />
    /// <summary>
    ///   Viewmodelbase class without a model.
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool Set<T>(ref T field, T newValue = default, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }

            field = newValue;

            OnPropertyChanged(propertyName);

            return true;
        }
        public virtual void Dispose() { }
    }

    public class ViewModelBase<TModel> : ViewModelBase<TModel, TModel>
      where TModel : class
    {
    }

    /// <inheritdoc />
    /// <summary>
    /// Viewmodelbase class with Model.
    /// </summary>
    /// <typeparam name="TModel">Model of viewmodel.</typeparam>
    public class ViewModelBase<TInterfaceModel, TModel> : ViewModelBase
        where TModel : class, TInterfaceModel
        where TInterfaceModel : class
    {
        private TInterfaceModel _model;

        protected ViewModelBase()
        {
            Model = Activator.CreateInstance<TModel>();
        }

        public TInterfaceModel Model
        {
            get => _model;
            set
            {
                if (Model == value) return;
                // get all properties
                var properties = GetType().GetProperties(BindingFlags.Public);
                // all values before the model has changed
                var oldValues = properties.Select(p => p.GetValue(this, null));

                using (var enumerator = oldValues.GetEnumerator())
                {
                    _model = value;

                    // call OnPropertyChanged for all changed properties
                    foreach (var property in properties)
                    {
                        enumerator.MoveNext();
                        var oldValue = enumerator.Current;
                        var newValue = property.GetValue(this, null);

                        if (oldValue != null && (newValue == null || (!oldValue.Equals(newValue))))
                        {
                            OnPropertyChanged(property.Name);
                        }
                    }
                   
                }
                OnPropertyChanged();
            }
        }
    }
}