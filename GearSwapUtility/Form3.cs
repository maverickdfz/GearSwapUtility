using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GearSwapUtility
{
    public partial class Form3 : Form
    {
        public class ComboBoxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public ComboBoxItem(string text, object value)
            {
                Text = text;
                Value = value;
            }

            public override string ToString()
            {
                return Text;
            }
        }

        public Form3()
        {
            InitializeComponent();
        }

        public void SetItem(int item_id)
        {
            string iconsDir = Properties.Settings.Default.IconsDirectory + @"\";
            string path = iconsDir + item_id + ".png";
            if (System.IO.File.Exists(path))
            {
                Bitmap bitmap = new Bitmap(path);

                pictureBox1.Image = bitmap;
            }

            int index = 0;
            for(int i = 0;i < comboBox1.Items.Count;i++)
            {
                if ((int)((ComboBoxItem)comboBox1.Items[i]).Value == item_id)
                {
                    index = i;
                    break;
                }
            }
            comboBox1.SelectedIndex = index;
        }

        public void SetItems(List<ComboBoxItem> items)
        {
            comboBox1.Items.Clear();
            //for(int i = 0;i < items.Length;i++)
            foreach(ComboBoxItem item in items)
            {
                //ComboBoxItem item = items[i];
                comboBox1.Items.Add(item);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        public int GetItemId()
        {
            if(comboBox1.SelectedItem != null)
                return (int)((ComboBoxItem)comboBox1.SelectedItem).Value;

            return -1;
        }

        public string GetItemName()
        {
            if (comboBox1.SelectedItem != null)
                return (string)((ComboBoxItem)comboBox1.SelectedItem).Text;

            return "";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string iconsDir = Properties.Settings.Default.IconsDirectory + @"\";

            int item_id = GetItemId();
            string path = iconsDir + item_id + ".png";
            if (System.IO.File.Exists(path))
            {
                Bitmap bitmap = new Bitmap(path);

                pictureBox1.Image = bitmap;
            }
        }

        private void comboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.Close();
            }
        }
    }
}
