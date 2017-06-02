using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace MGStudio.RunTime
{
    public class ProjectGame : IGameOverload
    {
        public List<Room> Rooms = new List<Room>();
        public Room ActiveRoom = null;
        public GraphicsDevice GraphicsDevice;
        public GraphicsDeviceManager Graphics;
        public SpriteBatch SpriteBatch;

        public Assembly ScriptAssembly;

        public GameState CurrentGameState;

        public void MoveToRoom(Room room)
        {
            ActiveRoom = room;
        }

        public ProjectGame(Assembly scriptAssembly)
        {
            ScriptAssembly = scriptAssembly;

            ActiveRoom = Rooms.FirstOrDefault();
        }

        public ProjectGame()
        {
           
        }

        public virtual void Ctor(GraphicsDevice _graphicsDevice, GraphicsDeviceManager _graphics)
        {
            Graphics = _graphics;
            GraphicsDevice = _graphicsDevice;
        }

        public virtual void LoadContent(SpriteBatch _spriteBatch)
        {            
            SpriteBatch = _spriteBatch;           
            CurrentGameState = new GameState(Graphics, _spriteBatch);
        }        

        public virtual void UnloadContent()
        {

        }

        public void Update(GameTime gameTime)
        {
            CurrentGameState.BeginUpdate();
            ActiveRoom?.Update(gameTime);

            CurrentGameState.EndUpdate();
        }

        public void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear((ActiveRoom?.BackColor) ?? Color.CornflowerBlue);

            ActiveRoom?.Draw(gameTime);
        }


        //UnloadContent

        //LoadContent
    }
}
