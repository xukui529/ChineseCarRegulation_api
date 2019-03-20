namespace NFine.Code.Enum
{
    /// <summary>
    /// 文档发布状态  发布状态 0未审核、1已提交、2已通过3、未通过4待发布  待审核 待发布 已发布
    /// 最新修改为 0未提交 1待审核 2待发布 3未通过 4已发布
    /// </summary>
    public enum DocumentStatus
    {
        /// <summary>
        /// 未提交
        /// </summary>
        [EnumTextValue("未提交")]
        未提交 = 0,
        /// <summary>
        /// 待审核
        /// </summary>
        [EnumTextValue("待审核")]
        待审核 = 1,
        /// <summary>
        /// 待发布
        /// </summary>
        [EnumTextValue("待发布")]
        待发布 = 2,
        /// <summary>
        /// 未通过
        /// </summary>
        [EnumTextValue("未通过")]
        未通过 = 3,
        /// <summary>
        /// 已发布
        /// </summary>
        [EnumTextValue("已发布")]
        已发布 = 4,
        
    }
    /// <summary>
    /// 1标准、2法规、3资讯
    /// </summary>
    public enum DocumentType
    {
        /// <summary>
        /// 标准
        /// </summary>
        [EnumTextValue("标准")]
        标准 =   1,
        /// <summary>
        /// 法规
        /// </summary>
        [EnumTextValue("2法规")]
        法规 = 2,
        /// <summary>
        /// 资讯
        /// </summary>
        [EnumTextValue("资讯")]
        资讯 = 3,
         
    }
    /// <summary>
    /// 接口返回值
    /// </summary>
    public enum EnumResultCode
    {
        /// <summary>
        /// 正常
        /// </summary>
        [EnumTextValue("正常")]
        正常 = 200,
        /// <summary>
        /// 异常
        /// </summary>
        [EnumTextValue("异常")]
        异常 = 300 
    }

    
}
