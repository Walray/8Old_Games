using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using _8Old_Games.Games.Frogger;

namespace _8Old_Games {
#if WINDOWS || XBOX

    static class Program
    {
        static void Main(string[] args)
        {
            MainSelector mainSelector=new MainSelector();
            mainSelector.Run();
        }
    } 
#endif 
}

