using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CYJH_OrderSystem.Model.eleme
{
    /// <summary>
    /// 针对eleme返回的json数据而建的类，因为要反序列化，所以未按命名规则
    /// </summary>
    public class ElemeMenu
    {
        /// <summary>
        /// 分类列表
        /// </summary>
        public List<ElemeMenuCategory> menucategorys { get; set; }
    }
    /// <summary>
    /// 菜类
    /// </summary>
    public class ElemeMenuCategory {
        /// <summary>
        /// 分类名，如热销榜，闽菜等 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 分类说明，如“大家喜欢吃，才叫真好吃。”
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 暂未知
        /// </summary>
        public string icon_url { get; set; }
        /// <summary>
        /// 菜类
        /// </summary>
        public List<ElemeFoodItem> foods { get; set; }
    }
    /// <summary>
    /// 菜品类
    /// </summary>
    public class ElemeFoodItem {
        /// <summary>
        /// 评分
        /// </summary>
        public string rating { get; set; }
        /// <summary>
        /// 图片路径，菜品的
        /// </summary>
        public string image_path { get; set; }
        /// <summary>
        /// 评分人数
        /// </summary>
        public string rating_count { get; set; }
        /// <summary>
        /// 菜的说明
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 菜的分类ID
        /// </summary>
        public int category_id { get; set; }
        /// <summary>
        /// 餐厅ID
        /// </summary>
        public int restaurant_id { get; set; }
        /// <summary>
        /// 菜名
        /// </summary>
        public string name { get; set; }
    }
    /// <summary>
    /// 菜品的更详细信息，如价格，是否售完等 
    /// </summary>
    public class SpecFood {
        /// <summary>
        /// 原价
        /// </summary>
        public string original_price { get; set; }
        /// <summary>
        /// 打包费用
        /// </summary>
        public double packing_fee { get; set; }
        /// <summary>
        /// 菜名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 餐厅ID
        /// </summary>
        public int restaurant_id { get; set; }
        /// <summary>
        /// 菜ID
        /// </summary>
        public int food_id { get; set; }
        /// <summary>
        /// 拼音名
        /// </summary>
        public string pinyin_name { get; set; }
        /// <summary>
        /// 月评分
        /// </summary>
        public string recent_rating { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public double price { get; set; }
        /// <summary>
        /// 是否售完
        /// </summary>
        public bool sold_out { get; set; }
        /// <summary>
        /// 月售几份
        /// </summary>
        public int recent_popularity { get; set; }
        /// <summary>
        /// 还有多少库存
        /// </summary>
        public int stock { get; set; }
    }

}

/*一个category对象数据，而menu由多个此对象组成
{\"icon_url\":\"5da3872d782f707b4c82ce4607c73d1ajpeg\",
\"foods\":[{\"rating\":4.38,
\"limitation\":{},
\"image_path\":\"4e5795909206ce3717c40d791c783bc8jpeg\",
\"rating_count\":50,
\"specifications\":[],
\"description\":\"优质的大排由特殊的秘制工艺，上等的调料精心腌制而成，食香扑鼻，肉质润滑......\",
\"pinyin_name\":\"guorongpaigufan\",
\"month_sales\":322,\"satisfy_count\":44,
\"specfoods\":[{\"original_price\":null,\"packing_fee\":0.0,\"name\":\"国荣排骨饭\",\"restaurant_id\":400283,\"food_id\":
29783858,\"pinyin_name\":\"guorongpaigufan\",\"recent_rating\":4.38,\"price\":22.0,\"sold_out\":false,\"recent_popularity\":322,\"specs\":[],\"stock\":9874}],
\"activity\":null,
\"satisfy_rate\":93,
\"attributes\":[],
\"tips\":\"50评价 月售322份\",
\"category_id\":2870578,
\"restaurant_id\":400283,
\"name\":\"国荣排骨饭\"}],
\"name\":\"热销榜\",
\"description\":\"大家喜欢吃，才叫真好吃。\"}
*/

/*接口返回的数据格式如下
[{"code":200,"body":"[
{\"icon_url\":\"5da3872d782f707b4c82ce4607c73d1ajpeg\",
\"foods\":[{\"rating\":4.38,
\"limitation\":{},
\"image_path\":\"4e5795909206ce3717c40d791c783bc8jpeg\",
\"rating_count\":50,
\"specifications\":[],
\"description\":\"优质的大排由特殊的秘制工艺，上等的调料精心腌制而成，食香扑鼻，肉质润滑......\",
\"pinyin_name\":\"guorongpaigufan\",
\"month_sales\":322,\"satisfy_count\":44,
\"specfoods\":[{\"original_price\":null,\"packing_fee\":0.0,\"name\":\"国荣排骨饭\",\"restaurant_id\":400283,\"food_id\":
29783858,\"pinyin_name\":\"guorongpaigufan\",\"recent_rating\":4.38,\"price\":22.0,\"sold_out\":false,\"recent_popularity\":322,\"specs\":[],\"stock\":9874}],
\"activity\":null,
\"satisfy_rate\":93,
\"attributes\":[],
\"tips\":\"50评价 月售322份\",
\"category_id\":2870578,
\"restaurant_id\":400283,
\"name\":\"国荣排骨饭\"}],
\"name\":\"热销榜\",
\"description\":\"大家喜欢吃，才叫真好吃。\"}
{\"icon_url\":\"5da3872d782f707b4c82ce4607c73d1ajpeg\",
\"foods\":[{\"rating\":4.38,
\"limitation\":{},
\"image_path\":\"4e5795909206ce3717c40d791c783bc8jpeg\",
\"rating_count\":50,
\"specifications\":[],
\"description\":\"优质的大排由特殊的秘制工艺，上等的调料精心腌制而成，食香扑鼻，肉质润滑......\",
\"pinyin_name\":\"guorongpaigufan\",
\"month_sales\":322,\"satisfy_count\":44,
\"specfoods\":[{\"original_price\":null,\"packing_fee\":0.0,\"name\":\"国荣排骨饭\",\"restaurant_id\":400283,\"food_id\":
29783858,\"pinyin_name\":\"guorongpaigufan\",\"recent_rating\":4.38,\"price\":22.0,\"sold_out\":false,\"recent_popularity\":322,\"specs\":[],\"stock\":9874}],
\"activity\":null,
\"satisfy_rate\":93,
\"attributes\":[],
\"tips\":\"50评价 月售322份\",
\"category_id\":2870578,
\"restaurant_id\":400283,
\"name\":\"国荣排骨饭\"}],
\"name\":\"热销榜\",
\"description\":\"大家喜欢吃，才叫真好吃。\"}]
*/
