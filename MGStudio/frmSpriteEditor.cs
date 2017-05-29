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

        public void AddNewCell(Bitmap image = null)
        {            
            var dt = CheckIfExists();
            
            DataRow dr = dt.NewRow();

            dr["Image"] = image;
            dr["Text"] = "image " + dt.Rows.Count;            

            dt.Rows.Add(dr);

            dt.AcceptChanges();
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
    }
}