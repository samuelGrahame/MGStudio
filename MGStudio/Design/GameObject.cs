using CSScriptLibrary;
using MGStudio.BaseObjects;
using MGStudio.RunTime;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGStudio.Design
{
    public class GameObject : BaseGameObject
    {
        public DesignSprite ActiveSprite { get; set; } = null;
        public DesignSprite ActiveMask { get; set; } = null;
        public GameObject ParentGameObject { get; set; }
        public List<GameObjectEvents> Events { get; set; } = new List<GameObjectEvents>();
        public List<GameObjectProperty> Properties { get; set; } = new List<GameObjectProperty>();

        private string ToCSharpName()
        {
            return this.Name;
        }

        private string ToCSharpParentName()
        {
            return ParentGameObject == null ? " : " + "Entity" : " : " + ParentGameObject.ToCSharpName();
        }        
        
        private string ToCSharpGetBodyCode()
        {
            // #TODO# Add No Mouse Click
            // #TODO# Handle Colision - Maybe we added a Listener - First Time New - Add For Each ColisionTypes WHERE IF (ObjectA IS TypeA && ObjectB IS TypeB) || (ObjectA IS TypeB && ObjectB IS TypeA) B ObjectA Collides With ObjectB Type Is Type

            var builder = new StringBuilder();
            
            var gameEvents = new List<GameObjectEvents>();
            gameEvents.AddRange(Events);

            foreach (var property in Properties)
            {
                builder.AppendFormat("\r\n        public {0} {1} {3} get; set; {4}{2}", property.ToCSharpTypeObjectName(), property.Name, property.ToCSharpExpression(), "{", "}");
            }
            StringBuilder tempBuilder;
            int length = 0;
            
            {
                tempBuilder = new StringBuilder(@"
        public override void HandleInput(KeyboardState New, KeyboardState Old KeyboardState __ckeyState, KeyboardState __pkeyState)
        {");

                length = tempBuilder.Length;
                foreach (var item in Events.Where(ev => ev.EventType == BaseGameObjectEventType.KeyDown))
                {
                    var kba = item.EventArguments as KeyboardArgument;
                    if (kba != null && item.EventNodes != null && item.EventNodes.Count > 0)
                    {
                        string funcName = "__keyDown_" + kba.KeyCode.ToString("G");
                        tempBuilder.AppendFormat(@"
            if(New.IsKeyDown(Keys.{0})){1}", kba.KeyCode.ToString("G"), "{ " + funcName + "(); }");
                        
                        builder.AppendLine(item.ToCSharpEvent(funcName));
                    }
                    gameEvents.Remove(item);
                }

                foreach (var item in Events.Where(ev => ev.EventType == BaseGameObjectEventType.KeyPress))
                {
                    var kba = item.EventArguments as KeyboardArgument;
                    if (kba != null && item.EventNodes != null && item.EventNodes.Count > 0)
                    {
                        string funcName = "__keyPress_" + kba.KeyCode.ToString("G");

                        tempBuilder.AppendFormat(@"
            if(New.IsKeyDown(Keys.{0}) && Old.IsKeyUp(Keys.{0})){1}", kba.KeyCode.ToString("G"), "{ " + funcName + "(); }");

                        
                        builder.AppendLine(item.ToCSharpEvent(funcName));
                    }
                    gameEvents.Remove(item);
                }

                foreach (var item in Events.Where(ev => ev.EventType == BaseGameObjectEventType.KeyRelease))
                {
                    var kba = item.EventArguments as KeyboardArgument;
                    if (kba != null && item.EventNodes != null && item.EventNodes.Count > 0)
                    {
                        string funcName = "__keyRelease_" + kba.KeyCode.ToString("G");
                        tempBuilder.AppendFormat(@"
            if(Old.IsKeyDown(Keys.{0}) && New.IsKeyUp(Keys.{0})){1}", kba.KeyCode.ToString("G"), "{ " + funcName + "(); }");
                        
                        builder.AppendLine(item.ToCSharpEvent(funcName));
                    }
                    gameEvents.Remove(item);
                }

                if (tempBuilder.Length != length)
                {
                    tempBuilder.Append(@"
        }");

                    builder.Append(tempBuilder.ToString());
                }
            }            
            // MouseEvents
            {
                tempBuilder = new StringBuilder(@"
        public override void HandleMouse(MouseState New, MouseState Old, bool MouseInSprite)
        {");
                length = tempBuilder.Length;

                foreach (var item in Events.Where(ev => ev.EventType == BaseGameObjectEventType.Mouse))
                {
                    if (item.EventArguments is MouseArgument ma && item.EventNodes != null && item.EventNodes.Count > 0)
                    {
                        string funcName = "__Mouse_" + ma.MouseCode.ToString("G");

                        switch (ma.MouseCode)
                        {
                            case Mouses.Left_button:
                                tempBuilder.AppendFormat(@"
            if(MouseInSprite && New.LeftButton == ButtonState.Pressed){0}", "{ " + funcName + "(); }");
                                break;
                            case Mouses.Right_button:
                                tempBuilder.AppendFormat(@"
            if(MouseInSprite && New.RightButton == ButtonState.Pressed){0}", "{ " + funcName + "(); }");
                                break;
                            case Mouses.Middle_button:
                                tempBuilder.AppendFormat(@"
            if(MouseInSprite && New.MiddleButton == ButtonState.Pressed){0}", "{ " + funcName + "(); }");
                                break;
                            case Mouses.No_button:
                                tempBuilder.AppendFormat(@"
            if(MouseInSprite && New.LeftButton == ButtonState.Released && New.RightButton == ButtonState.Released && New.MiddleButton == ButtonState.Released){0}", "{ " + funcName + "(); }");
                                break;
                            case Mouses.Left_pressed:
                                tempBuilder.AppendFormat(@"
            if(MouseInSprite && New.LeftButton == ButtonState.Pressed && Old.LeftButton == ButtonState.Released){0}", "{ " + funcName + "(); }");
                                break;
                            case Mouses.Right_pressed:
                                tempBuilder.AppendFormat(@"
            if(MouseInSprite && New.RightButton == ButtonState.Pressed && Old.RightButton == ButtonState.Released){0}", "{ " + funcName + "(); }");
                                break;
                            case Mouses.Middle_pressed:
                                tempBuilder.AppendFormat(@"
            if(MouseInSprite && New.MiddleButton == ButtonState.Pressed && Old.MiddleButton == ButtonState.Released){0}", "{ " + funcName + "(); }");
                                break;
                            case Mouses.Left_released:
                                tempBuilder.AppendFormat(@"
            if(MouseInSprite && New.LeftButton == ButtonState.Released && Old.LeftButton == ButtonState.Pressed){0}", "{ " + funcName + "(); }");
                                break;
                            case Mouses.Right_released:
                                tempBuilder.AppendFormat(@"
            if(MouseInSprite && New.RightButton == ButtonState.Released && Old.RightButton == ButtonState.Pressed){0}", "{ " + funcName + "(); }");
                                break;
                            case Mouses.Middle_released:
                                tempBuilder.AppendFormat(@"
            if(MouseInSprite && New.MiddleButton == ButtonState.Released && Old.MiddleButton == ButtonState.Pressed){0}", "{ " + funcName + "(); }");
                                break;
                            case Mouses.Mouse_enter:
                                break;
                            case Mouses.Mouse_leave:
                                break;
                            case Mouses.Mouse_wheel_up:
                                break;
                            case Mouses.Mouse_wheel_down:
                                break;
                            case Mouses.Window_left_button:
                                tempBuilder.AppendFormat(@"
            if(New.LeftButton == ButtonState.Pressed){0}", "{ " + funcName + "(); }");
                                break;
                            case Mouses.Window_right_button:
                                tempBuilder.AppendFormat(@"
            if(New.RightButton == ButtonState.Pressed){0}", "{ " + funcName + "(); }");
                                break;
                            case Mouses.Window_middle_button:
                                tempBuilder.AppendFormat(@"
            if(New.MiddleButton == ButtonState.Pressed){0}", "{ " + funcName + "(); }");
                                break;
                            case Mouses.Window_left_pressed:
                                tempBuilder.AppendFormat(@"
            if(New.LeftButton == ButtonState.Pressed && Old.LeftButton == ButtonState.Released){0}", "{ " + funcName + "(); }");
                                break;
                            case Mouses.Window_right_pressed:
                                tempBuilder.AppendFormat(@"
            if(New.RightButton == ButtonState.Pressed && Old.RightButton == ButtonState.Released){0}", "{ " + funcName + "(); }");
                                break;
                            case Mouses.Window_middle_pressed:
                                tempBuilder.AppendFormat(@"
            if(New.MiddleButton == ButtonState.Pressed && Old.MiddleButton == ButtonState.Released){0}", "{ " + funcName + "(); }");
                                break;
                            case Mouses.Window_left_released:
                                tempBuilder.AppendFormat(@"
            if(New.LeftButton == ButtonState.Released && Old.LeftButton == ButtonState.Pressed){0}", "{ " + funcName + "(); }");
                                break;
                            case Mouses.Window_right_released:
                                tempBuilder.AppendFormat(@"
            if(New.RightButton == ButtonState.Released && Old.RightButton == ButtonState.Pressed){0}", "{ " + funcName + "(); }");
                                break;
                            case Mouses.Window_middle_released:
                                tempBuilder.AppendFormat(@"
            if(New.MiddleButton == ButtonState.Released && Old.MiddleButton == ButtonState.Pressed){0}", "{ " + funcName + "(); }");
                                break;
                            default:
                                break;
                        }

                        
                        builder.AppendLine(item.ToCSharpEvent(funcName));
                    }

                    gameEvents.Remove(item);
                }

                if (tempBuilder.Length != length)
                {
                    tempBuilder.Append(@"
        }");

                    builder.Append(tempBuilder.ToString());

                    tempBuilder = new StringBuilder();
                }
            }

            tempBuilder = new StringBuilder(string.Format(@"
        public {0}()
        {1}", ToCSharpName(), "{"));
            length = tempBuilder.Length;
            int x = 0;

            if(Visible)
                tempBuilder.Append("\r\n            Visible = true;");            
            if (Solid)            
                tempBuilder.Append("\r\n            Solid = true;");            
            if (Persistent)            
                tempBuilder.Append("\r\n            Persistent = true;");            
            if (Depth != 0)            
                tempBuilder.Append("\r\n            Depth = " + Depth + ";");            

            foreach (var property in Properties)
            {
                string Expression = property.ToCSharpExpression();
                if(!string.IsNullOrWhiteSpace(Expression))
                    tempBuilder.AppendFormat("\r\n            {0} = {1};", property.Name, property.ToCSharpExpression());
            }            

            foreach (var item in Events.Where(ev => ev.EventType == BaseGameObjectEventType.New))
            {
                string funcName = "__New_" + x;
                tempBuilder.Append(@"
           " + funcName + "();");

                gameEvents.Remove(item);
                builder.AppendLine(item.ToCSharpEvent(funcName));

                x++;
            }

            if (tempBuilder.Length != length)
            {
                tempBuilder.Append(@"
        }");

                builder.Append(tempBuilder.ToString());
            }
            
            tempBuilder = new StringBuilder(@"
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {");
            length = tempBuilder.Length;
            x = 0;            
            foreach (var item in Events.Where(ev => ev.EventType == BaseGameObjectEventType.Draw))
            {
                if(x == 0)
                {                    
                    tempBuilder.Append(item.ToCSharpEventBody());                    
                }
                gameEvents.Remove(item);

                x++;
            }

            if (tempBuilder.Length != length)
            {
                tempBuilder.Append(@"
        }");

                builder.Append(tempBuilder.ToString());
            }

            tempBuilder = new StringBuilder(@"
        public override void Step(GameTime gameTime)
        {");
            length = tempBuilder.Length;
            x = 0;
            foreach (var item in Events.Where(ev => ev.EventType == BaseGameObjectEventType.Step))
            {
                if (x == 0)
                {
                    tempBuilder.Append(item.ToCSharpEventBody());
                }

                gameEvents.Remove(item);                

                x++;
            }

            if (tempBuilder.Length != length)
            {
                tempBuilder.Append(@"
        }");

                builder.Append(tempBuilder.ToString());
            }

            foreach (var eventfunc in gameEvents)
            {
                builder.AppendLine(eventfunc.ToCSharpEvent());
            }

            return builder.ToString();
        }

        public string ToCSharp(bool IsFirst = false, bool IsLast = false)
        {
            string StartPadding;
            
            if(IsFirst)
            {
                StartPadding = @"
namespace MGStudio.Script
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using MGStudio;
    using MGStudio.RunTime;
";
            }
            else
            {
                StartPadding = "";
            }
            
            return StartPadding + string.Format(
@"
    public class {0}{1}
    {3}{2}
    {4}
", ToCSharpName(), ToCSharpParentName(), ToCSharpGetBodyCode(), "{", "}") + (IsLast ? "}" : "");
        }
    }
}
