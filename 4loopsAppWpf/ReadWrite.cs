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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace _4loopsAppWpf {
    public class ReadWrite {

        TextBox text, textbox, textBox2;
        CheckBox checkBox, checkBox1, checkBox2, checkBox3, button;
        Button but;

        private bool canWrite = false;

        public bool WriteToFile {
            get {
                return canWrite;
            }
            set {
                canWrite = value;
            }
        }

        public ReadWrite(TextBox text, TextBox textBox, TextBox textBox2, CheckBox checkBox, CheckBox checkBox1,
                        CheckBox checkBox2, CheckBox checkBox3, Button button) {
            this.text = text;
            this.textbox = textBox;
            this.textBox2 = textBox2;
            this.checkBox = checkBox;
            this.checkBox1 = checkBox1;
            this.checkBox2 = checkBox2;
            this.checkBox3 = checkBox3;
            this.but = button;
        }

        public string FilePath { get; set; }

        public string FileXmlPath { get; set; }

        public string FileJsonPath { get; set; }

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
            try {
                text.Text = File.ReadAllText(FilePath);
            } catch (FileNotFoundException e) {
                MessageBox.Show("Tiedosto ei löytynyt: " + FilePath);
            }
            
        }

        //parse xml data to stringbuilder and then output text
        public void ReadXML() {

            try {
                StringBuilder build = new StringBuilder();
                XmlTextReader reader = new XmlTextReader(FileXmlPath);

                while(reader.Read()) {
                    switch(reader.NodeType) {
                        case XmlNodeType.Element:
                            build.Append(reader.Value + "\n");
                            break;
                        case XmlNodeType.Text:
                            build.Append(reader.Value);
                            break;
                    }
                }

                text.Text = build.ToString();
            } catch (FileNotFoundException ex) {
                MessageBox.Show("Tiedosto ei löytynyt: " + FileXmlPath);
            }
            
        }

        //creates and writes xml file from given string
        public void WriteXML(HashSet<string> randString) {

            if(canWrite == true) {

                XDocument doc = new XDocument();

                //create root element
                XElement xRoot = new XElement("root");
                doc.Add(xRoot);

                //loop hashset strings and add each string in new element
                foreach(var str in randString) {
                    XElement ele = new XElement("string", str);
                    xRoot.Add(ele);
                }

                if(File.Exists(FileXmlPath)) {
                    MessageBoxResult result = MessageBox.Show("Tiedosto " + FileXmlPath + " on olemassa. Ylikirjoitetaanko?", "Kirjoita tiedostoon pointcollege", MessageBoxButton.YesNo);

                    if(result == MessageBoxResult.Yes) {
                        //create new xml file


                        //save xml file
                        doc.Save(FileXmlPath);
                    } else {
                        return;
                    }
                   
                } else {
                    doc.Save(FileXmlPath);
                }

            } 
        }

        public void WriteJson(HashSet<string> randString) {
           
            JArray array = new JArray();

            foreach(var str in randString) {
                array.Add(str);
            }

            JObject obj = new JObject();
            obj["stringarray"] = array;

            File.WriteAllText(FileJsonPath, obj.ToString());
        }


        //reads from json
        public void ReadJson() {

            try {
                StreamReader reader = new StreamReader(FileJsonPath);

                string json = reader.ReadToEnd();
                dynamic array = JsonConvert.DeserializeObject(json);

                foreach(var item in array) {
                    text.Text = (string)item.ToString();
                }
            } catch (FileNotFoundException e) {
                MessageBox.Show("Tiedostoa ei löytynyt " + FileJsonPath);
            }
            
        }

        public void ReadFromLocation(string filename) {
            string extension = Path.GetExtension(filename);

            if(extension == ".txt") {
                text.Text = File.ReadAllText(filename);

            } else if(extension == ".xml") {
                StringBuilder build = new StringBuilder();
                XmlTextReader reader = new XmlTextReader(filename);

                while(reader.Read()) {
                    switch(reader.NodeType) {
                        case XmlNodeType.Element:
                            build.Append(reader.Value + "\n");
                            break;
                        case XmlNodeType.Text:
                            build.Append(reader.Value);
                            break;
                    }
                }

                text.Text = build.ToString();

            } else if(extension == ".json") {

                StreamReader reader = File.OpenText(filename);

                //read from file
                JsonTextReader read = new JsonTextReader(reader);
                read.SupportMultipleContent = true;
                JObject obj = (JObject)JToken.ReadFrom(read);
                text.Text = (string)obj.ToString();
                Console.WriteLine(obj.ToString());
            }

            but.IsEnabled = true;
            text.IsEnabled = true;
            textbox.IsEnabled = true;
            textBox2.IsEnabled = true;
            checkBox.IsEnabled = true;
            checkBox1.IsEnabled = true;
            checkBox2.IsEnabled = true;
            checkBox3.IsEnabled = true;
            
        }
    }
}
