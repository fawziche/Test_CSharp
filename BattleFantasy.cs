using System;

public enum EtatEtreVivant {vivant, dead}

public enum Deplacement {Bas, Haut, Gauche, Droite}

public class Entite
{
    public int coordX;
    public int coordY;
    public int PV;
    public int MP;
    public int atq;
    public int def;
    public char corps;

    public Entite()  {this.corps = ' ';}
    public Entite(int arPosX, int arPosY)  {this.coordX=arPosX; this.coordY=arPosY; this.corps = ' ';}

    public virtual char afficher() {return this.corps;}
}

public class Fiole : Entite
{
    public Fiole(int arPosX, int arPosY):base(arPosX, arPosY)  {this.PV = 3;this.corps='F';}
}

public class Potion : Entite
{
    public Potion(int arPosX, int arPosY):base(arPosX, arPosY)  {this.MP = 3;this.corps='P';}
}

public class EtreVivant : Entite
{
    public EtatEtreVivant etat;
    public int pas;

    public EtreVivant(int arPosX, int arPosY):base(arPosX, arPosY)
    {
        this.PV = 10;
        this.MP = 10;
        this.atq = 1;
        this.def = 1;

        this.etat = EtatEtreVivant.vivant;
        this.pas = 1;
    }
}


public class Heros : EtreVivant
{
    public Heros (int arPosX, int arPosY):base(arPosX, arPosY)
    {
        this.corps = 'H';
    }

    public int attaquer()
    {
        return this.atq;
    }
}

public class Monstre : EtreVivant
{
    public Monstre (int arPosX, int arPosY):base(arPosX, arPosY)
    {
        this.PV = 3;
        this.MP = 3;
        this.corps = 'M';
    }

    public int attaquer() {return this.atq;}

    public override char afficher() {return char.Parse(this.PV.ToString());}
}


public class Jeu
{
    public Entite[,] terrain;
    public string msg;

    public Heros unHeros;

    public Jeu()
    {
        this.terrain = new Entite[10, 10];
        initJeu();

        this.msg = "";

        this.unHeros = new Heros(5,5);
        placer(unHeros);
    }

    public void initJeu()
    {
        for (int i=0; i < 10; i++)
            for (int j=0; j<10; j++)
                this.terrain[i,j] = null;
    }

    public void afficheEtatJeu()
    {
        char tmp;

        // Msg alerte / info
        if (this.msg != "")
            Console.WriteLine("Info : " + this.msg);

        // Statut du heros
        Console.WriteLine($"PV : {unHeros.PV} / MP : {unHeros.MP}");

        // MAP
        for (int y=0; y < 10; y++)
        {
            Console.Write(y.ToString() + "-");
            for (int x=0; x<10; x++)
            {
                tmp = (this.terrain[x,y]==null)?' ':this.terrain[x,y].afficher();
                Console.Write(tmp + "-");
            }
            Console.WriteLine(" ");
        }

        // Action possible
        Console.WriteLine("Quelle est votre action : X (quitter) O (haut), L (bas), K (gauche), M (droite), A (atq) :");
    }

    public void placer(Entite elt)
    {
        this.terrain[elt.coordX, elt.coordY] = elt;
    }

    public void attaqueHeros()
    {
        if (this.unHeros.MP > 0)
        {
            this.unHeros.MP--;

            attaqueZone(this.unHeros.coordX + 1, this.unHeros.coordY - 1); //droite haut
            attaqueZone(this.unHeros.coordX + 1, this.unHeros.coordY    ); //droite
            attaqueZone(this.unHeros.coordX + 1, this.unHeros.coordY + 1); //droite bas
            attaqueZone(this.unHeros.coordX    , this.unHeros.coordY + 1); //bas
            attaqueZone(this.unHeros.coordX - 1, this.unHeros.coordY + 1); //gauche bas
            attaqueZone(this.unHeros.coordX - 1, this.unHeros.coordY    ); //gauche
            attaqueZone(this.unHeros.coordX - 1, this.unHeros.coordY - 1); //gauche haut
            attaqueZone(this.unHeros.coordX    , this.unHeros.coordY - 1); //haut
        }
    }

