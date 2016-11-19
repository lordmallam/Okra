using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Drawing;
using System.IO;

namespace Okra.UI
{
    public class UI
    {
        public UI()
        {

        }

        /// <summary>
        /// Displays dialog box with message. Based on JQueryUI
        /// </summary>
        /// <param name="message">Message to display</param>
        /// <param name="page">Containing page</param>
        public void MessageAlertJQueryUI(string message, System.Web.UI.Page page)
        {
            string strScript = "";
            String dia = "<div>" + message + "</div>";
            strScript = "var $dialog = $('" + dia + "');";

            strScript += "$dialog.dialog({buttons: { 'Ok': function () { $(this).dialog('close'); } },modal: true});";
            Guid guidKey = Guid.NewGuid();
            ClientScriptManager cs = page.ClientScript;
            //cs.RegisterClientScriptBlock(page.GetType(), guidKey.ToString(), strScript);
            cs.RegisterStartupScript(page.GetType(),guidKey.ToString(),strScript,true);
            
        }
        public void DialogJQueryUIUpdatePanel(string message, UpdatePanel up )
        {
            string strScript = "";
            String dia = "<div>" + message + "</div>";
            strScript = "var $dialog = $('" + dia + "');";

            strScript += "$dialog.dialog({modal: true});";
            Guid guidKey = Guid.NewGuid();
            ScriptManager.RegisterClientScriptBlock(up, up.GetType(), guidKey.ToString(), strScript, true);
            //cs.RegisterStartupScript(page, page.GetType(), guidKey.ToString(), strScript,true);

        }

        public void MessageBoxjQueryUpdatePanel(string message, UpdatePanel up)
        {
            string strScript = "";
            String dia = "<div>" + message + "</div>";
            strScript = "var $dialog = $('" + dia + "');";

            strScript += "$dialog.dialog({buttons: { 'Ok': function () { $(this).dialog('close'); } },modal: true});";
            Guid guidKey = Guid.NewGuid();
            ScriptManager.RegisterClientScriptBlock(up, up.GetType(), guidKey.ToString(), strScript, true);
            //cs.RegisterStartupScript(page, page.GetType(), guidKey.ToString(), strScript,true);

        }

        public Bitmap ResizeImageByWidth(int newWidth, Bitmap bm)
        {
            int oldwidth = bm.Width;
            int oldheight = bm.Height;
            float ratio=0;
            if (oldheight!=oldwidth)
            {
                ratio = (float)oldwidth / (float)oldheight;
            }
            else
            {
                ratio = 1;
            }

            int newhieght = (int)(newWidth / ratio);
            

            Bitmap resized = new Bitmap(newWidth, newhieght);
            using (Graphics gr = Graphics.FromImage(resized))
            {
                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                gr.DrawImage(bm, new Rectangle(0, 0, newWidth, newhieght));
            }
            return resized;
        }

        public byte[] ResizeImageByWidthToArray(int newWidth, Bitmap bm)
        {
            MemoryStream ms = new MemoryStream();
            ResizeImageByWidth(newWidth, new Bitmap(bm)).Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();

        }
 
        public Bitmap ResizeImageByHieght(int newhieght, Bitmap bm)
        {
            int oldwidth = bm.Width;
            int oldheight = bm.Height;
            float ratio = 0;
            if (oldheight != oldwidth)
            {
                ratio = (float)oldheight / (float)oldwidth;
            }
            else
            {
                ratio = 1;
            }

            int newWidth = (int)(newhieght / ratio);


            Bitmap resized = new Bitmap(newWidth, newhieght);
            using (Graphics gr = Graphics.FromImage(resized))
            {
                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                gr.DrawImage(bm, new Rectangle(0, 0, newWidth, newhieght));
            }
            return resized;
        }

        public byte[] ResizeImageByHeightToArray(int newhieght, Bitmap bm)
        {
            MemoryStream ms = new MemoryStream();
            ResizeImageByHieght(newhieght, new Bitmap(bm)).Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();

        }

        public Bitmap GetThumbnail120x120(Bitmap bm)
        {
            Bitmap newImage = new Bitmap(120, 120);
            using(Graphics gr = Graphics .FromImage(newImage)){
                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                gr.DrawImage(bm, new Rectangle(0, 0, 120, 120));
            }
            
            
            return newImage;
        }

        public Bitmap GetThumbnail(Bitmap bm, int size)
        {
            Bitmap newImage = new Bitmap(size, size);
            using (Graphics gr = Graphics.FromImage(newImage))
            {
                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                gr.DrawImage(bm, new Rectangle(0, 0, size, size));
            }
            
            return newImage;
        }

        public byte[] GetThumbnailToArray(Bitmap bm, int size)
        {
            MemoryStream ms = new MemoryStream();
            GetThumbnail(bm, size).Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }

    }
}
