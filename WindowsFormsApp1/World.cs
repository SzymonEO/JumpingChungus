﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    internal class World
    {
        public List<Rectangle> PlatformHB = new List<Rectangle>(); // hitboxy platform
        public List<Rectangle> clouds = new List<Rectangle>(); // hitboxy platform
        public List<Rectangle> carrots = new List<Rectangle>();
        public List<Rectangle> emptys = new List<Rectangle>();

        int carrotWidth = Properties.Resources.Carrot.Width;
        int platWidth = Properties.Resources._311.Width;
        private Graphics gBackground;
        private int resolutionWidth;
        private int resolutionHeight;
        
        public int screenScrollSpeed = 0;
        public int screenScrollSpeed2 = 0;

        public int xplat;
        public int yplat;
        public int platw;

        Player p;

        public World(Graphics gBackground, int resoultionWidth, int resolutionHeight)
        {
            this.gBackground = gBackground;
            this.resolutionWidth = resoultionWidth;
            this.resolutionHeight = resolutionHeight;
        }

        public void SetPlayer(Player p) { this.p = p; }

        public void generateGround(int posX, int posY, int _width)                                               //generowanie platformy startowej
        {

            gBackground.DrawImage(Properties.Resources._311, posX, posY);
            Rectangle rect = new Rectangle(posX, posY, platWidth * _width, 1);
            PlatformHB.Add(rect);
            Bitmap plat = Properties.Resources._311;
            int len = 0;
            for (int i = 0; i < _width; i++)
            {
                gBackground.DrawImage(plat, posX + len, posY);
                len += platWidth;
            }

        }

        //adds new platform to the list

        public void RenderPlatforms()                                                                                 //renderowanie platform
        {
            //przygotowac zestaw platform o roznych rozmiarach
            Rectangle temp;

            Bitmap plat = Properties.Resources._311;
            int dlen;
            int begLen;
            for (int i = 0; i < PlatformHB.Count; i++)
            {

                temp = PlatformHB[i];
                PlatformHB[i] = new Rectangle(temp.X, temp.Y + screenScrollSpeed, temp.Width, temp.Height);
                if (temp.Y > resolutionHeight + 100)
                    PlatformHB.RemoveAt(i);
                if (temp != null)
                {
                    dlen = 0;
                    begLen = temp.Width / platWidth;
                    for (int j = 0; j < begLen; j++)
                    {

                        gBackground.DrawImage(plat, temp.Left + dlen, temp.Top);
                        dlen += platWidth;
                    }
                }
            }
            p.playerBox.Y += screenScrollSpeed;
        }

        public void generatePlatformRandom(int numberOf)                                                            //generowanie platform
        {
            Random rand = new Random();

            int ranX = 0;
            int ranY = 0;
            int ranWidth = 0;

            int prevRanX = 0;
            int prevRanY = 0;
            int prevRanWidth = 0;
            xplat = PlatformHB.LastOrDefault().X;
            yplat = ranY = PlatformHB.LastOrDefault().Y;
            platw = PlatformHB.LastOrDefault().Width / platWidth;
            if (PlatformHB.Count > 1)
            {
                ranX = PlatformHB.LastOrDefault().X;
                ranY = PlatformHB.LastOrDefault().Y;
                ranWidth = PlatformHB.LastOrDefault().Width/platWidth;
            }
            else
            {
                ranX = rand.Next(50, resolutionWidth/2);
                ranY = resolutionHeight - 100;
                ranWidth = 10;
            }
            if (ranY < -200)
                return;

            int jumpDistance = 70;

            int ranXRight;
            int minY = 120; // ponad glowa gracza
            int maxY = 270; //wysokosc skoku

            for (int i = 0; i < numberOf; i++)
            {
                //zapamietuje poprzednie wartosci na ktorych bazie tworzymy kolejne
                prevRanWidth = ranWidth;
                prevRanX = ranX;
                prevRanY = ranY;

                ranY = prevRanY - rand.Next(minY, maxY);
                ///////////////////////////

                //losuje poczatek nowej platformy
                ranX = rand.Next(10, prevRanX + prevRanWidth * platWidth + jumpDistance);

                //uzupelniamy ewentualna luke szerokoscia platformy          
                if (ranX < prevRanX) // jesli zaczyna sie przed poprzednia platforma
                {
                    System.Diagnostics.Debug.WriteLine("condition 1");
                    if (prevRanX - jumpDistance > ranX)
                        ranXRight = rand.Next(prevRanX - jumpDistance, prevRanX + (prevRanWidth - 3) * platWidth - jumpDistance);
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("ranX: " + ranX);

                        System.Diagnostics.Debug.WriteLine("prevRanX - jumpDistance + prevRanWidth = " + (prevRanX - jumpDistance + prevRanWidth * platWidth));
                        //probably tutaj jest blad gdy nastepna platforma zaczyna sie mniej niz jumpdistance przed a konczy za platforma
                        ranXRight = rand.Next(ranX, prevRanX - jumpDistance + (prevRanWidth - 3) * platWidth);
                    }
                    ranWidth = (ranXRight - ranX) / platWidth + 5;
                }
                else if (ranX >= prevRanX) // jesli zaczyna sie na poprzedniej platformie lub za nia
                {

                    System.Diagnostics.Debug.WriteLine("condition else if");
                    //ranX = ranX + 100;
                    if (ranX < prevRanX + prevRanWidth * platWidth + jumpDistance)
                    {
                        System.Diagnostics.Debug.WriteLine("ranX: " + ranX);

                        System.Diagnostics.Debug.WriteLine("prevRanX + prevRanWidth*platWidth - jumpDistance = " + (prevRanX - jumpDistance + prevRanWidth * platWidth));

                        ranXRight = rand.Next(ranX, prevRanX + prevRanWidth * platWidth + jumpDistance);

                        ranWidth = (ranXRight - ranX) / platWidth + 5;
                    }
                    else
                    {
                        ranX = prevRanX + prevRanWidth * platWidth + jumpDistance;
                        ranWidth = rand.Next(7, 10);

                        System.Diagnostics.Debug.WriteLine("condition else");
                    }
                }
                for (int n = 0; n < 10; n++)
                {
                    if (ranX + ranWidth * platWidth >= resolutionWidth)
                    {

                        System.Diagnostics.Debug.WriteLine("too far right");
                        ranX -= 2 * (ranX + ranWidth * platWidth - resolutionWidth);
                    }
                }
                Rectangle rect = new Rectangle(ranX, ranY, ranWidth*platWidth, 1);
                PlatformHB.Add(rect);
            }
        }

        public void RenderCarrots()                                                                                         //renderowanie marchewek
        {
            Rectangle temp;

            Bitmap carrot = Properties.Resources.Carrot;
            int dlen;
            int begLen;
            for (int i = 0; i < carrots.Count; i++)
            {

                temp = carrots[i];
                carrots[i] = new Rectangle(temp.X, temp.Y + screenScrollSpeed, temp.Width, temp.Height);
                if (temp.Y > resolutionHeight + 100)
                    carrots.RemoveAt(i);
                if (temp != null)
                {
                    dlen = 0;
                    begLen = temp.Width / carrotWidth;
                    for (int j = 0; j < begLen; j++)
                    {

                        gBackground.DrawImage(carrot, temp.Left + dlen, temp.Top);
                        dlen += carrotWidth;
                    }
                }
            }

        }

        public void generateCarrotRandom(int numberOf)                                                                    //generowanie marchewek
        {
            Random rand = new Random();

            int ranX = 0;

            if (carrots.Count > 1)
            {
                ranX = carrots.LastOrDefault().X;             
            }
            else
            {
                ranX = rand.Next(xplat, xplat + platw);
            }

            for (int i = 0; i < numberOf; i++)
            {
                ranX = rand.Next(xplat, xplat + platw);

                Rectangle rect = new Rectangle(xplat + ranX, yplat - 50,carrotWidth, 1);
                carrots.Add(rect);

            }

        }

        public void RenderEmpty()                                                                                         //renderowanie marchewek
        {
            Rectangle temp;

            Bitmap carrot = Properties.Resources.Empty;
            int dlen;
            int begLen;
            for (int i = 0; i < emptys.Count; i++)
            {

                temp = emptys[i];
                emptys[i] = new Rectangle(temp.X, temp.Y + screenScrollSpeed, temp.Width, temp.Height);
                if (temp.Y > resolutionHeight + 100)
                    emptys.RemoveAt(i);
                if (temp != null)
                {
                    dlen = 0;
                    begLen = temp.Width / carrotWidth;
                    for (int j = 0; j < begLen; j++)
                    {

                        gBackground.DrawImage(carrot, temp.Left + dlen, temp.Top);
                        dlen += carrotWidth;
                    }
                }
            }


        }

        public void generateEmpty(int numberOf)                                                                    //generowanie marchewek
        {
                Rectangle rect = new Rectangle(p.ttx,p.tty, carrotWidth, 1);
            emptys.Add(rect);
        }

        public void RenderClouds()                                                                                        //renderowanie chmur
        {
            //przygotowac zestaw platform o roznych rozmiarach
            Rectangle temp;
            Bitmap cloud = Properties.Resources.cloudsT;
            for (int i = 0; i < clouds.Count; i++)
            {
                temp = clouds[i];
                clouds[i] = new Rectangle(temp.X, temp.Y + screenScrollSpeed2, temp.Width, temp.Height);
                if (temp.Y > resolutionHeight)
                    clouds.RemoveAt(i);

                if (temp.Top < resolutionHeight && temp!=null)
                    gBackground.DrawImage(cloud, temp.Left, temp.Top);
            }
        }
        public void generateCloud()                                                                                         //generowanie chmur
        {
            int cloudWidth = Properties.Resources.cloudsT.Width;
            int cloudHeight = Properties.Resources.cloudsT.Height;
            Random rand = new Random();

            int ranX = rand.Next(-50, (resolutionWidth + 50)/2-cloudWidth);
            int ranY = -cloudHeight;


           Rectangle rect = new Rectangle(ranX, ranY, cloudWidth, 1);
           clouds.Add(rect);
           
           ranX = rand.Next((resolutionWidth + 50) / 2 - cloudWidth, (resolutionWidth + 50) / 2+ (resolutionWidth + 50) / 2);
           ranY = rand.Next(-2 * cloudHeight, -3*cloudHeight/2);  
           
           rect = new Rectangle(ranX, ranY, cloudWidth, 1);
           clouds.Add(rect);
        }
    }
}