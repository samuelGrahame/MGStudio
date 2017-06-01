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
using DevExpress.XtraTreeList.Nodes;

namespace MGStudio
{
    public partial class frmSprites : DevExpress.XtraEditors.XtraForm
    {
        public DesignSprite ActiveSprite { get; set; }
        public TreeListNode Node { get; set; }

        public frmSprites()
        {
            InitializeComponent();
        }

        private void frmSprites_Load(object sender, EventArgs e)
        {
            textEdit1.Text = ActiveSprite.Name;
            calcEdit1.EditValue = ActiveSprite.OriginX;
            calcEdit2.EditValue = ActiveSprite.OriginY;

            labelControl2.Text = "Width: " + ActiveSprite.Width;
            labelControl3.Text = "Height: " + ActiveSprite.Height;

            NewWidth = ActiveSprite.Width;
            NewHeight = ActiveSprite.Height;

            Bitmaps = ActiveSprite.GetImages();
            if (Bitmaps.Count > 0)
            {
                pictureEdit1.Image = Bitmaps[0];
            }
            else
            {
                pictureEdit1.Image = null;
            }
            pictureEdit1.Size = new Size(NewWidth, NewHeight);

        }

        private void textEdit1_EditValueChanged(object sender, EventArgs e)
        {
            
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            Node.SetValue(0, textEdit1.Text);
            ActiveSprite.Name = textEdit1.Text;

            ActiveSprite.OriginX = int.Parse(calcEdit1.Text);
            ActiveSprite.OriginY = int.Parse(calcEdit2.Text);

            if(Changed)
            {
                ActiveSprite.Height = NewHeight;
                ActiveSprite.Width = NewWidth;
                ActiveSprite.SetImages(Bitmaps, true);
                Node.TreeList.Refresh();
            }

            this.Close();
        }

        public bool Changed = false;
        public List<Bitmap> Bitmaps = new List<Bitmap>();
        public int NewWidth;
        public int NewHeight;
        
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            using (frmSpriteEditor frm = new frmSpriteEditor())
            {
                frm.Images = Bitmaps;
                frm.spr_Width = NewWidth;
                frm.spr_Height = NewHeight;

                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    Changed = true;
                    Bitmaps = frm.Images;
                    NewWidth = frm.spr_Width;
                    NewHeight = frm.spr_Height;

                    labelControl2.Text = "Width: " + NewWidth;
                    labelControl3.Text = "Height: " + NewHeight;

                    if (Bitmaps.Count > 0)
                    {
                        pictureEdit1.Image = Bitmaps[0];
                    }
                    else
                    {
                        pictureEdit1.Image = null;
                    }
                    pictureEdit1.Size = new Size(NewWidth, NewHeight);
                }
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            calcEdit1.EditValue = (int)(NewWidth / 2.0f);
            calcEdit2.EditValue = (int)(NewHeight / 2.0f);
        }

        private void calcEdit1_EditValueChanged(object sender, EventArgs e)
        {
            pictureEdit1.Refresh();
        }

        private void calcEdit2_EditValueChanged(object sender, EventArgs e)
        {
            pictureEdit1.Refresh();
        }

        private void pictureEdit1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(Pens.Purple, 0.0f, (int)calcEdit2.EditValue, pictureEdit1.Width, (int)calcEdit2.EditValue);
            e.Graphics.DrawLine(Pens.Purple, (int)calcEdit1.EditValue, 0.0f, (int)calcEdit1.EditValue, pictureEdit1.Height);
        }
    }
}