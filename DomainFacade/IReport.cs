using System;
using System.Collections.Generic;
using DianPing.BA.Framework.CommonSolution.Common.Util;
using DianPing.BA.ReportCenter.Domain.Entity;
using StrongCutIn.Util;

namespace DianPing.BA.ReportCenter.Domain.DomainFacade
{
    [AOPHelper(typeof(CatAopAspect), 0)]
    public interface IReport
    {
        /// <summary>
        ///   获取多维度下的度量值
        /// </summary>
        /// <param name="latitudes"> </param>
        /// <param name="numericalDefineID"> </param>
        /// <returns> </returns>
        NumericalValue GetNumericalValue(IEnumerable<LatitudeValue> latitudes, int numericalDefineID);

        /// <summary>
        ///   根据维度值列表和度量值定义ID，找到多维度下的度量值
        /// </summary>
        /// <param name="latitudes"> 维度值列表 </param>
        /// <param name="numericalDefineID"> 度量值定义ID </param>
        /// <returns> </returns>
        /*
         * 传入的维度值列表是二维列表，结构是这样的[["2013-01-01","2013-01-02"],["按月多次支付模板","新电商支付模板"]]
         * 先要把传入的维度值二维列表进行转换，转换成[["2013-01-01","按月多次支付模板"],["2013-01-01","新电商支付模板"],["2013-01-02","按月多次支付模板"]，["2013-01-02","新电商支付模板"]]
         * 返回的IEnumerable<KeyValuePair<IEnumerable<LatitudeValue>, NumericalValue>>中的IEnumerable<LatitudeValue>是转换后的维度值列表
         */
        IEnumerable<KeyValuePair<IEnumerable<LatitudeValue>, NumericalValue>> GetNumericalValue(
            IEnumerable<IEnumerable<LatitudeValue>> latitudes, int numericalDefineID);

        /// <summary>
        ///   根据时间范围找到时间维度的维度值列表，从而进一步找到数据库里的时间维度的维度值记录
        /// </summary>
        /// <param name="beginDate"> </param>
        /// <param name="endDate"> </param>
        /// <param name="defineID"> 维度定义ID </param>
        /// <param name="flag"> </param>
        /// <returns> </returns>
        IEnumerable<LatitudeValue> GetLatitudes(DateTime beginDate, DateTime endDate, int defineID,
                                                TimeLatitude flag = TimeLatitude.Date);

        /// <summary>
        ///   根据维度定义ID和维度值列表，找到数据库里的维度值记录
        /// </summary>
        /// <param name="defineID"> 维度定义ID </param>
        /// <param name="values"> 维度值列表 </param>
        /// <returns> </returns>
        IEnumerable<LatitudeValue> GetLatitudes(int defineID, IEnumerable<string> values);

        /// <summary>
        ///   根据度量值获取其所属的维度值列表
        /// </summary>
        /// <param name="numericalValue"> </param>
        /// <returns> </returns>
        IEnumerable<LatitudeValue> CheckLatitudes(NumericalValue numericalValue);

        /// <summary>
        ///   插入一个报表项（报表项由多个维度值和一个度量值组成）
        /// </summary>
        /// <param name="reportItem"> </param>
        /// <returns> </returns>
        bool InsertReportItem(ReportItem reportItem);
    }
}