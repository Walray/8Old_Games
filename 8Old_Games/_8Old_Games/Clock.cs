using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace _8Old_Games {
    public class Clock {
        private double mHour;
        private double mMin;
        private double mSecond;

        public double TotalHour { get { return mHour; } set { mHour = value; } }
        public double TotalMin { get { return mMin; } set { mMin = value; } }
        public double TotalSecond { get { return mSecond; } set { mSecond = value; } }

        public Clock() {; }
        public void initialize() { mHour = mMin = mSecond = 0.0; }
        public void update(GameTime gameTime) {
            mHour += gameTime.ElapsedGameTime.TotalHours;
            mMin += gameTime.ElapsedGameTime.TotalMinutes;
            mSecond += gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void draw(SpriteBatch sprtieBatch, SpriteFont spriteFont, Vector2 position, Color color) {
            sprtieBatch.DrawString(spriteFont, ((int)mMin) % 60 + " : " + ((int)mSecond) % 60, position, color);
        }
    }
}