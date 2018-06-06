using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Neuroshima
{
        public interface ITester
        {
            int Result();
            void LoadTestValues(TextBox currentAttributeBox, TextBox currentSkillBox, TextBox currentPenaltyPercentageBox);
            void LoadDices(TextBox diceBox1, TextBox diceBox2, TextBox diceBox3);
        }
}
