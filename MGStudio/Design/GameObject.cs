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
        public Sprite ActiveSprite { get; set; } = null;
        public Sprite ActiveMask { get; set; } = null;
        public GameObject ParentGameObject { get; set; }
        public List<GameObjectEvents> Events { get; set; } = new List<GameObjectEvents>();
        public List<GameObjectProperty> Properties { get; set; } = new List<GameObjectProperty>();

        private string ToCSharpName()
        {
            return this.Name;
        }

        private string ToCSharpParentName()
        {
            return ParentGameObject == null ? "" : " : " + ParentGameObject.ToCSharpName();
        }        
        
        private string ToCSharpGetBodyCode()
        {
            // #TODO# Add No Mouse Click
            // #TODO# Handle Colision - Maybe we added a Listener - First Time New - Add For Each ColisionTypes WHERE IF (ObjectA IS TypeA && ObjectB IS TypeB) || (ObjectA IS TypeB && ObjectB IS TypeA) B ObjectA Collides With ObjectB Type Is Type

            var builder = new StringBuilder();
            
            builder.Append(
string.Format(@"
        public bool Visible {4} get; set; {5}{0}
        public bool Solid {4} get; set; {5}{1}
        public bool Persistent {4} get; set; {5}{2}
        public bool Depth {4} get; set; {5}{3}
        public float X {4} get; set; {5}
        public float Y {4} get; set; {5}        
        public float Rotation {4} get; set; {5}
        public SpriteM Sprite {4} get; set; {5}", 
        Visible ? " = true;" : "",
        Solid ? " = true;" : "",
        Persistent ? " = true;" : "",
        " = " + Depth.ToString() + ";", "{", "}"));

            var gameEvents = new List<GameObjectEvents>();
            gameEvents.AddRange(Events);

            foreach (var property in Properties)
            {
                builder.AppendFormat("\r\n        public {0} {1} {3} get; set; {4}{2}", property.Name, property.ToCSharpTypeObjectName(), property.ToCSharpExpression(), "{", "}");
            }
            StringBuilder tempBuilder;
            int length = 0;
            
            {
                tempBuilder = new StringBuilder(@"
        public void __keyboard(KeyboardState __ckeyState, KeyboardState __pkeyState)
        {");

                length = tempBuilder.Length;
                foreach (var item in Events.Where(ev => ev.EventType == BaseGameObjectEventType.KeyDown))
                {
                    var kba = item.EventArguments as KeyboardArgument;
                    if (kba != null && item.EventNodes != null && item.EventNodes.Count > 0)
                    {
                        string funcName = "__keyDown_" + kba.KeyCode.ToString("G");
                        tempBuilder.AppendFormat(@"
            if(__ckeyState.IsKeyDown(Keys.{0})){1}", kba.KeyCode.ToString("G"), "{ " + funcName + "(); }");
                        
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
            if(__ckeyState.IsKeyDown(Keys.{0}) && __pkeyState.IsKeyUp(Keys.{0})){1}", kba.KeyCode.ToString("G"), "{ " + funcName + "(); }");

                        
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
            if(__pkeyState.IsKeyDown(Keys.{0}) && __ckeyState.IsKeyUp(Keys.{0})){1}", kba.KeyCode.ToString("G"), "{ " + funcName + "(); }");
                        
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
        public void __Mouse(MouseState __cmouseState, MouseState __pmouseState, bool MouseInSprite)
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
            if(MouseInSprite && __cmouseState.LeftButton == ButtonState.Pressed){0}", "{ " + funcName + "(); }");
                                break;
                            case Mouses.Right_button:
                                tempBuilder.AppendFormat(@"
            if(MouseInSprite && __cmouseState.RightButton == ButtonState.Pressed){0}", "{ " + funcName + "(); }");
                                break;
                            case Mouses.Middle_button:
                                tempBuilder.AppendFormat(@"
            if(MouseInSprite && __cmouseState.MiddleButton == ButtonState.Pressed){0}", "{ " + funcName + "(); }");
                                break;
                            case Mouses.No__button:
                                tempBuilder.AppendFormat(@"
            if(MouseInSprite && __cmouseState.LeftButton == ButtonState.Released && __cmouseState.RightButton == ButtonState.Released && __cmouseState.MiddleButton == ButtonState.Released){0}", "{ " + funcName + "(); }");
                                break;
                            case Mouses.Left_pressed:
                                tempBuilder.AppendFormat(@"
            if(MouseInSprite && __cmouseState.LeftButton == ButtonState.Pressed && __pmouseState.LeftButton == ButtonState.Released){0}", "{ " + funcName + "(); }");
                                break;
                            case Mouses.Right_pressed:
                                tempBuilder.AppendFormat(@"
            if(MouseInSprite && __cmouseState.RightButton == ButtonState.Pressed && __pmouseState.RightButton == ButtonState.Released){0}", "{ " + funcName + "(); }");
                                break;
                            case Mouses.Middle_pressed:
                                tempBuilder.AppendFormat(@"
            if(MouseInSprite && __cmouseState.MiddleButton == ButtonState.Pressed && __pmouseState.MiddleButton == ButtonState.Released){0}", "{ " + funcName + "(); }");
                                break;
                            case Mouses.Left_released:
                                tempBuilder.AppendFormat(@"
            if(MouseInSprite && __cmouseState.LeftButton == ButtonState.Released && __pmouseState.LeftButton == ButtonState.Pressed){0}", "{ " + funcName + "(); }");
                                break;
                            case Mouses.Right_released:
                                tempBuilder.AppendFormat(@"
            if(MouseInSprite && __cmouseState.RightButton == ButtonState.Released && __pmouseState.RightButton == ButtonState.Pressed){0}", "{ " + funcName + "(); }");
                                break;
                            case Mouses.Middle_released:
                                tempBuilder.AppendFormat(@"
            if(MouseInSprite && __cmouseState.MiddleButton == ButtonState.Released && __pmouseState.MiddleButton == ButtonState.Pressed){0}", "{ " + funcName + "(); }");
                                break;
                            case Mouses.Mouse_enter:
                                break;
                            case Mouses.Mouse_leave:
                                break;
                            case Mouses.Mouse_wheel_up:
                                break;
                            case Mouses.Mouse_wheel_down:
                                break;
                            case Mouses.Global_left_button:
                                tempBuilder.AppendFormat(@"
            if(__cmouseState.LeftButton == ButtonState.Pressed){0}", "{ " + funcName + "(); }");
                                break;
                            case Mouses.Global_right_button:
                                tempBuilder.AppendFormat(@"
            if(__cmouseState.RightButton == ButtonState.Pressed){0}", "{ " + funcName + "(); }");
                                break;
                            case Mouses.Global_middle_button:
                                tempBuilder.AppendFormat(@"
            if(__cmouseState.MiddleButton == ButtonState.Pressed){0}", "{ " + funcName + "(); }");
                                break;
                            case Mouses.Global_left_pressed:
                                tempBuilder.AppendFormat(@"
            if(__cmouseState.LeftButton == ButtonState.Pressed && __pmouseState.LeftButton == ButtonState.Released){0}", "{ " + funcName + "(); }");
                                break;
                            case Mouses.Global_right_pressed:
                                tempBuilder.AppendFormat(@"
            if(__cmouseState.RightButton == ButtonState.Pressed && __pmouseState.RightButton == ButtonState.Released){0}", "{ " + funcName + "(); }");
                                break;
                            case Mouses.Global_middle_pressed:
                                tempBuilder.AppendFormat(@"
            if(__cmouseState.MiddleButton == ButtonState.Pressed && __pmouseState.MiddleButton == ButtonState.Released){0}", "{ " + funcName + "(); }");
                                break;
                            case Mouses.Global_left_released:
                                tempBuilder.AppendFormat(@"
            if(__cmouseState.LeftButton == ButtonState.Released && __pmouseState.LeftButton == ButtonState.Pressed){0}", "{ " + funcName + "(); }");
                                break;
                            case Mouses.Global_right_released:
                                tempBuilder.AppendFormat(@"
            if(__cmouseState.RightButton == ButtonState.Released && __pmouseState.RightButton == ButtonState.Pressed){0}", "{ " + funcName + "(); }");
                                break;
                            case Mouses.Global_middle_released:
                                tempBuilder.AppendFormat(@"
            if(__cmouseState.MiddleButton == ButtonState.Released && __pmouseState.MiddleButton == ButtonState.Pressed){0}", "{ " + funcName + "(); }");
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
        public void __Draw()
        {");
            length = tempBuilder.Length;
            x = 0;
            foreach (var item in Events.Where(ev => ev.EventType == BaseGameObjectEventType.Draw))
            {
                string funcName = "__Draw_" + x;
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
        public void __Step()
        {");
            length = tempBuilder.Length;
            x = 0;
            foreach (var item in Events.Where(ev => ev.EventType == BaseGameObjectEventType.Step))
            {
                string funcName = "__Step_" + x;
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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MGStudio;

namespace MGStudio.Script
{";
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
