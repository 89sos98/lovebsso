using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Configuration;


public sealed class SqlManage
{
    private string connString;

    private OleDbConnection conn;

    private SqlManage() { }

    /// <summary>
    /// 获取实例
    /// </summary>
    /// <param name="connConfigString">在Web.config文件中配置的数据库链接的KEY</param>
    /// <returns>返回SqlManage实例</returns>
    public static SqlManage GetInstance()
    {
        SqlManage newDemond = new SqlManage();
        newDemond.connString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        newDemond.conn = new OleDbConnection(newDemond.connString);
        return newDemond;
    }

    /// <summary>
    /// 打开数据库连接
    /// </summary>
    private void OpenConn()
    {
        if (this.conn.State == ConnectionState.Closed)
        {
            this.conn.Open();
        }
    }

    /// <summary>
    /// 关闭数据库连接
    /// </summary>
    private void Dispose()
    {
        if (this.conn.State != ConnectionState.Closed)
        {
            this.conn.Close();
        }
    }

    /// <summary>
    /// 返回第一行第一列的数据
    /// </summary>
    /// <param name="_commandtype"></param>
    /// <param name="_commandtext"></param>
    /// <param name="_parameters"></param>
    /// <returns></returns>
    public object GetFistColumn(CommandType _commandtype, string _commandtext, params OleDbParameter[] _parameters)
    {
        OleDbCommand cmd = new OleDbCommand();
        cmd.CommandType = _commandtype;
        cmd.CommandText = _commandtext;
        cmd.Connection = this.conn;

        if (null != _parameters)
        {
            foreach (OleDbParameter para in _parameters)
            {
                cmd.Parameters.Add(para);
            }
        }
        try
        {
            OpenConn();
            object obj = cmd.ExecuteScalar();
            Dispose();
            return obj;
        }
        catch (Exception ex)
        {
            throw new Exception("数据库连接错误或表不存在，请检查！" + ex.Message);
        }
        finally
        {
            cmd.Dispose();
            Dispose();
        }
    }

    /// <summary>
    /// 执行sql命令
    /// </summary>
    /// <param name="_commandtype"></param>
    /// <param name="_commandtext"></param>
    /// <param name="_parameters"></param>
    /// <returns></returns>
    public int ExecuteSql(CommandType _commandtype, string _commandtext, params OleDbParameter[] _parameters)
    {
        OleDbCommand cmd = new OleDbCommand();
        cmd.CommandType = _commandtype;
        cmd.CommandText = _commandtext;
        cmd.Connection = this.conn;
        if (null != _parameters)
        {
            foreach (OleDbParameter para in _parameters)
            {
                cmd.Parameters.Add(para);
            }
        }
        try
        {
            OpenConn();
            int n = cmd.ExecuteNonQuery();
            Dispose();
            return n;
        }
        catch (Exception ex)
        {
            throw new Exception("数据库连接错误或传入空SqlParameter参数，请检查！" + ex.Message);
        }
        finally
        {
            cmd.Dispose();
            Dispose();
        }
    }

    /// <summary>
    /// 执行带事物的命令集
    /// </summary>
    /// <param name="_tran"></param>
    /// <param name="_commandtype"></param>
    /// <param name="_commandtext"></param>
    /// <param name="_parameters"></param>
    /// <returns></returns>
    public int ExecuteSql(List<CommandType> _commandtype, List<string> _commandtext, List<OleDbParameter[]> _parameters)
    {
        if (_commandtype.Count != _commandtext.Count)
            return 0;

        if (null != _parameters)
        {
            if (_parameters.Count != _commandtext.Count)
                return 0;
        }

        int r = 0;
        List<OleDbCommand> listCmd = new List<OleDbCommand>();
        for (int i = 0; i < _commandtype.Count; i++)
        {
            OleDbCommand cmd = new OleDbCommand();
            cmd.CommandType = _commandtype[i];
            cmd.CommandText = _commandtext[i];
            cmd.Connection = this.conn;

            if (null != _parameters)
            {
                if (null != _parameters[i])
                {
                    foreach (OleDbParameter para in _parameters[i])
                    {
                        cmd.Parameters.Add(para);
                    }
                }
            }

            listCmd.Add(cmd);
        }

        OpenConn();
        using (OleDbTransaction tran = conn.BeginTransaction())
        {
            try
            {
                foreach (OleDbCommand c in listCmd)
                {
                    c.Transaction = tran;
                    r += c.ExecuteNonQuery();
                }

                tran.Commit();
            }
            catch
            {
                tran.Rollback(); r = 0;
            }
        }
        Dispose();
        return r;
    }

    /// <summary>
    /// 获取OleDbDataReader
    /// </summary>
    /// <param name="_commandtype"></param>
    /// <param name="_commandtext"></param>
    /// <param name="_parameters"></param>
    /// <returns></returns>
    public OleDbDataReader GetSqlDataReader(CommandType _commandtype, string _commandtext, params OleDbParameter[] _parameters)
    {
        OleDbCommand cmd = new OleDbCommand();
        cmd.CommandType = _commandtype;
        cmd.CommandText = _commandtext;
        cmd.Connection = this.conn;

        if (_parameters != null)
        {
            foreach (OleDbParameter para in _parameters)
            {
                cmd.Parameters.Add(para);
            }
        }

        try
        {
            OpenConn();
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
        catch (Exception ex)
        {
            throw new Exception("数据库连接错误或传入空SqlParameter参数，请检查!" + ex.Message);
        }
    }


    /// <summary>
    /// 获取DataSet
    /// </summary>
    /// <param name="_commandtype"></param>
    /// <param name="_commandtext"></param>
    /// <param name="_parameters"></param>
    /// <returns></returns>
    public DataSet GetDataSet(CommandType _commandtype, string _commandtext, params OleDbParameter[] _parameters)
    {
        OleDbCommand cmd = new OleDbCommand();
        cmd.CommandType = _commandtype;
        cmd.CommandText = _commandtext;
        cmd.Connection = this.conn;

        foreach (OleDbParameter para in _parameters)
        {
            cmd.Parameters.Add(para);
        }
        OleDbDataAdapter adp = new OleDbDataAdapter(cmd);
        DataSet ds = new DataSet();

        try
        {
            OpenConn();
            adp.Fill(ds);
            Dispose();
            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception("数据库连接错误或传入空OleDbParameter参数，请检查！" + ex.Message);
        }
        finally
        {
            adp.Dispose();
            cmd.Dispose();
            Dispose();
        }
    }
}

