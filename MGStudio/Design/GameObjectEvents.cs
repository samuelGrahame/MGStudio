using MGStudio.BaseObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGStudio.Design
{
    public class GameObjectEvents : BaseGameObjectEvents
    {
        public List<GameObjectEventNode> EventNodes { get; set; } = new List<GameObjectEventNode>();
        public static string EventArugmentTemplate = @"
        public void {0}(object Other = null)
        {
{1}
        }";

        public string ToCSharpEvent(string OverrideName = "")
        {
            string _name = string.IsNullOrWhiteSpace(OverrideName) ? this.Name : OverrideName;
            return string.Format(EventArugmentTemplate, string.IsNullOrWhiteSpace(OverrideName) ? this.Name : OverrideName, ToCSharpEventBody());
        }

        public string ToCSharpEventBody()
        {
            var builder = new StringBuilder();

            foreach (var node in EventNodes)
            {
                string code = node.GetScriptCode();
                if(!string.IsNullOrWhiteSpace(code))
                    builder.Append(@"           " + code);
            }

            return builder.ToString();
        }
    }
}
