using System;
using System.Collections.Generic;
using System.Linq;
using DianPing.BA.Framework.CommonSolution.Common.Util;
using DianPing.BA.Framework.DAL.DACBase;
using DianPing.BA.ReportCenter.Domain.Entity;
using DianPing.BA.ReportCenter.Domain.Util;

namespace DianPing.BA.ReportCenter.Domain.DataFacade
{
    public class ReportDF : IReportDF
    {
        #region IReportDF Members

        /// <summary>
        ///   根据维度定义ID和维度值列表，找到数据库里的维度值记录
        /// </summary>
        /// <param name="defineID"> 维度定义ID </param>
        /// <param name="values"> 维度值列表 </param>
        /// <returns> </returns>
        public virtual IEnumerable<LatitudeValue> GetLatitudes(int defineID, IEnumerable<string> values)
        {
            // var result = new List<LatitudeValue>();
            var query = DependencyResolver.UnityDependencyResolver.Resolve<LinqDAC<LatitudeValue>>();
            query.SqlWhere(c => c.DefineID.SqlEqual(defineID))
                .SqlWhere(c => c.Value.SqlIn(values));

            return query.ToList();
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
            List<string> values = new List<string>();
            if (flag == TimeLatitude.Date)
            {
                for (; beginDate.Date <= endDate.Date; beginDate = beginDate.AddDays(1))
                {
                    values.Add(beginDate.Date.ToString("yyyy-MM-dd"));
                }
            }
            else if (flag == TimeLatitude.Month)
            {
                for (; beginDate.Date <= endDate.Date; beginDate = beginDate.AddMonths(1))
                {
                    values.Add(beginDate.Date.ToString("yyyy-MM"));
                }
            }
            else if (flag == TimeLatitude.Quarter)
            {
                for (; beginDate.Date <= endDate.Date; beginDate = beginDate.AddMonths(3))
                {
                    values.Add(beginDate.Date.ToString("yyyy-") + beginDate.Month/3);
                }
            }
            return GetLatitudes(defineID, values);
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
            List<List<LatitudeValue>> vlatitudes = new List<List<LatitudeValue>>();

            foreach (var latitude in latitudes)
            {
                if (!vlatitudes.Any())
                {
                    foreach (var latitudeValue in latitude)
                    {
                        vlatitudes.Add(new List<LatitudeValue>
                                           {
                                               latitudeValue
                                           });
                    }
                }
                else
                {
                    vlatitudes = vlatitudes.Join(latitude, values => true, value => true, (values, value) =>
                                                                                              {
                                                                                                  var newList =
                                                                                                      new List
                                                                                                          <LatitudeValue
                                                                                                              >();
                                                                                                  newList.AddRange(
                                                                                                      values);
                                                                                                  newList.Add(value);
                                                                                                  return newList;
                                                                                              }).ToList();
                }
            }

            return
                vlatitudes.Select(
                    ls =>
                    new KeyValuePair<IEnumerable<LatitudeValue>, NumericalValue>(ls,
                                                                                 GetNumericalValue(ls, numericalDefineID)))
                    .ToList();
        }

