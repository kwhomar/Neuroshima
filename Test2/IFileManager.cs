﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Neuroshima
{
    interface IFileManager
    {
        void SavePlayers(ListBox gracze);
        ObservableCollection<Player> LoadPlayers();
    }
}
