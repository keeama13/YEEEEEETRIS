using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System;

/// <summary>
/// A class for representing the game world.
/// This contains the grid, the falling block, and everything else that the player can see/do.
/// </summary>
class GameWorld
{
    //TO DO LIST

    //PLAY KNOP HULP KNOP
    //MUZIEK ACHTERGROND EN SOUND EFFECTS
    //BETER UITZIENDE MENUTJSES
    //COMMENTAAR PLAATSEN BIJ DINGEN
    //SPAWN LOCATIE TWEAKEN
    //DEZE LIJST WEGHALEN ALS ALLES KLAAR IS EN INLEVEREN
    //NAAR BENEDEN IN GEDRUKT HOUDEN MOET DE SNELHEID OP MAX SPEED ZETTEN DOE DIT MET BEHULP VAN INPUTHELPER BIJ HANDLEINPUT ( OOK DIT MOET PUNTEN GEVEN XD ROFL LMAO)
    //IPV SCORE WERKEN MET LINES CLEARED OM LEVEL TE BEPALEN MEER LINES WEG HALEN IS MEER PUNTEN AKA 1 line 10 punten 2 lines 100 punten 1000 10000 OOK PUNTEN VOOR BLOK IN EEN KEER DOEN
    //NA x AANTAL CLEARED LINES GAAT LVL OMHOOG NIET MEER WERKEN 


    //BUG FIXES

    //ROTATIE FIXEN
    //GEEN ONEINDIG OPHOGENDE LEVELS MEER
    


    /// <summary>
    /// An enum for the different game states that the game can have.
    /// </summary>
    enum GameState
    {
        Title,
        Playing,
        Paused,
        GameOver
    }

    /// <summary>
    /// The random-number generator of the game.
    /// </summary>
    public static Random Random { get { return random; } }
    static Random random;


    /// <summary>
    /// The main font of the game.
    /// </summary>
    SpriteFont font;

    /// <summary>
    /// The current game state.
    /// </summary>
    GameState gameState;

    /// <summary>
    /// The main grid of the game.
    /// </summary>
    TetrisGrid grid;
    TetrisBlocks block = new TetrisBlocks(Vector2.Zero);
    TetrisBlocks nextBlock = new TetrisBlocks(Vector2.Zero);
    TetrisBlocks holdBlock, saveBlock, testBlock;
    

    bool hold, space;
    int speed = 15;
    int score = 0;
    int level = 1;

    public GameWorld()
    {
        random = new Random();
        gameState = GameState.Title;

        font = TetrisGame.ContentManager.Load<SpriteFont>("SpelFont");
        grid = new TetrisGrid();
    }

    public void HandleInput(GameTime gameTime, InputHelper inputHelper)
    {
        if (gameState == GameState.Title)
        {
            if (inputHelper.MouseLeftButtonPressed())
            {
                gameState = GameState.Playing;
            }
        }
        if (gameState == GameState.GameOver)
        {
            if (inputHelper.MouseLeftButtonPressed())
            {
                gameState = GameState.Title;
            }
        }
        if (gameState == GameState.Playing)
        {
            if (inputHelper.KeyPressed(Keys.Left))
            {
                if (block.Location.X > 0 - LeftClear() && CanGoLeft())
                    block.Location.X--;
                if (!NoCollision())
                    grid.tijd = 0;
            }

            if (inputHelper.KeyPressed(Keys.Right))
            {
                if (block.Location.X + block.blockSize < grid.Width + RightClear() && CanGoRight())
                    block.Location.X++;
                if (!NoCollision())
                    grid.tijd = 0;
            }

            if (inputHelper.KeyPressed(Keys.Space))
            {
                while (NoCollision())
                {
                    block.Location.Y += 1;
                }
                space = true;
            }

            if (inputHelper.KeyPressed(Keys.A))
            {

                MoveIfBlocked();
                if (CanGoLeft() && CanGoRight())
                    block.curBool = Rotate(block.curBool, false, block.blockSize);
                if (!NoCollision())
                    grid.tijd = 0;

            }

            if (inputHelper.KeyPressed(Keys.D))
            {
                MoveIfBlocked();
                if (CanGoLeft() && CanGoRight())
                    block.curBool = Rotate(block.curBool, true, block.blockSize);
                if (!NoCollision())
                    grid.tijd = 0;
            }


            if (inputHelper.KeyPressed(Keys.C))
            {
                if (holdBlock == null)
                {
                    holdBlock = block;
                    block = nextBlock;
                    nextBlock = block.GetRandomBlock(random.Next(0, 7));
                    block.Location = block.SpawnLocation;
                }
                if (hold)
                {
                    saveBlock = block;
                    block = holdBlock;
                    holdBlock = saveBlock;
                    block.Location = block.SpawnLocation;
                    hold = false;
                }

            }

        }
    }
    void HandleRotate(bool whichWay)
    {

        Rotate(testBlock.curBool, whichWay, block.blockSize);
        HandleCollisionMove();
    }

