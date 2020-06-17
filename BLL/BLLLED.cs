using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LEDControl_UDP;
using System.Threading;

namespace BLL
{
    public class BLLLED : BLLBase
    {
        WaitCallback workItem = new WaitCallback(SendMessageToLED);

        static void SendMessageToLED(object state)
        {
            AddParams LEDShow = (AddParams)state;
            LED_UDP LED = LEDShow.LEDshow;
            //byte[] str = System.Text.ASCIIEncoding.Default.GetBytes(LEDShow.LEDshow.ToString());
            if (!LED.sendText(LEDShow.strToShow))
            {
                LED.Reset();
                Thread.Sleep(300);
                LED.sendText(LEDShow.strToShow);
            }
            LED.close();
        }

        public void send(LED_UDP led_udp, string ledtext)
        {
            AddParams param = new AddParams(led_udp, ledtext);
            ThreadPool.QueueUserWorkItem(workItem, param);
        }
    }

    /// <summary>
    /// LED thread
    /// </summary>
    class AddParams
    {
        public string strToShow;
        public LED_UDP LEDshow;

        public AddParams(LED_UDP led_udp, string str)
        {
            LEDshow = led_udp;
            strToShow = str;
        }
    }
}