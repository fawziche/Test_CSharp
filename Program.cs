using System;


public class Principale
{
    public static void Main(string[] args)
    {
        try
        {
            BattleFantasy.Test();

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

    }
}