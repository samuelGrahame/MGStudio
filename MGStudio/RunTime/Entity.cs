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
    public class Entity
    {
        public bool Visible { get; set; } = true;
        public bool Solid { get; set; } = false;
        public bool Persistent { get; set; } = false;
        public int Depth { get; set; } = 0;
        public static EntityService MovedService;
        internal bool MovedToService = false;

        internal float __X; // Processed X
        internal float __Y; // Processed Y
        internal bool __New = true; // Processed Y

        private float x;



        public float X
        {
            get { return x; }
            set {
                if (x == value)
                    return;

                x = value;
                if((!MovedToService && (MovedToService = true)))
                {                    
                    MovedService.Add(this);
                }                                    
            }
        }

        private float y;

        public float Y
        {
            get { return y; }
            set {
                if (y == value)
                    return;

                y = value;
                if ((!MovedToService && (MovedToService = true)))
                {
                    MovedService.Add(this);
                }
            }
        }
        
        public float Rotation { get; set; }

        public Sprite ActiveSprite { get; set; } = null;

        private Room room = null;

        public Room ActiveRoom
        {
            get { return room; }
            set {
                room = value;
            }
        }


        public virtual void Step(GameTime gameTime)
        {

        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            
        }

        public virtual void HandleInput(KeyboardState New, KeyboardState Old)
        {
            
        }

        public virtual void HandleMouse(MouseState New, MouseState Old, bool MouseInSprite)
        {

        }        
    }
}
