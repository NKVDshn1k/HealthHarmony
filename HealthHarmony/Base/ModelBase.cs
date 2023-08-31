using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HealthHarmony.Base
{
    public class ModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        protected virtual bool Set<User>(ref User field, User value, [CallerMemberName] string PropertyName = null)
        {
            if (Equals(field, value)) 
                return false;
            field = value;
            OnPropertyChanged(PropertyName);
            return true;
        }
    }
}
