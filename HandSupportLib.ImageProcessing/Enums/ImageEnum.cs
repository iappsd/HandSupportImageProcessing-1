namespace HandSupportLib.ImageProcessing.Enums
{
    /// <summary>
    /// ترميزات معالجة الصور
    /// </summary>
    public static class ImageEnum
    {
        /// <summary>
        /// اتجاه لغة النص
        /// </summary>
        public enum TextDirection
        {
            /// <summary>
            /// من اليمين لليسار
            /// </summary>
            RTL = 0,
            /// <summary>
            /// من اليسار لليمين
            /// </summary>
            LTR = 1,
        }

        /// <summary>
        /// محاذات النص الأفقية
        /// </summary>
        public enum HorizontalAlignment
        {
            /// <summary>
            /// يمين
            /// </summary>
            Right = 0,
            /// <summary>
            /// وسط
            /// </summary>
            Center = 1,
            /// <summary>
            /// يسار
            /// </summary>
            Left = 2
        }

    }
}
