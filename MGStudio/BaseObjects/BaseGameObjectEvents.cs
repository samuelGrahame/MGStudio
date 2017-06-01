using MGStudio.Design;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGStudio.BaseObjects
{
    public class BaseGameObjectEvents : ProjectItem
    {
        public BaseGameObjectEventType EventType { get; set; }
        public EventArguments EventArguments { get; set; }
    }

    public enum BaseGameObjectEventType
    {
        New,
        Delete,
        Mouse,
        Draw,
        Step,
        KeyPress,
        KeyRelease,
        KeyDown,
        Trigger,
        Collision,
        Timer
    }

    public class EventArguments
    {

    }

    public class KeyboardArgument : EventArguments
    {
        public Keys KeyCode { get; set; }        
    }

    public class MouseArgument : EventArguments
    {
        public Mouses MouseCode { get; set; }
    }

    public enum Mouses
    {        
        Left_button,
        Right_button,
        Middle_button,
        No__button,
        Left_pressed,
        Right_pressed,
        Middle_pressed,
        Left_released,
        Right_released,
        Middle_released,
        Mouse_enter,
        Mouse_leave,
        Mouse_wheel_up,
        Mouse_wheel_down,
        Global_left_button,
        Global_right_button,
        Global_middle_button,
        Global_left_pressed,
        Global_right_pressed,
        Global_middle_pressed,
        Global_left_released,
        Global_right_released,
        Global_middle_released
    }
}
