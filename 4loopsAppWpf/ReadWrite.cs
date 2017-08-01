using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace _4loopsAppWpf {
    public class ReadWrite {

        private bool canWrite = false;

        public bool WriteToFile {
            get {
                return canWrite;
            }
            set {
                canWrite = true;
            }
        }

        public string FilePath { get; set; }

        public void Write(string write, string text) {
            if(WriteToFile == true) {
                File.WriteAllText(FilePath, text);
            }
        }
    }
}
