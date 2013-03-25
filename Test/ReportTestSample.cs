using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using DianPing.BA.Framework.CommonSolution.Common.Util;
using DianPing.BA.ReportCenter.Domain.DomainFacade;
using DianPing.BA.ReportCenter.Domain.Entity;
using DianPing.BA.ReportCenter.Domain.Util;

namespace DianPing.BA.ReportCenter.Domain.Test
{
    public class ReportTestSample
    {
        public object PayPlanInfoReport(DateTime beginDate, DateTime endDate, int chartType)
        {
            //��ȡʱ��ά��ֵ����ID��������������ʱ��ά�ȹ�ѡ��
            int dateltDefineID =
                int.Parse(chartType == 0
                              ? ConfigurationManager.AppSettings["Ӧ������"]
                              : ConfigurationManager.AppSettings["�������"]);

            return CommonReport(beginDate, endDate, dateltDefineID);
        }

        private object CommonReport(DateTime beginDate, DateTime endDate, int xLatitudeDefinedID)
        {
            var reports = DependencyResolver.UnityDependencyResolver.Resolve<IReport>();

            var dictEnumDict = EnumDictionary.GetEnumIDictionary(typeof (PayTemplateType));
            //��ȡģ��ά��ֵ����ID
            int templateTypeltDefineID = int.Parse(ConfigurationManager.AppSettings["ģ��"]);

            //��ȡ�ܽ�����ֵ����ID
            int amountLNDefineID = int.Parse(ConfigurationManager.AppSettings["�ܽ��"]);
            //��ȡ�ܱ�������ֵ����ID
            int countLNDefineID = int.Parse(ConfigurationManager.AppSettings["�ܱ���"]);

            //ģ��ά��ֵ�б�
            var templateTypelLatitudes = reports.GetLatitudes(templateTypeltDefineID, dictEnumDict.Values).ToList();
            //����ά��ֵ�б�
            var datelLatitudes = reports.GetLatitudes(beginDate, endDate, xLatitudeDefinedID).ToList();

            //ȡ�����е�ģ��ά��+����ά��ά���µ��ܽ����ֵ
            var amounts = reports.GetNumericalValue(new List<IEnumerable<LatitudeValue>>
                                                        {
                                                            templateTypelLatitudes,
                                                            datelLatitudes
                                                        }, amountLNDefineID).ToList();

            //���������ά����Ϊ��ά�����X��ά�ȣ����Ұ�X��ά��ֵ����
            amounts = amounts.OrderBy(c => DateTime.Parse(ReportManager.GetXValue(c.Key, xLatitudeDefinedID))).ToList();

            //ȡ�����е�ģ��ά��+����ά��ά���µ��ܱ�����ֵ
            var counts = reports.GetNumericalValue(new List<IEnumerable<LatitudeValue>>
                                                       {
                                                           templateTypelLatitudes,
                                                           datelLatitudes
                                                       }, countLNDefineID).ToList();

            //���������ά����Ϊ��ά�����X��ά�ȣ����Ұ�X��ά������
            counts = counts.OrderBy(c => DateTime.Parse(ReportManager.GetXValue(c.Key, xLatitudeDefinedID))).ToList();

            //����ı�����һ��X������Y�ᣨ��Y�����Y�ᣩ,ÿ��Y������ж��������
            var report = new ReportObject<string>
                             {
                                 XAxis =
                                     datelLatitudes.OrderBy(c => c.Value).Select(
                                         l => DateTime.Parse(l.Value).ToString("yyyy-MM-dd")),
                                 YAxisL = new List<SingleReport>(),
                                 YAxisR = new List<SingleReport>()
                             };

            //��ά�Ȳ�ѯ�����ת���ɵ���������Y��ĵ���������
            ReportManager.Transform(amounts, report.YAxisL, xLatitudeDefinedID,
                                    s => s == null ? default(decimal) : decimal.Parse(s));
            ReportManager.Transform(counts, report.YAxisR, xLatitudeDefinedID,
                                    s => s == null ? default(int) : int.Parse(s));

            //�����ս��ת���������󣨽�һ�����л���JSON��ǰ��չʾ�������ˣ�
            var obj = new
                          {
                              xAxis = report.XAxis,
                              amount = report.YAxisL,
                              count = report.YAxisR
                          };

            return obj;
        }
    }
}