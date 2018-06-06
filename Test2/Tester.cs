using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Neuroshima
{
    public class Tester :ITester
    {
        enum DifficultyPoints
        {
            Latwy = 2,
            Normalny = 0,
            Problematyczny = -2,
            Trudny = -5,
            BardzoTrudny = -8,
            CholernieTrudny = -11,
            Fart = -15,
            Mistrzowski = -20,
            Arcymistrzowski = -24
        };

        int currentPenaltyPercentage;
        int currentSkill;
        int currentAttribute;
        const int numberOfDices = 3;
        int[] dices = new int[numberOfDices];

        public int Result()
        {
            DifficultyPoints difficultyPoints = PenaltyPercentageToDifficultyPoints();
            currentAttribute += Convert.ToInt32(difficultyPoints);
            int finalTestResult = OneAndTwentyBonus() + SkillBonus();
            int testPoints = currentAttribute - DiceAfterSkillUse();
            finalTestResult += ResultPoints(testPoints);
            return finalTestResult;
        }

        DifficultyPoints PenaltyPercentageToDifficultyPoints()
        {
            if (currentPenaltyPercentage < 0) return DifficultyPoints.Latwy;
            else if (currentPenaltyPercentage <= 10) return DifficultyPoints.Normalny;
            else if (currentPenaltyPercentage <= 30) return DifficultyPoints.Problematyczny;
            else if (currentPenaltyPercentage <= 60) return DifficultyPoints.Trudny;
            else if (currentPenaltyPercentage <= 90) return DifficultyPoints.BardzoTrudny;
            else if (currentPenaltyPercentage <= 120) return DifficultyPoints.CholernieTrudny;
            else if (currentPenaltyPercentage <= 160) return DifficultyPoints.Fart;
            else if (currentPenaltyPercentage <= 200) return DifficultyPoints.Mistrzowski;
            else return DifficultyPoints.Arcymistrzowski;
        }

        int OneAndTwentyBonus()
        {
            int bonus = 0;
            for (int i = 0; i < numberOfDices; i++)
                if (dices[i] == 1) bonus++;
                else if (dices[i] == 20) bonus--;
            return bonus;
        }

        int SkillBonus()
        {
            const int skillLevelThreshold = 4;
            int bonus = 0;
            if (currentSkill == 0) bonus--;
            for (int i = currentSkill; i - skillLevelThreshold >= 0; i -= skillLevelThreshold)
                bonus++;
            return bonus;
        }

        int DiceAfterSkillUse()
        {
            int usedDice1, usedDice2, higherDice;
            RemoveHighestDice();
            usedDice1 = dices[0];
            usedDice2 = dices[1];
            int skillPointsLeft = currentSkill;
            while (skillPointsLeft > 0 && NotBothZero(usedDice1, usedDice2))
            {
                if (usedDice1 >= usedDice2)
                    usedDice1--;
                else if (usedDice1 < usedDice2)
                    usedDice2--;
                skillPointsLeft--;
            }
            if (usedDice1 < usedDice2)
                higherDice = usedDice2;
            else
                higherDice = usedDice1;
            return higherDice;
        }

        void RemoveHighestDice()
        {
            if ((dices[0] >= dices[1]) && (dices[0] >= dices[2]))
                dices[0] = dices[2];
            else if ((dices[1] >= dices[0]) && (dices[1] >= dices[2]))
                dices[1] = dices[2];
        }

        static bool NotBothZero(int term1, int term2)
        {
            return (term1 * term2 > 0);
        }

        static int ResultPoints(int testPoints)
        {

            if (testPoints + 2 < 0) return -2;
            else if (testPoints < 0) return -1;
            else if (testPoints < 2) return 0;
            else if (testPoints < 5) return 1;
            else if (testPoints < 8) return 2;
            else if (testPoints < 11) return 3;
            else if (testPoints < 15) return 4;
            else if (testPoints < 20) return 5;
            else if (testPoints < 24) return 6;
            else return 7;
        }
        
        public void LoadDices(TextBox diceBox1, TextBox diceBox2, TextBox diceBox3)
        {
            if (!String.IsNullOrEmpty(diceBox1.Text))
                dices[0] = Convert.ToInt32(diceBox1.Text);
            if (!String.IsNullOrEmpty(diceBox2.Text))
                dices[1] = Convert.ToInt32(diceBox2.Text);
            if (!String.IsNullOrEmpty(diceBox3.Text))
                dices[2] = Convert.ToInt32(diceBox3.Text);
        }

        public void LoadTestValues(TextBox currentAttributeBox, TextBox currentSkillBox, TextBox currentPenaltyPercentageBox)
        {
            if (!String.IsNullOrEmpty(currentPenaltyPercentageBox.Text))
                currentPenaltyPercentage = Convert.ToInt32(currentPenaltyPercentageBox.Text);
            if (!String.IsNullOrEmpty(currentSkillBox.Text))
                currentSkill = Convert.ToInt32(currentSkillBox.Text);
            if (!String.IsNullOrEmpty(currentAttributeBox.Text))
                currentAttribute = Convert.ToInt32(currentAttributeBox.Text);
        }

        public Tester()
        {

        }
    }
}
