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
using DevExpress.XtraTreeList.Nodes;
using MGStudio.Design;
using MGStudio.BaseObjects;

namespace MGStudio
{
    public partial class frmObject : DevExpress.XtraEditors.XtraForm
    {
        public TreeListNode Node;
        public GameObject gameObject;

        public frmObject()
        {
            InitializeComponent();
        }

        private void frmObject_Load(object sender, EventArgs e)
        {
            textEdit1.Text = gameObject.Name;
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            gameObject.Name = textEdit1.Text;

            this.Close();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            // Open Dialog..
            //GameObjectEvents
            using (var dlg = new frmEventChange())
            {
                if(dlg.ShowDialog() == DialogResult.OK)
                {
                    AddEvent(dlg.gameObjectEvent);
                }
            }
        }

        public void AddEvent(GameObjectEvents gameObjectEvent)
        {
            var dt = CheckEventDataTable();

            DataRow dr = dt.NewRow();

            dr["Event"] = gameObjectEvent;

            string description;
            string eventName = gameObjectEvent.EventType.ToString("G").ToLower();
            if (eventName.Contains("mouse") || eventName.Contains("key"))
            {
                if (gameObjectEvent.EventType == BaseObjects.BaseGameObjectEventType.Mouse)
                {
                    description = (gameObjectEvent.EventArguments as MouseArgument).MouseCode.ToString("G").Replace("_", " ");
                }
                else
                {                    
                    description = gameObjectEvent.EventType.ToString("G") + " - " + (gameObjectEvent.EventArguments as KeyboardArgument).KeyCode.ToString("G");
                }
            }
            else
            {
                description = gameObjectEvent.EventType.ToString("G");
            }

            dr["Description"] = description;

            dt.Rows.Add(dr);

            dt.AcceptChanges();
        }

        public DataTable CheckEventDataTable()
        {
            DataTable dt = gridControl1.DataSource as DataTable;
            if(dt == null)
            {
                dt = new DataTable();
                
                dt.Columns.Add("Event", typeof(GameObjectEvents));
                dt.Columns.Add("Description", typeof(string));

                dt.AcceptChanges();

                gridControl1.DataSource = dt;
            }

            return dt;
        }

        private void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if(e.IsGetData && e.Column.FieldName == "Icon")
            {
                var gameEvent = gridView1.GetRowCellValue(gridView1.GetRowHandle(e.ListSourceRowIndex), "Event") as GameObjectEvents;
                if(gameEvent == null)
                {
                    e.Value = null;
                }
                else
                {
                    switch (gameEvent.EventType)
                    {
                        case BaseObjects.BaseGameObjectEventType.New:
                            e.Value = MGStudio.Properties.Resources.suggestion_16x16;
                            break;
                        case BaseObjects.BaseGameObjectEventType.Delete:
                            e.Value = MGStudio.Properties.Resources.delete_16x162;
                            break;
                        case BaseObjects.BaseGameObjectEventType.Mouse:
                            e.Value = MGStudio.Properties.Resources.pointer_16x16;
                            break;
                        case BaseObjects.BaseGameObjectEventType.Draw:
                            e.Value = MGStudio.Properties.Resources.image_16x161;
                            break;
                        case BaseObjects.BaseGameObjectEventType.Step:
                            e.Value = MGStudio.Properties.Resources.walking_16x16;
                            break;
                        case BaseObjects.BaseGameObjectEventType.KeyPress:
                            e.Value = MGStudio.Properties.Resources.next_16x16;
                            break;
                        case BaseObjects.BaseGameObjectEventType.KeyRelease:
                            e.Value = MGStudio.Properties.Resources.previous_16x16;
                            break;
                        case BaseObjects.BaseGameObjectEventType.KeyDown:
                            e.Value = MGStudio.Properties.Resources.download_16x16;
                            break;
                        case BaseObjects.BaseGameObjectEventType.Trigger:
                            e.Value = MGStudio.Properties.Resources.csharp_16x16;
                            break;
                        case BaseObjects.BaseGameObjectEventType.Collision:
                            e.Value = MGStudio.Properties.Resources.squeeze_16x16;
                            break;
                        default:
                        case BaseObjects.BaseGameObjectEventType.Timer:
                            e.Value = null;
                            break;                        
                    }
                }
            }
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {

        }

        public void RefreshEventNodes()
        {

        }

        private void gridView2_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if(e.Column.FieldName == "Description" && e.RowHandle > -1)
            {                
                var goen = gridView2.GetRowCellValue(e.RowHandle, "EventNode") as GameObjectEventNode;

                if (goen != null)
                {

                }

                e.Handled = true;
            }
        }
    }
}