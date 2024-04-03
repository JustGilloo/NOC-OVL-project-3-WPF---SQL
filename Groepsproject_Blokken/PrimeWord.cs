
using System;

namespace Groepsproject_Blokken
{
    public class PrimeWord
    {
        string _primeword;
        string _hint;
        public PrimeWord()
        {

        }
        public PrimeWord(string primeword, string hint)
        {
            Primeword = primeword;
            Hint = hint;
        }
        public string Primeword
        {
            get { return _primeword; }
            set { _primeword = value; }
        }
        public string Hint
        {
            get { return _hint; }
            set { _hint = value; }
        }

        public bool CheckAnswerIfPrimeWord(string answer)
        {
            try //added a try catch, in case the answer would be not validated before it gets here
            {
                if (answer.ToLower() == Primeword)
                {
                    return true;
                }
                else
                { return false; }
            }
            catch { return false; }
        }

        //public void ReadAndFillPrimeWordList() //i want to make a method that will fill in a 2d array of (prime)words, read from a txt
        //{
        //    try
        //    {
        //        var filepath = "../../PrimeWords/List1.txt";
        //        try
        //        { 
        //            StreamReader readobject = new StreamReader(filepath);
        //            string[] temperaryLines;
        //            string[,] wordsThatAreParted; 
        //            var teller = 0;

        //            while (!readobject.EndOfStream)
        //            {
        //                readobject.ReadLine();
        //                if (readobject.ToString() == null || readobject.ToString() == "") //this is when txt1 is empty
        //                {
        //                    File.Copy("../../PrimeWords/List2.txt", "../../PrimeWords/List1.txt");
        //                    System.IO.File.WriteAllText("../../PrimeWords/file2.txt", string.Empty);
        //                }
        //                else //here i just need the lenght of the list, so i need to increase "teller"
        //                {
        //                    teller++;
        //                }
        //            }
        //            temperaryLines = new string[teller]; //now that i have teller, i can make my length
        //            for (int i = 0; i < teller; i++)
        //            {
        //                temperaryLines[i] = readobject.ReadLine();
        //            }
        //            wordsThatAreParted = new string[teller, 2];
        //            for (int i = 0; i < teller; i++)
        //            {
        //                string[] parts = temperaryLines[i].Split(';');
        //                if (parts.Length >= 2)
        //                {
        //                    wordsThatAreParted[i, 0] = parts[0];  // Left part this will be the primeword then
        //                    wordsThatAreParted[i, 1] = parts[1];  // Right part this will be the hint
        //                }
        //            }
        //        }
        //        catch (FileNotFoundException)
        //        {
        //            MessageBox.Show("File not found. Please check the file path", "Error");
        //        }
        //    }
        //    catch { }
        //}
        ////dit is gewoon placeholder code
        //public void RandomPrimeWordConstruction()
        //{
        //    string[,] wordsThatAreParted; 
        //    wordsThatAreParted = new string[10, 2];
        //    Random myRandom = new Random();
        //    PrimeWord myPrimeWord;
        //    var randomNumberToPickAndRemove = myRandom.Next(0, wordsThatAreParted.Length);
        //    myPrimeWord = new PrimeWord((wordsThatAreParted[randomNumberToPickAndRemove, 0]), (wordsThatAreParted[randomNumberToPickAndRemove, 1])); //here we have our random prime word with the random Hint
        //    string[,] writeAwayString = new string[wordsThatAreParted.Length - 1, 2]; 
        //    for (int i = 0, j = 0; i < wordsThatAreParted.GetLength(0); i++) //ik wil hier een 2d string opvullen zonder het gekozen woord
        //    {
        //        if (i == randomNumberToPickAndRemove)
        //            continue;

        //        for (int k = 0, u = 0; k < wordsThatAreParted.GetLength(1); k++)
        //        {
        //            if (k == randomNumberToPickAndRemove)
        //                continue;

        //            writeAwayString[j, u] = wordsThatAreParted[i, k];
        //            u++;
        //        }
        //        j++;
        //    }
        //    WriteAwayToTxtOne(writeAwayString);
        //    WriteAwayToTxtTwo(myPrimeWord);
        //    PrimeWordCuttingAndShowing();
        //}
        ////dit is gewoon placeholder code
        //private void WriteAwayToTxtTwo(PrimeWord wegschrijvenWord) //ik heb de Primeword nodig
        //{
        //    var filepath = "../../PrimeWords/List2.txt";
        //    string toWriteAway = "";
        //    StreamReader readobject = new StreamReader(filepath);
        //    string temperaryLines;
        //    while (!readobject.EndOfStream)
        //    {
        //        toWriteAway += readobject.ReadLine();
        //    }
        //    using (StreamWriter writer = new StreamWriter(filepath))
        //    {
        //        for (int i = 0; i < toWriteAway.Length; i++) //we write away first the lines that we aleady can read
        //        {
        //            writer.WriteLine(toWriteAway[i]);
        //        }
        //        temperaryLines = wegschrijvenWord.Primeword + ";" + wegschrijvenWord.Hint;
        //        writer.WriteLine(temperaryLines); //we write away the new used primeword
        //    }
        //}
        ////dit is gewoon placeholder code
        //private void WriteAwayToTxtOne(string[,] writeAwayString) //overschrijven List1 maar dan zonder de gekozen Primeword
        //{
        //    var filepath = "../../PrimeWords/List1.txt";
        //    using (StreamWriter writer = new StreamWriter(filepath))
        //    {
        //        for (int i = 0; i < writeAwayString.Length; i++) //we write away first the lines that we aleady can read
        //        {
        //            writer.WriteLine(writeAwayString[i, 0] + ";" + writeAwayString[i, 1]);
        //        }
        //    }
        //}
        ////dit is gewoon placeholder code
        ////laten we eens een methode maken om eenPrimeword to snippen en letter per letter te tonen
        public static void PrimeWordCuttingAndShowing(PrimeWord primeWord)
        {
            PrimeWord myPrimeWord = primeWord; //hier moet er een deel in de code ervoor of erbuiten of degelijke maar niet hier, deze gaat dus ook weg
            Random myRandom = new Random();
            char[] wordForDisplay = "________".ToCharArray(); //dit is wat we tonen op het scherm
            char[] versnipperdPrimeWord = new char[8]; //dit is wat we gaan onthouden en bijhouden voor ermee te werken
            versnipperdPrimeWord = myPrimeWord.Primeword.ToCharArray();
            int randomInt;
            bool reroll = true;
            randomInt = myRandom.Next(0, 8);
            while (reroll)
            {
                if (!(versnipperdPrimeWord[randomInt] == '_'))
                {
                    reroll = false;
                    wordForDisplay[randomInt] = versnipperdPrimeWord[randomInt];
                    versnipperdPrimeWord[randomInt] = '_';
                }
                else
                {
                    randomInt = myRandom.Next(0, 8);
                }
            }
            //als we dan uit de loop komen, dan is er een versnipperd woord in een array, en een array voor te showen waar er maar
        }
    }
}
