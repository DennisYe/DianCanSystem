using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Shared;
using CYJH_OrderSystem.Admin.Base.Model;
using CYJH_OrderSystem.Admin.Base.Factorys;
using CYJH_OrderSystem.Admin.Base.Contract;
using System.Web.Security;
using System.Xml.Linq;
using System.Xml;
using CYJH_OrderSystem.Admin.Base.Bll;
using System.Web.Caching;
using System.IO;
using System.Text;

namespace CYJH_OrderSystem.Admin.Base {

    public static class SiteRuleCheck {

        #region 私有变量
        private static BR_PageInfo _pageBLL = new BR_PageInfo();
        private static BR_AdminRight _adminRightBLL = new BR_AdminRight();
        private static BR_GroupRight _groupRightBLL = new BR_GroupRight();
        private const string _allPagesCacheName = "R_AllPages";
        private const string _allPageRightCacheName = "R_AllPageRight";
        /// <summary>
        /// 页面完全权限的表达式
        /// </summary>
        private const string _pageAllRightExp = "1111111";
        /// <summary>
        /// 页面无操作权限的表达式
        /// </summary>
        private const string _pageNoRightExp = "0000000";
        #endregion

        #region 私有方法
        /// <summary>
        /// 处理原始url,得到相对当前应用程序池的相对URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string GetPagePath(string url) {
            url = url.Substring(HttpContext.Current.Request.GetApplicationURL().Length);
            int rndIndex = -1;
            if ((rndIndex = url.IndexOf("?rnd=")) > -1) {
                url = url.Substring(0, rndIndex);
            }
            if ((rndIndex = url.IndexOf("&rnd=")) > -1) {
                url = url.Substring(0, rndIndex);
            }
            return url.ToLower();
        }

        /// <summary>
        /// 获取当前页面相对URL
        /// </summary>
        /// <returns></returns>
        private static string GetCurrPagePath() {
            return GetPagePath(HttpContext.Current.Request.Url.AbsoluteUri);
        }

