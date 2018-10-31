using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TickTick5.gameobjects
{
    class Throwable : AnimatedGameObject
    {
        Vector2 speed;
        Vector2 location;
        bool alive;

        public Throwable(Vector2 loc, Vector2 sped, SpriteSheet tex)
        {
            speed = sped;
            location = loc;
            sprite = tex;
            alive = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateSpeed(gameTime);

        }

        protected void UpdateSpeed(GameTime gameTime)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            position.X += speed.X * delta;
            position.Y += speed.Y * delta;
            speed.Y += 9.81f * delta;
        }

        public void Thrown(bool right, Vector2 loc)
        {
            if (right)
            {
                speed = new Vector2(200.0f, 0.0f);
            }
            else
            {
                speed = new Vector2(-200.0f, 0.0f);
            }
            location = loc;
            this.visible = true;
        }
    }
}
