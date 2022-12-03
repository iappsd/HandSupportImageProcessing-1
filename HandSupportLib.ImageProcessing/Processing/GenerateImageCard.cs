using HandSupportLib.ImageProcessing.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using static HandSupportLib.ImageProcessing.Enums.ImageEnum;

namespace HandSupportLib.ImageProcessing.Processing
{


    /// <summary>
    /// معالجة محتويات الصور
    /// </summary>
    public class GenerateImageCard
    {
        /// <summary>
        /// مسار مجلد الخط المستخدم على النظام
        /// </summary>
        public string FontsDirectoryPath { get; set; }
        /// <summary>
        /// مسار الصورة المستخدمة كخلفية
        /// </summary>
        public string BackGroundCardImagePath { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="backgroundCardImagePath">مسار الصورة التي بالخلفية</param>
        /// <param name="fontsDirectoryPath">مجلد الخطوط</param>
        public GenerateImageCard(string backgroundCardImagePath, string fontsDirectoryPath = "")
        {
            BackGroundCardImagePath = backgroundCardImagePath;
            FontsDirectoryPath = fontsDirectoryPath;
        }

        /// <summary>
        /// جلب مقاسات صورة الخلفية
        /// </summary>
        /// <returns></returns>
        public (int width, int height) GetCardSize()
        {
            Image img = Image.FromFile(BackGroundCardImagePath);
            return (img.Width, img.Height);
        }

        /// <summary>
        /// انشاء الكرت وارجاعه كنص Base64
        /// </summary>
        /// <param name="drawTexts">النصوص</param>
        /// <param name="drawImages">الصورة</param>
        /// <returns></returns>
        public string GetCardAsBase64(List<DrawTextOptions> drawTexts, List<DrawImageOptions> drawImages = null)
        {
            var card = CreateCard(drawTexts, drawImages);
            if (card != null)
                return $@"data:image/png;base64,{Convert.ToBase64String(card)}";
            else
                return null;
        }

        /// <summary>
        /// انشاء الكرت وارجاعه كمصفوفة byte[]
        /// </summary>
        /// <param name="drawTexts">النصوص</param>
        /// <param name="drawImages">الصورة</param>
        /// <returns></returns>
        public byte[] GetCardAsByteArray(List<DrawTextOptions> drawTexts, List<DrawImageOptions> drawImages = null)
        {
            var card = CreateCard(drawTexts, drawImages);
            return card ?? null;
        }

        /// <summary>
        /// قص الصورة بشكل دائرة
        /// </summary>
        /// <param name="imagePath">مسار الصورة</param>
        /// <returns></returns>
        private Image ClipImageIntoCircle(string imagePath)
        {
            if (File.Exists(imagePath) == false)
                return null;

            Image img = Image.FromFile(imagePath);
            int x = img.Width / 2;
            int y = img.Height / 2;
            int r = Math.Min(x, y);

            Bitmap tmp = new Bitmap(2 * r, 2 * r);
            using (Graphics g = Graphics.FromImage(tmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TranslateTransform(tmp.Width / 2, tmp.Height / 2);
                GraphicsPath gp = new GraphicsPath();
                gp.AddEllipse(0 - r, 0 - r, 2 * r, 2 * r);
                Region rg = new Region(gp);
                g.SetClip(rg, CombineMode.Replace);
                Bitmap bmp = new Bitmap(img);
                g.DrawImage(bmp, new Rectangle(-r, -r, 2 * r, 2 * r), new Rectangle(x - r, y - r, 2 * r, 2 * r), GraphicsUnit.Pixel);
            }
            return tmp;
        }

        /// <summary>
        /// انشاء كرت صورة
        /// </summary>
        /// <param name="drawTexts"></param>
        /// <param name="drawImages"></param>
        /// <returns></returns>
        private byte[] CreateCard(List<DrawTextOptions> drawTexts, List<DrawImageOptions> drawImages = null)
        {
            if (drawTexts == null || drawTexts.Count <= 0) return null;

            if (File.Exists(BackGroundCardImagePath) == false)
                return null;

            // ضبط الاعدادات العامة للنصوص
            var stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            stringFormat.Trimming = StringTrimming.Character;

            byte[] cardBytes;
            using (var bgImage = Image.FromFile(BackGroundCardImagePath))
            using (var newImage = new Bitmap(bgImage.Width, bgImage.Height, PixelFormat.Format64bppPArgb))
            using (var gr = Graphics.FromImage(newImage))
            {
      

                gr.Clear(Color.White);
                // تحديد جودة ظهور الاضافات المرسومه على الصورة
                gr.SmoothingMode = SmoothingMode.HighQuality;
                gr.InterpolationMode = InterpolationMode.HighQualityBilinear;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.CompositingQuality = CompositingQuality.HighQuality;
                gr.TextRenderingHint = TextRenderingHint.SystemDefault;

                // اضافة الطبقة الأولى وهي الصورة الخاصة بالخلفية
                gr.DrawImage(bgImage, 0, 0, bgImage.Width, bgImage.Height);
                // اضافة الصور الى مربع العمل
                if (drawImages != null)
                    foreach (var drawImage in drawImages)
                    {
                        //Image clipedImage = ClipImageIntoCircle(drawImage.ImagePath);
                        //if (clipedImage != null)
                        var imageLayer = Image.FromFile(drawImage.ImagePath);
                        gr.DrawImage(imageLayer, drawImage.PositionX, drawImage.PositionY, drawImage.Width, drawImage.Hight);
                    }
                //-----------------------------------------------------
                foreach (var drawText in drawTexts)
                {
                    if (!string.IsNullOrWhiteSpace(drawText.Text))
                    {
                        var privateFonts = new System.Drawing.Text.PrivateFontCollection();
                        // تحديد موقع الخط الذي سيتم استخدامه
                        try
                        {
                            // خط مثبت على الجهاز
                            if (drawText.IsInstalledFont == false)
                                privateFonts.AddFontFile($@"{FontsDirectoryPath}\{drawText.FontName}");
                            else
                                // استخدام خط موجود على مسار محدد وغير مثبت على الجهاز
                                privateFonts.AddFontFile($@"{Environment.GetEnvironmentVariable("windir")}\fonts\{drawText.FontName}");
                        }
                        catch (Exception) { privateFonts.AddFontFile($@"{Environment.GetEnvironmentVariable("windir")}\fonts\tahoma.ttf"); }

                        var textFont = new Font(privateFonts.Families[0], drawText.FontSize, drawText.TextStyle);
                        using (Brush textBrush = new SolidBrush(ColorTranslator.FromHtml(drawText.HexColor)))
                        {
                            // Size textSize = TextRenderer.MeasureText(drawText.Text, usingTextFont); // For Core 3
                            // حساب حجم النص من خلال رسمه بالمحاكاه قبل تركيبه على الصورة الحقيقه من أجل الحصول على الأبعاد
                            Size textSize = MeasureString(drawText.Text, textFont);// For Core 2.2

                            float textPositionXAtCenter = 0;
                            // حساب لجعل النص يتوسط موقع المعطى بشكل افقي وذلك بقسمت حجم النص دائماً على اثنين وتنقيصها من الاحداثي السيني X
                            if (drawText.HorizontalAlignment == HorizontalAlignment.Center)
                                textPositionXAtCenter = textSize.Width / 2;
                            // تحديد أبعاد المستطيل الذي سيتم رسم النص بداخله
                            var rectangleText = default(RectangleF);
                            // طريقة حساب الاحداثيات في حالة كان النص عربي
                            if (drawText.TextDirection == TextDirection.RTL)
                            {
                                if (drawText.HorizontalAlignment == HorizontalAlignment.Right)
                                    rectangleText = new RectangleF(bgImage.Width - textSize.Width - drawText.PositionX + textPositionXAtCenter, drawText.PositionY, textSize.Width + 20, textSize.Height + 10);
                                else
                                    rectangleText = new RectangleF(drawText.PositionX - textPositionXAtCenter, drawText.PositionY, textSize.Width + 20, textSize.Height + 10);
                            }
                            else
                            {
                                if (drawText.HorizontalAlignment == HorizontalAlignment.Right)
                                    rectangleText = new RectangleF(drawText.PositionX - textPositionXAtCenter, drawText.PositionY, textSize.Width + 20, textSize.Height + 10);
                                else
                                    rectangleText = new RectangleF(bgImage.Width - textSize.Width - drawText.PositionX + textPositionXAtCenter, drawText.PositionY, textSize.Width + 20, textSize.Height + 10);

                            }

                            // ملئ الخلفية بلون في حالة تم تحديد لون لخلفية النص
                            if (!string.IsNullOrWhiteSpace(drawText.BackGroundColor))
                            {
                                var rectangleBackGroundBrush = new SolidBrush(ColorTranslator.FromHtml(drawText.BackGroundColor));
                                gr.FillRectangle(rectangleBackGroundBrush, rectangleText);
                            }
                            // رسم النص في الصورة المحدده
                            gr.DrawString(drawText.Text, textFont, textBrush, rectangleText, stringFormat);
                        }
                    }

                }
                cardBytes = ConvertImageToBytes(newImage);
            }
            return cardBytes;
        }

        // For Core 3
        //public SizeF MeasureString(string s, Font font)
        //{
        //    SizeF result;
        //    using (var image = new Bitmap(1, 1))
        //    {
        //        using (var g = Graphics.FromImage(image))
        //        {
        //            result = g.MeasureString(s, font);
        //        }
        //    }

        //    return result;
        //}

        /// <summary>
        /// جلب مقاسات النص كصورة
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        private Size MeasureString(string text, Font font)
        {
            SizeF result;
            using (var gr = Graphics.FromHwnd(IntPtr.Zero))
            {
                gr.SmoothingMode = SmoothingMode.HighQuality;
                result = gr.MeasureString(text, font);
            }
            return Size.Truncate(result); ;
        }

        /// <summary>
        /// تحويل الصورة بايتات
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        private byte[] ConvertImageToBytes(Bitmap bmp)
        {
            byte[] byteImage;
            using (var ms = new MemoryStream())
            {
                bmp.Save(ms, ImageFormat.Jpeg);
                byteImage = ms.ToArray();
                ms.Close();
            }
            return byteImage;
        }

        /// <summary>
        /// جلب الصورة كنص Base64
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="imageFormat"></param>
        /// <returns></returns>
        private string GetImageBase64(Bitmap bmp, ImageFormat imageFormat)
        {
            string result = "";
            using (var ms = new MemoryStream())
            {
                bmp.Save(ms, imageFormat);
                byte[] byteImage = ms.ToArray();
                result = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                ms.Close();
            }
            return result;
        }
        /// <summary>
        /// تحويل نص الصورة الى مصفوفة byte
        /// </summary>
        /// <param name="stringInBase64"></param>
        /// <returns></returns>
        private byte[] ConvertBase64ToByteArray(string stringInBase64)
        {
            byte[] bytes = System.Convert.FromBase64String(stringInBase64);
            return bytes;
        }

        /// <summary>
        /// قص الصورة
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="cropX"></param>
        /// <param name="cropY"></param>
        /// <param name="cropWidth"></param>
        /// <param name="cropHeight"></param>
        /// <returns></returns>
        private Bitmap CropBitmap(ref Bitmap bmp, int cropX, int cropY, int cropWidth, int cropHeight)
        {
            var rect = new Rectangle(cropX, cropY, cropWidth, cropHeight);
            Bitmap cropped = bmp.Clone(rect, bmp.PixelFormat);
            return cropped;
        }

        /// <summary>
        /// تغيير حجم الصورة
        /// </summary>
        /// <param name="bmSource"></param>
        /// <param name="TargetWidth"></param>
        /// <param name="TargetHeight"></param>
        /// <returns></returns>
        private Bitmap ResizeImage(Bitmap bmSource, int TargetWidth, int TargetHeight)
        {
            var bmDest = new Bitmap(TargetWidth, TargetHeight, PixelFormat.Format32bppArgb);
            var nSourceAspectRatio = bmSource.Width / bmSource.Height;
            var nDestAspectRatio = bmDest.Width / bmDest.Height;
            int NewX = 0;
            int NewY = 0;
            var NewWidth = bmDest.Width;
            var NewHeight = bmDest.Height;
            if (nDestAspectRatio == nSourceAspectRatio)
            {
            }
            // same ratio
            else if (nDestAspectRatio > nSourceAspectRatio)
            {
                // Source is taller
                NewWidth = Convert.ToInt32(Math.Floor((double)(nSourceAspectRatio * NewHeight)));
                NewX = Convert.ToInt32(Math.Floor((double)((bmDest.Width - NewWidth) / 2)));
            }
            else
            {
                // Source is wider
                NewHeight = Convert.ToInt32(Math.Floor((double)(1 / nSourceAspectRatio * NewWidth)));
                NewY = Convert.ToInt32(Math.Floor((double)((bmDest.Height - NewHeight) / 2)));
            }

            using (var grDest = Graphics.FromImage(bmDest))
            {
                grDest.CompositingQuality = CompositingQuality.HighQuality;
                grDest.InterpolationMode = InterpolationMode.HighQualityBicubic;
                grDest.PixelOffsetMode = PixelOffsetMode.HighQuality;
                grDest.SmoothingMode = SmoothingMode.AntiAlias;
                grDest.CompositingMode = CompositingMode.SourceOver;
                grDest.DrawImage(bmSource, NewX, NewY, NewWidth, NewHeight);
            }

            return bmDest;
        }

        ///// <summary>
        ///// اضافة اطار حول الصورة
        ///// </summary>
        ///// <param name="original"></param>
        ///// <param name="BorderColor"></param>
        ///// <param name="borderWidth"></param>
        ///// <returns></returns>
        //private Image AppendBorder(Image original, Color BorderColor, int borderWidth)
        //{
        //    var mypen = new Pen(BorderColor, borderWidth * 2);
        //    var newSize = new Size(original.Width + borderWidth * 2, original.Height + borderWidth * 2);
        //    var img = new Bitmap(newSize.Width, newSize.Height);
        //    Graphics g = Graphics.FromImage(img);
        //    // g.Clear(borderColor)   
        //    g.DrawImage(original, new Point(borderWidth, borderWidth));
        //    g.DrawRectangle(mypen, 0, 0, newSize.Width, newSize.Height);
        //    g.Dispose();
        //    return img;
        //}


    }
}