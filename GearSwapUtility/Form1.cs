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
    public partial class Form1 : Form
    {
        private ContextMenu contextMenu1;

        private TreeNode CurrentNode = null;
        private TreeNode node_under_mouse = null;

        private Dictionary<TreeNode, Dictionary<SlotType, KeyValuePair<int, string>>> Sets;

        public Form1()
        {
            InitializeComponent();

            ClearSets();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //pictureBox1.BackColor = Color.DarkRed;
            CurrentNode = e.Node;
            SelectSet(CurrentNode);
        }

        private Dictionary<SlotType, KeyValuePair<int, string>> GetBlankSet()
        {
            Dictionary<SlotType, KeyValuePair<int, string>> BlankSet = new Dictionary<SlotType, KeyValuePair<int, string>>();
            BlankSet.Add(SlotType.Main, new KeyValuePair<int, string>(-1, ""));
            BlankSet.Add(SlotType.Sub, new KeyValuePair<int, string>(-1, ""));
            BlankSet.Add(SlotType.Range, new KeyValuePair<int, string>(-1, ""));
            BlankSet.Add(SlotType.Ammo, new KeyValuePair<int, string>(-1, ""));
            BlankSet.Add(SlotType.Head, new KeyValuePair<int, string>(-1, ""));
            BlankSet.Add(SlotType.Neck, new KeyValuePair<int, string>(-1, ""));
            BlankSet.Add(SlotType.LEar, new KeyValuePair<int, string>(-1, ""));
            BlankSet.Add(SlotType.REar, new KeyValuePair<int, string>(-1, ""));
            BlankSet.Add(SlotType.Body, new KeyValuePair<int, string>(-1, ""));
            BlankSet.Add(SlotType.Hands, new KeyValuePair<int, string>(-1, ""));
            BlankSet.Add(SlotType.LRing, new KeyValuePair<int, string>(-1, ""));
            BlankSet.Add(SlotType.RRing, new KeyValuePair<int, string>(-1, ""));
            BlankSet.Add(SlotType.Back, new KeyValuePair<int, string>(-1, ""));
            BlankSet.Add(SlotType.Waist, new KeyValuePair<int, string>(-1, ""));
            BlankSet.Add(SlotType.Legs, new KeyValuePair<int, string>(-1, ""));
            BlankSet.Add(SlotType.Feet, new KeyValuePair<int, string>(-1, ""));
            return BlankSet;
        }

        private void SelectSet(TreeNode Node)
        {
            if (!Sets.ContainsKey(Node))
            {
                Sets.Add(Node, GetBlankSet());
            }
            SetupPictureBoxes(Node);
        }

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                TreeNode Node = treeView1.GetNodeAt(e.X, e.Y);
                if (Node == null)
                    DeselectSet();
                else
                {
                    DeselectSet();
                    CurrentNode = Node;
                    SelectSet(CurrentNode);
                }
            }

            if (e.Button != MouseButtons.Right) return;

            if (contextMenu1 == null) {
                contextMenu1 = new ContextMenu();
            }
            contextMenu1.MenuItems.Clear();

            node_under_mouse = treeView1.GetNodeAt(e.X, e.Y);

            // Add Node
            MenuItem menuItem1 = new MenuItem("Add &Node", MenuItem_AddNode_Click);
            contextMenu1.MenuItems.Add(menuItem1);
            
            if (node_under_mouse != null)
            {
                // Add Child Node
                MenuItem menuItem2 = new MenuItem("Add &Child Node", MenuItem_AddChildNode_Click);
                contextMenu1.MenuItems.Add(menuItem2);

                // Clear Set
                MenuItem menuItem3 = new MenuItem("&Clear Set", MenuItem_ClearSet_Click);
                contextMenu1.MenuItems.Add(menuItem3);

                // Delete Node
                MenuItem menuItem4 = new MenuItem("&Delete Node", MenuItem_DeleteNode_Click);
                contextMenu1.MenuItems.Add(menuItem4);
            }

            contextMenu1.Show(treeView1, new Point(e.X, e.Y));
        }

        private void MenuItem_AddNode_Click(Object sender, System.EventArgs e)
        {
            Form2 form = new Form2();
            form.ShowDialog(this);

            string text = form.GetText();

            if (text.Length > 0)
            {
                if (!treeView1.Nodes.ContainsKey(text))
                {
                    treeView1.Nodes.Add(text, text);
                }
            }
        }

        private void MenuItem_AddChildNode_Click(Object sender, System.EventArgs e)
        {
            Form2 form = new Form2();
            form.ShowDialog(this);

            int index = -1;
            if(node_under_mouse.Parent != null)
            {
                index = node_under_mouse.Parent.Nodes.IndexOf(node_under_mouse); ;
            }
            else
            {
                index = treeView1.Nodes.IndexOf(node_under_mouse); ;
            }

            string text = form.GetText();
            if (text.Length > 0)
            {
                if (node_under_mouse.Parent != null)
                {
                    if (!node_under_mouse.Parent.Nodes[index].Nodes.ContainsKey(text))
                    {
                        node_under_mouse.Parent.Nodes[index].Nodes.Add(text, text);
                    }
                }
                else
                {
                    if (!treeView1.Nodes[index].Nodes.ContainsKey(text))
                    {
                        treeView1.Nodes[index].Nodes.Add(text, text);
                    }
                }
            }
        }

        private void ClearSet(TreeNode Node)
        {
            Sets[Node] = GetBlankSet();
        }

        private void ClearSets()
        {
            Sets = new Dictionary<TreeNode, Dictionary<SlotType, KeyValuePair<int, string>>>();
        }

        private void MenuItem_ClearSet_Click(Object sender, System.EventArgs e)
        {
            ClearSet(node_under_mouse);

            if(node_under_mouse == CurrentNode)
            {
                ClearPictureBoxes();
            }
        }

        private void DeleteSet(TreeNode Node)
        {
            if(Sets.ContainsKey(Node))
            {
                Sets.Remove(Node);
            }
        }

        private void MenuItem_DeleteNode_Click(Object sender, System.EventArgs e)
        {
            foreach(TreeNode treeNode in node_under_mouse.Nodes)
            {
                DeleteSet(treeNode);
            }
            DeleteSet(node_under_mouse);

            if (node_under_mouse == CurrentNode)
            {
                ClearPictureBoxes();
            }

            node_under_mouse.Remove();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            ClearPictureBoxes();
        }

        enum SlotType
        {
            Main = 0,
            Sub = 1,
            Range = 2,
            Ammo = 3,
            Head = 4,
            Body = 5,
            Hands = 6,
            Legs = 7,
            Feet = 8,
            Neck = 9,
            Waist = 10,
            LEar = 11,
            REar = 12,
            LRing = 13,
            RRing = 14,
            Back = 15
        }

        private List<Form3.ComboBoxItem> GetItems(SlotType slot)
        {
            string resDir = GetResDir();
            List<Form3.ComboBoxItem> items = new List<Form3.ComboBoxItem>();
            string[] lines = System.IO.File.ReadAllLines(resDir + "items.lua");
            foreach (string line in lines)
            {
                //System.Text.RegularExpressions.Match(line, pattern)
                int index = line.IndexOf(",category=\"");
                if (index == -1)
                    continue;
                int start = index + 11;
                int end = line.IndexOf("\"", start);
                string category = line.Substring(start, end - start);
                if (category.Equals("Weapon") || category.Equals("Armor"))
                {
                    int id_index = line.IndexOf("[");
                    int id_start = id_index + 1;
                    int id_end = line.IndexOf("]", id_start);
                    string id_string = line.Substring(id_start, id_end - id_start);
                    int id = Int32.Parse(id_string);

                    int en_index = line.IndexOf(",en=\"");
                    int en_start = en_index + 5;
                    int en_end = line.IndexOf("\"", en_start);
                    string en = line.Substring(en_start, en_end - en_start);

                    int slots_index = line.IndexOf(",slots=");
                    int slots_start = slots_index + 7;
                    int slots_end = line.IndexOf(",", slots_start);
                    string slots_string = line.Substring(slots_start, slots_end - slots_start);
                    int slots = Int32.Parse(slots_string);

                    int shift = 1 << (int)SlotType.Main;
                    int result = slots & shift;

                    if ((slots & (1 << (int)slot)) != 0)
                    {
                        items.Add(new Form3.ComboBoxItem(en, id));
                    }
                }
            }
            return items;
        }

        private string GetResDir()
        {
            return Properties.Settings.Default.WindowerDirectory + @"\res\";
        }

        private string GetIconsDir()
        {
            return Properties.Settings.Default.IconsDirectory + @"\";
        }

        private void SelectItem(SlotType Slot, PictureBox pictureBox)
        {
            string resDir = GetResDir();

            if(!System.IO.Directory.Exists(resDir))
            {
                MessageBox.Show($"Your Windower path could not be found\n\nCurrent value: {resDir}");
                return;
            }

            string iconsDir = GetIconsDir();

            if (!System.IO.Directory.Exists(iconsDir))
            {
                MessageBox.Show($"Your icon path could not be found\n\nCurrent value: {iconsDir}");
                return;
            }

            Form3 form = new Form3();
            
            List<Form3.ComboBoxItem> items = GetItems(Slot);

            form.SetItems(items);
            if (Sets[CurrentNode][Slot].Key >= 0)
            {
                form.SetItem(Sets[CurrentNode][Slot].Key);
            }
            form.ShowDialog(this);

            int item_id = form.GetItemId();
            string item_name = form.GetItemName();

            if (item_id >= 0)
            {
                Sets[CurrentNode][Slot] = new KeyValuePair<int, string>(item_id, item_name);
                
                string path = iconsDir + item_id + ".png";
                if (System.IO.File.Exists(path))
                {
                    Bitmap bitmap = new Bitmap(path);

                    pictureBox.Image = bitmap;
                }
            }
            else
            {
                Sets[CurrentNode][Slot] = new KeyValuePair<int, string>(-1, "");

                if (pictureBox.Image != null)
                {
                    pictureBox.Image.Dispose();
                    pictureBox.Image = null;
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (CurrentNode == null) return;

            SelectItem(SlotType.Main, pictureBox1);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (CurrentNode == null) return;

            SelectItem(SlotType.Sub, pictureBox2);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (CurrentNode == null) return;

            SelectItem(SlotType.Range, pictureBox3);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (CurrentNode == null) return;

            SelectItem(SlotType.Ammo, pictureBox4);
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if (CurrentNode == null) return;

            SelectItem(SlotType.Head, pictureBox5);
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            if (CurrentNode == null) return;

            SelectItem(SlotType.Neck, pictureBox6);
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            if (CurrentNode == null) return;

            SelectItem(SlotType.LEar, pictureBox7);
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            if (CurrentNode == null) return;

            SelectItem(SlotType.REar, pictureBox8);
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            if (CurrentNode == null) return;

            SelectItem(SlotType.Body, pictureBox9);
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            if (CurrentNode == null) return;

            SelectItem(SlotType.Hands, pictureBox10);
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            if (CurrentNode == null) return;

            SelectItem(SlotType.LRing, pictureBox11);
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            if (CurrentNode == null) return;

            SelectItem(SlotType.RRing, pictureBox12);
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            if (CurrentNode == null) return;

            SelectItem(SlotType.Back, pictureBox13);
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            if (CurrentNode == null) return;

            SelectItem(SlotType.Waist, pictureBox14);
        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {
            if (CurrentNode == null) return;

            SelectItem(SlotType.Legs, pictureBox15);
        }

        private void pictureBox16_Click(object sender, EventArgs e)
        {
            if (CurrentNode == null) return;

            SelectItem(SlotType.Feet, pictureBox16);
        }

        private void ClearPictureBoxes()
        {
            List<PictureBox> pictureBoxes = new List<PictureBox>
            {
                pictureBox1,
                pictureBox2,
                pictureBox3,
                pictureBox4,
                pictureBox5,
                pictureBox6,
                pictureBox7,
                pictureBox8,
                pictureBox9,
                pictureBox10,
                pictureBox11,
                pictureBox12,
                pictureBox13,
                pictureBox14,
                pictureBox15,
                pictureBox16
            };
            foreach (PictureBox pictureBox in pictureBoxes)
            {
                if (pictureBox.Image != null)
                    pictureBox.Image.Dispose();
                pictureBox.Image = null;
            }
        }

        private void SetupPictureBoxes(TreeNode Node)
        {
            Dictionary<SlotType, PictureBox> pictureBoxes = new Dictionary<SlotType, PictureBox>
            {
                {SlotType.Main, pictureBox1},
                {SlotType.Sub, pictureBox2},
                {SlotType.Range, pictureBox3},
                {SlotType.Ammo, pictureBox4},
                {SlotType.Head, pictureBox5},
                {SlotType.Neck, pictureBox6},
                {SlotType.LEar, pictureBox7},
                {SlotType.REar, pictureBox8},
                {SlotType.Body, pictureBox9},
                {SlotType.Hands, pictureBox10},
                {SlotType.LRing, pictureBox11},
                {SlotType.RRing, pictureBox12},
                {SlotType.Back, pictureBox13},
                {SlotType.Waist, pictureBox14},
                {SlotType.Legs, pictureBox15},
                {SlotType.Feet, pictureBox16}
            };

            foreach(KeyValuePair<SlotType, PictureBox> Pair in pictureBoxes)
            {
                if(Sets[Node][Pair.Key].Key >= 0)
                {
                    int item_id = Sets[Node][Pair.Key].Key;

                    string iconsDir = GetIconsDir();
                    string path = iconsDir + item_id + ".png";
                    if (System.IO.File.Exists(path))
                    {
                        Bitmap bitmap = new Bitmap(path);

                        Pair.Value.Image = bitmap;
                    }
                }
            }
        }

        private void DeselectSet()
        {
            CurrentNode = null;
            ClearPictureBoxes();
        }

        private string GetNameForSlot(SlotType Slot)
        {
            Dictionary<SlotType, string> slotNames = new Dictionary<SlotType, string>
            {
                {SlotType.Main, "main"},
                {SlotType.Sub, "sub"},
                {SlotType.Range, "range"},
                {SlotType.Ammo, "ammo"},
                {SlotType.Head, "head"},
                {SlotType.Neck, "neck"},
                {SlotType.LEar, "ear1"},
                {SlotType.REar, "ear2"},
                {SlotType.Body, "body"},
                {SlotType.Hands, "hands"},
                {SlotType.LRing, "ring1"},
                {SlotType.RRing, "ring2"},
                {SlotType.Back, "back"},
                {SlotType.Waist, "waist"},
                {SlotType.Legs, "legs"},
                {SlotType.Feet, "feet"}
            };
            if(slotNames.ContainsKey(Slot))
            {
                return slotNames[Slot];
            }
            return "unknown";
        }

        private SlotType GetSlotForName(string Slot)
        {
            Dictionary<string, SlotType> slotTypes = new Dictionary<string, SlotType>
            {
                {"main", SlotType.Main},
                {"sub", SlotType.Sub},
                {"range", SlotType.Range},
                {"ammo", SlotType.Ammo},
                {"head", SlotType.Head},
                {"neck", SlotType.Neck},
                {"ear1", SlotType.LEar},
                {"ear2", SlotType.REar},
                {"body", SlotType.Body},
                {"hands", SlotType.Hands},
                {"ring1", SlotType.LRing},
                {"ring2", SlotType.RRing},
                {"back", SlotType.Back},
                {"waist", SlotType.Waist},
                {"legs", SlotType.Legs},
                {"feet", SlotType.Feet}
            };
            if (slotTypes.ContainsKey(Slot))
            {
                return slotTypes[Slot];
            }
            return SlotType.Main;
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "lua|*.lua";
            saveFileDialog1.Title = "Export set";
            saveFileDialog1.DefaultExt = "lua";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (saveFileDialog1.FileName != "")
                {
                    List<string> lines = new List<string>();

                    lines.Add("local setsUtil = {}");
                    lines.Add("");
                    lines.Add("setsUtil.apply = function(sets)");
                    foreach (KeyValuePair<TreeNode, Dictionary<SlotType, KeyValuePair<int, string>>> Pair in Sets)
                    {
                        bool empty = true;
                        foreach (KeyValuePair<SlotType, KeyValuePair<int, string>> Set in Pair.Value)
                        {
                            if (Set.Value.Key >= 0)
                            {
                                empty = false;
                                break;
                            }
                        }
                        if (empty) continue;
                        TreeNode treeNode = Pair.Key;
                        string set_name = treeNode.Name.Contains(" ") ? "['" + treeNode.Name + "']" : treeNode.Name;
                        TreeNode parent = treeNode.Parent;
                        while(parent != null)
                        {
                            string join = set_name.StartsWith("['") ? "" : ".";
                            set_name = (parent.Name.Contains(" ") ? "['" + parent.Name + "']" : parent.Name) + join + set_name;
                            parent = parent.Parent;
                        }
                        string outer_join = set_name.StartsWith("['") ? "" : ".";
                        lines.Add("sets" + outer_join + set_name + " = {");
                        bool added = false;
                        foreach (KeyValuePair<SlotType, KeyValuePair<int, string>> Set in Pair.Value)
                        {
                            if (Set.Value.Key >= 0)
                            {
                                if (added) lines[lines.Count - 1] += ",";
                                string slot_name = GetNameForSlot(Set.Key);
                                string item_name = Set.Value.Value;
                                lines.Add(slot_name + " = \"" + item_name + "\"");
                                added = true;
                            }
                        }
                        lines.Add("}");
                    }
                    lines.Add("return sets");
                    lines.Add("end");
                    lines.Add("");
                    lines.Add("return setsUtil");

                    /*System.IO.StreamWriter writer = new System.IO.StreamWriter(saveFileDialog1.FileName);
                    writer.Write(lines);
                    writer.Close();
                    writer.Dispose();*/
                    System.IO.File.WriteAllLines(saveFileDialog1.FileName, lines);
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dictionary<string, Dictionary<string, Dictionary<string, string>>> exportSets = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();

            foreach (KeyValuePair<TreeNode, Dictionary<SlotType, KeyValuePair<int, string>>> Pair in Sets)
            {
                TreeNode treeNode = Pair.Key;
                string set_name = treeNode.Name.Contains(" ") ? "['" + treeNode.Name + "']" : treeNode.Name;
                TreeNode parent = treeNode.Parent;
                while (parent != null)
                {
                    string join = set_name.StartsWith("['") ? "" : ".";
                    set_name = (parent.Name.Contains(" ") ? "['" + parent.Name + "']" : parent.Name) + join + set_name;
                    parent = parent.Parent;
                }

                exportSets.Add(set_name, new Dictionary<string, Dictionary<string, string>>());

                foreach (KeyValuePair<SlotType, KeyValuePair<int, string>> Set in Pair.Value)
                {
                    if (Set.Value.Key >= 0)
                    {
                        string slot_name = GetNameForSlot(Set.Key);
                        int item_id = Set.Value.Key;
                        string item_name = Set.Value.Value;
                        exportSets[set_name][slot_name] = new Dictionary<string, string>();
                        exportSets[set_name][slot_name][""+item_id] = item_name;
                    }
                }
            }

            System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            string json = jss.Serialize(exportSets);
            Console.Write(json);

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "json|*.json";
            saveFileDialog1.Title = "Save";
            saveFileDialog1.DefaultExt = "json";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (saveFileDialog1.FileName != "")
                {
                    try
                    {
                        System.IO.File.WriteAllText(saveFileDialog1.FileName, json);
                    }
                    catch (System.Security.SecurityException ex)
                    {
                        MessageBox.Show($"Security error.\n\nError message: {ex.Message}");
                    }
                }
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.FileName = "Select a file";
            openFileDialog1.Filter = "json|*.json";
            openFileDialog1.Title = "Save";
            openFileDialog1.DefaultExt = "json";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (openFileDialog1.FileName != "")
                {
                    try
                    {
                        string json = System.IO.File.ReadAllText(openFileDialog1.FileName);
                        Console.Write(json);
                        treeView1.Nodes.Clear();
                        Sets.Clear();
                        System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
                        Dictionary<string, Dictionary<string, Dictionary<string, string>>> importSets = jss.Deserialize<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(json);

                        foreach (KeyValuePair<string, Dictionary<string, Dictionary<string, string>>> Pair in importSets)
                        {
                            string name = Pair.Key;
                            TreeNode Node;
                            bool shouldParseAsMulti = false;
                            if (name.Contains("."))
                            {
                                shouldParseAsMulti = true;
                            }
                            if (!shouldParseAsMulti)
                            {
                                if (name.Contains("["))
                                {
                                    if (name.Count(f => f == '[') > 1)
                                    {
                                        shouldParseAsMulti = true;
                                    }
                                    else if(name[0] != '[')
                                    {
                                        shouldParseAsMulti = true;
                                    }
                                }
                            }
                            if (shouldParseAsMulti)
                            {
                                char[] separator = { '.', '[' };
                                string[] parts = name.Split(separator);
                                name = parts[parts.Length - 1];
                                name = name.Replace("'", "");
                                name = name.Replace("]", "");
                                if (parts.Length == 1)
                                {
                                    Node = treeView1.Nodes.Add(name, name);
                                }
                                else
                                {
                                    Array.Resize<string>(ref parts, parts.Length - 1);
                                    TreeNodeCollection current = treeView1.Nodes;
                                    TreeNode parent = current[0];
                                    foreach (string part in parts)
                                    {
                                        string p = part.Replace("['", "");
                                        p = p.Replace("']", "");
                                        int index = current.IndexOfKey(p);
                                        parent = current[index];
                                        current = parent.Nodes;
                                    }
                                    Node = parent.Nodes.Add(name, name);
                                }
                            }
                            else
                            {
                                name = name.Replace("['", "");
                                name = name.Replace("']", "");
                                Node = treeView1.Nodes.Add(name, name);
                            }

                            Sets.Add(Node, GetBlankSet());

                            foreach (KeyValuePair<string, Dictionary<string, string>> Set in Pair.Value)
                            {
                                SlotType Slot = GetSlotForName(Set.Key);
                                foreach (KeyValuePair<string, string> Item in Set.Value)
                                {
                                    Sets[Node][Slot] = new KeyValuePair<int, string>(Int32.Parse(Item.Key), Item.Value);
                                }
                            }
                        }
                    }
                    catch (System.Security.SecurityException ex)
                    {
                        MessageBox.Show($"Security error.\n\nError message: {ex.Message}");
                    }
                }
            }
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form4 form = new Form4();
            form.ShowDialog(this);
        }
    }
}