using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class ExtraTimeItem : PickupItem
{
    public ExtraTimeItem(int layer = 0, string id = "") : base("Sprites/spr_extraTime", layer, id)
    {
    }

    protected override void OnPickupItem()
    {
        TimerGameObject timer = GameWorld.Find("timer") as TimerGameObject;
        timer.AddSecondsToTimer(10);
    }
}


