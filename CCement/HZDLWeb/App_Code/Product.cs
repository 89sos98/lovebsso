using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
///Product 的摘要说明
/// </summary>
public class Product
{
	public Product()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}
    private int id;

    public int Id
    {
        get { return id; }
        set { id = value; }
    }

    private int isCorP;

    public int IsCorP
    {
        get { return isCorP; }
        set { isCorP = value; }
    }
    private int categoryId;

    public int CategoryId
    {
        get { return categoryId; }
        set { categoryId = value; }
    }
    private int dengJi;

    public int DengJi
    {
        get { return dengJi; }
        set { dengJi = value; }
    }
    private string pName;

    public string PName
    {
        get { return pName; }
        set { pName = value; }
    }
    private string pContent;

    public string PContent
    {
        get { return pContent; }
        set { pContent = value; }
    }
    private DateTime upTime;

    public DateTime UpTime
    {
        get { return upTime; }
        set { upTime = value; }
    }
    private DateTime publishTime;

    public DateTime PublishTime
    {
        get { return publishTime; }
        set { publishTime = value; }
    }
}
