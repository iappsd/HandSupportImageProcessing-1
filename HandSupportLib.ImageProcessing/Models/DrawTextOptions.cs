using System.Drawing;
using static HandSupportLib.ImageProcessing.Enums.ImageEnum;

namespace HandSupportLib.ImageProcessing.Models
{

    /// <summary>
    /// خيارات اضافة نص على الصورة
    /// </summary>
    public class DrawTextOptions
    {
        /// <summary>
        /// احداثية الصورة بال X
        /// </summary>
        public float PositionX { get; set; }
        /// <summary>
        /// احداثية الصورة بال Y
        /// </summary>
        public float PositionY { get; set; }
        /// <summary>
        /// محاذات ظهور النص
        /// </summary>
        public virtual HorizontalAlignment HorizontalAlignment { get; set; }
        /// <summary>
        /// النص
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// حجم النص
        /// </summary>
        public int FontSize { get; set; }
        /// <summary>
        /// لون النص بصيغة الهكس
        /// </summary>
        public string HexColor { get; set; }
        /// <summary>
        /// اسم الخط
        /// </summary>
        public string FontName { get; set; }
        /// <summary>
        /// تنسيق الخط وأوزانه
        /// </summary>
        public virtual FontStyle TextStyle { get; set; }
        /// <summary>
        /// استخدام خط مثبت على الجهاز أو السيرفر
        /// </summary>
        public bool IsInstalledFont { get; set; }
        /// <summary>
        /// لون الخلفية بصيغة الهكس التي تظهر خلف النص يمكن التحكم بالشفافية
        /// </summary>
        public string BackGroundColor { get; set; }
        /// <summary>
        /// في حالة كان النص يحوي عربي
        /// </summary>
        public virtual TextDirection TextDirection { get; set; }


        /// <summary>
        /// خيارات اضافة نص على الصورة
        /// </summary>
        public DrawTextOptions() { }

        /// <summary>
        /// خيارات اضافة نص على الصورة
        /// </summary>
        /// <param name="positionX">الموقع على محور ال X</param>
        /// <param name="positionY">الموقع على محور ال Y</param>
        /// <param name="text">النص</param>
        /// <param name="fontSize">حجم الخط</param>
        /// <param name="hexColor">لون النص</param>
        /// <param name="textStyle">استايل النص</param>
        /// <param name="fontName">اسم الخط</param>
        public DrawTextOptions(float positionX, float positionY, string text, int fontSize, string hexColor = "#000000", FontStyle textStyle = FontStyle.Regular, string fontName = "Arial.ttf")
        {
            PositionX = positionX;
            PositionY = positionY;
            Text = text;
            FontSize = fontSize;
            HexColor = hexColor;
            TextStyle = textStyle;
            FontName = fontName;
            IsInstalledFont = true;
            BackGroundColor = string.Empty;
            HorizontalAlignment = HorizontalAlignment.Right;
            TextDirection = TextDirection.RTL;
        }

        /// <summary>
        /// خيارات اضافة نص على الصورة
        /// </summary>
        /// <param name="positionX">الموقع على محور ال X</param>
        /// <param name="positionY">الموقع على محور ال Y</param>
        /// <param name="horizontalAlignment">محاذات النص</param>
        /// <param name="text">النص</param>
        /// <param name="fontSize">حجم الخط</param>
        /// <param name="hexColor">لون النص</param>
        /// <param name="textStyle">استايل النص</param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <param name="backGroundColor">لون الخلفية</param>
        /// <param name="textDirection">اتجاه النص</param>
        /// <param name="isInstalledFont">خط افتراضي في نظام الويندوز</param>
        /// <param name="fontName">اسم الخط</param>
        public DrawTextOptions(float positionX, float positionY, HorizontalAlignment horizontalAlignment, string text, int fontSize, string hexColor = "#000000", FontStyle textStyle = FontStyle.Regular, string backGroundColor = "", TextDirection textDirection = TextDirection.RTL, bool isInstalledFont = false, string fontName = "Arial.ttf")
        {
            PositionX = positionX;
            PositionY = positionY;
            Text = text;
            FontSize = fontSize;
            HexColor = hexColor;
            TextStyle = textStyle;
            FontName = fontName;
            IsInstalledFont = isInstalledFont;
            BackGroundColor = backGroundColor;
            HorizontalAlignment = horizontalAlignment;
            TextDirection = textDirection;
        }


    }

}
