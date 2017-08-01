using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Controls;
using System.Windows;
using System.Xml.Linq;
using System.Xml;

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

        public string FileXmlPath { get; set; }

        public void Write(string write, string text) {
            
            if(canWrite == true) {
                if(File.Exists(FilePath)) {
                    MessageBoxResult result = MessageBox.Show("Tiedosto " + FilePath + " on olemassa. Ylikirjoitetaanko?", "Kirjoita tiedostoon pointcollege",  MessageBoxButton.YesNo);

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

        public void ReadXML() {
            XDocument doc = XDocument.Load(FileXmlPath);
            IEnumerable<XElement> wholeStory = doc.Elements();

            foreach(var allElements in wholeStory) {
                text.Text = allElements.ToString();
            }
        }

    }
}
