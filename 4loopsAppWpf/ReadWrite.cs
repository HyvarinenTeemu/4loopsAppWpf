using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Controls;
using System.Windows;

namespace _4loopsAppWpf {
    public class ReadWrite {

        TextBox text;

        private bool canWrite = false;

        public bool WriteToFile {
            get {
                return canWrite;
            }
            set {
                canWrite = value;
            }
        }

        public ReadWrite(TextBox text) {
            this.text = text;
        }

        public string FilePath { get; set; }

        public void Write(string write, string text) {
            
            if(canWrite == true) {
                if(File.Exists(FilePath)) {
                    MessageBoxResult result = MessageBox.Show("Kirjoita tiedostoon pointcollege", "Tiedosto " + FilePath + " on olemassa. Ylikirjoitetaanko?", MessageBoxButton.YesNo);

                    if(result == MessageBoxResult.Yes) {
                        File.WriteAllText(FilePath, text);
                    } else {
                        return;
                    }

                } else {
                    File.WriteAllText(FilePath, text);
                }
            }
        }

        public void Read() {
            text.Text = File.ReadAllText(FilePath);
        }

    }
}
