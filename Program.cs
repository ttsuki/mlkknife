using System;
using System.IO;
using System.Reflection;
[assembly: AssemblyVersion("0.1.*")]
[assembly: AssemblyFileVersion("0.1.0.0")]

namespace mlkknife
{
   class Program
    {
        static void Main(string[] args)
        {
            Console.Error.WriteLine("mlkknife version 0.1");

            if (args.Length == 0)
            {
                Console.Error.WriteLine("usage: mlkknife mlkfile [mlkfile...]");
                Environment.Exit(1);
            }

            foreach (var s in args)
            {
                Console.Error.WriteLine("Input: " + s);
                try
                {
                    var srcFile = s;
                    var data = File.ReadAllBytes(srcFile);

                    using (var ms = new MemoryStream(data, false))
                    using (var br = new BinaryReader(ms))
                    {

                        int count = br.ReadByte();
                        Console.Error.WriteLine("  Count: " + count);

                        string dstPath = Path.GetDirectoryName(srcFile) + Path.DirectorySeparatorChar;
                        Console.Error.WriteLine("  Output: " + dstPath);
                        for (int i = 0; i < count; i++)
                        {
                            byte flag = br.ReadByte();
                            int pos = br.ReadInt32();
                            int len = br.ReadInt32();
                            string dstFileName = Path.GetFileNameWithoutExtension(srcFile) + "." + i.ToString("000") + ".mid";
                            Console.Error.WriteLine("  File#" + i + ": flag=" + flag + ", pos=" + pos + ", len=" + len + " => " + dstFileName);

                            try
                            {
                                using (FileStream wf = File.Create(dstPath + dstFileName))
                                {
                                    wf.Write(data, pos, len);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine("  " + ex.Message);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("  " + ex.Message);
                }
            }
        }
    }
}

