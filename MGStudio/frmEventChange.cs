using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using MGStudio.Design;
using MGStudio.BaseObjects;

namespace MGStudio
{
    public partial class frmEventChange : DevExpress.XtraEditors.XtraForm
    {
        public GameObjectEvents gameObjectEvent;

        public frmEventChange()
        {
            InitializeComponent();
        }

        private void frmEventChange_Load(object sender, EventArgs e)
        {
            simpleButton1.Left = (this.ClientSize.Width / 2) - (simpleButton1.Width / 2);
            if(gameObjectEvent == null)
            {
                gameObjectEvent = new GameObjectEvents();
            }
        }
        public bool CanDoCodeObject()
        {
            if(string.IsNullOrWhiteSpace(textEdit1.Text) || textEdit1.Text.StartsWith("__"))
            {
                MessageBox.Show("Please enter in a valid name.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textEdit1.Focus();
                return false;
            }
            else
            {
                return true;
            }
        }
        
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void simpleButton13_MouseDown(object sender, MouseEventArgs e)
        {
            if(CanDoCodeObject())
                popupMenuFunctionType.ShowPopup(Control.MousePosition);
        }

        private void simpleButton12_MouseDown(object sender, MouseEventArgs e)
        {
            if (CanDoCodeObject())
                popupMenuPropertyType.ShowPopup(Control.MousePosition);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            gameObjectEvent.EventType = BaseObjects.BaseGameObjectEventType.New;
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            gameObjectEvent.EventType = BaseObjects.BaseGameObjectEventType.Draw;
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            gameObjectEvent.EventType = BaseObjects.BaseGameObjectEventType.Delete;
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            gameObjectEvent.EventType = BaseObjects.BaseGameObjectEventType.Step;
        }

        private void simpleButton9_KeyDown(object sender, KeyEventArgs e)
        {

        }
        int KeyboardType;
        private void simpleButton9_MouseDown(object sender, MouseEventArgs e)
        {
            KeyboardType = Convert.ToInt32((sender as SimpleButton).Tag);
            popupMenuKey.ShowPopup(MousePosition);


        }
        public void SetMouseCode(int code)
        {
            var ma = new MouseArgument()
            {
                MouseCode = (Mouses)code
            };
            gameObjectEvent.EventArguments = ma;
            gameObjectEvent.EventType = BaseGameObjectEventType.Mouse;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        public void SetKeyCode(Microsoft.Xna.Framework.Input.Keys key)
        {
            var ka = new KeyboardArgument()
            {
                KeyCode = key
            };
            gameObjectEvent.EventArguments = ka;
            gameObjectEvent.EventType = KeyboardType == 0 ? BaseGameObjectEventType.KeyPress : KeyboardType == 1 ? BaseGameObjectEventType.KeyRelease : BaseGameObjectEventType.KeyDown;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetMouseCode(Convert.ToInt32(e.Item.Tag));
        }

        private void simpleButton3_MouseDown(object sender, MouseEventArgs e)
        {
            popupMenuMouse.ShowPopup(Control.MousePosition);
        }

        private void barButtonItem23_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetMouseCode(Convert.ToInt32(e.Item.Tag));
        }

        private void barButtonItem36_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetKeyCode(Microsoft.Xna.Framework.Input.Keys.Left);
        }

        private void barButtonItem37_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetKeyCode(Microsoft.Xna.Framework.Input.Keys.Up);
        }

        private void barButtonItem38_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetKeyCode(Microsoft.Xna.Framework.Input.Keys.Down);
        }

        private void barButtonItem39_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetKeyCode(Microsoft.Xna.Framework.Input.Keys.Right);
        }

        private void barButtonItem40_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetKeyCode(Microsoft.Xna.Framework.Input.Keys.A);
        }

        private void barButtonItem41_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetKeyCode(Microsoft.Xna.Framework.Input.Keys.W);
        }

        private void barButtonItem42_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetKeyCode(Microsoft.Xna.Framework.Input.Keys.S);
        }

        private void barButtonItem43_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetKeyCode(Microsoft.Xna.Framework.Input.Keys.D);
        }

        private void barButtonItem47_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetKeyCode(Microsoft.Xna.Framework.Input.Keys.Q);
        }

        private void barButtonItem48_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetKeyCode(Microsoft.Xna.Framework.Input.Keys.E);
        }

        private void barButtonItem49_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetKeyCode(Microsoft.Xna.Framework.Input.Keys.R);
        }

        private void barButtonItem50_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetKeyCode(Microsoft.Xna.Framework.Input.Keys.Space);
        }

        private void barButtonItem51_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetKeyCode(Microsoft.Xna.Framework.Input.Keys.Enter);
        }

        private void barButtonItem52_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetKeyCode(Microsoft.Xna.Framework.Input.Keys.Tab);
        }

        private void barButtonItem54_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetKeyCode(Microsoft.Xna.Framework.Input.Keys.NumPad0);
        }

        private void barButtonItem55_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetKeyCode(Microsoft.Xna.Framework.Input.Keys.NumPad1);
        }

        private void barButtonItem56_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetKeyCode(Microsoft.Xna.Framework.Input.Keys.NumPad2);
        }

        private void barButtonItem57_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetKeyCode(Microsoft.Xna.Framework.Input.Keys.NumPad3);
        }

        private void barButtonItem58_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetKeyCode(Microsoft.Xna.Framework.Input.Keys.NumPad4);
        }

        private void barButtonItem59_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetKeyCode(Microsoft.Xna.Framework.Input.Keys.NumPad5);
        }

        private void barButtonItem60_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetKeyCode(Microsoft.Xna.Framework.Input.Keys.NumPad6);
        }

        private void barButtonItem61_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetKeyCode(Microsoft.Xna.Framework.Input.Keys.NumPad7);
        }

        private void barButtonItem62_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetKeyCode(Microsoft.Xna.Framework.Input.Keys.NumPad8);
        }

        private void barButtonItem63_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetKeyCode(Microsoft.Xna.Framework.Input.Keys.NumPad9);
        }
    }
}