    public void attaqueZone(int arPosX, int arPosY)
    {
        Monstre unMonstre;

        // Easter egg
        if (arPosX == -1 && arPosY == -1)
            this.msg = "@Fawsoft company";

        if (arPosX >= 0 && arPosX <= 9 && arPosY >= 0 && arPosY <= 9)
        {
            // S'il y a quelque chose ...
            if (this.terrain[arPosX,arPosY] != null)
            {
                // ... et que ce quelque chose est un monstre ...
                if (this.terrain[arPosX,arPosY].GetType().Name == "Monstre")
                {
                    // ... il est blessé
                    unMonstre = (Monstre) this.terrain[arPosX,arPosY];
                    unMonstre.PV -= this.unHeros.attaquer();
             
                    // ... et S'il n'a plus de PV, il sort du terrain
                    if (unMonstre.PV <= 0)
                        this.terrain[arPosX,arPosY] = null;
                }
            }
        }
    }

    public void deplacementHeros (Deplacement sens)
    {
        // Calcul de la nouvelle position sur le terrain
        int newPosX = unHeros.coordX;
        int newPosY = unHeros.coordY;

        switch (sens)
        {
            case Deplacement.Droite :
                if (unHeros.coordX + unHeros.pas <= 9) 
                    newPosX = unHeros.coordX + unHeros.pas;
                break;

            case Deplacement.Gauche :
                if (unHeros.coordX - unHeros.pas >= 0) 
                    newPosX = unHeros.coordX - unHeros.pas;
                break;

            case Deplacement.Haut :
                if (unHeros.coordY - unHeros.pas >= 0)
                    newPosY = unHeros.coordY - unHeros.pas;
                break;

            case Deplacement.Bas :
                if (unHeros.coordY + unHeros.pas <= 9)
                    newPosY = unHeros.coordY + unHeros.pas;
                break;
        }
        
        // Interaction avec une entité ?
            // Si monstre ...
            if (this.terrain[newPosX,newPosY] != null && this.terrain[newPosX,newPosY].GetType().Name == "Monstre")
            {
                // Perte de PV
                unHeros.PV -= ((Monstre) this.terrain[newPosX,newPosY]).atq;

                // Le heros ne bouge pas
                newPosX = unHeros.coordX;
                newPosY = unHeros.coordY;
            }

            // Si fiole ...
            if (this.terrain[newPosX,newPosY] != null && this.terrain[newPosX,newPosY].GetType().Name == "Fiole")
            {
                //Gagne les PV de la fiole
                unHeros.PV += ((Fiole) this.terrain[newPosX,newPosY]).PV;

                //La fiole sort du terrain
                this.terrain[newPosX,newPosY] = null;
            }

            // Si potion ...
            if (this.terrain[newPosX,newPosY] != null && this.terrain[newPosX,newPosY].GetType().Name == "Potion")
            {
                //Gagne les MP de la potion
                unHeros.MP += ((Potion) this.terrain[newPosX,newPosY]).MP;

                //La potion sort du terrain
                this.terrain[newPosX,newPosY] = null;
            }
        

        //Deplacement du héros vers le nouvel emplacement calculé
        this.terrain[unHeros.coordX,unHeros.coordY] = null;
        this.terrain[newPosX,newPosY] = unHeros;

        //Svg de son nouvel emplacement
        unHeros.coordX = newPosX;
        unHeros.coordY = newPosY;
    }

    public void jouer()
    {
        string saisie;
        bool herosEstMort = false;

        // Affichage et saisie
        this.afficheEtatJeu();
        saisie = Console.ReadLine().ToUpper();
        
        // Jeu
        while (saisie != "X" && !herosEstMort)
        {
            this.msg = "";

            switch (saisie)
            {
                case "O" : deplacementHeros(Deplacement.Haut); break;
                case "L" : deplacementHeros(Deplacement.Bas); break;
                case "M" : deplacementHeros(Deplacement.Droite); break;
                case "K" : deplacementHeros(Deplacement.Gauche); break;
                case "A" : attaqueHeros(); break;
                default : this.msg = "mauvaise saisie"; break;
            }

            if (unHeros.PV <= 0)
            {
                // Info
                this.msg = "Heros mort !";

                // Le héros sort du terrain
                this.terrain[unHeros.coordX,unHeros.coordY] = null;

                herosEstMort = true;
            }


            // Affichage et saisie
            this.afficheEtatJeu();
            saisie = Console.ReadLine().ToUpper();
        }
    }
}






public class BattleFantasy
{
    public static void Test()
    {
        Jeu unJeu = new Jeu();
        
        // Paramétrage du jeu
        unJeu.unHeros.PV = 12;
        unJeu.placer (new Monstre(7,3));
        unJeu.placer (new Monstre(3,7));
        unJeu.placer (new Monstre(2,6));
        unJeu.placer (new Fiole(2,2));
        unJeu.placer (new Potion(9,0));

        // Jouer
        unJeu.jouer();

    }
}