using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGStudio.Design
{
    public class GameObjectEventNode : ProjectItem
    {
        public Bitmap Icon;
        public string Category;
        public string TabPage;
        public string ScriptCode;

        public virtual string GetScriptCode()
        {
            return ScriptCode;
        }

        public virtual T CreateNewFromThis<T>() where T : GameObjectEventNode
        {
            var x = Activator.CreateInstance<T>();
            if(this.Icon != null)
            {
                x.Icon = new Bitmap(this.Icon);
            }

            x.Category = Category;
            x.TabPage = TabPage;
            x.ScriptCode = ScriptCode;

            return x;
        }
    }
}
