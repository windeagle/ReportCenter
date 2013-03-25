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
            //获取时间维度值定义ID（在这里有两个时间维度供选择）
            int dateltDefineID =
                int.Parse(chartType == 0
                              ? ConfigurationManager.AppSettings["应付日期"]
                              : ConfigurationManager.AppSettings["添加日期"]);

            return CommonReport(beginDate, endDate, dateltDefineID);
        }

        private object CommonReport(DateTime beginDate, DateTime endDate, int xLatitudeDefinedID)
        {
            var reports = DependencyResolver.UnityDependencyResolver.Resolve<IReport>();

            var dictEnumDict = EnumDictionary.GetEnumIDictionary(typeof (PayTemplateType));
            //获取模板维度值定义ID
            int templateTypeltDefineID = int.Parse(ConfigurationManager.AppSettings["模板"]);

            //获取总金额度量值定义ID
            int amountLNDefineID = int.Parse(ConfigurationManager.AppSettings["总金额"]);
            //获取总笔数度量值定义ID
            int countLNDefineID = int.Parse(ConfigurationManager.AppSettings["总笔数"]);

            //模版维度值列表
            var templateTypelLatitudes = reports.GetLatitudes(templateTypeltDefineID, dictEnumDict.Values).ToList();
            //日期维度值列表
            var datelLatitudes = reports.GetLatitudes(beginDate, endDate, xLatitudeDefinedID).ToList();

            //取到所有的模版维度+日期维度维度下的总金额数值
            var amounts = reports.GetNumericalValue(new List<IEnumerable<LatitudeValue>>
                                                        {
                                                            templateTypelLatitudes,
                                                            datelLatitudes
                                                        }, amountLNDefineID).ToList();

            //这里把日期维度作为二维报表的X轴维度，并且按X轴维度值排序
            amounts = amounts.OrderBy(c => DateTime.Parse(ReportManager.GetXValue(c.Key, xLatitudeDefinedID))).ToList();

            //取到所有的模版维度+日期维度维度下的总笔数数值
            var counts = reports.GetNumericalValue(new List<IEnumerable<LatitudeValue>>
                                                       {
                                                           templateTypelLatitudes,
                                                           datelLatitudes
                                                       }, countLNDefineID).ToList();

            //这里把日期维度作为二维报表的X轴维度，并且按X轴维度排序
            counts = counts.OrderBy(c => DateTime.Parse(ReportManager.GetXValue(c.Key, xLatitudeDefinedID))).ToList();

            //这里的报表有一个X轴两个Y轴（左Y轴和右Y轴）,每个Y轴可以有多个单报表
            var report = new ReportObject<string>
                             {
                                 XAxis =
                                     datelLatitudes.OrderBy(c => c.Value).Select(
                                         l => DateTime.Parse(l.Value).ToString("yyyy-MM-dd")),
                                 YAxisL = new List<SingleReport>(),
                                 YAxisR = new List<SingleReport>()
                             };

            //把维度查询结果集转换成单报表并放在Y轴的单报表集合中
            ReportManager.Transform(amounts, report.YAxisL, xLatitudeDefinedID,
                                    s => s == null ? default(decimal) : decimal.Parse(s));
            ReportManager.Transform(counts, report.YAxisR, xLatitudeDefinedID,
                                    s => s == null ? default(int) : int.Parse(s));

            //把最终结果转成匿名对象（进一步序列化成JSON给前端展示就完美了）
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