    bool HandleCollisionMove()
    {
        for (int y = 0; y < testBlock.blockSize; y++)
        {
            for (int x = 0; x < testBlock.blockSize; x++)
            {
                if (testBlock.curBool[y, x] && grid.grid[(int)testBlock.Location.Y + y, (int)testBlock.Location.X + x] == null)
                {
                    return true;//De plek is vrij
                }
                if (testBlock.curBool[y, x] && grid.grid[(int)testBlock.Location.Y + y, (int)testBlock.Location.X + x] != null)
                {
                    if (CanGoLeft())
                    {
                        testBlock.Location.X -= 1;
                        return HandleCollisionMove();
                    }
                    if (CanGoRight())
                    {
                        testBlock.Location.X += 1;
                        return HandleCollisionMove();
                    }


                }
            }
            
        }
        return false;
    }

    int LeftClear()
    {
        int amount = 0;
        for (int x = 0; x < block.blockSize; x++)
        {
            for (int y = 0; y < block.blockSize; y++)
            {
                if (block.Location.X < grid.Width)
                {
                    if (block.curBool[y, x])
                    {
                        return amount;
                    }
                }
            }
            amount++;
        }
        return amount;
    }

    int RightClear() //amount of columns that is free on the right side inside the block
    {
        int amount = 0;
        for (int x = block.blockSize - 1; x > 0; x--)
        {
            for (int y = 0; y < block.blockSize; y++)
            {
                if (block.Location.X > 0)
                {
                    if (this.block.curBool[y, x])
                    {
                        return amount;
                    }
                }

            }
            amount++;

        }
        return amount;
    }

    int UnderClear() //amount of columns that is free on the bottom inside the block
    {
        int amount = 0;
        for (int y = block.blockSize - 1; y > 0; y--)
        {
            for (int x = 0; x < block.blockSize; x++)
            {
                if (block.Location.Y > 0)
                {
                    if (this.block.curBool[y, x])
                    {
                        return amount;
                    }
                }

            }
            amount++;

        }
        return amount;
    }



    void MoveIfBlocked()
    {
        if (LeftClear() > 0 && block.Location.X < 0 && CanGoRight())
        {
            block.Location.X += LeftClear();
        }
        if (RightClear() > 0 && block.Location.X + block.blockSize > grid.Width && CanGoLeft())
        {
            block.Location.X -= RightClear();
        }
        if(UnderClear() > 0 && block.Location.Y + block.blockSize > grid.Height)
        {
            block.Location.Y -= UnderClear();
        }

    }

    bool[,] Rotate(bool[,] block, bool whichWay, int blockSize) //rotate an array (true is right and false is left)
    {
        bool[,] Rotated = new bool[blockSize, blockSize];

        for (int i = 0; i < blockSize; ++i)
        {
            for (int j = 0; j < blockSize; ++j)
            {
                if (whichWay)
                    Rotated[i, j] = block[blockSize - j - 1, i];
                else
                    Rotated[i, j] = block[j, blockSize - i - 1];
            }
        }
        return Rotated;
    }


  

    void DeleteLine(int x)
    {
        for (int i = x; i > 0; i--)
        {
            for (int j = 0; j < grid.Width; j++)
            {
                if (i > 0)
                {
                    grid.grid[i, j] = grid.grid[i - 1, j];

                }
                else
                    grid.grid[i, j] = null;

            }

        }
    }

