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
using System.Windows.Threading;

namespace _4loopsAppWpf {

    public class ReadWrite {

        DispatcherTimer dispatcher;

        TextBox text, textbox, textBox2;
        CheckBox checkBox, checkBox1, checkBox2, checkBox3, button;
        Button but;
        Label label2, label3;
        TextBlock textBlock1, textBlock2;
        
        private bool canWrite = false;

        public bool WriteToFile {
            get {
                return canWrite;
            }
            set {
                canWrite = value;
            }
        }

        int lineCount = 0;

        public ReadWrite(TextBox text, TextBox textBox, TextBox textBox2, CheckBox checkBox, CheckBox checkBox1,
                        CheckBox checkBox2, CheckBox checkBox3, Button button, TextBlock textblock1, TextBlock textblock2) {
            this.text = text;
            this.textbox = textBox;
            this.textBox2 = textBox2;
            this.checkBox = checkBox;
            this.checkBox1 = checkBox1;
            this.checkBox2 = checkBox2;
            this.checkBox3 = checkBox3;
            this.but = button;
            this.textBlock1 = textblock1;
            this.textBlock2 = textblock2;
        }

        public string FilePath { get; set; }

        public string FileXmlPath { get; set; }

        public string FileJsonPath { get; set; }

        public void Write(string write, string text, string looptimes) {

            StringBuilder build = new StringBuilder();

            for(int i = 0; i < Int32.Parse(looptimes); i++) {
                build.Append(text);
            }

            if(File.Exists(FilePath)) {
                MessageBoxResult result = MessageBox.Show("Tiedosto " + FilePath + " on olemassa. Ylikirjoitetaanko?", "Kirjoita tiedostoon pointcollege", MessageBoxButton.YesNo);

                if(result == MessageBoxResult.Yes) {
                    File.WriteAllText(FilePath, build.ToString());
                } else {
                    return;
                }

            } else {
                File.WriteAllText(FilePath, build.ToString());
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
        public void WriteXML(HashSet<string> randString, string looptimes) {
            XDocument doc = new XDocument();

            //create root element
            XElement xRoot = new XElement("root");
            doc.Add(xRoot);

            //loop hashset strings and add each string in new element
            for(int i = 0; i < Int32.Parse(looptimes); i++) {
                foreach(var str in randString) {
                    XElement ele = new XElement("string", str);
                    xRoot.Add(ele);
                }
            }
            

            if(File.Exists(FileXmlPath)) {
                MessageBoxResult result = MessageBox.Show("Tiedosto " + FileXmlPath + " on olemassa. Ylikirjoitetaanko?", "Kirjoita tiedostoon pointcollege", MessageBoxButton.YesNo);

                if(result == MessageBoxResult.Yes) {
                    //save xml file
                    doc.Save(FileXmlPath);
                } else {
                    return;
                }

            } else {
                doc.Save(FileXmlPath);
            }
        }

        public void WriteJson(HashSet<string> randString, string looptimes) {
 
            JArray array = new JArray();

            //add values to array beforehand
            for(int i = 0; i < Int32.Parse(looptimes); i++) {
                foreach(var str in randString) {
                    array.Add(str);
                }
            }

            //Read values to JsonOject before asking save
            JObject obj = new JObject();
            obj["stringarray"] = array;

            if(File.Exists(FileJsonPath)) {
                MessageBoxResult result = MessageBox.Show("Tiedosto " + FileJsonPath + " on olemassa. Ylikirjoitetaanko?", "Kirjoita tiedostoon pointcollege", MessageBoxButton.YesNo);

                if(result == MessageBoxResult.Yes) {
                    File.WriteAllText(FileJsonPath, obj.ToString());
                } else {
                    return;
                }
            } else {
                File.WriteAllText(FileJsonPath, obj.ToString());
            }
            
        }

        //reads from json
        public void ReadJson() {

            try {
                
                StreamReader reader = new StreamReader(FileJsonPath);
                string json = reader.ReadToEnd();

                if(json[0] == '[') {
                    dynamic array = JsonConvert.DeserializeObject(json);

                    foreach(var item in array) {
                        lineCount = lineCount + 1;
                        text.Text = (string)item.ToString();
                    }
                } else {
                    JObject obj2 = JObject.Parse(json);
                    text.Text = (string)obj2.ToString();                    
                }
                

            } catch (FileNotFoundException e) {
                MessageBox.Show("Tiedostoa ei löytynyt " + FileJsonPath);
            }
            
        }
        internal void StartTimerForUpdateFilesCount() {
            dispatcher = new DispatcherTimer();
            dispatcher.Interval = TimeSpan.FromSeconds(1);
            dispatcher.Tick += UpdateFilesCount;
            dispatcher.Start();
        }

        private void UpdateFilesCount(object sender, EventArgs e) {
            textBlock1.Text = lineCount.ToString();
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
                JsonTextReader read = new JsonTextReader(reader);
                string json = reader.ReadToEnd();

                if(json[0] == '[') {

                    //run this if first char is '[' meaning it's array of objects
                    
                    read.SupportMultipleContent = true;

                    JArray obj = JArray.Parse(File.ReadAllText(filename));
                    text.Text = (string)obj.ToString();
                } else {
                    JObject obj2 = JObject.Parse(File.ReadAllText(filename));
                    text.Text = (string)obj2.ToString();
                }

                
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
