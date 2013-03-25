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
        ///   ��ȡ��ά���µĶ���ֵ
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
                    throw new Exception(string.Format("����ֵ�����ڡ�����ֵ����ID{0},����ֵ{1}", latitude.DefineID, latitude.Value));
            }
            return reportDF.GetNumericalValue(latitudeList, numericalDefineID);
        }

        /// <summary>
        ///   ����ά��ֵ�б�Ͷ���ֵ����ID���ҵ���ά���µĶ���ֵ
        /// </summary>
        /// <param name="latitudes"> ά��ֵ�б� </param>
        /// <param name="numericalDefineID"> ����ֵ����ID </param>
        /// <returns> </returns>
        /*
         * �����ά��ֵ�б��Ƕ�ά�б��ṹ��������[["2013-01-01","2013-01-02"],["���¶��֧��ģ��","�µ���֧��ģ��"]]
         * ��Ҫ�Ѵ����ά��ֵ��ά�б����ת����ת����[["2013-01-01","���¶��֧��ģ��"],["2013-01-01","�µ���֧��ģ��"],["2013-01-02","���¶��֧��ģ��"]��["2013-01-02","�µ���֧��ģ��"]]
         * ���ص�IEnumerable<KeyValuePair<IEnumerable<LatitudeValue>, NumericalValue>>�е�IEnumerable<LatitudeValue>��ת�����ά��ֵ�б�
         */
        public virtual IEnumerable<KeyValuePair<IEnumerable<LatitudeValue>, NumericalValue>> GetNumericalValue(
            IEnumerable<IEnumerable<LatitudeValue>> latitudes, int numericalDefineID)
        {
            var reportDF = DependencyResolver.UnityDependencyResolver.Resolve<IReportDF>();
            var ret = reportDF.GetNumericalValue(latitudes, numericalDefineID);
            return ret;
        }

        /// <summary>
        ///   ����ʱ�䷶Χ�ҵ�ʱ��ά�ȵ�ά��ֵ�б��Ӷ���һ���ҵ����ݿ����ʱ��ά�ȵ�ά��ֵ��¼
        /// </summary>
        /// <param name="beginDate"> </param>
        /// <param name="endDate"> </param>
        /// <param name="defineID"> ά�ȶ���ID </param>
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
        ///   ����ά�ȶ���ID��ά��ֵ�б��ҵ����ݿ����ά��ֵ��¼
        /// </summary>
        /// <param name="defineID"> ά�ȶ���ID </param>
        /// <param name="values"> ά��ֵ�б� </param>
        /// <returns> </returns>
        public virtual IEnumerable<LatitudeValue> GetLatitudes(int defineID, IEnumerable<string> values)
        {
            var reportDF = DependencyResolver.UnityDependencyResolver.Resolve<IReportDF>();
            var ret = reportDF.GetLatitudes(defineID, values);
            return ret;
        }

        /// <summary>
        ///   ���ݶ���ֵ��ȡ��������ά��ֵ�б�
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
        ///   ����һ��������������ɶ��ά��ֵ��һ������ֵ��ɣ�
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