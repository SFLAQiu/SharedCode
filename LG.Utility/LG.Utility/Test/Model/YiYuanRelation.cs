using LG.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Model {
    /// <summary>
    /// 一元购关系
    /// </summary>

    public class YiYuanRelation {
        ///<summary>
        ///自增ID
        ///</summary>

        public int Id { get; set; }
        ///<summary>
        ///邀请者用户ID
        ///</summary>
        public int UserId { get; set; }

        ///<summary>
        ///邀请者用户名
        ///</summary>
        public string UserName { get; set; }

        ///<summary>
        ///被邀请者用户ID
        ///</summary>
        public string IOuterUserId { get; set; }

        ///<summary>
        ///被邀请者用户名
        ///</summary>
        public string IOuterUserName { get; set; }

        ///<summary>
        ///被邀请者头像
        ///</summary>
        public string IOuterUserHeadUrl { get; set; }

        ///<summary>
        ///被邀请者返还网用户ID
        ///</summary>
        public int IUserId { get; set; }

        ///<summary>
        ///被邀请的手机号
        ///</summary>
        public string IPhoneNum { get; set; }

        ///<summary>
        ///修改前的手机号
        ///</summary>
        public string IPhoneNumOld { get; set; }

        ///<summary>
        ///活动ID
        ///</summary>
        public int ActId { get; set; }

        ///<summary>
        ///添加时间
        ///</summary>
        public DateTime AddDateTime { get; set; }

        ///<summary>
        ///注册时间
        ///</summary>
        public DateTime RegDate { get; set; }

        ///<summary>
        ///是否分享激活过
        ///</summary>
        public bool SharedActivate { get; set; }

        ///<summary>
        ///是否已经购买
        ///</summary>
        public bool AlreadyBuy { get; set; }
    }
    /// <summary>
    /// 用户状态
    /// </summary>
    public enum EUserStatus {
        /// <summary>
        /// 注册新用户
        /// </summary>
        RegistUser = 1,
        /// <summary>
        /// 分享
        /// </summary>
        Shared = 2,
        /// <summary>
        /// 可能购买
        /// </summary>
        UserMayBuy = 3,
        /// <summary>
        /// 已经购买
        /// </summary>
        UserBuy = 4

    }
    public class YiYuanRelationSearch {
        [SqlParameter(FieldName = "UserId", ERelation = EWhereRelation.OR)]
        public int? OtherUserId { get; set; }
        ///<summary>
        ///活动ID
        ///</summary>
        [SqlParameter]
        public int? ActId { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        [SqlParameter]
        public int? UserId { get; set; }
        [SqlParameter(FieldName ="UserId",EOperator =EWhereOperator.IN)]
        public List<int> UserIds { get; set; }
        [SqlParameter(EOperator =EWhereOperator.LessThan)]
        public DateTime RegDate { get; set; }
        public string TestParame { get; set; }
    }
}
