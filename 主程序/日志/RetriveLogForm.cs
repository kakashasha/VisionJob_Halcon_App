﻿using dotNetLab.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace shikii.VisionJob
{
    public partial class RetriveLogForm : Form
    {
        public RetriveLogForm()
        {
            InitializeComponent();
            this.KeyDown += (sender, e) =>
             {
                 if(e.KeyData == (Keys.Control|Keys.Q))
                 {
                     AppManager.ShowCompactDBEditor("Log.mdf");
                 }
             };

        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            String WhichLogTable = null;
             bool isSearchDay = false;
            if(dateTimePicker1.Text.Equals(dateTimePicker2.Text))
            {
                isSearchDay = true;
            }
            if(dateTimePicker2.Value <dateTimePicker2.Value)
            {
                dotNetLab.Tipper.Error = "第二个日期控件的日期必须大于或者等于前一个日期控件的日期。";
                return;
            }
            if (isSearchDay)
            {
                String searchTime = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                WhichLogTable = String.Format("_{0}_{1}", dateTimePicker1.Value.Year, dateTimePicker1.Value.Month);
               this.dataGridView1.DataSource =  dotNetLab.Common.R.LogDB.ProvideTable(String.Format("SELECT Fire_Time as 触发时间,Message as 基本信息 FROM {0} where Fire_Time like '{1}%';", WhichLogTable,searchTime), dotNetLab.Data.DBOperator.OPERATOR_QUERY_TABLE);
                
                 
            }
            else
            {
                String searchTime1 = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                DateTime dt  = dateTimePicker2.Value.AddDays(1);
                String searchTime2 = dt.ToString("yyyy-MM-dd");
                dotNetLab.Common.R.LogDB.GetAllTableNames();
                DataTable dtx = new DataTable();
                dtx.Columns.Add();
                dtx.Columns[0].ColumnName = "触发时间";
                dtx.Columns.Add();
                dtx.Columns[1].ColumnName = "基本信息";
                int nIndex = 0;
                for (int i = 0; i < dotNetLab.Common.R.LogDB.AllTableNames.Count; i++)
                {
                    if (!dotNetLab.Common.R.LogDB.DefaultTable.Equals(dotNetLab.Common.R.LogDB.AllTableNames[i]))
                    {
                        WhichLogTable = dotNetLab.Common.R.LogDB.AllTableNames[i];
                        DataTable xdt = dotNetLab.Common.R.LogDB.ProvideTable(String.Format("SELECT Fire_Time as 触发时间,Message  as 基本信息 FROM {0} where Fire_Time >='{1}' and Fire_Time <'{2}';", WhichLogTable, searchTime1, searchTime2), dotNetLab.Data.DBOperator.OPERATOR_QUERY_TABLE);
                        
                        if (xdt != null)
                        {
                            if (xdt.Rows.Count > 0)
                            {
                               
                                for (int j = 0; j < xdt.Rows.Count; j++)
                                {
                                    dtx.Rows.Add();
                                    for (int z = 0; z < xdt.Columns.Count; z++)
                                    {

                                        dtx.Rows[nIndex][z] = xdt.Rows[j][z];
                                    }
                                    
                                    nIndex++;
                                }

                            }
                        }
                    }
                }
                this.dataGridView1.DataSource = dtx;
            }
        }

        private void lnk_ExportExecel_Clicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PortableOffice.ExportToExcel(this.dataGridView1);
        }
    }
}
