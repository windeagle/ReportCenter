using System;
using System.Collections.Generic;
using System.Linq;
using DianPing.BA.Framework.CommonSolution.Common.Util;
using DianPing.BA.ReportCenter.Domain.DataFacade;
using DianPing.BA.ReportCenter.Domain.Entity;

namespace DianPing.BA.ReportCenter.Domain.DomainFacade
{
    public class Report : IReport
    {
        #region IReport Members

        /// <summary>
        ///   获取多维度下的度量值
        /// </summary>
        /// <param name="latitudes"> </param>
        /// <param name="numericalDefineID"> </param>
        /// <returns> </returns>
        public virtual NumericalValue GetNumericalValue(IEnumerable<LatitudeValue> latitudes, int numericalDefineID)
        {
            var reportDF = DependencyResolver.UnityDependencyResolver.Resolve<IReportDF>();
            var latitudeList = new List<LatitudeValue>();

            foreach (var latitude in latitudes)
            {
                var lt = reportDF.GetLatitudes(latitude.DefineID, new List<string> {latitude.Value}).ToList();
                if (lt.Any())
                    latitudeList.AddRange(lt);
                else
                    throw new Exception(string.Format("度量值不存在。度量值定义ID{0},度量值{1}", latitude.DefineID, latitude.Value));
            }
            return reportDF.GetNumericalValue(latitudeList, numericalDefineID);
        }

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
        public virtual IEnumerable<KeyValuePair<IEnumerable<LatitudeValue>, NumericalValue>> GetNumericalValue(
            IEnumerable<IEnumerable<LatitudeValue>> latitudes, int numericalDefineID)
        {
            var reportDF = DependencyResolver.UnityDependencyResolver.Resolve<IReportDF>();
            var ret = reportDF.GetNumericalValue(latitudes, numericalDefineID);
            return ret;
        }

        /// <summary>
        ///   根据时间范围找到时间维度的维度值列表，从而进一步找到数据库里的时间维度的维度值记录
        /// </summary>
        /// <param name="beginDate"> </param>
        /// <param name="endDate"> </param>
        /// <param name="defineID"> 维度定义ID </param>
        /// <param name="flag"> </param>
        /// <returns> </returns>
        public virtual IEnumerable<LatitudeValue> GetLatitudes(DateTime beginDate, DateTime endDate, int defineID,
                                                               TimeLatitude flag = TimeLatitude.Date)
        {
            var reportDF = DependencyResolver.UnityDependencyResolver.Resolve<IReportDF>();
            var ret = reportDF.GetLatitudes(beginDate, endDate, defineID, flag);
            return ret;
        }

        /// <summary>
        ///   根据维度定义ID和维度值列表，找到数据库里的维度值记录
        /// </summary>
        /// <param name="defineID"> 维度定义ID </param>
        /// <param name="values"> 维度值列表 </param>
        /// <returns> </returns>
        public virtual IEnumerable<LatitudeValue> GetLatitudes(int defineID, IEnumerable<string> values)
        {
            var reportDF = DependencyResolver.UnityDependencyResolver.Resolve<IReportDF>();
            var ret = reportDF.GetLatitudes(defineID, values);
            return ret;
        }

        /// <summary>
        ///   根据度量值获取其所属的维度值列表
        /// </summary>
        /// <param name="numericalValue"> </param>
        /// <returns> </returns>
        public virtual IEnumerable<LatitudeValue> CheckLatitudes(NumericalValue numericalValue)
        {
            var reportDF = DependencyResolver.UnityDependencyResolver.Resolve<IReportDF>();
            var ret = reportDF.CheckLatitudes(numericalValue);
            return ret;
        }

        /// <summary>
        ///   插入一个报表项（报表项由多个维度值和一个度量值组成）
        /// </summary>
        /// <param name="reportItem"> </param>
        /// <returns> </returns>
        public virtual bool InsertReportItem(ReportItem reportItem)
        {
            var reportDF = DependencyResolver.UnityDependencyResolver.Resolve<IReportDF>();
            var ret = reportDF.InsertReportItem(reportItem);
            return ret;
        }

        #endregion
    }
}