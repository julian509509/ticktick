using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

partial class Player : AnimatedGameObject
{
    protected Vector2 startPosition;
    protected bool isOnTheGround;
    protected float previousYPosition;
    protected bool isAlive;
    protected bool exploded;
    protected bool finished;
    protected bool walkingOnIce, walkingOnHot;
    protected float bombThrowCooldown = 0.0f; //tracks the cooldown between throwing bombs
    protected bool hasShield; //Whether or not the player currently has a shield
    protected bool wasJustHitByEnemy; // Stores whether the player was just hit by an enemy

    //The amount of time the player has protection from taking damage after being hit and having a shield
    protected const double AfterHitProtectionInSeconds = 3;
    //Stores the reamining amount of milliseconds before the hit proctection is removed
    protected double hitCooldown;
    public Vector2 PreviousPosition { get; private set; }

    public Player(Vector2 start) : base(2, "player")
    {
        LoadAnimation("Sprites/Player/spr_idle", "idle", true); 
        LoadAnimation("Sprites/Player/spr_run@13", "run", true, 0.05f);
        LoadAnimation("Sprites/Player/spr_jump@14", "jump", false, 0.05f); 
        LoadAnimation("Sprites/Player/spr_celebrate@14", "celebrate", false, 0.05f);
        LoadAnimation("Sprites/Player/spr_die@5", "die", false);
        LoadAnimation("Sprites/Player/spr_explode@5x5", "explode", false, 0.04f);

        startPosition = start;
        PreviousPosition = start;
        Reset();
    }

    public override void Reset()
    {
        position = startPosition;
        velocity = Vector2.Zero;
        isOnTheGround = true;
        isAlive = true;
        exploded = false;
        finished = false;
        walkingOnIce = false;
        walkingOnHot = false;
        hasShield = false;
        wasJustHitByEnemy = false;
        PlayAnimation("idle");
        PreviousPosition = new Vector2(BoundingBox.Left, BoundingBox.Bottom);
        previousYPosition = BoundingBox.Bottom;
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        GameEnvironment.Camera.UpdatePosition(GlobalPosition);

        float walkingSpeed = 400;
        if (walkingOnIce)
        {
            walkingSpeed *= 1.5f;
        }
        if (!isAlive)
        {
            return;
        }
        if (inputHelper.IsKeyDown(Keys.E) && bombThrowCooldown == 0)
        {
            ThrowBomb(true);
            bombThrowCooldown = 3.0f;
        }
        if (inputHelper.IsKeyDown(Keys.Q) && bombThrowCooldown == 0)
        {
            ThrowBomb(false);
            bombThrowCooldown = 3.0f;
        }
        if (inputHelper.IsKeyDown(Keys.Left))
        {
            velocity.X = -walkingSpeed;
        }
        else if (inputHelper.IsKeyDown(Keys.Right))
        {
            velocity.X = walkingSpeed;
        }
        else if (!walkingOnIce && isOnTheGround)
        {
            velocity.X = 0.0f;
        }
        if (velocity.X != 0.0f)
        {
            Mirror = velocity.X < 0;
        }
        if ((inputHelper.KeyPressed(Keys.Space) || inputHelper.KeyPressed(Keys.Up)) && isOnTheGround)
        {
            Jump();
        }
    }

    public void ThrowBomb(bool right)
    {
        Throwable bomb = GameWorld.Find("bomb") as Throwable;
        bomb.Thrown(right, position);
    }

    public override void Update(GameTime gameTime)
    {
        //Store the previous position. Used for parallax effect
        PreviousPosition = position;

        base.Update(gameTime);
        if(bombThrowCooldown > 0)
        {
            bombThrowCooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (bombThrowCooldown < 0){
                bombThrowCooldown = 0;
            }
        }

        if (wasJustHitByEnemy)
        {
            hitCooldown -= gameTime.ElapsedGameTime.TotalSeconds;
            if(hitCooldown <= 0)
            {
                wasJustHitByEnemy = false;
            }
        }

        if (!finished && isAlive)
        {
            if (isOnTheGround)
            {
                if (velocity.X == 0)
                {
                    PlayAnimation("idle");
                }
                else
                {
                    PlayAnimation("run");
                }
            }
            else if (velocity.Y < 0)
            {
                PlayAnimation("jump");
            }

            TimerGameObject timer = GameWorld.Find("timer") as TimerGameObject;
            if (walkingOnHot)
            {
                timer.Multiplier = 2;
            }
            else if (walkingOnIce)
            {
                timer.Multiplier = 0.5;
            }
            else
            {
                timer.Multiplier = 1;
            }

            TileField tiles = GameWorld.Find("tiles") as TileField;
            if (BoundingBox.Top >= tiles.Rows * tiles.CellHeight)
            {
                Die(true);
            }
        }
        
        DoPhysics();
    }

    public void Explode()
    {
        if (!isAlive || finished)
        {
            return;
        }
        isAlive = false;
        exploded = true;
        velocity = Vector2.Zero;
        position.Y += 15;
        PlayAnimation("explode");
    }

    public void ApplyShield()
    {
        Shield shield = GameWorld.Find("playerShield") as Shield;
        shield.ActivateShield();
        hasShield = true;
    }

    public void DestroyShield()
    {
        Shield shield = GameWorld.Find("playerShield") as Shield;
        shield.StopShield();
        hasShield = false;
        wasJustHitByEnemy = true;
        hitCooldown = AfterHitProtectionInSeconds;
    }

    public void Die(bool falling)
    {
        if (!isAlive || finished)
        {
            return;
        }

        if (hasShield)
        {
            DestroyShield();
            return;
        }else if (wasJustHitByEnemy)
        {
            return;
        }

        isAlive = false;
        velocity.X = 0.0f;
        if (falling)
        {
            GameEnvironment.AssetManager.PlaySound("Sounds/snd_player_fall");
        }
        else
        {
            velocity.Y = -900;
            GameEnvironment.AssetManager.PlaySound("Sounds/snd_player_die");
        }
        PlayAnimation("die");
    }

    public bool IsAlive
    {
        get { return isAlive; }
    }

    public bool Finished
    {
        get { return finished; }
    }

    public void LevelFinished()
    {
        finished = true;
        velocity.X = 0.0f;
        PlayAnimation("celebrate");
        GameEnvironment.AssetManager.PlaySound("Sounds/snd_player_won");
    }
}
