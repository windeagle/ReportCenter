using System;
using System.Collections.Generic;
using System.Linq;
using DianPing.BA.ReportCenter.Domain.Entity;

namespace DianPing.BA.ReportCenter.Domain.DomainFacade
{
    public class ReportManager
    {
        /// <summary>
        ///   ��ȡX��ά��ֵ
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
        ///   ��ȡ�������ƣ�����X��ά���⣬����ά�ȶ��ǰ����ڱ���ά������/�ָ�
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
        ///   ��ά�Ȳ�ѯ�����ת���ɵ���������Y��ĵ���������
        /// </summary>
        /// <typeparam name="TY"> ����ֵ���� </typeparam>
        /// <param name="orgResults"> ά�Ȳ�ѯ����� </param>
        /// <param name="yAxis"> Y�ᵥ������ </param>
        /// <param name="xLatitudeDefinedID"> X��ά�ȶ���ID </param>
        /// <param name="func"> ת��ί�� </param>
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