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
        No_button,
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
        Window_left_button,
        Window_right_button,
        Window_middle_button,
        Window_left_pressed,
        Window_right_pressed,
        Window_middle_pressed,
        Window_left_released,
        Window_right_released,
        Window_middle_released
    }
}
