using System;

class Yatzy
{
    Random rand = new Random(); //Slumpar 
    int[] dice = new int[5]; //En array som håller tärningarnas värden
    bool[] saved = new bool[5];
    int[] scoreBoard = new int[13]; // Poängtavla för olika kategorier
    string[] categories = { "Ettor", "Tvåor", "Treor", "Fyror", "Femmor", "Sexor", "Par", "Två par", "Triss", "Fyrtal", "Liten stege", "Stor stege", "Kåk" };

    void PlayGame()
    {    
        for (int i = 0; i < 13; i++) //sätter alla kategorier till -1
        {
            scoreBoard[i] = -1;
        }

        Console.WriteLine("Välkommen till Yatzy!");

        for (int round = 0; round < 13; round++) // kör 13 rundor
        {
            Console.WriteLine("\nRunda " + (round + 1));
            PlayRound(); //Spelar en runda

            Console.WriteLine("Välj en kategori att spara din poäng i:"); //Visar alla kategorierna som är tillgängliga
            for (int i = 0; i < categories.Length; i++)
            {
                int possibleScore = CalculateScore(i); //Räknar ut poängen för kategorirna
                if (scoreBoard[i] == -1)
                {
                    Console.WriteLine((i + 1) + ". " + categories[i] + " - Möjlig poäng: " + possibleScore);
                }
                else
                {
                    Console.WriteLine((i + 1) + ". " + categories[i] + " (Redan använd: " + scoreBoard[i] + " poäng)");
                }
            }

            int categoryIndex = -1; //låter spelaren välja en kategori som inte är använd
            bool valid = false;
            while (!valid)
            {
                Console.Write("Ange siffran för önskad kategori: ");
                string input = Console.ReadLine();
                categoryIndex = Convert.ToInt32(input) - 1;
                if (categoryIndex >= 0 && categoryIndex < 13 && scoreBoard[categoryIndex] == -1)
                {
                    valid = true;
                }
                else
                {
                    Console.WriteLine("Ogiltigt val eller kategori redan använd. Försök igen.");
                }
            }

            scoreBoard[categoryIndex] = CalculateScore(categoryIndex); //Sparar poäng i vald kategori
            Console.WriteLine("Poäng för " + categories[categoryIndex] + ": " + scoreBoard[categoryIndex]);
        }

        int totalScore = 0; //När spelet är klart räknar den ihop alla poäng
        for (int i = 0; i < 13; i++)
        {
            totalScore += scoreBoard[i];
        }
        Console.WriteLine("Spelet är slut! Din totala poäng: " + totalScore);
    }

    void PlayRound()
    {
        int rolls = 0; //Hur många gånger man slagit

        while (rolls < 3)//Max 3 slag per runda
        {
            for (int i = 0; i < 5; i++)
            {
                if (!saved[i])
                {
                    dice[i] = rand.Next(1, 7); //Slå tärningen om den inte är sparad
                }
            }

            Console.Write("Tärningar: ");
            for (int i = 0; i < 5; i++)
            {
                Console.Write(dice[i] + " ");
            }
            Console.WriteLine();

            if (rolls < 2)//Bara fråga om sparning efter de första 2 slagen
            {
                Console.Write("Ange vilka tärningar du vill spara (ex: 1 3 5): ");
                string input = Console.ReadLine();
                string[] choices = input.Split(' ');

                for (int i = 0; i < 5; i++)
                {
                    saved[i] = false;//Börja med att resetta
                }

                for (int i = 0; i < choices.Length; i++)//Markera vilka tärningar som ska sparas
                {
                    int index = Convert.ToInt32(choices[i]);
                    if (index >= 1 && index <= 5)
                    {
                        saved[index - 1] = true;
                    }
                }
            }
            rolls++; //Ett slag klart
        }

        Console.WriteLine("Rundan är slut!");
        for (int i = 0; i < 5; i++)
        {
            saved[i] = false;//Återställ sparade tärningar till nästa runda
        }
    }

    int CalculateScore(int category)//Beräkna poängen beroende på kategorin
    {
        int[] counts = new int[7];
        for (int i = 0; i < 5; i++)
        {
            counts[dice[i]]++;
        }

        if (category >= 0 && category <= 5) //ettor till 6or
        {
            return counts[category + 1] * (category + 1);
        }
        else if (category == 6) // Par
        {
            for (int i = 6; i >= 1; i--)
            {
                if (counts[i] >= 2) return i * 2;
            }
        }
        else if (category == 7) // Två par
        {
            int pairs = 0;
            int score = 0;
            for (int i = 6; i >= 1; i--)
            {
                if (counts[i] >= 2)
                {
                    pairs++;
                    score += i * 2;
                }
            }
            if (pairs == 2) return score;
        }
        else if (category == 8) // Triss
        {
            for (int i = 6; i >= 1; i--)
            {
                if (counts[i] >= 3) return i * 3;
            }
        }
        else if (category == 9) // Fyrtal
        {
            for (int i = 6; i >= 1; i--)
            {
                if (counts[i] >= 4) return i * 4;
            }
        }
        else if (category == 10) // Liten stege
        {
            if (counts[1] == 1 && counts[2] == 1 && counts[3] == 1 && counts[4] == 1 && counts[5] == 1) return 15;
        }
        else if (category == 11) // Stor stege
        {
            if (counts[2] == 1 && counts[3] == 1 && counts[4] == 1 && counts[5] == 1 && counts[6] == 1) return 20;
        }
        else if (category == 12) // Kåk
        {
            bool hasThree = false;
            bool hasTwo = false;
            for (int i = 1; i <= 6; i++)
            {
                if (counts[i] == 3) hasThree = true;
                if (counts[i] == 2) hasTwo = true;
            }
            if (hasThree && hasTwo) return 25;
        }
        return 0;//Om inget passar blir det noll poäng
    }

    static void Main()
    {
        Yatzy game = new Yatzy();//Skapar ett nytt spel
        game.PlayGame();// Startar spelet
    }
}
