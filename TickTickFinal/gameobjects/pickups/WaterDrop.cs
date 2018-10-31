using System;
using Microsoft.Xna.Framework;

class WaterDrop : PickupItem
{
    public WaterDrop(int layer=0, string id="") : base("Sprites/spr_water", layer, id) 
    {
    }

    protected override void OnPickupItem()
    {
        GameEnvironment.AssetManager.PlaySound("Sounds/snd_watercollected");
    }
}
