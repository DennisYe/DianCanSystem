using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CYJH_OrderSystem.Admin.Base.Contract;
using CYJH_OrderSystem.Admin.Base.Dal;
using CYJH_OrderSystem.Admin.Base.Model;
using System.IO;
using Shared;

namespace CYJH_OrderSystem.Admin.Base.Bll {
    /// <summary>
    /// Mgr下,文件
    /// </summary>
    public class ImplCheckMgrDirNewPages : ICheckNewMgrPages {

        protected BR_PageInfo pageBLL = new BR_PageInfo();

        protected class Dir {
            public Dir() {
                Files = new List<string>();
                Dirs = new Dictionary<string, Dir>();
            }
            public string Name { get; set; }
            public List<string> Files { get; set; }
            public Dictionary<string, Dir> Dirs { get; set; }
        }

        /// <summary>
        /// 将页面地址列表解析为树形结构
        /// </summary>
        /// <param name="urls"></param>
        /// <returns></returns>
        protected List<MR_PageInfo> ParseToTreePages(List<string> urls) {
            /*
             * 先不实现层级了
            Dictionary<string, Dir> Dirs = new Dictionary<string, Dir>();
            foreach (var url in urls) {
                var dirNames = url.Split('/');
                Dir currDir = null;
                for (int i = 0, len = dirNames.Length - 1; i < len; i++) {
                    
                }
            }*/
            return urls.Select(u => new MR_PageInfo { PUrl = u }).ToList();
        }

        /// <summary>
        /// 转化全路径为相对路径
        /// </summary>
        /// <param name="basePath"></param>
        /// <param name="filePaths"></param>
        /// <returns></returns>
        protected IEnumerable<string> FullPathToRePath(string basePath, string[] filePaths) {
            if (filePaths == null) return new List<string>();
            return filePaths.Where(f => f.IsNotEmpty()).Select(f => f.Substring(basePath.Length).Replace("\\", "/"));
        }

        #region ICheckNewMgrPages 成员

        public List<MR_PageInfo> CheckNewMgrPages() {
            string checkPath = HttpContext.Current.Server.MapPath("~/Mgr/");
            string basePath = HttpContext.Current.Server.MapPath("~/");
            if (!Directory.Exists(basePath)) return new List<MR_PageInfo>();
            var allFiles = Directory.GetFiles(checkPath, "*.aspx", SearchOption.AllDirectories);
            var allPaths = FullPathToRePath(basePath, allFiles);
            var newPats = pageBLL.CheckPageURLs(allPaths);
            return ParseToTreePages(newPats);
        }

        #endregion
    }
}