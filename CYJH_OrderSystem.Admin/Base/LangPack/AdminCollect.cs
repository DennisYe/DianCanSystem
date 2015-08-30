using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CYJH_OrderSystem.Admin.Base.LangPack {
    public static class AdminCollect {

        #region "管理员管理"


        /// <summary>
        /// 请至少选择一项
        /// </summary>
        public const string COMM_ERR_PLEASE_CHESED_MORE_THEN_ONE = "请至少选择一项！";

        /// <summary>
        /// 用户名或密码不正确
        /// </summary>
        public const string LOGIN_ERR = "用户名或密码不正确！";

        /// <summary>
        /// 验证码不正确
        /// </summary>
        public const string CHECKCODE_ERR = "验证码不正确！";

        /// <summary>
        /// 用户名为3-15位英文，数字或下划线！
        /// </summary>
        public const string EDITOR_INFO_ERR_USERNAME_ERR = "用户名为3-15位英文，数字或下划线！";

        /// <summary>
        /// 密码为6-20位英文，数字或下划线！
        /// </summary>
        public const string EDITOR_INFO_ERR_PWD_ERR = "密码为6-20位字符！";

        /// <summary>
        /// 两个密码不一致！
        /// </summary>
        public const string EDITOR_INFO_ERR_PWD_NOT_THE_SAME = "两个密码不一致！";

        /// <summary>
        /// 旧密码不正确！
        /// </summary>
        public const string EDITOR_INFO_ERR_OLD_PWD_ERR = "旧密码不正确！";

        /// <summary>
        /// 邮箱没填写或格式不正确
        /// </summary>
        public const string EDITOR_INFO_ERR_EMAIL_ERR = "邮箱没填写或格式不正确！";

        /// <summary>
        /// 昵称只支持10个以内的字符
        /// </summary>
        public const string EDITOR_INFO_ERR_NICKNAME_ERR = "昵称只支持10个以内的字符！";

        /// <summary>
        /// 请选择预设菜单
        /// </summary>
        public const string EDITOR_INFO_ERR_GROUP_ERR = "请选择预设菜单！";

        /// <summary>
        /// 保存失败
        /// </summary>
        public const string EDITOR_INFO_ERR_SAVE_ERR = "保存失败！";

        /// <summary>
        /// 添加失败
        /// </summary>
        public const string ADD_FAILED = "添加失败";

        /// <summary>
        /// 添加成功
        /// </summary>
        public const string ADD_SUCCESS = "添加成功";

        /// <summary>
        /// 更新预设组信息失败

        /// </summary>
        public const string EDITOR_INFO_ERR_SAVE_GROUP_ERR = "更新预设组信息失败！";

        /// <summary>
        /// 密码修改成功
        /// </summary>
        public const string EDITOR_INFO_SUCCESS_PWD_BUT_BASEINFO = "密码修改成功，基本信息修改失败！";

        /// <summary>
        /// 保存成功
        /// </summary>
        public const string EDITOR_INFO_SUCCESS = "保存成功！";

        /// <summary>
        /// 删除失败！请确认您的身份是否是超级管理员！

        /// </summary>
        public const string DELETE_ADMIN_ERR = "删除失败！请确认您的身份是否是超级管理员！";
        #endregion

        #region "用户组管理"
        public const string ERR_GROUPNAME_NULL = "用户组名称不能为空";

        #endregion

        #region "页面管理"
        public const string DELETE_PAGEINFO_ERR = "删除失败，可能是：该页面存在子页面";
        #endregion

        /// <summary>
        /// 请选择游戏专区
        /// </summary>
        public const string GAMECLASS_NOT_CHOOSE = "请选择游戏专区";

        /// <summary>
        /// 请填写用户名
        /// </summary>
        public const string USERNAME_NOT_NULL = "请填写用户名";
        /// <summary>
        /// 不存在此会员
        /// </summary>
        public const string USERNAMT_NOT_EXIST = "不存在此会员";
        /// <summary>
        /// 该特权区中已经存在了此会员,不能重复添加
        /// </summary>
        public const string USERROLE_IS_EXIST = "该特权区中已经存在了此会员,不能重复添加";

        /// <summary>
        /// 卡类
        /// </summary>
        public const string PRIZE_TYPE_CARD = "卡类";
        /// <summary>
        /// 实物
        /// </summary>
        public const string PRIZE_TYPE_SHIWU = "实物";
        /// <summary>
        /// 花瓶
        /// </summary>
        public const string PRIZE_TYPE_HUAPING = "花瓶";

        /// <summary>
        /// 未留有联系方式
        /// </summary>
        public const string LINKMAN_NOT_LEAVE_CONTACT = "未留有联系方式";

        /// <summary>
        /// 添加奖品
        /// </summary>
        public const string PRIZE_TO_ADD = "添加奖品";

        /// <summary>
        /// 修改奖品
        /// </summary>
        public const string PRIZE_TO_EDIT = "修改奖品";

        /// <summary>
        /// 票数格式不正确
        /// </summary>
        public const string GAMERECOMMAND_TICKET_ERROR_FORMAT = "票数格式不正确";

        /// <summary>
        /// 星级至少0，最多5
        /// </summary>
        public const string STAR_NUMBER_LIMIT = "星级至少0，最多5";
        /// <summary>
        /// 请填写图片地址
        /// </summary>
        public const string IMAGE_ADDRESS_NOT_NULL = "请填写图片地址";
        /// <summary>
        /// 输入格式不正确

        /// </summary>
        public const string INPUT_FORMAT_ERROR = "输入格式不正确";

        /// <summary>
        /// 隐藏
        /// </summary>
        public const string TEXT_HIDDEN = "隐藏";

        /// <summary>
        /// 新增
        /// </summary>
        public const string TEXT_ADD_NEW = "新增";
        /// <summary>
        /// 类型名称不能为空
        /// </summary>
        public const string TYPE_NOT_NULL = "类型名称不能为空";

        /// <summary>
        /// 删除失败
        /// </summary>
        public const string DELETE_FAILED = "删除失败";
        /// <summary>
        /// 删除成功
        /// </summary>
        public const string DELETE_SUCCESSFUL = "删除成功";
        /// <summary>
        /// 该类型正在使用中无法删除
        /// </summary>
        public const string GAMETYPE_NO_ALLOW_DELETE = "该类型正在使用中无法删除";

        /// <summary>
        /// 类型名称不能为空
        /// </summary>
        public const string GAMETYPE_NAME_NOT_ALLOW_NULL = "类型名称不能为空";

        /// <summary>
        /// 游戏编号, 推荐星级以及首页排序应该为正整数
        /// </summary>
        public const string STAR_FORMAT_ERROR = "游戏编号, 推荐星级以及首页排序应该为正整数";

        /// <summary>
        /// 游戏不存在

        /// </summary>
        public const string GAME_NOT_EXIST = "游戏不存在";

        /// <summary>
        /// 已经推荐过了
        /// </summary>
        public const string GAME_HAVA_RECOMMENDED = "已经推荐过了";
        /// <summary>
        /// 请选择一项

        /// </summary>
        public const string COMMON_CHOOSE_ONE = "请选择一项";

        /// <summary>
        /// 名称不能为空
        /// </summary>
        public const string ACT_ACTNAMNE_NOTNULL = "名称不能为空";

        /// <summary>
        /// 所需点数 不能为空！

        /// </summary>
        public const string ACT_DIANSHU_NOTNULL = "所需点数 不能为空！";

        /// <summary>
        /// 所需点数 必须为数字

        /// </summary>
        public const string ACT_DIANSHU_FORMAT_ERR = "所需点数 必须为数字";

        /// <summary>
        /// 请填写所有必填项目

        /// </summary>
        public const string GAME_PC_ERR_NOT_NULL = "请填写所有必填项目";
        /// <summary>
        /// 游戏编号格式不正确

        /// </summary>
        public const string GAME_PC_ERR_GAME_CODE = "游戏编号格式不正确";
        /// <summary>
        /// PK月份格式不正确

        /// </summary>
        public const string GAME_PC_ERR_MONTH = "PK月份格式不正确";
        /// <summary>
        /// 左边游戏不存在，请确认后再添加

        /// </summary>
        public const string GAME_PC_ERR_Left_GAME = "左边游戏不存在，请确认后再添加";
        /// <summary>
        /// 右边的游戏不存在，请确认后再添加
        /// </summary>
        public const string GAME_PC_ERR_RIGHT_GAME = "右边的游戏不存在，请确认后再添加";

        /// <summary>
        /// 修改成功
        /// </summary>
        public const string UPDATE_SUCCESSFULL = "修改成功";
        /// <summary>
        /// 修改失败
        /// </summary>
        public const string UPDATE_FAILED = "修改失败";
        /// <summary>
        /// 保存成功
        /// </summary>
        public const string SAVE_SUCCESSFULL = "保存成功";
        /// <summary>
        /// 保存失败
        /// </summary>
        public const string SAVE_FAILED = "保存失败";

        /// <summary>
        /// 2级确认支付密码错误
        /// </summary>
        public const string SECONDPWD_ERROR = "支付密码错误";

        /// <summary>
        /// 卡库不足，请补足后再发卡
        /// </summary>
        public const string CODE_ERROR_NOCODE = "卡库不足，请补足后再发卡";

        /// <summary>
        /// 发送成功
        /// </summary>
        public const string SEND_SUCCESSFULL = "发送成功";

        /// <summary>
        /// 发送失败
        /// </summary>
        public const string SEND_FAILED = "发送失败";
    }
}