        /// <summary>
        /// 根据页面url匹配页面ID列表(如果是前部匹配的话,就可能有多个页面被匹配到)
        /// </summary>
        /// <param name="urlPath"></param>
        /// <returns></returns>
        private static List<int> GetPageIdUseCache(string urlPath) {
            urlPath = urlPath.ToLower();
            var allPages = (Dictionary<string, int>)HttpContext.Current.Cache[_allPagesCacheName];
            if (allPages == null) {
                allPages = _pageBLL.GetAllList().Where(p => p.PUrl.IsNotEmpty()).Distinct(new PageInfoURLComparint()).ToDictionary(p => p.PUrl.ToLower(), p => p.PID);
                HttpContext.Current.Cache.Add(_allPagesCacheName, allPages, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
            }
            int pageId = 0;
            if (allPages.TryGetValue(urlPath, out pageId) && pageId >= 0) {
                //完全匹配到的URL,则就一个ID
                return new List<int>() { pageId };
            } else {
                //完全匹配不到时,采用前部匹配,可能匹配到多个URL
                return allPages.Where(p => urlPath.StartsWith(p.Key)).Select(t => t.Value).ToList();
            }
        }

        /// <summary>
        /// 获取所有管理员和用户组的权限配置
        /// </summary>
        /// <param name="adminRights">Dictionary&lt;管理员ID, Dictionary&lt;页面ID, MR_PageInfo&gt;&gt;</param>
        /// <param name="groupRights"></param>
        private static void GetAllAdminAndGroupRights(out Dictionary<int, Dictionary<int, MR_PageInfo>> adminRights, out Dictionary<int, Dictionary<int, MR_PageInfo>> groupRights) {
            //获取缓存中的所有权限配置
            var allRights = (List<Dictionary<int, Dictionary<int, MR_PageInfo>>>)HttpContext.Current.Cache[_allPageRightCacheName];
            adminRights = groupRights = null;
            if (allRights == null) {
                adminRights = new Dictionary<int, Dictionary<int, MR_PageInfo>>();
                groupRights = new Dictionary<int, Dictionary<int, MR_PageInfo>>();

                foreach (var right in _adminRightBLL.GetAllList()) {
                    Dictionary<int, MR_PageInfo> tmpAR = null;
                    if (!adminRights.TryGetValue(right.AID, out tmpAR)) {
                        adminRights.Add(right.AID, tmpAR = new Dictionary<int, MR_PageInfo>());
                    }

                    if (tmpAR.ContainsKey(right.PID)) {
                        tmpAR[right.PID] = right;
                    } else {
                        tmpAR.Add(right.PID, right);
                    }

                    adminRights[right.AID] = tmpAR;
                }

                foreach (var right in _groupRightBLL.GetAllList()) {
                    Dictionary<int, MR_PageInfo> tmpAR = null;
                    if (!groupRights.TryGetValue(right.GID, out tmpAR)) {
                        groupRights.Add(right.GID, tmpAR = new Dictionary<int, MR_PageInfo>());
                    }

                    if (tmpAR.ContainsKey(right.PID)) {
                        tmpAR[right.PID] = right;
                    } else {
                        tmpAR.Add(right.PID, right);
                    }

                    groupRights[right.GID] = tmpAR;
                }


                allRights = new List<Dictionary<int, Dictionary<int, MR_PageInfo>>>();
                allRights.Add(adminRights);
                allRights.Add(groupRights);
                HttpContext.Current.Cache.Add(_allPageRightCacheName, allRights, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
            } else {
                adminRights = allRights[0];
                groupRights = allRights[1];
            }
        }


        /// <summary>
        /// 获取指定管理员和用户组获得权限
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="groupId"></param>
        /// <param name="ar"></param>
        /// <param name="gr"></param>
        private static void GetAdminRights(int adminId, int groupId, out Dictionary<int, MR_PageInfo> ar, out Dictionary<int, MR_PageInfo> gr) {

            Dictionary<int, Dictionary<int, MR_PageInfo>> adminRights, groupRights;
            GetAllAdminAndGroupRights(out adminRights, out groupRights);

            ar = null;
            gr = null;
            adminRights.TryGetValue(adminId, out ar);
            groupRights.TryGetValue(groupId, out gr);
        }

        /// <summary>
        /// 获取管理员以及所在组在指定页面的权限表达式,返回null则表示没权限
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="groupId"></param>
        /// <param name="pageId"></param>
        /// <returns></returns>
        private static string GetAdminRightExpByPage(int adminId, int groupId, int pageId) {
            Dictionary<int, MR_PageInfo> ar;
            Dictionary<int, MR_PageInfo> gr;
            GetAdminRights(adminId, groupId, out ar, out gr);

            string rightExp = null;

            if (ar != null) {
                MR_PageInfo pageInfo = null;
                if (ar.TryGetValue(pageId, out pageInfo) && pageInfo != null) {
                    rightExp = pageInfo.BtnRightExp.MergePageRightExp(rightExp);
                }
            }

            if (gr != null) {
                MR_PageInfo pageInfo = null;
                if (gr.TryGetValue(pageId, out pageInfo) && pageInfo != null) {
                    rightExp = pageInfo.BtnRightExp.MergePageRightExp(rightExp);
                }
            }


            return rightExp;
        }


        /// <summary>
        /// 检测url是否不需要权限验证
        /// </summary>
        /// <param name="urlPath"></param>
        /// <param name="noRoleToUrl"></param>
        /// <returns></returns>
        private static bool InNoRolePage(string urlPath, out string noRoleToUrl) {
            string cacheName = "R_NoRolePages";
            string[] noPages = (string[])HttpContext.Current.Cache[cacheName];
            if (noPages == null) {
                string cfgPathSys = HttpContext.Current.Server.MapPath("~/ConfigFile/RoleSettingSys.xml");
                var sysCfg = XDocument.Load(cfgPathSys).Root;
                var addNoRolePages = new List<string> { sysCfg.Attribute("NoRule").Value };
                addNoRolePages.AddRange(sysCfg.Elements("Page").Select(p => p.Attribute("Url").Value.ToLower()).Distinct());

                string cfgPathUser = HttpContext.Current.Server.MapPath("~/ConfigFile/RoleSetting.xml");
                if (File.Exists(cfgPathUser)) {
                    //如果存在用户配置的排除权限页面,则需要并入
                    var userCfg = XDocument.Load(cfgPathUser).Root;
                    addNoRolePages = addNoRolePages.Union(userCfg.Elements("Page").Select(p => p.Attribute("Url").Value.ToLower())).Distinct().ToList();
                }

                noPages = addNoRolePages.ToArray();
                HttpContext.Current.Cache.Add(cacheName, noPages, new CacheDependency(new string[] { cfgPathSys, cfgPathUser })
                    , Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
            }
            noRoleToUrl = noPages[0];
            return noPages.FirstOrDefault(u => urlPath.StartsWith(u)) != null;
        }


        #endregion

        /// <summary>
        /// 合并两个权限表达式
        /// </summary>
        /// <param name="pageRight1"></param>
        /// <param name="pageRight2"></param>
        /// <returns></returns>
        public static string MergePageRightExp(this string pageRight1, string pageRight2) {
            if (pageRight1 == null) return pageRight2;
            if (pageRight2 == null) return pageRight1;
            StringBuilder exp = new StringBuilder(_pageNoRightExp);
            int len1 = pageRight1.Length, len2 = pageRight2.Length;
            for (int i = 0, len = exp.Length; i < len; i++) {
                var e1 = i < len1 ? pageRight1[i] : '0';
                var e2 = i < len2 ? pageRight2[i] : '0';
                if (e1 == '1' || e2 == '1') exp[i] = '1';
            }
            return exp.ToString();
        }

        /// <summary>
        /// 获取管理员在指定页面的操作权限表达式,返回null则表示无访问权限
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="gId"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetAdminRightExpByPageURL(int adminId, int gId, string url) {
            var pageIds = GetPageIdUseCache(url);
            string rightExp = null;
            if (pageIds.Count > 0 && adminId > 0) {
                foreach (var pId in pageIds) {
                    //每个匹配到的页面都去找下,合并所有配置的操作权限,都没匹配到有权限的,则表示没权限
                    rightExp = GetAdminRightExpByPage(adminId, gId, pId).MergePageRightExp(rightExp);
                }
            }
            return rightExp;
        }

        /// <summary>
        /// 当前用户在当前页面的访问权限验证,并返回页面的操作权限(如果有页面访问权限则返回类似"1110011"的权限表达式,如果没访问权限则返回null)
        /// </summary>
        /// <returns></returns>
        public static string CurrPageRoleCheck() {
            string rightExp = null;

            const string cacheKey = "CurrPageRight";
            if (HttpContext.Current.Items.Contains(cacheKey)) {
                //已经在请求周期中缓存了
                rightExp = HttpContext.Current.Items[cacheKey].GetString(null);
            } else {
                //当前请求周期中未缓存

                //当前登录用户
                var adminInfo = AdminPageStatic.GetLoginUserInfo(false);

                if (adminInfo != null) {
                    //必须已登录

                    //如果有登录且是超级管理员,则直接放行,并拥有完全操作权限
                    if (adminInfo.GID == -1) {
                        rightExp = _pageAllRightExp;
                    } else {
                        var currUrlPath = GetCurrPagePath();
                        string noRoleToUrlPath = "";//没权限需要跳转到的地址

                        if (InNoRolePage(currUrlPath, out noRoleToUrlPath)) {
                            //如果页面在排除权限验证页面配置中,则直接放行,并拥有完全操作权限
                            rightExp = _pageAllRightExp;
                        } else {
                            //获取当前管理员对当前页面的操作权限(或是无访问权限)
                            rightExp = (adminInfo == null ? null : GetAdminRightExpByPageURL(adminInfo.AID, adminInfo.GID, currUrlPath));

                            if (rightExp == null) {
                                HttpContext.Current.Response.Redirect(HttpContext.Current.Request.GetApplicationURL().GetUrlRelativePath(noRoleToUrlPath), true);
                            }
                        }

                    }
                }

                HttpContext.Current.Items[cacheKey] = rightExp;
            }

            return rightExp;

        }

        /// <summary>
        /// [未实现]当前页面当下操作权限验证
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public static bool CurrPageOperateRoleCheck() {
            return false;
        }

        /// <summary>
        /// 清理页面以及权限缓存
        /// </summary>
        public static void FlushPageAndRightCache() {
            HttpContext.Current.Cache.Remove(_allPagesCacheName);
            HttpContext.Current.Cache.Remove(_allPageRightCacheName);
        }

        /// <summary>
        /// 获取管理员的有权限的页面列表(使用缓存)
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="groupId"></param>
        /// <param name="parentPageId"></param>
        /// <returns></returns>
        public static List<MR_PageInfo> GetAdminPages(int adminId, int groupId, int parentPageId, bool includeAllChilds) {
            Dictionary<int, MR_PageInfo> ar;
            Dictionary<int, MR_PageInfo> gr;
            GetAdminRights(adminId, groupId, out ar, out gr);

            List<MR_PageInfo> pages = new List<MR_PageInfo>();

            if (ar != null) {
                pages.AddRange(ar.Select(f => f.Value));
            }
            if (gr != null) {
                pages.AddRange(gr.Select(f => f.Value));
            }
            pages = pages.Where(p => p.ParentID == parentPageId).Distinct(new PageInfoIdComparint()).OrderBy(p => p.Queue).ToList();

            if (includeAllChilds) {
                List<MR_PageInfo> childs = new List<MR_PageInfo>();
                foreach (var p in pages) {
                    p.Childs = (GetAdminPages(adminId, groupId, p.PID, includeAllChilds));
                }
            }

            return pages;
        }



        #region 内部使用类
        private class PageInfoURLComparint : IEqualityComparer<MR_PageInfo> {
            bool IEqualityComparer<MR_PageInfo>.Equals(MR_PageInfo x, MR_PageInfo y) {
                if (x == null && y == null) {
                    return false;
                }
                return x.PUrl.ToLower().Equals(y.PUrl.ToLower());
            }
            int IEqualityComparer<MR_PageInfo>.GetHashCode(MR_PageInfo obj) {
                return obj.ToString().GetHashCode();
            }
        }
        private class PageInfoIdComparint : IEqualityComparer<MR_PageInfo> {
            bool IEqualityComparer<MR_PageInfo>.Equals(MR_PageInfo x, MR_PageInfo y) {
                if (x == null && y == null) {
                    return false;
                }
                return x.PID.Equals(y.PID);
            }
            int IEqualityComparer<MR_PageInfo>.GetHashCode(MR_PageInfo obj) {
                return obj.ToString().GetHashCode();
            }
        }
        #endregion
    }



}
