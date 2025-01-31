using System;
using System.Xml.Linq;
using Microsoft.VisualBasic;

namespace Test
{
    public class Point
    {
        public int x;
        public int y;
        public int value;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.value = 1;
        }

        public void touche()
        {
            this.value = 0;
        }

        public string Affiche()
        {
            return $"Point {this.x} : {this.y} = {this.value}";
        }
    }

    public enum TypeNavire {destroyer, croiseur, cuirasse, porteAvion}
    public enum SensNavire {horizontal, vertical}

    public class Navire
    {        
        public IList<Point> listePoint;
        public TypeNavire type;


        public Navire(TypeNavire arType, Point arPosDepart, SensNavire arSens)
        {
            this.type = arType;

            this.listePoint = new List<Point>();
            int longueur = Navire.getLongueurNavire(arType);  
            if (arSens == SensNavire.horizontal)
            {
                for (int i = arPosDepart.x; i < arPosDepart.x + longueur; i++)
                     this.listePoint.Add(new Point(i, arPosDepart.y));
            }
            else
            {
                for (int i = arPosDepart.y; i < arPosDepart.y + longueur; i++)
                     this.listePoint.Add(new Point(arPosDepart.x, i));
            }
        }

        public static int getLongueurNavire (TypeNavire arType)
        {
            int longueur=0;

            switch (arType)
            {
                case TypeNavire.destroyer :
                    longueur = 2; break;
                case TypeNavire.croiseur :
                    longueur = 3; break;
                case TypeNavire.cuirasse :
                    longueur = 4; break;
                case TypeNavire.porteAvion :
                    longueur = 5; break;
            }

            return longueur;
        }
        public string msgTouche ()
        {
            string msg="";

            switch (this.type)
            {
                case TypeNavire.destroyer :
                    msg = "destroyer"; break;
                case TypeNavire.croiseur :
                    msg = "croiseur"; break;
                case TypeNavire.cuirasse :
                    msg = "cuirassé"; break;
                case TypeNavire.porteAvion :
                    msg = "porte avion"; break;
            }

            return msg;
        }

        public bool isDestroyed ()
        {
            bool res = true;

            for (int i = 0; i < this.listePoint.Count; i++)
            {
                if (this.listePoint[i].value > 0)
                    res = false;
            }

            return res;
        }
    }

    public class Terrain
    {
        public int tailleX;
        public int tailleY;
        public int[,] tabPos = new int[100, 100];
        ISet<Navire> armee = new HashSet<Navire>();

        public Terrain()
        {
            this.tailleX = 10;
            this.tailleY = 10;

            initTerrain();
        }

        public void initTerrain()
        {
            for (int y = 0; y < this.tailleY; y++)
                for (int x = 0; x < this.tailleX; x++)
                    tabPos[x, y] = 0;
        }

        public void afficheTerrain()
        {
            Console.WriteLine("    0 1 2 3 4 5 6 7 8 9");
            Console.WriteLine("    | | | | | | | | | |");
            for (int y = 0; y < this.tailleY; y++)
            {
                Console.Write(y + " - ");
                for (int x = 0; x < this.tailleX; x++)
                    Console.Write(tabPos[x, y] + " ");
                Console.WriteLine(" ");
            }
        }

        public void ajouterNavire (Navire unNavire)
        {
            // Ajoute le navire à l'armada
            armee.Add(unNavire);

            // On transpose les navires au terrain
            Point p;
            for (int i = 0; i < unNavire.listePoint.Count; i++)
            {
                p = unNavire.listePoint[i];
                tabPos[p.x,p.y] = p.value;
            }
        }

        public void attaquer(int arPosX, int arPosY)
        {
            //string msg = "";
            Point p;

            // Atq raté !
            if (tabPos[arPosX, arPosY] == 0)
                Console.WriteLine("raté !");
            else
            {
                // Pour chaque navire
                foreach (Navire unNavire in armee)
                {
                    // pour chaque "point"
                    for (int i = 0; i < unNavire.listePoint.Count; i++)
                    {
                        p = unNavire.listePoint[i];
                        if (p.x == arPosX && p.y == arPosY)
                        {
                            // Point détruit dans le navire
                            p.value = 0;

                            // Point détruit, donc, dans le terrain
                            tabPos[arPosX, arPosY] = 0;

                            // Msg de retour
                            Console.WriteLine(unNavire.msgTouche() + " touché en " + arPosX + " : " + arPosY);

                            if (unNavire.isDestroyed())
                                Console.WriteLine(unNavire.msgTouche() + " coulé ! ");
                        }
                    }
                }
            }
        }

    }




    public class BattleNaval
    {
        public static void Test()
        {
            try
            {
                Navire unPorteAvion = new Navire(TypeNavire.porteAvion, new Point(3,2), SensNavire.vertical);
                Navire unDestroyer = new Navire(TypeNavire.destroyer, new Point(5,2), SensNavire.horizontal);
                Navire unCroiseur = new Navire(TypeNavire.croiseur, new Point(8,4), SensNavire.vertical);
                Navire unCuirasse = new Navire(TypeNavire.cuirasse, new Point(4,8), SensNavire.horizontal);
                

                Terrain unTerrain = new Terrain();
                unTerrain.ajouterNavire(unPorteAvion);
                unTerrain.ajouterNavire(unDestroyer);
                unTerrain.ajouterNavire(unCroiseur);
                unTerrain.ajouterNavire(unCuirasse);

                unTerrain.afficheTerrain();

                unTerrain.attaquer(3,3);
                unTerrain.attaquer(6,2);
                unTerrain.attaquer(5,2);

                unTerrain.afficheTerrain();


            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur : " + e.Message);
            }
            finally
            {
                Console.WriteLine("Traitement terminée."); 
            }
        }
    }
}