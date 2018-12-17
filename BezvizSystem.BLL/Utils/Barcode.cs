using BarcodeLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.Utils
{
    public class Barcode
    {

        public virtual string GetText(int code)
        {
            string result = "BRESTVISAFREE  ";

            string codeInStr = code.ToString();

            if (codeInStr.Length < 6)
                codeInStr = codeInStr.PadLeft(6, '0');

            return result + codeInStr;
        }


        public Image GetImage(int code)
        {
            BarcodeLib.Barcode barcode = new BarcodeLib.Barcode()
            {
                IncludeLabel = true,
                Alignment = AlignmentPositions.LEFT,
                LabelPosition = LabelPositions.BOTTOMLEFT,
                Width = 400,
                Height = 100,
                RotateFlipType = RotateFlipType.Rotate270FlipNone,
                BackColor = Color.White,
                ForeColor = Color.Black,
            };

            var img = barcode.Encode(TYPE.CODE39Extended, GetText(code));           

            return img;
        }
    }

    public class BarcodeChild : Barcode
    {
        public override string GetText(int code)
        {
            return "Hello";
        }
    }
}
