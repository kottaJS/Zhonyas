using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace LoLFirewallManager.Services
{
    public static class AssetLoader
    {
        public static string ResolveAssetPath(string fileName, string subFolder)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string[] candidates = new string[]
            {
                Path.Combine(baseDir, "assets", subFolder, fileName),
                Path.Combine(baseDir, "assets", fileName),
                Path.Combine(baseDir, fileName),
                Path.GetFullPath(Path.Combine("assets", subFolder, fileName)),
                Path.GetFullPath(Path.Combine("assets", fileName)),
                Path.GetFullPath(fileName)
            };

            foreach (string p in candidates)
            {
                if (File.Exists(p)) return p;
            }
            return null;
        }

        public static Icon LoadAppIcon()
        {
            try
            {
                string iconPath = ResolveAssetPath("zhonyas.ico", "icons");
                if (string.IsNullOrEmpty(iconPath))
                    iconPath = ResolveAssetPath("3157-_1_.ico", "icons");

                if (!string.IsNullOrEmpty(iconPath) && File.Exists(iconPath))
                {
                    return new Icon(iconPath);
                }

                var asm = System.Reflection.Assembly.GetExecutingAssembly();
                using (Stream stream = asm.GetManifestResourceStream("zhonyas.ico"))
                {
                    if (stream != null)
                    {
                        return new Icon(stream);
                    }
                }

                return Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            }
            catch
            {
                return null;
            }
        }

        public static Bitmap[] LoadLuluFrames()
        {
            try
            {
                string gifPath = ResolveAssetPath("luluzhonya.gif", "images");
                if (!string.IsNullOrEmpty(gifPath) && File.Exists(gifPath))
                {
                    using (Image img = Image.FromFile(gifPath))
                    {
                        return ParseFrames(img);
                    }
                }

                var asm = System.Reflection.Assembly.GetExecutingAssembly();
                using (Stream stream = asm.GetManifestResourceStream("luluzhonya.gif"))
                {
                    if (stream != null)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            stream.CopyTo(ms);
                            ms.Position = 0;
                            using (Image img = Image.FromStream(ms))
                            {
                                return ParseFrames(img);
                            }
                        }
                    }
                }
            }
            catch { }
            return new Bitmap[0];
        }

        private static Bitmap[] ParseFrames(Image img)
        {
            FrameDimension dim = new FrameDimension(img.FrameDimensionsList[0]);
            int count = img.GetFrameCount(dim);
            Bitmap[] frames = new Bitmap[count];
            for (int i = 0; i < count; i++)
            {
                img.SelectActiveFrame(dim, i);
                frames[i] = new Bitmap(img);
            }
            return frames;
        }
    }
}
