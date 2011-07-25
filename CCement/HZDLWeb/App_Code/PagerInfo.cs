/*
 *  作者：向彩虹
 *  描述：企业站之自定义模型
*/
using System;


#region 翻页控件模型类
/// <summary>
/// 翻页控件模型
/// </summary>
[Serializable]
public class PagerInfo
{
    private int _recordcount;

    /// <summary>
    /// 总记录数
    /// </summary>
    public int Recordcount
    {
        get { return _recordcount; }
        set { _recordcount = value; }
    }

    private int _currentPageIndex;

    /// <summary>
    /// 当前页码
    /// </summary>
    public int CurrentPageIndex
    {
        get { return _currentPageIndex; }
        set { _currentPageIndex = value; }
    }

    private int _totalPage;

    /// <summary>
    /// 总页数
    /// </summary>
    public int TotalPage
    {
        get { return _totalPage; }
        set { _totalPage = value; }
    }

    private int _pageSize;

    /// <summary>
    /// 页面大小
    /// </summary>
    public int PageSize
    {
        get { return _pageSize; }
        set { _pageSize = value; }
    }
}
#endregion

#region 搜索方式枚举
[Serializable]
public enum KeyMode : sbyte
{
    /// <summary>
    /// 关键字
    /// </summary>
    Key = 1,
    /// <summary>
    /// 标题
    /// </summary>
    Title = 2,
    /// <summary>
    /// 内容
    /// </summary>
    Content = 3,
    /// <summary>
    /// 标题+内容
    /// </summary>
    TitleAndContent = 4
}
#endregion


/*
 *  作者：向彩虹
 *  描述：企业站之数据字典枚举
*/
#region 类别域枚举
[Serializable]
public enum CategoryArea : sbyte
{
    /// <summary>
    /// 文章
    /// </summary>
    Article = 1,
    /// <summary>
    /// 产品
    /// </summary>
    Product = 2
}
#endregion

#region 互动类别枚举
[Serializable]
public enum MessageType : sbyte
{
    /// <summary>
    /// 在线留言
    /// </summary>
    Message = 1,
    /// <summary>
    /// 订购产品
    /// </summary>
    Product = 2
}
#endregion