    void LineFull()
    {
        for (int y = 0; y < grid.Height; y++)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                if (grid.grid[y, x] != null && x == grid.Width - 1)
                {
                    DeleteLine(y);
                    score++;
                }
                else if (grid.grid[y, x] == null)
                {
                    break;
                }

            }
        }
    }

    bool CanGoRight()
    {
        for (int y = 0; y < block.blockSize; y++)
        {
            for (int x = 0; x < block.blockSize; x++)
            {
                if (block.Location.Y > 0 && (int)block.Location.X + x + 1 < grid.Width)
                {
                    if (block.curBool[y, x] && grid.grid[(int)block.Location.Y + y, (int)block.Location.X + x + 1] != null)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }



    bool CanGoLeft()
    {
        for (int y = 0; y < block.blockSize; y++)
        {
            for (int x = 0; x < block.blockSize; x++)
            {
                if (block.Location.Y > 0 && (int)block.Location.X + x - 1 >= 0)
                {
                    if (block.curBool[y, x] && grid.grid[(int)block.Location.Y + y, (int)block.Location.X + x - 1] != null)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    bool NoCollision()
    {
        for (int h = block.blockSize - 1; h >= 0; h--)
        {
            for (int j = 0; j < block.blockSize; j++)
            {
                if (block.curBool[h, j] && block.Location.Y + h + 1 >= grid.Height)
                {

                    return false;

                }
                if (block.Location.Y + h + 1 >= 0)   //Het blok wordt buiten de grid geplaats in het begin, dus zonder dit if statement denkt C# dat het buiten de matrixgrenzen van de grid valt.
                {
                    if (block.curBool[h, j] && grid.grid[(int)block.Location.Y + h + 1, (int)block.Location.X + j] != null)
                    {
                        return false;
                    }
                }

            }
        }
        return true;
    }

   

    void AddBlock(int x, int y)
    {
        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                if (block.curBool[i, j] && block.Location.Y >= 0 && block.Location.X + j >= 0 && block.Location.Y + i < grid.Height && block.Location.X + j <= grid.Width)
                {
                    grid.grid[(int)block.Location.Y + i, (int)block.Location.X + j] = block;
                }
            }
        }
    }

    void BlockUpdate()
    {
        AddBlock(block.blockSize, block.blockSize);
        LineFull();
        block = nextBlock;
        nextBlock = block.GetRandomBlock(random.Next(0, 7));
        hold = true;
    }



    public void Update(GameTime gameTime)
    {
        if (gameState == GameState.Title)
        {
        }
        if (gameState == GameState.GameOver)
        {
            //this.block = null;

            Reset();
        }
        if (gameState == GameState.Playing)
        {
            if (score % 10 == 0 && score > 0)
            {
                speed -= 1;
                level += 1;
            }
            if (speed <= 3)
            {
                speed = 3;
            }
            grid.tijd++;
            if (grid.tijd == speed)
            {
                if (NoCollision())
                {
                    block.Location.Y += 1;

                }
                else
                {
                    if (this.block.Location.Y < 0)
                    {
                        gameState = GameState.GameOver;
                    }
                    else
                    {
                        BlockUpdate();
                    }
                }
                grid.tijd = 0;
            }
            if(space)
            {
                BlockUpdate();
                space = false;
            }

        }
    }


    public void DrawBlocks(SpriteBatch spriteBatch)
    {
        for (int a = 0; a < block.blockSize; a++)
        {
            for (int b = 0; b < block.blockSize; b++)
            {
                if (block.curBool[a, b] && a >= 0 && b >= 0 && a <= grid.Height && b <= grid.Width)
                {
                    spriteBatch.Draw(grid.emptyCell, new Vector2((b + block.Location.X) * grid.emptyCell.Width + grid.position.X, (a + block.Location.Y) * grid.emptyCell.Height + grid.position.Y), block.curColor);
                }
            }
        }
        for (int a = 0; a < nextBlock.blockSize; a++)
        {
            for (int b = 0; b < nextBlock.blockSize; b++)
            {
                if (nextBlock.curBool[a, b] && a >= 0 && b >= 0 && a <= grid.Height && b <= grid.Width)
                {
                    spriteBatch.Draw(grid.emptyCell, new Vector2((b + grid.Width) * grid.emptyCell.Width + grid.position.X, (a) * grid.emptyCell.Height + grid.position.Y), nextBlock.curColor);
                }
            }
        }
        if (holdBlock != null)
        {
            for (int a = 0; a < holdBlock.blockSize; a++)
            {
                for (int b = 0; b < holdBlock.blockSize; b++)
                {
                    if (holdBlock.curBool[a, b] && a >= 0 && b >= 0 && a <= grid.Height && b <= grid.Width)
                    {
                        spriteBatch.Draw(grid.emptyCell, new Vector2(b * grid.emptyCell.Width, a * grid.emptyCell.Height), holdBlock.curColor);
                    }
                }
            }
        }

    }




    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        if (gameState == GameState.Title)
        {
            spriteBatch.DrawString(font, "Smash die linkermuisbutton om te starten!", new Vector2(100, 0), Color.Blue);
        }
        if (gameState == GameState.GameOver)
        {
            spriteBatch.DrawString(font, "Smash die linkermuisbutton om te opnieuw naar het beginscherm te gaan!", new Vector2(100, 0), Color.Blue);
        }
        if (gameState == GameState.Playing)
        {

            grid.Draw(gameTime, spriteBatch);
            spriteBatch.DrawString(font, "Score: " + score, new Vector2(0, 0 + 4 * grid.emptyCell.Height), Color.Blue);
            spriteBatch.DrawString(font, "Level: " + level, new Vector2(0, 40 + 4 * grid.emptyCell.Height), Color.Blue);
            DrawBlocks(spriteBatch);

            
        }
        spriteBatch.End();
    }

    public void Reset()
    {
        for (int x = 0; x < grid.Width; x++)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                grid.grid[y, x] = null;

            }
        }
        score = 0;
        speed = 15;
        level = 1;
        block = block.GetRandomBlock(random.Next(0, 7));
        nextBlock = block.GetRandomBlock(random.Next(0, 7));
    }
}
  
