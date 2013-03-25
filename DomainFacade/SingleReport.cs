using System.Collections.Generic;
using System.Linq;

namespace DianPing.BA.ReportCenter.Domain.DomainFacade
{
    //三维的数据变成二维XY报表，多余的一维就是NAME
    //三维以上的数据变成二维XY报表，多余的维度值就是NAME数组，显示时以/分隔, 如“V6模版/上海地区”的最近30天交易额报表
    /// <summary>
    ///   单报表
    /// </summary>
    public abstract class SingleReport
    {
        public string Name { set; get; }
        public IEnumerable<object> ObjectData { get; set; }
    }

    //三维的数据变成二维XY报表，多余的一维就是NAME
    //三维以上的数据变成二维XY报表，多余的维度值就是NAME数组，显示时以/分隔, 如“V6模版/上海地区”的最近30天交易额报表
    /// <summary>
    ///   单报表
    /// </summary>
    /// <typeparam name="TY"> </typeparam>
    public class SingleReport<TY> : SingleReport
    {
        public List<TY> Data
        {
            get { return ObjectData.Cast<TY>().ToList(); }
            set { ObjectData = value.Cast<object>(); }
        }
    }
}