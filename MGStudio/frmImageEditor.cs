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
        public LockBitmap UnlockedBitmap { get; set; } = null;

        public static List<Color> ColorHistory = new List<Color>();
        
        public int DrawingImageWidth { get; set; }
        public int DrawingImageHeight { get; set; }

        public DrawTool DrawToolActive { get; set; } = DrawTool.Pencil;
        public ColorMode ColorModeActive { get; set; } = ColorMode.Blend;

        public enum DrawTool
        {
            Pencil,
            Rubber,
            PaintBucket
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
                using (Graphics g = Graphics.FromImage(DrawingImage))
                {
                    g.Clear(Color.Transparent);
                }
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

            pictureEdit2.Image = DrawingImage;
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

            pictureEdit2.Refresh();
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
            UnlockedBitmap = new LockBitmap(DrawingImage);
            UnlockedBitmap.LockBits();

            DrawLine(prevMouse.X, prevMouse.Y, e.Location.X, e.Location.Y, e.Button);

            UnlockedBitmap.UnlockBits();
            UnlockedBitmap = null;

            fastDraw1.Refresh();

            prevMouse = e.Location;
        }

        private void fastDraw1_MouseUp(object sender, MouseEventArgs e)
        {
            IsMouseDown = false;
            Path = new Dictionary<Tuple<int, int>, Color>();
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

        public Dictionary<Tuple<int, int>, Color> Path = new Dictionary<Tuple<int, int>, Color>();

        public Color Merge(int x, int y, Color ColorB, Color ColorA)
        {
            Tuple<int, int> key = new Tuple<int, int>(x, y);
            if (Path.ContainsKey(key))
                return Path[key];

            int rA = ColorA.R;
            int rB = ColorB.R;

            int aA = ColorA.A;
            int aB = ColorB.A;

            int gA = ColorA.G;
            int gB = ColorB.G;

            int bA = ColorA.B;
            int bB = ColorB.B;

            int rOut = (rA * aA / 255) + (rB * aB * (255 - aA) / (255 * 255));
            int gOut = (gA * aA / 255) + (gB * aB * (255 - aA) / (255 * 255));
            int bOut = (bA * aA / 255) + (bB * aB * (255 - aA) / (255 * 255));
            int aOut = aA + (aB * (255 - aA) / 255);

            return (Path[key] = Color.FromArgb(aOut, rOut, gOut, bOut));            
        }

        public Color NoColor = Color.FromArgb(0, 0, 0, 0);

        public Point TranslatePoint(int x, int y)
        {
            var scale = (decimal)(Zoom / 100.0f);

            x = (int)(x / scale);
            y = (int)(y / scale);

            if (x < 0)
                x = 0;
            if (y < 0)
                y = 0;

            if (x >= DrawingImageWidth)
                x = DrawingImageWidth - 1;
            if (y >= DrawingImageHeight)
                y = DrawingImageHeight - 1;

            return new Point(x, y);
        }

        public void DrawPixel(int X, int Y, MouseButtons button, bool DelayRefresh = false)
        {            
            Color foreColor = GetCurrentColor(button);
            
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

            if(ColorModeActive == ColorMode.Blend && ((OpacityPen != 0 && OpacityPen != 255) || (foreColor.A != 0 && foreColor.A != 255)))
            {            
                var ColorA = (foreColor.A != 0 && foreColor.A != 255) ? foreColor : Color.FromArgb(OpacityPen, foreColor);
                var ColorB = UnlockedBitmap.GetPixel(x, y);
                if (ColorB == Color.Transparent || ColorB == NoColor)
                {
                    UnlockedBitmap.SetPixel(x, y, ColorA);
                }
                else
                {
                    UnlockedBitmap.SetPixel(x, y, Merge(x, y, ColorA, ColorB));                    
                }                
            }
            else
            {
                if(OpacityPen != 255)
                {
                    UnlockedBitmap.SetPixel(x, y, Color.FromArgb(OpacityPen, foreColor));
                }
                else
                {
                    UnlockedBitmap.SetPixel(x, y, foreColor);
                }                
            }
            
            if(!DelayRefresh)
            {
                fastDraw1.Refresh();
            }
            
        }
        Point prevMouse;

        
        public bool GoBack = false;
        public int StartX;
        public int StartY;        

        public class Pixel
        {
            public int X;
            public int Y;
            public Color Color;
        }

        public void FloodArea(int x, int y, Color colorToSet, Color colorClickedOn)
        {
            Dictionary<Tuple<int, int>, bool> PosDone = new Dictionary<Tuple<int, int>, bool>();
            Queue<Pixel> PixelStack = new Queue<Pixel>();

            PixelStack.Enqueue(new Pixel() { Color = colorClickedOn, X = x, Y = y });

            while (PixelStack.Count > 0)
            {
                var pixel = PixelStack.Dequeue();
                x = pixel.X;
                y = pixel.Y;

                var key = new Tuple<int, int>(x, y);
                
                if (x < 0 || y < 0 || y > DrawingImageHeight - 1 || x > DrawingImageWidth - 1 || PosDone.ContainsKey(key))
                {                    
                    continue;
                }
                PosDone[key] = true;
                var color = UnlockedBitmap.GetPixel(x, y); // DrawingImage.GetPixel(x, y);

                if (color.A == 0)
                {
                    color = Color.Transparent;
                }
                if (color == colorClickedOn)
                {
                    UnlockedBitmap.SetPixel(x, y, colorToSet);                    
                }
                else
                {
                    continue;
                }

                PixelStack.Enqueue(new Pixel() { Color = color, X = x - 1, Y = y });
                PixelStack.Enqueue(new Pixel() { Color = color, X = x, Y = y - 1 });

                PixelStack.Enqueue(new Pixel() { Color = color, X = x, Y = y + 1 });
                PixelStack.Enqueue(new Pixel() { Color = color, X = x + 1, Y = y });                
            }            
        }

        private void fastDraw1_MouseDown(object sender, MouseEventArgs e)
        {

            if (DrawToolActive == DrawTool.PaintBucket)
            {
                if (e.Button != MouseButtons.Left && e.Button != MouseButtons.Right)
                    return;

                var point = TranslatePoint(e.X, e.Y);

                Color ColorClicked = DrawingImage.GetPixel(point.X, point.Y);
                if(ColorClicked.A == 0)
                {
                    ColorClicked = Color.Transparent;
                }

                Color ColorToSet = Color.Empty;
                if (e.Button == MouseButtons.Left)
                    ColorToSet = colorPickEdit1.Color;
                else if (e.Button == MouseButtons.Right)
                    ColorToSet = colorPickEdit2.Color;
                UnlockedBitmap = new LockBitmap(DrawingImage);
                UnlockedBitmap.LockBits();

                FloodArea(point.X, point.Y, ColorToSet, ColorClicked);

                UnlockedBitmap.UnlockBits();
                UnlockedBitmap = null;

                fastDraw1.Refresh();
            }
            else
            {
                IsMouseDown = true;
                prevMouse = e.Location;
                UnlockedBitmap = new LockBitmap(DrawingImage);
                UnlockedBitmap.LockBits();

                DrawPixel(e.X, e.Y, e.Button, true);

                UnlockedBitmap.UnlockBits();
                UnlockedBitmap = null;

                fastDraw1.Refresh();
            }                       
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

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {                        
            using (Graphics g = Graphics.FromImage(DrawingImage))
            {
                g.Clear(Color.Transparent);
            }
            fastDraw1.Refresh();
        }

        private void radioGroup3_SelectedIndexChanged(object sender, EventArgs e)
        {
            colorPickEdit3.Enabled = radioGroup3.SelectedIndex == 1;
            RefreshBackground();
        }

        public void RefreshBackground()
        {
            if(radioGroup3.SelectedIndex == 0)
            {
                fastDraw1.BackgroundImage = Properties.Resources.x32x32Trans;
            }
            else
            {
                fastDraw1.BackgroundImage = null;
                fastDraw1.BackColor = colorPickEdit3.Color;
            }
        }

        private void colorPickEdit3_EditValueChanged(object sender, EventArgs e)
        {
            RefreshBackground();
        }

        private void gridView1_MouseUp(object sender, MouseEventArgs e)
        {
            
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            
        }

        private void gridView1_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                var rowHandles = gridView1.GetSelectedRows();

                if(rowHandles != null && rowHandles.Length > 0)
                {
                    var Rows = new DataRow[rowHandles.Length];

                    for (int i = 0; i < rowHandles.Length; i++)
                    {
                        Rows[i] = ((DataRowView)gridView1.GetRow(rowHandles[i])).Row;
                        var color = (Color)Rows[i]["Color"];                        
                        ColorHistory.Remove(color);
                    }
                    var dt = GetColorDataTable();
                    for (int i = 0; i < rowHandles.Length; i++)
                    {
                        dt.Rows.Remove(Rows[i]);
                    }
                    dt.AcceptChanges();                    
                }
            }
        }

        private void gridControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (gridView1.FocusedRowHandle > -1)
            {
                ColorMouseDown = true;
                if (e.Button == MouseButtons.Left)
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
    }
}