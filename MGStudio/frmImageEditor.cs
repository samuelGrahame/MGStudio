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
using System.Reflection;
using DevExpress.XtraEditors.ViewInfo;

namespace MGStudio
{
    public partial class frmImageEditor : DevExpress.XtraEditors.XtraForm
    {
        public Bitmap DrawingImage { get; set; }

        public int DrawingImageWidth { get; set; }
        public int DrawingImageHeight { get; set; }

        public double Zoom = 100;

        public frmImageEditor()
        {
            InitializeComponent();
        }

        private void frmImageEditor_Load(object sender, EventArgs e)
        {
            if(DrawingImage == null)
            {
                DrawingImage = new Bitmap(DrawingImageWidth, DrawingImageHeight);
            }
            
            fastDraw1.Size = new Size(DrawingImageWidth, DrawingImageHeight);            
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }        

        private void pictureEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }
        bool IsMouseDown = false;
        

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Zoom *= 2;
            ZoomChanged();
        }

        public void ZoomChanged()
        {
            var scale = (Zoom / 100.0f);
            fastDraw1.Size = new Size((int)Math.Round(DrawingImageWidth * scale, 0), (int)Math.Round(DrawingImageHeight * scale, 0));         
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Zoom /= 2;
            ZoomChanged();
        }

        private void fastDraw1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.DrawImage(DrawingImage, new Rectangle(0, 0, fastDraw1.Width, fastDraw1.Height));
        }

        public void DrawLine(int x0, int y0, int x1, int y1, MouseButtons button)
        {
            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = (dx > dy ? dx : -dy) / 2, e2;
            for (;;)
            {
                DrawPixel(x0, y0, button, true);
                if (x0 == x1 && y0 == y1) break;
                e2 = err;
                if (e2 > -dx) { err -= dy; x0 += sx; }
                if (e2 < dy) { err += dx; y0 += sy; }
            }
        }

        private void fastDraw1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsMouseDown)
                return;
            
            DrawLine(prevMouse.X, prevMouse.Y, e.Location.X, e.Location.Y, e.Button);
            fastDraw1.Refresh();

            prevMouse = e.Location;
        }

        private void fastDraw1_MouseUp(object sender, MouseEventArgs e)
        {
            IsMouseDown = false;
        }

        public void DrawPixel(int X, int Y, MouseButtons button, bool DelayRefresh = false)
        {            
            Color foreColor = Color.Black;
            if (button == MouseButtons.Left)
            {
                foreColor = Color.Black;
            }
            else if (button == MouseButtons.Right)
            {
                foreColor = Color.Transparent;
            }
            else
            {
                return;
            }
            Bitmap bmp = DrawingImage;
            var scale = (decimal)(Zoom / 100.0f);
            
            var x = (int)(X / scale);
            var y = (int)(Y / scale);

            if (x < 0)
                x = 0;
            if (y < 0)
                y = 0;

            if (x >= DrawingImageWidth)
                x = DrawingImageWidth - 1;
            if (y >= DrawingImageHeight)
                y = DrawingImageHeight - 1;

            bmp.SetPixel(x, y, foreColor);
            if(!DelayRefresh)
            {
                fastDraw1.Refresh();
            }
            
        }
        Point prevMouse;

        private void fastDraw1_MouseDown(object sender, MouseEventArgs e)
        {
            IsMouseDown = true;
            prevMouse = e.Location;

            DrawPixel(e.X, e.Y, e.Button);
        }
    }
}