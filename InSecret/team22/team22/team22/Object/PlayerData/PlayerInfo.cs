using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace team22.Object
{
    static class PlayerInfo
    {
        private static bool isMove = true;
        public static bool IsMove
        {
            get { return isMove; }
            set { isMove = value; }
        }

        private static bool isSwitchable = true;

        public static bool IsSwitchable
        {
            get { return isSwitchable; }
            set { isSwitchable = value; }
        }

        private static bool forceSwitch = false;

        public static bool ForceSwitch
        {
            get { return forceSwitch; }
            set { forceSwitch = value; }
        }

        private static int rightWall;
        public static int RightWall
        {
            get { return rightWall; }
            set { rightWall = value; }
        }

        private static bool isVisible;
        public static bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; }
        }

        //男かどうか？
        private static bool isMan;
        public static bool IsMan
        {
            get { return isMan; }
            set { isMan = value; }
        }

        private static bool isFinish;

        public static bool IsFinish
        {
            get { return isFinish; }
            set { isFinish = value; }
        }

        private static bool autoMove = false;

        public static bool AutoMove
        {
            get { return autoMove; }
            set { autoMove = value; }
        }

        private static bool inElevator;

        public static bool InElevator
        {
            get { return inElevator; }
            set { inElevator = value; }
        }
    }
}
