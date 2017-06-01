using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;
using MGStudio.Design;
using DevExpress.XtraTreeList.Nodes;
using MGStudio.BaseObjects;
using CSScriptLibrary;
using MGStudio.RunTime;

namespace MGStudio
{
    public partial class frmMainForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public Project ActiveProject { get; set; } = new Project();

        public frmMainForm()
        {
            InitializeComponent();
        }               

        public void LoadDataToScreen()
        {
            foreach (TreeListNode item in treeList1.Nodes)
            {
                if (item != null)
                    item.Nodes.Clear();
            }
        }

        private void treeList1_ShowingEditor(object sender, CancelEventArgs e)
        {
            if(treeList1.FocusedNode == null || treeList1.FocusedNode.ParentNode == null || !ShowEditor)
            {
                e.Cancel = true;
            }
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            var sprite = new DesignSprite() { Height = 32, Width = 32, Name = "sprite_" + ActiveProject.SpriteDirectory.Count };
            TreeListNode node;
            if (treeList1.FocusedNode == null)
            {
                node = treeList1.AppendNode(new object[] { sprite.Name }, treeList1.FocusedNode);
            }
            else
            {
                node = treeList1.AppendNode(new object[] { sprite.Name }, treeList1.FocusedNode.RootNode);
            }

            node.Tag = sprite;
            node.ParentNode.Expanded = true;
            node.ImageIndex = -1;
            node.SelectImageIndex = -1;
            node.StateImageIndex = -1;

            ActiveProject.SpriteDirectory.Add(sprite);

            var newx = new frmSprites();
            newx.ActiveSprite = sprite;
            newx.MdiParent = this; 
            newx.Node = node;
            newx.TopLevel = false;
            newx.Show();
                
        }

        private void treeList1_MouseUp(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                if(IsFocusedNodeThisIndex(0))
                {
                    popupMenu1.ShowPopup(Control.MousePosition);
                }
            }
            else
            {
                if(ShowEditor)
                {
                    treeList1.ShowEditor();
                }
                else
                {
                    ShowEditor = true;
                }

            }
        }

        public bool IsFocusedNodeThisIndex(int index)
        {
            return (treeList1.FocusedNode.RootNode.Id == 0);
        }

        private void frmMainForm_Load(object sender, EventArgs e)
        {

        }

        private void treeList1_GetSelectImage(object sender, DevExpress.XtraTreeList.GetSelectImageEventArgs e)
        {
           
        }

        private void treeList1_CustomDrawNodeImages(object sender, DevExpress.XtraTreeList.CustomDrawNodeImagesEventArgs e)
        {
            if (e.Node.ParentNode != null)
            {
                if (e.Node.RootNode.Id == 0)
                {
                    var x = ((DesignSprite)e.Node.Tag).GetImages();
                    if (x.Count > 0)
                    {                        
                        e.Graphics.DrawImage(x[0], e.Bounds);                        
                        e.Handled = true;

                        for (int i = 0; i < x.Count; i++)
                        {
                            if (x[i] != null)
                                x[i].Dispose();
                        }
                    }
                    else
                    {

                    }
                }
            }
        }

        private void treeList1_CustomDrawNodeCell(object sender, DevExpress.XtraTreeList.CustomDrawNodeCellEventArgs e)
        {
            
           
        }

        private void treeList1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowEditor = false;
            treeList1.CloseEditor();
            if (treeList1.FocusedNode != null)
            {
                if (treeList1.FocusedNode.ParentNode != null)
                {
                    if(treeList1.FocusedNode.RootNode.Id == 0)
                    {
                        var newx = new frmSprites();
                        newx.ActiveSprite = treeList1.FocusedNode.Tag as DesignSprite;
                        newx.MdiParent = this;
                        newx.Node = treeList1.FocusedNode;
                        newx.TopLevel = false;
                        newx.Show();
                    }
                }
            }
        }
        bool ShowEditor = false;
        private void treeList1_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            ShowEditor = false;
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            var x = new GameObject() { Name = "Human" };
            x.Properties.Add(new GameObjectProperty() { Name = "Name", Expression = "", PropertyType = GameObjectPropertyType.String });
            x.Events.Add(new GameObjectEvents() { EventType = BaseObjects.BaseGameObjectEventType.KeyPress, EventArguments = new KeyboardArgument() { KeyCode = Microsoft.Xna.Framework.Input.Keys.Up } });

            var assembly = CSScript.LoadCode(x.ToCSharp(true, true));

            if(assembly != null)
            {
                var entity = (Entity)assembly.CreateObject("*");

                if(entity != null)
                {

                }
            }
        }
    }
}