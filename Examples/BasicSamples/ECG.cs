using Keras;
using Keras.Layers;
using Keras.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

using Numpy;

namespace BasicSamples
{
    public class ECG
    {

        public static void Run()
        {
            Console.Out.WriteLine(Directory.GetCurrentDirectory());
            Console.Out.WriteLine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

            var loaded_model = Model.LoadModel("../../../SimpleModel.h5");

            string path = @"..\..\..\000e236aac0438c056cdbae3e0ea29c5.bin";
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            int[,] input = new int[1, 12 * 500 * 15];

            using (BinaryReader br = new BinaryReader(fs))
            using (StreamWriter writer = new StreamWriter(@".\..\..\..\000e236aac0438c056cdbae3e0ea29c5.csv"))
            {
                int pos = 0;

                int length = (int)br.BaseStream.Length;
                Console.WriteLine("File length is {0}", length);

                while (pos < length / 2)
                {

                    int num = br.ReadInt16();
                    //  Console.Write("{0}, ", num);
                    input[pos/ (12 * 500 * 15), pos% (12 * 500 * 15)] = num;
                    
                    writer.Write(num);
                    writer.Write(",");

                    pos++;

                    if (pos % 12 == 0)
                    {
                        writer.WriteLine();
                    }

                    //   Console.Write("{0}, ", pos);
                }
            }

            NDarray input2 = np.array(input).reshape(-1, 12, 500, 15);

            input2.tofile(@".\..\..\..\1.csv", ",", "%s");

            var ret = loaded_model.Predict(input2);

            Console.WriteLine(ret);
        }
    }
}
