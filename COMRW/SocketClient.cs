using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;

/// <summary>
/// Summary description for Class1
/// </summary>
class TimeOutSocket
{
    private static bool IsConnectionSuccessful = false;
    private static Exception socketexception;
    private static ManualResetEvent TimeoutObject = new ManualResetEvent(false);

    public static TcpClient Connect(IPEndPoint remoteEndPoint, int timeoutMSec)
    {
        TimeoutObject.Reset();
        socketexception = null;

        string serverip = Convert.ToString(remoteEndPoint.Address);
        int serverport = remoteEndPoint.Port;
        TcpClient tcpclient = new TcpClient();

        tcpclient.BeginConnect(serverip, serverport,
            new AsyncCallback(CallBackMethod), tcpclient);

        if (TimeoutObject.WaitOne(timeoutMSec, false))
        {
            if (IsConnectionSuccessful)
            {
                return tcpclient;
            }
            else
            {
                return null;
            }
        }
        else
        {
            tcpclient.Close();
            return null;
        }
    }
    private static void CallBackMethod(IAsyncResult asyncresult)
    {
        try
        {
            IsConnectionSuccessful = false;
            TcpClient tcpclient = asyncresult.AsyncState as TcpClient;

            if (tcpclient.Client != null)
            {
                tcpclient.EndConnect(asyncresult);
                IsConnectionSuccessful = true;
            }
        }
        catch (Exception ex)
        {
            IsConnectionSuccessful = false;
            socketexception = ex;
        }
        finally
        {
            TimeoutObject.Set();
        }
    }
}
