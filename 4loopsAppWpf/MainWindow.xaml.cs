﻿using System;
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

namespace _4loopsAppWpf {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        ReadWrite readWrite = new ReadWrite();
        
        public MainWindow() {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e) {

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
            readWrite.Write(readWrite.FilePath, textBox1.Text);
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

        private void checkBox_Checked(object sender, RoutedEventArgs e) {
            if(checkBox.IsChecked == true) {
                readWrite.WriteToFile = true;
                readWrite.FilePath = @"C:\Temp\write.txt";
            }
        }
    }
}
