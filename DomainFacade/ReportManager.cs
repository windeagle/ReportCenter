using System;
using System.Collections.Generic;
using System.Linq;
using DianPing.BA.ReportCenter.Domain.Entity;

namespace DianPing.BA.ReportCenter.Domain.DomainFacade
{
    public class ReportManager
    {
        /// <summary>
        ///   获取X轴维度值
        /// </summary>
        /// <param name="latitudes"> </param>
        /// <param name="xLatitudeDefinedID"> </param>
        /// <returns> </returns>
        public static string GetXValue(IEnumerable<LatitudeValue> latitudes, int xLatitudeDefinedID)
        {
            var latitudesExceptX = latitudes.First(l => l.DefineID == xLatitudeDefinedID);
            return latitudesExceptX.Value;
        }

        /// <summary>
        ///   获取报表名称，除了X轴维度外，其他维度都是包含在报表维度中以/分隔
        /// </summary>
        /// <param name="latitudes"> </param>
        /// <param name="xLatitudeDefinedID"> </param>
        /// <returns> </returns>
        public static string GetReportName(IEnumerable<LatitudeValue> latitudes, int xLatitudeDefinedID)
        {
            var latitudesExceptX = latitudes.Where(l => l.DefineID != xLatitudeDefinedID);
            var latitudeValues = latitudesExceptX.Select(l => l.Value);
            var name = latitudeValues.Aggregate(string.Empty,
                                                (current, latitudeValue) => current + (latitudeValue + "/"));
            return name.Trim('/');
        }

        /// <summary>
        ///   把维度查询结果集转换成单报表并放在Y轴的单报表集合中
        /// </summary>
        /// <typeparam name="TY"> 度量值类型 </typeparam>
        /// <param name="orgResults"> 维度查询结果集 </param>
        /// <param name="yAxis"> Y轴单报表集合 </param>
        /// <param name="xLatitudeDefinedID"> X轴维度定义ID </param>
        /// <param name="func"> 转换委托 </param>
        public static void Transform<TY>(
            IEnumerable<KeyValuePair<IEnumerable<LatitudeValue>, NumericalValue>> orgResults, List<SingleReport> yAxis,
            int xLatitudeDefinedID, Func<string, TY> func)
        {
            foreach (var keyValuePair in orgResults)
            {
                var name = GetReportName(keyValuePair.Key, xLatitudeDefinedID);
                var tmp = yAxis.FirstOrDefault(yl => yl.Name.Equals(name));
                if (tmp != null)
                {
                    var singleReport = tmp as SingleReport<TY>;
                    if (singleReport != null)
                    {
                        var data = singleReport.Data;
                        if (data != null)
                            data.Add(func(keyValuePair.Value.Value));
                        singleReport.Data = data;
                    }
                }
                else
                {
                    yAxis.Add(new SingleReport<TY>
                                  {
                                      Name = name,
                                      Data = new List<TY>
                                                 {
                                                     func(keyValuePair.Value.Value)
                                                 }
                                  });
                }
            }
        }
    }
}