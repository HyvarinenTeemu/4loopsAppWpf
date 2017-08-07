using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.VisualBasic;
using System.Windows.Threading;

namespace _4loopsAppWpf {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        ReadWrite readWrite;
        string filename;

        public MainWindow() {
            InitializeComponent();
            
            readWrite = new ReadWrite(textBox1, textBox, textBox2, checkBox, checkBox1, checkBox2, checkBox3, button, textBlock1, textBlock2);
            readWrite.FilePath = @"C:\Temp\writetext.txt";
            readWrite.FileXmlPath = @"c:\Temp\writexml.xml";
            readWrite.FileJsonPath = @"c:\Temp\writejson.json";
        }

        private void button_Click(object sender, RoutedEventArgs e) {

            //read txt file created
            if(!checkBox.IsEnabled && !checkBox2.IsEnabled && !checkBox3.IsEnabled) {
                readWrite.Read();

            //read xml file created
            } else if(!checkBox.IsEnabled && !checkBox1.IsEnabled && !checkBox3.IsEnabled) {
                readWrite.ReadXML();

            //rad json created
            } else if(!checkBox.IsEnabled && !checkBox1.IsEnabled && !checkBox2.IsEnabled) {
                readWrite.StartTimerForUpdateFilesCount();
                readWrite.ReadJson();

            //generates text from given string
            //if first checkbox is checked this will also write to file
            } else {
            
                //at first delete any content before starting new one
                if(textBox1.Text != "") {
                    textBox1.Text = "";

                }

                String givenString = textBox.Text;

                //get unique char as new string for while loop usage
                String uniqCharStr = new string(givenString.Distinct().ToArray());

                StringBuilder builder = new StringBuilder();
                builder.Append(givenString);

                //get unique string as number
                int uniqCharCount = uniqCharStr.Length;

                Random rand = new Random();
                HashSet<string> randString = new HashSet<string>();

                //on each unique char loop all strings on given string
                //ie. if "testi" will be 5 chars so it will be 4*3*2*1
                while(uniqCharCount > 0) {
                    for(int i = 0; i < builder.Length; i++) {
                        int index1 = (rand.Next() % builder.Length);
                        int index2 = (rand.Next() % builder.Length);

                        Char temp = builder[index1];
                        builder[index1] = builder[index2];
                        builder[index2] = temp;

                        //add new string to SET ( set is holding only unique strings so it will not output same string twice
                        randString.Add(builder.ToString());
                    }

                    uniqCharCount--;
                }

                //initialize linecount for textbox so new line will be outputted after 4 strings
                int linecount = 0;

                //initialize unique string count for GUI
                int totalCount = 0;

                do {
                    //loop trough set with enchanced for loop and output to textbox
                    foreach(string uniqueRandString in randString) {
                        textBox1.AppendText(uniqueRandString + " | ");
                        totalCount += 1;
                        linecount++;

                        if(linecount == 5) {
                            textBox1.AppendText(Environment.NewLine);
                            linecount = 0;
                        }

                    }

                } while(randString.Count < totalCount);

                //add number how many unique strings comes from given output
                textBlock.Text = totalCount.ToString();

                //check if writing to files to disk is true then run this block
                if(readWrite.WriteToFile == true) {
                    string looptimes = Interaction.InputBox("Kuinka monta kertaa kirjoitetaan?", "Tiedostoon kirjoitus", "1");

                    //write output to txt file
                    readWrite.Write(readWrite.FilePath, textBox1.Text, looptimes);

                    //create and write xml from output
                    readWrite.WriteXML(randString, looptimes);

                    //write json
                    readWrite.WriteJson(randString, looptimes);
                }
            }
        }
          
        private void textBox_GotFocus(object sender, RoutedEventArgs e) {
            if(textBox.Text != "") {
                textBox.Text = "";
            }
        }

        private void textBox_LostFocus(object sender, RoutedEventArgs e) {

            if(textBox.Text == "") {
                textBox.Text = "Anna sana";
            }
        }

        //if checkbox is checked then write to file
        private void checkBox_Checked(object sender, RoutedEventArgs e) {
            checkBox1.IsEnabled = false;
            checkBox2.IsEnabled = false;
            checkBox3.IsEnabled = false;
            button1.IsEnabled = false;
            button2.IsEnabled = false;
            readWrite.WriteToFile = true;
        }

        //if not checked then just ignore writing
        private void checkBox_Unchecked(object sender, RoutedEventArgs e) {
            checkBox1.IsEnabled = true;
            checkBox2.IsEnabled = true;
            checkBox3.IsEnabled = true;
            button1.IsEnabled = true;
            button2.IsEnabled = true;
            readWrite.WriteToFile = false;
        }

        private void checkBox1_Checked(object sender, RoutedEventArgs e) {
            checkBox.IsEnabled = false;
            checkBox2.IsEnabled = false;
            textBox.IsEnabled = false;
            checkBox3.IsEnabled = false;
            button1.IsEnabled = false;
            button2.IsEnabled = false;
            textBox1.Text = "";
        }

        private void checkBox1_Unchecked(object sender, RoutedEventArgs e) {
            checkBox.IsEnabled = true;
            checkBox2.IsEnabled = true;
            checkBox3.IsEnabled = true;
            button1.IsEnabled = true;
            button2.IsEnabled = true;
            textBox.IsEnabled = true;
        }

        private void checkBox2_Checked(object sender, RoutedEventArgs e) {
            checkBox.IsEnabled = false;
            checkBox1.IsEnabled = false;
            checkBox3.IsEnabled = false;
            button1.IsEnabled = false;
            button2.IsEnabled = false;
            textBox.IsEnabled = false;
        }

        private void checkBox2_Unchecked(object sender, RoutedEventArgs e) {
            checkBox.IsEnabled = true;
            checkBox1.IsEnabled = true;
            checkBox3.IsEnabled = true;
            button1.IsEnabled = true;
            button2.IsEnabled = true;
            textBox.IsEnabled = true;
        }

        private void checkBox3_Checked(object sender, RoutedEventArgs e) {
            checkBox.IsEnabled = false;
            checkBox1.IsEnabled = false;
            checkBox2.IsEnabled = false;
            button1.IsEnabled = false;
            button2.IsEnabled = false;
            textBox.IsEnabled = false;
        }

        private void checkBox3_Unchecked(object sender, RoutedEventArgs e) {
            checkBox.IsEnabled = true;
            checkBox1.IsEnabled = true;
            checkBox2.IsEnabled = true;
            button1.IsEnabled = true;
            button2.IsEnabled = true;
            textBox.IsEnabled = true;
        }

        //button for opening file system to search files
        private void button1_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.DefaultExt = ".txt";
            dialog.Filter = "txt file (*.txt) |*.txt| XML files (*.xml)|*.xml| Json Files (*.json)|*.json";

            Nullable<bool> result = dialog.ShowDialog();

            //if correct extension is selected file name will be added to textbox
            //and all other components will be disabled for that time
            if(result == true) {
                filename = dialog.FileName;
                textBox2.Text = filename;

                button.IsEnabled = false;
                textBox.IsEnabled = false;
                textBox1.IsEnabled = false;
                checkBox.IsEnabled = false;
                checkBox1.IsEnabled = false;
                checkBox2.IsEnabled = false;
                checkBox3.IsEnabled = false;
                
            }
        }

        //send selected file to readfromlocation method
        private void button2_Click(object sender, RoutedEventArgs e) {
            readWrite.StartTimerForUpdateFilesCount();
            readWrite.ReadFromLocation(filename);
        }
    }
}
