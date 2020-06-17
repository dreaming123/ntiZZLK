using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.ServiceModel;
using Service;

namespace ComputeClient
{
    public class CComputeClient
    {
        private string rv = "调用WCF接口成功;";

        private ChannelFactory<IService> cf = new ChannelFactory<IService>("ICClient");
        private IService iob;
        public IService IOB
        {
            get
            {
                IClientChannel channel = iob as IClientChannel;

                //if (channel == null || channel.State != CommunicationState.Opened)
                if (channel == null)
                    iob = cf.CreateChannel();
                else
                {
                    try
                    {
                        iob.ConnectCheck();
                    }
                    catch
                    {
                        iob = cf.CreateChannel();
                    }
                }
                return iob;
            }
        }

        public string SortingLineItemBarcodeChecking(string sortingLineID, string itemBarcode)
        {
            string str = "分拣烟包:" + itemBarcode;
            try
            {
                str += " 返回月台:" + IOB.SortingLineItemBarcodeChecking(sortingLineID, itemBarcode);
            }
            catch (Exception ex)
            {
                str += ex.Message.ToString();
            }
            return str;
        }

        public string RobotLineContainerIn(string robotLineID, string containerBarcode)
        {
            string str = containerBarcode + "到达缓存位" + robotLineID;
            try
            {
                IOB.RobotLineContainerIn(robotLineID, containerBarcode);
            }
            catch (Exception ex)
            {
                str += ex.Message.ToString();
            }
            return str;
        }

        public string RobotLinePickingContainerOut(string robotLineID)
        {
            string str = "托盘到达拣选位" + robotLineID;
            try
            {
                IOB.RobotLinePickingContainerOut(robotLineID);
            }
            catch (Exception ex)
            {
                str += ex.Message.ToString();
            }
            return str;
        }

        public string RobotPickingFinished(string robotID)
        {
            string str = robotID + "拣选完成 ";
            try
            {
                IOB.RobotPickingFinished(robotID);
            }
            catch (Exception ex)
            {
                str += ex.Message.ToString();
            }
            return str;
        }

        public string DockItemReceived(string dockID, string itemBarcode)
        {
            string str = "月台扫码:" + itemBarcode;
            try
            {
                IOB.DockItemReceived(dockID, itemBarcode);
            }
            catch (Exception ex)
            {
                str += ex.Message.ToString();
            }
            return str;
        }
    }
}
