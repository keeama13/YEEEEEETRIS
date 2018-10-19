using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

/// <summary>
/// A class for representing the Tetris playing grid.
/// </summary>
class TetrisGrid
{
    public int tijd = 0;    

    //block = new Blocks(Content);
    /// The sprite of a single empty cell in the grid.
    public Texture2D emptyCell;

    /// The position at which this TetrisGrid should be drawn.
    public Vector2 position;

    public TetrisBlocks[,] grid;
    /// The number of grid elements in the x-direction.
    public int Width { get { return 10; } }
    
    /// The number of grid elements in the y-direction.
    public int Height { get { return 20; } }

    
    /// <summary>
    /// Creates a new TetrisGrid.
    /// </summary>
    /// <param name="b"></param>
    public TetrisGrid()
    {
        emptyCell = TetrisGame.ContentManager.Load<Texture2D>("block");
        position = new Vector2(5 * emptyCell.Width, 0);
        grid = new TetrisBlocks[Height , Width];
        Clear();
    }

    

    public void Reset() 
    {

        
    }
    
    /// <summary>
    /// Draws the grid on the screen.
    /// </summary>
    /// <param name="gameTime">An object with information about the time that has passed in the game.</param>
    /// <param name="spriteBatch">The SpriteBatch used for drawing sprites and text.</param>
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        for(int z = 0; z < Height; z++)
        {
            for (int i = 0; i < Width; i++)
            {
                if (grid[z, i] != null)
                {
                    spriteBatch.Draw(emptyCell, new Vector2(position.X + i * emptyCell.Width, position.Y + z * emptyCell.Height), grid[z, i].curColor);
                }
                else
                {
                    spriteBatch.Draw(emptyCell, new Vector2(position.X + i * emptyCell.Width, position.Y + z * emptyCell.Height), Color.White);
                }
               
            }
        }
    }
        
    
    
    /// <summary>
    /// Clears the grid.
    /// </summary>
    public void Clear()
    {
    }
}

