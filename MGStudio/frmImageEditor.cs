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
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;

namespace MGStudio
{
    public partial class frmImageEditor : DevExpress.XtraEditors.XtraForm
    {
        public Bitmap DrawingImage { get; set; }
        public static List<Color> ColorHistory = new List<Color>();
        
        public int DrawingImageWidth { get; set; }
        public int DrawingImageHeight { get; set; }

        public DrawTool DrawToolActive { get; set; } = DrawTool.Pencil;
        public ColorMode ColorModeActive { get; set; } = ColorMode.Blend;

        public enum DrawTool
        {
            Pencil,
            Rubber
        }

        public enum ColorMode
        {
            Blend,
            Replace
        }
        

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

            var dt = GetColorDataTable();

            for (int i = ColorHistory.Count - 1; i >= 0; i--)
            {
                DataRow dr = dt.NewRow();

                dr["Color"] = ColorHistory[i];

                dt.Rows.Add(dr);
            }

            dt.AcceptChanges();            
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
            //MGStudio.Properties.Resources.x32x32Trans      
            e.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
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
            Path = new Dictionary<long, Color>();
        }

        public Color GetCurrentColor(MouseButtons button)
        {
            switch (DrawToolActive)
            {
                case DrawTool.Pencil:
                    if(button == MouseButtons.Left)
                    {
                        return colorPickEdit1.Color;
                    }
                    else if (button == MouseButtons.Right)
                    {
                        return colorPickEdit2.Color;
                    }
                    break;
                case DrawTool.Rubber:
                    return Color.Transparent;
                default:
                    break;
            }

            return Color.Transparent;
        }

        //public List<Tuple<int, int, Color>> Path = new List<Tuple<int, int, Color>>();

        public Dictionary<long, Color> Path = new Dictionary<long, Color>();

        public Color Merge(int x, int y, Color ColorB, Color ColorA)
        {
            long key =  (x * y + DrawingImage.Width);
            if (Path.ContainsKey(key))
                return Path[key];

            return (Path[key] = Color.FromArgb(ColorA.A * ColorB.A / 255, ColorA.R * ColorB.R / 255, ColorA.G * ColorB.G / 255, ColorA.B * ColorB.B / 255));            
        }

        public Color NoColor = Color.FromArgb(0, 0, 0, 0);
        public void DrawPixel(int X, int Y, MouseButtons button, bool DelayRefresh = false)
        {            
            Color foreColor = GetCurrentColor(button);
            
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

            if(ColorModeActive == ColorMode.Blend && OpacityPen != 0 && OpacityPen != 255)
            {
                var ColorA = Color.FromArgb(OpacityPen, foreColor);
                var ColorB = bmp.GetPixel(x, y);
                if (ColorB == Color.Transparent || ColorB == NoColor)
                {                    
                    bmp.SetPixel(x, y, ColorA);
                }
                else
                {
                    bmp.SetPixel(x, y, Merge(x, y, ColorA, ColorB));                    
                }                
            }
            else
            {
                if(OpacityPen != 255)
                {
                    bmp.SetPixel(x, y, Color.FromArgb(OpacityPen, foreColor));
                }
                else
                {
                    bmp.SetPixel(x, y, foreColor);
                }                
            }
            
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

        sealed class Win32
        {
            [DllImport("user32.dll")]
            static extern IntPtr GetDC(IntPtr hwnd);

            [DllImport("user32.dll")]
            static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);

            [DllImport("gdi32.dll")]
            static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

            static public System.Drawing.Color GetPixelColor(int x, int y)
            {
                IntPtr hdc = GetDC(IntPtr.Zero);
                uint pixel = GetPixel(hdc, x, y);
                ReleaseDC(IntPtr.Zero, hdc);
                Color color = Color.FromArgb((int)(pixel & 0x000000FF),
                             (int)(pixel & 0x0000FF00) >> 8,
                             (int)(pixel & 0x00FF0000) >> 16);
                return color;
            }
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DrawToolActive = (DrawTool)radioGroup1.SelectedIndex;
        }

        bool ColorMouseDown = false;
        public Color Prev1;
        public Color Prev2;

