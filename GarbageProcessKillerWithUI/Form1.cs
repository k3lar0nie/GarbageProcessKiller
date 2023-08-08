using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace GarbageProcessKillerWithUI
{
    public partial class Form1 : Form
    {
        static private Process[] processes;
        static private List<string> processesNames = new List<string>();


        public Form1()
        {

            InitializeComponent();

            listBox1.BackColor = Color.WhiteSmoke;
            listBox2.BackColor = Color.WhiteSmoke;
            if (File.Exists("killList.txt"))
            {
                foreach (var x in File.ReadAllLines("killList.txt"))
                {
                    listBox2.Items.Add(x);
                }
            }
            if (File.Exists("killListAuto.txt")) 
            {
                foreach(var x in File.ReadAllLines("killListAuto.txt"))
                {
                    listBox3.Items.Add(x);
                }
            }


            UpdateMain();
            

            // хз как оно работает, но оно работает (it just works)
            buttonUpdate.Location = new Point(listBox1.Size.Width + 17 - 3, buttonUpdate.Location.Y);
            buttonUpdate.Size = new Size(this.Width - listBox1.Size.Width - 42 + 2, buttonUpdate.Size.Height);

            buttonAdd.Location = new Point(listBox1.Size.Width + 17 - 3, buttonAdd.Location.Y);
            buttonAdd.Size = new Size(this.Width - listBox1.Size.Width - 42 + 2, buttonAdd.Size.Height);


            listBox2.Location = new Point(listBox1.Size.Width + 18 - 3, listBox2.Location.Y);
            listBox2.Size = new Size((this.Width - listBox1.Size.Width - 42 - 5) / 2, listBox2.Size.Height);

            listBox3.Location = new Point(listBox2.Size.Width + 18 - 3 + listBox1.Size.Width + 5, listBox3.Location.Y);
            listBox3.Size = new Size((this.Width - listBox1.Size.Width - 42 - 5) / 2 + 1, listBox3.Size.Height);


            textBox1.Location = new Point(listBox1.Size.Width + 18 - 3, textBox1.Location.Y);
            textBox1.Size = new Size(this.Width - listBox1.Size.Width - 42, textBox1.Size.Height);

            buttonSave.Location = new Point(listBox1.Size.Width + 17 - 3, buttonSave.Location.Y);
            buttonSave.Size = new Size(this.Width - listBox1.Size.Width - 42 + 2, buttonSave.Size.Height);

            buttonAddAuto.Location = new Point(listBox1.Size.Width + 17 - 3, buttonAddAuto.Location.Y);
            buttonAddAuto.Size = new Size(this.Width - listBox1.Size.Width - 42 + 2, buttonAddAuto.Size.Height);
        }
        private void UpdateMain()
        {

            processesNames.Clear();
            processes = Process.GetProcesses();

            for (int i = 0; i < processes.Length; i++)
            {
                processesNames.Add(processes[i].ProcessName);
            }

            processesNames.Sort();

            listBox1.Items.Clear();
            listBox1.Items.AddRange(removeDublicates(processesNames).ToArray());
            listBox1.Size = new Size(getLongestStringInList(processesNames).Length * 6, listBox1.Size.Height);
        }


        static string getLongestStringInList(List<string> list)
        {
            string longest = list[0];
            foreach (string s in list)
            {
                if (s.Length > longest.Length) { longest = s; }
            }
            return longest;
        }

        static List<string> removeDublicates(List<string> list)
        {
            List<String> newList = new List<String>();

            for (int i = 0; i < list.Count; i++)
            {
                if (!newList.Contains(list[i]))
                {
                    newList.Add(list[i]);
                }
            }

            return newList;
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            UpdateMain();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void buttonAdd_Click_1(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0 && textBox1.Text.Length == 0) { return; }

            if (!listBox2.Items.Contains(textBox1.Text) && textBox1.Text.Length != 0)
            {
                listBox2.Items.Add(textBox1.Text);
                textBox1.Clear();
            }
            else if (!listBox2.Items.Contains(listBox1.SelectedItem))
            {
                listBox2.Items.Add(listBox1.SelectedItem.ToString());
            }
        }
        private void buttonAddAuto_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0 && textBox1.Text.Length == 0) { return; }

            if (!listBox3.Items.Contains(textBox1.Text) && textBox1.Text.Length != 0)
            {
                listBox3.Items.Add(textBox1.Text);
                textBox1.Clear();
            }
            else if (!listBox3.Items.Contains(listBox1.SelectedItem))
            {
                listBox3.Items.Add(listBox1.SelectedItem.ToString());
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox2.SelectedIndex < 0)
            {
                return;
            }
            listBox2.Items.RemoveAt(listBox2.SelectedIndex);
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        static void Save(ListBox listbox)
        {
            if (!File.Exists("killList.txt"))
            {
                File.Create("killList.txt").Close();
            }
    
            using(StreamWriter sWriter = new StreamWriter("killList.txt", false))
            {
                sWriter.Write(string.Empty);
                for (int i = 0; i < listbox.Items.Count; i++)
                {
                    sWriter.WriteLine(listbox.Items[i]);
                }
            }
        }
        static void SaveAuto(ListBox listbox)
        {
            if (!File.Exists("killListAuto.txt"))
            {
                File.Create("killListAuto.txt").Close();
            }

            using (StreamWriter sWriter = new StreamWriter("killListAuto.txt", false))
            {
                sWriter.Write(string.Empty);
                for (int i = 0; i < listbox.Items.Count; i++)
                {
                    sWriter.WriteLine(listbox.Items[i]);
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Save(listBox2);
            SaveAuto(listBox3);
        }
        
        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex < 0)
            {
                return;
            }
            listBox3.Items.RemoveAt(listBox3.SelectedIndex);
        }
    }
}
