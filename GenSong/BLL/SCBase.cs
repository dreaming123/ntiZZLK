using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenSong
{
    public abstract class SCBase
    {

        public struct ResponseStatusStruct
        {
            int reltype;//0:状态信息；1：任务确认信息
            string scStatuScode;
            string scStatuDiscrip;


        }

        public struct ResponseStruct
        {
            int reltype;//0:状态信息；1：任务确认信息

        }

        public abstract byte[] RequestStatusBuffer(byte[] head);

        public abstract byte[] SendTaskBuffer(string SCNo, string SourcePosition, string Destination, string order, string TaskNo);

        public abstract byte[] SendTaskBuffer(string SCNo, string SourcePosition, string Destination, string SecSourcePosition, string SecDestination, string order, string TaskNo);

        public abstract byte[] SendTaskMsg(string SCNo, string SourcePosition, string Destination, string order, string TaskNo);

        public abstract byte[] SendTaskMsg(string SCNo, string SourcePosition, string Destination, string SecSourcePosition, string SecDestination, string order, string TaskNo);

        public abstract byte[] SCEStop();

        public abstract byte[] TaskCancelEStop();

        public abstract byte[] SCBackOrigin();

        public abstract byte[] RequestSRMState();

        public abstract object Analysis(string msg);

        public abstract CRNStatus ResponseStatus(string msg);

        public abstract string[] ResponseTaskConfirm(string msg);



        public SCBase()
        {
            // TODO: Complete member initialization
        }
    }
}