        private void pictureEdit1_MouseDown(object sender, MouseEventArgs e)
        {
            Prev1 = colorPickEdit1.Color;
            Prev2 = colorPickEdit2.Color;

            ColorMouseDown = true;
            SetColorBy(e);
        }
        public bool LastChosen = true;
        public bool ControlDown = false;
        public void SetColorBy(MouseEventArgs e)
        {
            if(ControlDown)
            {
                var color = Win32.GetPixelColor(MousePosition.X, MousePosition.Y);
                if (e.Button == MouseButtons.Left)
                {
                    colorPickEdit1.Color = color;                    
                }
                else if (e.Button == MouseButtons.Right)
                {
                    colorPickEdit2.Color = color;                    
                }
            }
            else
            {
                using (Bitmap image = new Bitmap(pictureEdit1.Image))
                {
                    var scaleX = image.Width / (float)pictureEdit1.Width;
                    var scaleY = image.Height / (float)pictureEdit1.Height;
                    var x = (int)(e.X * scaleX);
                    var y = (int)(e.Y * scaleY);

                    if (x < 0)
                        x = 0;
                    if (y < 0)
                        y = 0;
                    if (x >= image.Width)
                        x = image.Width - 1;
                    if (y >= image.Height)
                        y = image.Height - 1;

                    var color = image.GetPixel(x, y);
                    if (e.Button == MouseButtons.Left)
                    {
                        colorPickEdit1.Color = color;
                        LastChosen = true;
                    }
                    else if (e.Button == MouseButtons.Right)
                    {
                        colorPickEdit2.Color = color;
                        LastChosen = false;
                    }
                }
            }
                    
        }

        private void pictureEdit1_MouseMove(object sender, MouseEventArgs e)
        {
            if(ColorMouseDown)
                SetColorBy(e);
        }

        private void pictureEdit1_MouseUp(object sender, MouseEventArgs e)
        {
            ColorMouseDown = false;
            if(Prev1 != colorPickEdit1.Color)
            {
                AddColorHistory(colorPickEdit1.Color);
            }
            if (Prev2 != colorPickEdit2.Color)
            {
                AddColorHistory(colorPickEdit2.Color);
            }
        }

        private void pictureEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void pictureEdit1_KeyUp(object sender, KeyEventArgs e)
        {
            
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            ControlDown = checkEdit1.Checked;
        }

        private void colorPickEdit1_EditValueChanged(object sender, EventArgs e)
        {
            if(!ColorMouseDown)
            {
                AddColorHistory(colorPickEdit1.Color);
            }
        }

        public void AddColorHistory(Color color)
        {
            var dt = GetColorDataTable();

            if (ColorHistory.Contains(color))
            {
                ColorHistory.Remove(color);
                
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if((Color)dt.Rows[i]["Color"] == color)
                    {
                        dt.Rows.RemoveAt(i);
                        dt.AcceptChanges();
                        break;
                    }
                }
            }
            
            ColorHistory.Add(color);
            DataRow dr = dt.NewRow();
            dr["Color"] = color;

            dt.Rows.InsertAt(dr, 0);
            dt.AcceptChanges();

            gridView1.MoveFirst();
            gridView1.MakeRowVisible(gridView1.FocusedRowHandle);
        }

        public DataTable GetColorDataTable()
        {
            var dt = gridControl1.DataSource as DataTable;
            if(dt == null)
            {
                dt = new DataTable();

                dt.Columns.Add("Color", typeof(Color));
                dt.AcceptChanges();
                gridControl1.DataSource = dt;
            }
            return dt;
        }

        private void gridView1_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle > -1)
            {
                e.Appearance.BackColor = (Color)gridView1.GetRowCellValue(e.RowHandle, "Color");
                //e.HighPriority = true;
            }
        }

        private void gridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if(gridView1.FocusedRowHandle > -1)
            {
                ColorMouseDown = true;
                if(e.Button == MouseButtons.Left)
                {
                    colorPickEdit1.Color = (Color)gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Color");
                }
                else if (e.Button == MouseButtons.Right)
                {
                    colorPickEdit2.Color = (Color)gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Color");
                }
                ColorMouseDown = false;
            }
        }
        byte OpacityPen = 255;
        private void spinEdit1_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                OpacityPen = (byte)int.Parse(spinEdit1.Text);
            }
            catch (Exception)
            {
                OpacityPen = 255;                
            }            
        }

        private void radioGroup2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ColorModeActive = (ColorMode)radioGroup2.SelectedIndex;
        }
    }
}