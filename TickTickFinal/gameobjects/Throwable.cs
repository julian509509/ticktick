using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Throwable : AnimatedGameObject
{
    Vector2 start;
    public Throwable(Vector2 start) : base(2, "bomb")
    {
        position = start;
        this.start = start;
        LoadAnimation("Sprites/Bomb", "idle", true);
        PlayAnimation("idle");
        this.visible = false;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        UpdateSpeed(gameTime);

    }

    protected void UpdateSpeed(GameTime gameTime)
    {
        var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
        position.X += velocity.X * delta;
        position.Y += velocity.Y * delta;
        velocity.Y += 300f * delta;
    }

    public override void Reset()
    {
        position = start;
        this.visible = false;
        velocity = Vector2.Zero;
    }

    public void Die()
    {
        this.Reset();
    }

    public void Thrown(bool right, Vector2 loc)
    {
        this.visible = true;
        if (right)
        {
            velocity = new Vector2(600, 0.0f);
        }
        else
        {
            velocity = new Vector2(-600, 0.0f);
        }
        position = loc;
    }
}
