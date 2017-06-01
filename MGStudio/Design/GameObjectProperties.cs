using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGStudio.Design
{
    public class GameObjectProperty : ProjectItem
    {
        public GameObjectPropertyType PropertyType { get; set; } = GameObjectPropertyType.Object;
        public string Expression { get; set; }

        public string ToCSharpExpression()
        {
            return string.IsNullOrWhiteSpace(Expression) ? "" : $" = {Expression};";
        }

        public string ToCSharpTypeObjectName()
        {            
            switch (PropertyType)
            {
                default:
                case GameObjectPropertyType.Object:
                    return "object";                    
                case GameObjectPropertyType.Number:
                    return "double";
                case GameObjectPropertyType.Date:
                    return "DateTime";                    
                case GameObjectPropertyType.String:
                    return "string";                
            }
        }
    }

    public enum GameObjectPropertyType
    {
        Object,
        Number,
        Date,
        String
    }
}
