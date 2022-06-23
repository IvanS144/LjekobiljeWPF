﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ljekobilje
{
    public class PlantProjection : BaseViewModel
    {
        private Plant plant;
        public PlantProjection(Plant plant)
        {
            Plant = plant;
        }
        public Plant Plant { get => plant; set { plant = value; base.NotifyPropertyChanged("Plant"); } }

        public int PlantId => Plant.PlantId;
        public decimal RetailPrice { get => Plant.RetailPrice; set { Plant.RetailPrice = value; base.NotifyPropertyChanged("RetailPrice"); } }
        public string PlantName { get => Plant.PlantName; set { Plant.PlantName = value; base.NotifyPropertyChanged("PlantName"); } }
        public string LatinName { get => Plant.LatinName; set { Plant.LatinName = value; base.NotifyPropertyChanged("LatinName"); } }
        public string MedicName { get => Plant.MedicName; set { Plant.MedicName = value; base.NotifyPropertyChanged("MedicName"); } }
    }
}
