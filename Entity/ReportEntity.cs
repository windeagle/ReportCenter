using System;
using System.Collections.Generic;

namespace DianPing.BA.ReportCenter.Domain.Entity
{
    public enum TimeLatitude
    {
        Date = 0,
        Month = 1,
        Quarter = 2
    }

    /// <summary>
    ///   维度值-数值 关系表
    /// </summary>
    public class LNRelation
    {
        public int LNRealtionID { get; set; }
        public int LatitudeID { get; set; }
        public int NumericalID { get; set; }
    }

    /// <summary>
    ///   维度值表
    /// </summary>
    public class LatitudeValue
    {
        public int LatitudeID { get; set; }
        public int DefineID { set; get; }
        ///// <summary>
        ///// 维度名，如“模版类型，月份等”
        ///// </summary>
        //public int LatitudeName { set; get; }
        public String Value { set; get; }
    }

    /// <summary>
    ///   数值表
    /// </summary>
    public class NumericalValue
    {
        public int NumericalID { get; set; }
        public int DefineID { set; get; }
        ///// <summary>
        ///// 数值名，如“总金额”
        ///// </summary>
        //public int NumericalName { set; get; }
        public String Value { set; get; }
    }

    public class ReportItem
    {
        public NumericalValue Value { get; set; }
        public IEnumerable<LatitudeValue> Latitudes { get; set; }
    }
}