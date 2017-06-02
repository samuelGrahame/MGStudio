using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGStudio.RunTime
{
    public class GameState
    {
        public GraphicsDeviceManager Graphics;
        public SpriteBatch SpriteBatch;

        public KeyboardState NewKeyboard = new KeyboardState();
        public KeyboardState OldKeyboard = new KeyboardState();

        public MouseState NewMouse = new MouseState();
        public MouseState OldMouse = new MouseState();

        public GameState(GraphicsDeviceManager _graphics, SpriteBatch _spriteBatch)
        {
            Graphics = _graphics; SpriteBatch = _spriteBatch;
        }

        public void BeginUpdate()
        {
            NewKeyboard = Keyboard.GetState();
            NewMouse = Mouse.GetState();
        }

        public void EndUpdate()
        {
            OldKeyboard = NewKeyboard;
            OldMouse = NewMouse;
        }
    }
}
