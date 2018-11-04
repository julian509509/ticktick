using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class ShieldItem : PickupItem
{
    public ShieldItem(int layer = 0, string id = "") : base("Sprites/Shield/spr_shield", layer, id)
    {
    }

    protected override void OnPickupItem()
    {
        Player player = GameWorld.Find("player") as Player;
        player.ApplyShield();
    }
}
