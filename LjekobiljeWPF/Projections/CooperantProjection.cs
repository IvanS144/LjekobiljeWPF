using Ljekobilje;
using LjekobiljeWPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LjekobiljeWPF.Projections
{
    public class CooperantProjection : BaseViewModel
    {
        private Cooperant _cooperant;

        public CooperantProjection(Cooperant cooperant)
        {
            _cooperant = cooperant;
        }

        public Cooperant Cooperant { get => _cooperant; set { _cooperant = value; NotifyPropertyChanged("cooperant"); } }

        public int CooperantId { get => _cooperant.CooperantId; set { _cooperant.CooperantId = value; NotifyPropertyChanged("CooperantId"); } }
        public string FirstName { get => _cooperant.FirstName; set { _cooperant.FirstName = value; NotifyPropertyChanged("FirstName"); } }
        public string LastName { get => _cooperant.LastName; set { _cooperant.LastName = value; NotifyPropertyChanged("LastName"); } }
        public string Address { get => _cooperant.Adress; set { _cooperant.Adress = value; NotifyPropertyChanged("Address"); } }
        public string Email { get => _cooperant.Email; set { _cooperant.Email = value; NotifyPropertyChanged("Email"); } }
        public string City { get => _cooperant.City; set { _cooperant.City = value; NotifyPropertyChanged("City"); } }
        public int StationId { get => _cooperant.StationId; set { _cooperant.StationId = value; NotifyPropertyChanged("StationId"); } }
        public string Phone { get => _cooperant.Phone; set { _cooperant.Phone = value; NotifyPropertyChanged("Phone"); } }
    }
}
