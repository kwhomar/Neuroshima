using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Neuroshima
{
    class ModelView
    {
        static ModelView modelView;
        public static ModelView GetModelView()
        {
            CurrentSkill = "Nowy";
            if (modelView == null)
                modelView = new ModelView();
            return modelView;
        }
        public static ObservableCollection<Player> CollectionOfPlayers { get; set; }
        public static string CurrentSkill { get; set; }



    }
}
