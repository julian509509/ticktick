using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;

partial class Level : GameObjectList
{
    public Vector2 WorldSize { get; private set; }

    public void LoadTiles(string path)
    {
        List<string> textLines = new List<string>();
        StreamReader fileReader = new StreamReader(path);
        string line = fileReader.ReadLine();
        width = line.Length;
        while (line != null)
        {
            textLines.Add(line);
            line = fileReader.ReadLine();
        }

        GameObjectList hintField = new GameObjectList(100);
        Add(hintField);

        string hint = textLines[textLines.Count - 1];
        SpriteGameObject hintFrame = new SpriteGameObject("Overlays/spr_frame_hint", 1);
        hintField.Position = new Vector2((GameEnvironment.Screen.X - hintFrame.Width) / 2, 10);
        hintField.Add(hintFrame);

        TextGameObject hintText = new TextGameObject("Fonts/HintFont", 2);
        hintText.Text = hint;
        hintText.Position = new Vector2(120, 25);
        hintText.Color = Color.Black;
        hintField.Add(hintText);

        //Simply stores the number of extra lines at the end of a file that aren't the map itself
        int addInfo = 1;

        string timeInMin = textLines[textLines.Count - 2];
        double time;
        TimerGameObject timer;
        if(double.TryParse(timeInMin, out time)){
            timer = new TimerGameObject(Camera.UILayer + 1, "timer", time);
            addInfo++;
        } else
        {
            timer = new TimerGameObject(Camera.UILayer + 1, "timer");
        }
        timer.Position = new Vector2(25, 30);
        Add(timer);

        VisibilityTimer hintTimer = new VisibilityTimer(hintField, 1, "hintTimer");
        Add(hintTimer);

        TileField tiles = new TileField(textLines.Count - addInfo, width, 1, "tiles");
        Add(tiles);

        height = textLines.Count - addInfo;
        tiles.CellWidth = 72;
        tiles.CellHeight = 55;
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                Tile t = LoadTile(textLines[y][x], x, y);
                tiles.Add(t, x, y);
            }
        }

        WorldSize = new Vector2(72 * width, 55 * height);
        AddBackgroundSprites();
    }

    public void PrepareCamera()
    {
        GameEnvironment.Camera.SetLevelBoundaries(WorldSize);
    }

    private void AddBackgroundSprites()
    {
        SpriteGameObject backgroundSky = new SpriteGameObject("Backgrounds/spr_sky");
        int numBackgrounds = (int)Math.Ceiling(WorldSize.X / backgroundSky.Width);

        GameObjectList mainBackground = new GameObjectList(0, "background");
        backgroundSky.Position = new Vector2(0, GameEnvironment.Screen.Y - backgroundSky.Height);
        mainBackground.Add(backgroundSky);

        //Create the backgrounds
        for (int i = 1; i < numBackgrounds; i++)
        {
            // Load the backgrounds
            backgroundSky = new SpriteGameObject("Backgrounds/spr_sky");
            backgroundSky.Position = new Vector2(backgroundSky.Width * i, GameEnvironment.Screen.Y - backgroundSky.Height);
            Add(backgroundSky);
        }

        //Create mountains and clouds
        for (int i = 0; i < numBackgrounds; i++)
        {
            Mountains mountains = new Mountains(WorldSize, 1);
            mainBackground.Add(mountains);

            Clouds clouds = new Clouds(WorldSize, mountains.Children.Count + 1);
            mainBackground.Add(clouds);
        }

        Add(mainBackground);
    }

    private Tile LoadTile(char tileType, int x, int y)
    {
        switch (tileType)
        {
            case '.':
                return new Tile();
            case '-':
                return LoadBasicTile("spr_platform", TileType.Platform);
            case '+':
                return LoadBasicTile("spr_platform_hot", TileType.Platform, true, false);
            case '@':
                return LoadBasicTile("spr_platform_ice", TileType.Platform, false, true);
            case 'X':
                return LoadEndTile(x, y);
            case 'W':
                return LoadWaterTile(x, y);
            case '1':
                return LoadStartTile(x, y);
            case '#':
                return LoadBasicTile("spr_wall", TileType.Normal);
            case '^':
                return LoadBasicTile("spr_wall_hot", TileType.Normal, true, false);
            case '*':
                return LoadBasicTile("spr_wall_ice", TileType.Normal, false, true);
            case 'T':
                return LoadTurtleTile(x, y);
            case 'R':
                return LoadRocketTile(x, y, true);
            case 'r':
                return LoadRocketTile(x, y, false);
            case 'S':
                return LoadSparkyTile(x, y);
            case 'P':
                return LoadShieldItemTile(x, y);
            case 'B':

            case 'C':
                return LoadFlameTile(x, y, tileType);
            default:
                return new Tile("");
        }
    }

    private Tile LoadBasicTile(string name, TileType tileType, bool hot = false, bool ice = false)
    {
        Tile t = new Tile("Tiles/" + name, tileType);
        t.Hot = hot;
        t.Ice = ice;
        return t;
    }

    private Tile LoadStartTile(int x, int y)
    {
        TileField tiles = Find("tiles") as TileField;
        Vector2 startPosition = new Vector2(((float)x + 0.5f) * tiles.CellWidth, (y + 1) * tiles.CellHeight);
        Player player = new Player(startPosition);
        Add(player);
        return new Tile("", TileType.Background);
    }

    private Tile LoadFlameTile(int x, int y, char enemyType)
    {
        GameObjectList enemies = Find("enemies") as GameObjectList;
        TileField tiles = Find("tiles") as TileField;
        GameObject enemy = null;
        switch (enemyType)
        {
            case 'A': enemy = new UnpredictableEnemy(); break;
            case 'B': enemy = new PlayerFollowingEnemy(); break;
            case 'C': 
            default:  enemy = new PatrollingEnemy(); break;
        }
        enemy.Position = new Vector2(((float)x + 0.5f) * tiles.CellWidth, (y + 1) * tiles.CellHeight);
        enemies.Add(enemy);
        return new Tile();
    }

    private Tile LoadTurtleTile(int x, int y)
    {
        GameObjectList enemies = Find("enemies") as GameObjectList;
        TileField tiles = Find("tiles") as TileField;
        Turtle enemy = new Turtle();
        enemy.Position = new Vector2(((float)x + 0.5f) * tiles.CellWidth, (y + 1) * tiles.CellHeight + 25.0f);
        enemies.Add(enemy);
        return new Tile();
    }

    private Tile LoadSparkyTile(int x, int y)
    {
        GameObjectList enemies = Find("enemies") as GameObjectList;
        TileField tiles = Find("tiles") as TileField;
        Sparky enemy = new Sparky((y + 1) * tiles.CellHeight - 100f);
        enemy.Position = new Vector2(((float)x + 0.5f) * tiles.CellWidth, (y + 1) * tiles.CellHeight - 100f);
        enemies.Add(enemy);
        return new Tile();
    }

    private Tile LoadRocketTile(int x, int y, bool moveToLeft)
    {
        GameObjectList enemies = Find("enemies") as GameObjectList;
        TileField tiles = Find("tiles") as TileField;
        Vector2 startPosition = new Vector2(((float)x + 0.5f) * tiles.CellWidth, (y + 1) * tiles.CellHeight);
        Rocket enemy = new Rocket(moveToLeft, startPosition);
        enemies.Add(enemy);
        return new Tile();
    }

    private Tile LoadShieldItemTile(int x, int y)
    {
        GameObjectList shields = Find("shields") as GameObjectList;
        TileField tiles = Find("tiles") as TileField;
        ShieldItem shieldItem = new ShieldItem();
        shieldItem.Origin = shieldItem.Center;
        shieldItem.Position = new Vector2(x * tiles.CellWidth, y * tiles.CellHeight - 10);
        shieldItem.Position += new Vector2(tiles.CellWidth, tiles.CellHeight) / 2;
        shields.Add(shieldItem);

        Shield shield = new Shield();
        Add(shield);
        return new Tile();
    }

    private Tile LoadEndTile(int x, int y)
    {
        TileField tiles = Find("tiles") as TileField;
        SpriteGameObject exitObj = new SpriteGameObject("Sprites/spr_goal", 1, "exit");
        exitObj.Position = new Vector2(x * tiles.CellWidth, (y+1) * tiles.CellHeight);
        exitObj.Origin = new Vector2(0, exitObj.Height);
        Add(exitObj);
        return new Tile();
    }

    private Tile LoadWaterTile(int x, int y)
    {
        GameObjectList waterdrops = Find("waterdrops") as GameObjectList;
        TileField tiles = Find("tiles") as TileField;
        WaterDrop w = new WaterDrop();
        w.Origin = w.Center;
        w.Position = new Vector2(x * tiles.CellWidth, y * tiles.CellHeight - 10);
        w.Position += new Vector2(tiles.CellWidth, tiles.CellHeight) / 2;
        waterdrops.Add(w);
        return new Tile();
    }
}