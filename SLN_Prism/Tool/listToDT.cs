using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLN_Prism.Tool
{
  public  class listToDT
    {
        /// <summary>
        /// 将List转换为DataTable
        /// </summary>
        /// <param name="list">请求数据</param>
        /// <returns></returns>
        public DataTable ListToDataTable<T>(List<T> list)
        {

            //创建一个名为"tableName"的空表
            DataTable dt = new DataTable("tableName");
            var aa = typeof(T).GetProperties();
            //创建传入对象名称的列
            foreach (var item in typeof(T).GetProperties())
            {
                dt.Columns.Add(item.Name);
            }
            //循环存储
            foreach (var item in list)
            {
                //新加行
                DataRow value = dt.NewRow();
                //根据DataTable中的值，进行对应的赋值
                foreach (DataColumn dtColumn in dt.Columns)
                {
                    int i = dt.Columns.IndexOf(dtColumn);

                    value[i] = item.GetType().GetProperty(dtColumn.ColumnName).GetValue(item);

                }
                dt.Rows.Add(value);
            }
            return dt;
        }
    }
}
