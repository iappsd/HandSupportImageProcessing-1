namespace HandSupportLib.ImageProcessing.Models
{

    /// <summary>
    /// خيارات اضافة صورة
    /// </summary>
    public class DrawImageOptions
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
        /// عرض الصورة
        /// </summary>
        public float Width { get; set; }
        /// <summary>
        /// ارتفاع الصورة
        /// </summary>
        public float Hight { get; set; }
        /// <summary>
        /// مسار الصورة في الجهاز
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// خيارات رسم الصورة
        /// </summary>
        public DrawImageOptions() { }

        /// <summary>
        /// خيارات رسم الصورة
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <param name="width"></param>
        /// <param name="hight"></param>
        public DrawImageOptions(string imagePath, float positionX, float positionY, float width, int hight)
        {
            ImagePath = imagePath;
            PositionX = positionX;
            PositionY = positionY;
            Width = width;
            Hight = hight;
        }

    }
}
