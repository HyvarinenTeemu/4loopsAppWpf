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

        //creates and writes xml file from given string
        public void WriteXML(HashSet<string> randString) {

            //create new xml file
            XDocument doc = new XDocument();

            //create root element
            XElement xRoot = new XElement("root");
            doc.Add(xRoot);

            //loop hashset strings and add each string in new element
            foreach(var str in randString) {
                XElement ele = new XElement("string",
                               new XElement("newstring", str));
                xRoot.Add(ele);
            }

            //save xml file
            doc.Save(FileXmlPath);
        }

    }
}
