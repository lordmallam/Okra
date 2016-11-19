using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Okra.UI;
using System.Drawing;
namespace Okra.File
{
    public class File
    {
        public File()
        {

        }

        /// <summary>
        /// Checks if supplied file extention exists in the supplied array of extentions
        /// </summary>
        /// <param name="filepath">The filename of the file</param>
        /// <param name="validExtenstions">Array of extenstions to check against (in lowercase)</param>
        /// <returns>True if exists and false if not</returns>
        public bool IsFileExtentionValid(String filepath, String[] validExtenstions)
        {
           
            String[] ext = filepath.Split('.');
            if(ext.Length<2){
                return false;
            }

            for (int i = 0; i < validExtenstions.Length; i++)
			{
                if (ext[ext.Length - 1].ToLower() == validExtenstions[i])
                return true;
			}

        
        return false;
    
        }
        public byte[] PostedImageToByte(Stream input, int size)
        {

            UI.UI nui = new UI.UI();
            MemoryStream ms = new MemoryStream();
            return nui.ResizeImageByWidthToArray(size, new Bitmap(input));
        }
    }
}
