using Microsoft.Xna.Framework;

class Shield : AnimatedGameObject
{
    //A constant to define the max duration of a shield
    protected double MaxDurationInSeconds = 10;

    //Stores the remaining duration of this shield
    protected double duration;

    public Shield() : base(3, "playerShield")
    {
        LoadAnimation("Sprites/Shield/spr_playerShield@9", "shield", true, 0.1f);
        Reset();
    }

    public void ActivateShield()
    {
        visible = true;
        PlayAnimation("shield");
    }

    public void StopShield()
    {
        Reset();
        StopAnimation("shield");
    }

    public override void Reset()
    {
        duration = MaxDurationInSeconds;
        visible = false;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        Player player = GameWorld.Find("player") as Player;
        if (visible)
        {
            duration -= gameTime.ElapsedGameTime.TotalSeconds;
            if(duration <= 0)
            {
                StopShield();
            } else
            {
                position = player.Position;
            }
        }
    }

}