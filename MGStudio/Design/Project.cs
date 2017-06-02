using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGStudio.Design
{
    public class Project
    {
        public List<ProjectItem> SpriteDirectory = new List<ProjectItem>();
        public List<ProjectItem> SoundDirectory = new List<ProjectItem>();
        public List<ProjectItem> BackgroundDirectory = new List<ProjectItem>();
        public List<ProjectItem> PathsDirectory = new List<ProjectItem>();
        public List<ProjectItem> ScriptsDirectory = new List<ProjectItem>();
        public List<ProjectItem> FontsDirectory = new List<ProjectItem>();
        public List<ProjectItem> TimeLinesDirectory = new List<ProjectItem>();
        public List<ProjectItem> ObjectsDirectory = new List<ProjectItem>();
        public List<ProjectItem> RoomsDirectory = new List<ProjectItem>();        
    }

    public class ProjectItem
    {
        public string Name { get; set; }
    }

    public class ProjectFolder
    {
        public List<ProjectItem> Items = new List<ProjectItem>();
    }
}
