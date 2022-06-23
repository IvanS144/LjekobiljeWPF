using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace Ljekobilje
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(String arg)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(arg));
        }

        public void Subscriber(object sender, PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
        }
    }
}
