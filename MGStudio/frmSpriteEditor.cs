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

namespace MGStudio
{
    public partial class frmSpriteEditor : DevExpress.XtraEditors.XtraForm
    {
        public List<Bitmap> Images = new List<Bitmap>();
        public int spr_Width { get; set; } = 32;
        public int spr_Height { get; set; } = 32;

        public frmSpriteEditor()
        {
            InitializeComponent();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (frmNewSprite frm = new frmNewSprite())
            {
                frm.calcEdit1.EditValue = spr_Width;
                frm.calcEdit2.EditValue = spr_Height;
                
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    spr_Width = int.Parse(frm.calcEdit1.Text.Replace(",", ""));
                    spr_Height = int.Parse(frm.calcEdit2.Text.Replace(",", ""));

                    winExplorerView1.OptionsViewStyles.Medium.ImageSize = new Size(spr_Width, spr_Height);

                    gridControl1.DataSource = null;

                    AddNewCell();
                }
            }
        }

        public void AddNewCell(Bitmap image = null, int index = -1)
        {
            var x = winExplorerView1.FocusedRowHandle;

            var dt = CheckIfExists();
            
            DataRow dr = dt.NewRow();

            dr["Image"] = image;
            dr["Text"] = "image " + dt.Rows.Count;            

            if(index == -1)
            {
                dt.Rows.Add(dr);
            }
            else
            {
                dt.Rows.InsertAt(dr, index);
            }
            

            dt.AcceptChanges();
          
            winExplorerView1.FocusedRowHandle = x;

            gridControl1.Refresh();
        }

        public DataTable CheckIfExists()
        {
            DataTable dt = gridControl1.DataSource as DataTable;
            if(dt == null)
            {
                dt = new DataTable();
                dt.Columns.Add("Image", typeof(Bitmap));
                dt.Columns.Add("Text", typeof(string));

                gridControl1.DataSource = dt;
            }
            return dt;
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AddNewCell();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Images = new List<Bitmap>();
            this.DialogResult = DialogResult.OK;

            foreach (DataRow rows in CheckIfExists().Rows)
            {
                Images.Add(rows["Image"] as Bitmap);
            }

            this.Close();
        }

        private void frmSpriteEditor_Load(object sender, EventArgs e)
        {
            winExplorerView1.OptionsViewStyles.Medium.ImageSize = new Size(spr_Width, spr_Height);

            foreach (var image in Images)
            {
                AddNewCell(image);
            }            
        }

        private void gridControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(winExplorerView1.FocusedRowHandle > -1)
            {
                using (frmImageEditor frm = new frmImageEditor())
                {
                    if(winExplorerView1.GetFocusedRowCellValue("Image") as Bitmap != null)
                    {
                        frm.DrawingImage = new Bitmap(winExplorerView1.GetFocusedRowCellValue("Image") as Bitmap);
                    }

                    frm.DrawingImageWidth = spr_Width;
                    frm.DrawingImageHeight = spr_Height;
                    this.Opacity = 0;
                    if (frm.ShowDialog(this) == DialogResult.OK)
                    {
                        if(winExplorerView1.GetFocusedRowCellValue("Image") as Bitmap != null)
                        {
                            (winExplorerView1.GetFocusedRowCellValue("Image") as Bitmap).Dispose();
                        }
                        winExplorerView1.SetFocusedRowCellValue("Image", frm.DrawingImage);
                    }
                    this.Opacity = 1;
                }
            }
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var index = winExplorerView1.GetDataSourceRowIndex(winExplorerView1.FocusedRowHandle);
            if (index < 0)
                index = -1;
            AddNewCell(null , index);
        }

        private void winExplorerView1_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete && winExplorerView1.FocusedRowHandle > -1)
            {
                var dt = CheckIfExists();
                ((DataRowView)winExplorerView1.GetFocusedRow()).Row.Delete();
                dt.AcceptChanges();
            }else if(ModifierKeys.HasFlag(Keys.Control))
            {
                if(e.KeyCode == Keys.V)
                {
                    try
                    {
                        var data = Clipboard.GetDataObject();
                        var currF = DataFormats.GetFormat(typeof(Bitmap).FullName);
                        if (data.GetDataPresent(currF.Name))
                        {
                            var bmp2 = (Bitmap)data.GetData(currF.Name);
                            if (CheckIfExists().Rows.Count == 0)
                            {
                                spr_Width = bmp2.Width;
                                spr_Height = bmp2.Height;

                                winExplorerView1.OptionsViewStyles.Medium.ImageSize = new Size(spr_Width, spr_Height);
                            }

                            Bitmap bmp = new Bitmap(bmp2, spr_Width, spr_Height);
                            AddNewCell(bmp);
                        }
                        else if (Clipboard.ContainsImage())
                        {
                            var image = Clipboard.GetImage();
                            if(CheckIfExists().Rows.Count == 0)
                            {
                                spr_Width = image.Width;
                                spr_Height = image.Height;

                                winExplorerView1.OptionsViewStyles.Medium.ImageSize = new Size(spr_Width, spr_Height);
                            }
                            Bitmap bmp = new Bitmap(Clipboard.GetImage(), spr_Width, spr_Height);
                            AddNewCell(bmp);
                        }
                        else if (data.GetDataPresent(DataFormats.FileDrop))
                        {                            
                            string[] paths = (string[])data.GetData(DataFormats.FileDrop);
                            foreach (var path in paths)
                            {
                                try
                                {
                                    if (System.IO.File.Exists(path))
                                    {
                                        using (Image img = Image.FromFile(path))
                                        {
                                            if (CheckIfExists().Rows.Count == 0)
                                            {
                                                spr_Width = img.Width;
                                                spr_Height = img.Height;

                                                winExplorerView1.OptionsViewStyles.Medium.ImageSize = new Size(spr_Width, spr_Height);
                                            }

                                            Bitmap bmp = new Bitmap(img, spr_Width, spr_Height);
                                            AddNewCell(bmp);
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                    
                                }                                
                            }                            
                        }
                    }
                    catch (Exception)
                    {

                    }
                }else if (e.KeyCode == Keys.C)
                {
                    CopyFocusedImage();
                }
                else if (e.KeyCode == Keys.X)
                {
                    CopyFocusedImage();

                    var dt = CheckIfExists();
                    ((DataRowView)winExplorerView1.GetFocusedRow()).Row.Delete();
                    dt.AcceptChanges();
                }
            }
        }

        public void CopyFocusedImage()
        {
            if (winExplorerView1.FocusedRowHandle > -1)
            {
                try
                {
                    var bitmap = winExplorerView1.GetFocusedRowCellValue("Image") as Bitmap;
                    if (bitmap == null)
                    {
                        bitmap = new Bitmap(spr_Width, spr_Height);
                        using (Graphics g = Graphics.FromImage(bitmap))
                        {
                            g.Clear(Color.Transparent);
                        }
                    }
                    IDataObject dataObj = new DataObject();
                    
                    dataObj.SetData(DataFormats.GetFormat(typeof(Bitmap).FullName).Name, bitmap);

                    Clipboard.SetDataObject(dataObj);                   
                }
                catch (Exception)
                {

                }
            }
        }
    }
}