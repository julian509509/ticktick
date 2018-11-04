using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

abstract class PickupItem : SpriteGameObject
{
    protected float bounce;

    public PickupItem(string assetName, int layer = 0, string id = "") : base(assetName, layer, id)
    {
    }

    public override void Update(GameTime gameTime)
    {
        double t = gameTime.TotalGameTime.TotalSeconds * 3.0f + Position.X;
        bounce = (float)Math.Sin(t) * 0.2f;
        position.Y += bounce;
        Player player = GameWorld.Find("player") as Player;
        if (visible && CollidesWith(player))
        {
            visible = false;
            OnPickupItem();
        }
    }

    //Event that runs whenever the pickup item is picked up
    protected virtual void OnPickupItem() { }
}