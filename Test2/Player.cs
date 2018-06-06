using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace Neuroshima
{



    public struct PlayerData
    {
        public const int SkillCount = 54;
        public const int AttributeCount = 5;
        public int[] Skill;
        public int[] Attribute;
        public int Penalty;
        public string Name;
    }

    public class Player :IPlayer
    {
        public PlayerData PlayerData;

        public override string ToString()
        {
            return PlayerData.Name;
        }

        public Player(string name, ObservableCollection<Player> collectionOfPlayers)
        {
            PlayerData.Name = name;
            PlayerData.Skill = new int[PlayerData.SkillCount];
            PlayerData.Attribute = new int[PlayerData.AttributeCount];
            collectionOfPlayers.Add(this);
        }

        public Player()
        {

        }


    }


}
