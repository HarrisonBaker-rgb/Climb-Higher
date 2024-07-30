using Emgu.CV.Structure;

namespace climb_higher
{
    /*
     * Ranges for each color defined in HSV format.
     */
    internal class ColorRanges
    {
        public ColorRanges() { }

        public static Hsv BlackUpper
        {
            get
            {
                return new Hsv(180, 255, 30);
            }
        }

        public static Hsv BlackLower
        {
            get
            {
                return new Hsv(0, 0, 0);
            }
        }

        public static Hsv WhiteUpper
        {
            get
            {
                return new Hsv(180, 18, 255);
            }
        }

        public static Hsv WhiteLower
        {
            get
            {
                return new Hsv(0, 0, 231);
            }
        }

        public static Hsv RedUpper
        {
            get
            {
                return new Hsv(180, 255, 255);
            }
        }

        public static Hsv RedLower
        {
            get
            {
                return new Hsv(159, 50, 70);
            }
        }

        public static Hsv RedUpper2
        {
            get
            {
                return new Hsv(9, 255, 255);
            }
        }

        public static Hsv RedLower2
        {
            get
            {
                return new Hsv(0, 50, 70);
            }
        }

        public static Hsv GreenUpper
        {
            get
            {
                return new Hsv(89, 255, 255);
            }
        }

        public static Hsv GreenLower
        {
            get
            {
                return new Hsv(36, 50, 70);
            }
        }

        public static Hsv BlueUpper
        {
            get
            {
                return new Hsv(128, 255, 255);
            }
        }

        public static Hsv BlueLower
        {
            get
            {
                return new Hsv(90, 50, 70);
            }
        }

        public static Hsv YellowUpper
        {
            get
            {
                return new Hsv(35, 255, 255);
            }
        }

        public static Hsv YellowLower
        {
            get
            {
                return new Hsv(25, 50, 70);
            }
        }

        public static Hsv PurpleUpper
        {
            get
            {
                return new Hsv(158, 255, 255);
            }
        }

        public static Hsv PurpleLower
        {
            get
            {
                return new Hsv(129, 50, 70);
            }
        }

        public static Hsv OrangeUpper
        {
            get
            {
                return new Hsv(24, 255, 255);
            }
        }

        public static Hsv OrangeLower
        {
            get
            {
                return new Hsv(10, 50, 70);
            }
        }

        public static Hsv GrayUpper
        {
            get
            {
                return new Hsv(158, 255, 255);
            }
        }

        public static Hsv GrayLower
        {
            get
            {
                return new Hsv(0, 0, 40);
            }
        }
    }
}
