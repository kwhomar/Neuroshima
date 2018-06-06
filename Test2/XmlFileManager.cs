using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.IO;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace Neuroshima
{
    class XmlFileManager : IFileManager
    {
        public void SavePlayers(ListBox gracze)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Player>));
            TextWriter textwriter = new StreamWriter("gracze.xml");
            serializer.Serialize(textwriter, gracze.ItemsSource);
            textwriter.Close();   
        }

        public ObservableCollection<Player> LoadPlayers()
        {
            ObservableCollection<Player> collectionOfPlayers;
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Player>));
            TextReader textreader = new StreamReader("gracze.xml");
            collectionOfPlayers = serializer.Deserialize(textreader) as ObservableCollection<Player>;
            textreader.Close();
            return collectionOfPlayers;
        }

        public XmlFileManager(ListBox gracze)
        {
        }
    }
}
