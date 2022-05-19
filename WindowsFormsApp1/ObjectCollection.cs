﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    internal class ObjectCollection
    {
        public Timer timer1;
        public Timer timer2;
        public Timer timer3;
        public Timer timer4;
        public Timer timer5;
        public Timer timer6;
        public Graphics Background;
        public Graphics Player;
        public Label label;
        public int resolutionHeight;
        public int resolutionWidth;

        public ObjectCollection( Timer timer1, Timer timer2, Timer timer3, Timer timer4, Timer timer5,Timer timer6, Graphics backgroundgraphics, Graphics playerGraphics, Label label1, int width, int height)
        {
            this.timer1 = timer1;
            this.timer2 = timer2;
            this.timer3 = timer3;
            this.timer4 = timer4;
            this.timer5 = timer5;
            this.timer6 = timer6;
            this.Background = backgroundgraphics;
            this.Player = playerGraphics;
            this.label = label1;
            this.resolutionHeight = height;
            this.resolutionWidth = width;
        }
    }
}