        /// <summary>
        ///   获取多维度下的度量值
        /// </summary>
        /// <param name="latitudes"> </param>
        /// <param name="numericalDefineID"> </param>
        /// <returns> </returns>
        public virtual NumericalValue GetNumericalValue(IEnumerable<LatitudeValue> latitudes, int numericalDefineID)
        {
            var latitudeValues = latitudes as LatitudeValue[] ?? latitudes.ToArray();

            //根据传入参数获取缓存Key
            var key = latitudeValues.Aggregate(string.Empty,
                                               (current, latitudeValue) => current + (latitudeValue.LatitudeID + "|"));
            key = key.Trim('|') + '_' + numericalDefineID;

            //读缓存
            object cacheObj;
            if (CacheHelper.GetFromCache(key, (t => t.AddMinutes(60) > DateTime.Now), out cacheObj))
            {
                var numericalValue = cacheObj as NumericalValue;
                if (numericalValue != null)
                {
                    return numericalValue;
                }
            }

            IList<NumericalValue> retList = new List<NumericalValue>();
            List<int> numericalIDs = null;
            foreach (var latitude in latitudeValues)
            {
                LatitudeValue latitude1 = latitude;
                //首先找到满足当前维度条件的数值ID
                var query0 = DependencyResolver.UnityDependencyResolver.Resolve<LinqDAC<LNRelation>>();
                query0.SqlWhere(c => c.LatitudeID.SqlEqual(latitude1.LatitudeID));
                if (numericalIDs != null)
                {
                    var tmpds = numericalIDs;
                    query0.SqlWhere(c => c.NumericalID.SqlIn(tmpds));
                }
                numericalIDs = query0.Select(r => r.NumericalID).ToList();

                //找到满足当前维度条件的数值
                var query = DependencyResolver.UnityDependencyResolver.Resolve<LinqDAC<NumericalValue>>();
                IList<int> ds = numericalIDs;
                query.SqlWhere(c => c.NumericalID.SqlIn(ds))
                    .SqlWhere(c => c.DefineID.SqlEqual(numericalDefineID));
                retList = query.ToList();
                numericalIDs = retList.Select(r => r.NumericalID).ToList();
            }

            if (numericalIDs == null || !numericalIDs.Any())
                return new NumericalValue();

            List<int> ds2 = numericalIDs;
            var query1 = DependencyResolver.UnityDependencyResolver.Resolve<LinqDAC<LNRelation>>();
            query1.SqlWhere(c => c.LatitudeID.SqlNotIn(latitudeValues.Select(l => l.LatitudeID)))
                .SqlWhere(c => c.NumericalID.SqlIn(ds2));
            //把还有其他维度的排除掉
            numericalIDs = numericalIDs.Except(query1.Select(l => l.NumericalID)).ToList();

            var ret = retList.FirstOrDefault(n => numericalIDs.Contains(n.NumericalID));

            //写缓存
            CacheHelper.SetCache(key, ret, DateTime.Now);

            return ret ?? new NumericalValue();
        }

        /// <summary>
        ///   根据度量值获取其所属的维度值列表
        /// </summary>
        /// <param name="numericalValue"> </param>
        /// <returns> </returns>
        public virtual IEnumerable<LatitudeValue> CheckLatitudes(NumericalValue numericalValue)
        {
            //找到维度的ID
            var query0 = DependencyResolver.UnityDependencyResolver.Resolve<LinqDAC<LNRelation>>();
            query0.SqlWhere(c => c.NumericalID.SqlEqual(numericalValue.NumericalID));

            var latitudeIDs = query0.ToList().Select(r => r.LatitudeID);

            //返回维度实体
            var query = DependencyResolver.UnityDependencyResolver.Resolve<LinqDAC<LatitudeValue>>();
            query.SqlWhere(c => c.LatitudeID.SqlIn(latitudeIDs));
            return query.ToList();
        }

        /// <summary>
        ///   插入一个报表项（报表项由多个维度值和一个度量值组成）
        /// </summary>
        /// <param name="reportItem"> </param>
        /// <returns> </returns>
        public virtual bool InsertReportItem(ReportItem reportItem)
        {
            var insert = DependencyResolver.UnityDependencyResolver.Resolve<LinqDAC<NumericalValue>>();
            var vRet = insert.SqlInsert(reportItem.Value);

            foreach (var latitude in reportItem.Latitudes)
            {
                int lRet;
                var existsLatitude = GetLatitudes(latitude.DefineID, new[] {latitude.Value}).ToList();
                if (existsLatitude.Any())
                    lRet = existsLatitude.First().LatitudeID;
                else
                {
                    var insert0 = DependencyResolver.UnityDependencyResolver.Resolve<LinqDAC<LatitudeValue>>();
                    lRet = insert0.SqlInsert(latitude);
                }

                var insert1 = DependencyResolver.UnityDependencyResolver.Resolve<LinqDAC<LNRelation>>();
                insert1.SqlInsert(new LNRelation
                                      {
                                          LatitudeID = lRet,
                                          NumericalID = vRet
                                      });
            }
            return true;
        }

        #endregion
    }
}