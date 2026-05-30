using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace XGame
{
    /// <summary>
    /// 商品信息
    /// </summary>
    [Serializable]
    [Preserve]
    public class ProductInfo
    {
        /// <summary>
        /// 商品id
        /// </summary>
        [Preserve] public string id;

        /// <summary>
        /// 商品名
        /// </summary>
        [Preserve] public string name;

        /// <summary>
        /// 描述
        /// </summary>
        [Preserve] public string desc;

        /// <summary>
        /// 商品类型 1是一次性类型，2是订阅类型
        /// </summary>
        [Preserve] public int type;

        /// <summary>
        /// 订阅商品优惠详情, 谷歌内购使用这个获取商品详情
        /// </summary>
        [Preserve] public List<SubsProductOfferDetails> subsOfferDetails;

        /// <summary>
        /// 一次性商品优惠详情, 谷歌内购使用这个获取商品详情
        /// </summary>
        [Preserve] public List<OneTimeProductOfferDetails> oneTimeOfferDetails;

        /// <summary>
        /// 商品价格
        /// </summary>
        [Preserve] public string price;

        /// <summary>
        /// 订阅商品原价格
        /// </summary>
        [Preserve] public string originalPrice;

        /// <summary>
        /// 订阅商品优惠标签
        /// </summary>
        [Preserve] public List<string> offerTags;

        /// <summary>
        /// 货币类型
        /// </summary>
        [Preserve] public string priceCurrencyCode;
    }


    [Serializable]
    [Preserve]
    public class ProductDiscountAmount
    {
        /// <summary>
        /// 商品价格，带货币符号，比如 9.9$
        /// </summary>
        [Preserve] public string price;

        /// <summary>
        /// 货币代码，比如 USD
        /// </summary>
        [Preserve] public string priceCurrencyCode;

        /// <summary>
        /// 商品金额，单位为微，比例为一百万比一，比如 9.9$为9990000，数据类型为long类型字符串
        /// </summary>
        [Preserve] public string priceMicros;
    }

    [Serializable]
    [Preserve]
    public class ProductValidTimeWindow
    {
        /// <summary>
        /// 优惠开始时间戳，毫秒
        /// </summary>
        [Preserve] public string startTimeMillis;

        /// <summary>
        /// 优惠结束时间戳，毫秒
        /// </summary>
        [Preserve] public string endTimeMillis;
    }

    [Serializable]
    [Preserve]
    public class ProductPreorderDetails
    {
        /// <summary>
        /// 预购发布时间戳，毫秒
        /// </summary>
        [Preserve] public string releaseTimeMillis;

        /// <summary>
        /// 预购结束时间戳，毫秒
        /// </summary>
        [Preserve] public string presaleEndTimeMillis;
    }

    [Serializable]
    [Preserve]
    public class ProductLimitedQuantityInfo
    {
        /// <summary>
        /// 限购商品最大数量
        /// </summary>
        [Preserve] public int maximumQuantity;

        /// <summary>
        /// 限购商品剩余数量
        /// </summary>
        [Preserve] public int remainingQuantity;
    }

    [Serializable]
    [Preserve]
    public class ProductDiscountDisplayInfo
    {
        /// <summary>
        /// 折扣百分比，比如 50，折扣百分比跟金额同时只存在一个
        /// </summary>
        [Preserve] public int percentageDiscount;

        /// <summary>
        /// 折扣金额，折扣百分比跟金额同时只存在一个
        /// </summary>
        [Preserve] public ProductDiscountAmount discountAmount;
    }

    [Serializable]
    [Preserve]
    public class ProductRentalDetails
    {
        /// <summary>
        /// 租赁期限，格式为ISO 8601，比如 P1W:一周，P1M:一个月，P1Y:一年
        /// </summary>
        [Preserve] public string rentalPeriod;

        /// <summary>
        /// 租赁到期期限，格式为ISO 8601，比如 P1W:一周，P1M:一个月，P1Y:一年
        /// </summary>
        [Preserve] public string rentalExpirationPeriod;
    }

    [Serializable]
    [Preserve]
    public class OneTimeProductOfferDetails
    {
        /// <summary>
        /// 商品价格，带货币符号，比如 9.9$
        /// </summary>
        [Preserve] public string price;

        /// <summary>
        /// 货币代码，比如 USD
        /// </summary>
        [Preserve] public string priceCurrencyCode;

        /// <summary>
        /// 优惠token，指定使用商品的优惠
        /// </summary>
        [Preserve] public string offerToken;

        /// <summary>
        /// 商品金额，单位为微，比例为一百万比一，比如 9.9$为9990000，数据类型为long类型字符串
        /// </summary>
        [Preserve] public string priceMicros;

        /// <summary>
        /// 商品原价金额，单位为微，比例为一百万比一，比如 9.9$为9990000，数据类型为long类型字符串
        /// </summary>
        [Preserve] public string fullPriceMicros;

        /// <summary>
        /// 优惠ID
        /// </summary>
        [Preserve] public string offerId;

        /// <summary>
        /// 优惠标签
        /// </summary>
        [Preserve] public List<string> offerTags;

        /// <summary>
        /// 购买选项ID
        /// </summary>
        [Preserve] public string purchaseOptionId;

        /// <summary>
        /// 租赁详情
        /// </summary>
        [Preserve] public ProductRentalDetails rentalDetails;

        /// <summary>
        /// 折扣信息
        /// </summary>
        [Preserve] public ProductDiscountDisplayInfo discountDisplayInfo;

        /// <summary>
        /// 限购信息
        /// </summary>
        [Preserve] public ProductLimitedQuantityInfo limitedQuantityInfo;

        /// <summary>
        /// 预购详情
        /// </summary>
        [Preserve] public ProductPreorderDetails preorderDetails;

        /// <summary>
        /// 优惠有效窗口
        /// </summary>
        [Preserve] public ProductValidTimeWindow validTimeWindow;
    }


    [Serializable]
    [Preserve]
    public class ProductInstallmentPlanDetails
    {
        /// <summary>
        /// 承诺付款计数
        /// </summary>
        [Preserve] public int commitmentPaymentsCount;

        /// <summary>
        /// 后续承诺付款计数
        /// </summary>
        [Preserve] public int subsequentCommitmentPaymentsCount;
    }

    [Serializable]
    [Preserve]
    public class PricingPhase
    {
        /// <summary>
        /// 商品价格，带货币符号，比如 9.9$
        /// </summary>
        [Preserve] public string price;

        /// <summary>
        /// 货币代码，比如 USD
        /// </summary>
        [Preserve] public string priceCurrencyCode;

        /// <summary>
        /// 商品金额，单位为微，比例为一百万比一，比如 9.9$为9990000，数据类型为long类型字符串
        /// </summary>
        [Preserve] public string priceMicros;

        /// <summary>
        /// 计费周期，格式为ISO 8601，比如 P1W:一周，P1M:一个月，P1Y:一年
        /// </summary>
        [Preserve] public string billingPeriod;

        /// <summary>
        /// 计费周期计数
        /// </summary>
        [Preserve] public int billingCycleCount;

        /// <summary>
        /// 订阅循环模式，1：无限循环，2：有限循环，判断循环次数 billingCycleCount，3：不循环
        /// </summary>
        [Preserve] public int recurrenceMode;
    }

    [Serializable]
    [Preserve]
    public class SubsProductOfferDetails
    {
        /// <summary>
        /// 优惠ID
        /// </summary>
        [Preserve] public string offerId;

        /// <summary>
        /// 优惠标签
        /// </summary>
        [Preserve] public List<string> offerTags;

        /// <summary>
        /// 优惠token
        /// </summary>
        [Preserve] public string offerToken;

        /// <summary>
        /// 订阅商品基础计划ID
        /// </summary>
        [Preserve] public string basePlanId;

        /// <summary>
        /// 定价阶段
        /// </summary>
        [Preserve] public List<PricingPhase> pricingPhaseList;

        /// <summary>
        /// 分期付款详情
        /// </summary>
        [Preserve] public ProductInstallmentPlanDetails installmentPlanDetails;
    }
}