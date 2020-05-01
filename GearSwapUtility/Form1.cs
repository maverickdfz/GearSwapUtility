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
        private ToolTip toolTip1;
        private ContextMenu contextMenu1;
        private ContextMenu contextMenu2;

        private TreeNode CurrentNode = null;
        private TreeNode node_under_mouse = null;

        private PictureBox CurrentPictureBox = null;

        private Dictionary<TreeNode, Dictionary<SlotType, Tuple<int, string, string>>> Sets;

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

        private Dictionary<SlotType, Tuple<int, string, string>> GetBlankSet()
        {
            Dictionary<SlotType, Tuple<int, string, string>> BlankSet = new Dictionary<SlotType, Tuple<int, string, string>>();
            BlankSet.Add(SlotType.Main, new Tuple<int, string, string>(-1, "", ""));
            BlankSet.Add(SlotType.Sub, new Tuple<int, string, string>(-1, "", ""));
            BlankSet.Add(SlotType.Range, new Tuple<int, string, string>(-1, "", ""));
            BlankSet.Add(SlotType.Ammo, new Tuple<int, string, string>(-1, "", ""));
            BlankSet.Add(SlotType.Head, new Tuple<int, string, string>(-1, "", ""));
            BlankSet.Add(SlotType.Neck, new Tuple<int, string, string>(-1, "", ""));
            BlankSet.Add(SlotType.LEar, new Tuple<int, string, string>(-1, "", ""));
            BlankSet.Add(SlotType.REar, new Tuple<int, string, string>(-1, "", ""));
            BlankSet.Add(SlotType.Body, new Tuple<int, string, string>(-1, "", ""));
            BlankSet.Add(SlotType.Hands, new Tuple<int, string, string>(-1, "", ""));
            BlankSet.Add(SlotType.LRing, new Tuple<int, string, string>(-1, "", ""));
            BlankSet.Add(SlotType.RRing, new Tuple<int, string, string>(-1, "", ""));
            BlankSet.Add(SlotType.Back, new Tuple<int, string, string>(-1, "", ""));
            BlankSet.Add(SlotType.Waist, new Tuple<int, string, string>(-1, "", ""));
            BlankSet.Add(SlotType.Legs, new Tuple<int, string, string>(-1, "", ""));
            BlankSet.Add(SlotType.Feet, new Tuple<int, string, string>(-1, "", ""));
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
                //if (Node == null)
                //    DeselectSet();
                //else
                if(Node != null)
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
                    TreeNode treeNode = treeView1.Nodes.Add(text, text);
                    SelectSet(treeNode);
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
                        TreeNode treeNode = node_under_mouse.Parent.Nodes[index].Nodes.Add(text, text);
                        SelectSet(treeNode);
                    }
                }
                else
                {
                    if (!treeView1.Nodes[index].Nodes.ContainsKey(text))
                    {
                        TreeNode treeNode = treeView1.Nodes[index].Nodes.Add(text, text);
                        SelectSet(treeNode);
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
            Sets = new Dictionary<TreeNode, Dictionary<SlotType, Tuple<int, string, string>>>();
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
            if (Sets[CurrentNode][Slot].Item1 >= 0)
            {
                form.SetItem(Sets[CurrentNode][Slot].Item1);
            }
            form.ShowDialog(this);

            int item_id = form.GetItemId();
            string item_name = form.GetItemName();

            if (item_id >= 0)
            {
                Sets[CurrentNode][Slot] = new Tuple<int, string, string>(item_id, item_name, "");
                
                string path = iconsDir + item_id + ".png";
                if (System.IO.File.Exists(path))
                {
                    Bitmap bitmap = new Bitmap(path);

                    pictureBox.Image = bitmap;
                }
            }
            else
            {
                Sets[CurrentNode][Slot] = new Tuple<int, string, string>(-1, "", "");

                if (pictureBox.Image != null)
                {
                    pictureBox.Image.Dispose();
                    pictureBox.Image = null;
                }
            }
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
                if(Sets[Node][Pair.Key].Item1 >= 0)
                {
                    int item_id = Sets[Node][Pair.Key].Item1;

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

        SlotType GetSlotTypeForPictureBox(PictureBox pictureBox)
        {
            Dictionary<PictureBox, SlotType> pictureBoxes = new Dictionary<PictureBox, SlotType>
            {
                {pictureBox1, SlotType.Main},
                {pictureBox2, SlotType.Sub},
                {pictureBox3, SlotType.Range},
                {pictureBox4, SlotType.Ammo},
                {pictureBox5, SlotType.Head},
                {pictureBox6, SlotType.Neck},
                {pictureBox7, SlotType.LEar},
                {pictureBox8, SlotType.REar},
                {pictureBox9, SlotType.Body},
                {pictureBox10, SlotType.Hands},
                {pictureBox11, SlotType.LRing},
                {pictureBox12, SlotType.RRing},
                {pictureBox13, SlotType.Back},
                {pictureBox14, SlotType.Waist},
                {pictureBox15, SlotType.Legs},
                {pictureBox16, SlotType.Feet}
            };
            if(pictureBoxes.ContainsKey(pictureBox))
            {
                return pictureBoxes[pictureBox];
            }
            return SlotType.Main;
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
                    foreach (KeyValuePair<TreeNode, Dictionary<SlotType, Tuple<int, string, string>>> Pair in Sets)
                    {
                        bool empty = true;
                        foreach (KeyValuePair<SlotType, Tuple<int, string, string>> Set in Pair.Value)
                        {
                            if (Set.Value.Item1 >= 0)
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
                        foreach (KeyValuePair<SlotType, Tuple<int, string, string>> Set in Pair.Value)
                        {
                            if (Set.Value.Item1 >= 0)
                            {
                                if (added) lines[lines.Count - 1] += ",";
                                string slot_name = GetNameForSlot(Set.Key);
                                string item_name = Set.Value.Item2;
                                if (Set.Value.Item3.Length > 0)
                                {
                                    string augments = Set.Value.Item3;
                                    string item = "{ name = \"" + item_name + "\", augments={" + augments + "}}";
                                    lines.Add(slot_name + " = " + item);
                                }
                                else
                                {
                                    lines.Add(slot_name + " = \"" + item_name + "\"");
                                }
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
            Dictionary<string, Dictionary<string, Tuple<string, string, string>>> exportSets = new Dictionary<string, Dictionary<string, Tuple<string, string, string>>>();

            foreach (KeyValuePair<TreeNode, Dictionary<SlotType, Tuple<int, string, string>>> Pair in Sets)
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

                exportSets.Add(set_name, new Dictionary<string, Tuple<string, string, string>>());

                foreach (KeyValuePair<SlotType, Tuple<int, string, string>> Set in Pair.Value)
                {
                    if (Set.Value.Item1 >= 0)
                    {
                        string slot_name = GetNameForSlot(Set.Key);
                        int item_id = Set.Value.Item1;
                        string item_name = Set.Value.Item2;
                        string item_augments = Set.Value.Item3;
                        exportSets[set_name][slot_name] = new Tuple<string, string, string>("" + item_id, item_name, item_augments);
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

        class ThreeTupleConverter : System.Web.Script.Serialization.JavaScriptConverter
        {
            public override IEnumerable<Type> SupportedTypes
            {
                get { return new List<Type> { typeof(Tuple<string, string, string>) }; }
            }

            public override object Deserialize(IDictionary<string, object> dictionary, Type type, System.Web.Script.Serialization.JavaScriptSerializer serializer)
            {
                string item1 = (string)dictionary["Item1"];
                string item2 = (string)dictionary["Item2"];
                string item3 = (string)dictionary["Item3"];
                return new Tuple<string, string, string>(item1, item2, item3);
            }

            public override IDictionary<string, object> Serialize(object obj, System.Web.Script.Serialization.JavaScriptSerializer serializer)
            {
                throw new NotImplementedException();
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
                        jss.RegisterConverters(new List<System.Web.Script.Serialization.JavaScriptConverter> { new ThreeTupleConverter() });
                        Dictionary<string, Dictionary<string, Tuple<string, string, string>>> importSets = jss.Deserialize<Dictionary<string, Dictionary<string, Tuple<string, string, string>>>>(json);

                        foreach (KeyValuePair<string, Dictionary<string, Tuple<string, string, string>>> Pair in importSets)
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

                            foreach (KeyValuePair<string, Tuple<string, string, string>> Set in Pair.Value)
                            {
                                SlotType Slot = GetSlotForName(Set.Key);
                                Tuple<string, string, string> Item = Set.Value;
                                Sets[Node][Slot] = new Tuple<int, string, string>(Int32.Parse(Item.Item1), Item.Item2, Item.Item3);
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

        private void AddTooltipForPictureBox(PictureBox pictureBox)
        {
            SlotType slotType = GetSlotTypeForPictureBox(pictureBox);
            if (Sets[CurrentNode][slotType].Item1 >= 0)
            {
                string toolTipText;
                if (Sets[CurrentNode][slotType].Item3.Length > 0)
                {
                    string item_name = Sets[CurrentNode][slotType].Item2;
                    string augments = Sets[CurrentNode][slotType].Item3;
                    toolTipText = "{ name = \"" + item_name + "\", augments={" + augments + "}}";
                }
                else
                {
                    toolTipText = Sets[CurrentNode][slotType].Item2;
                }

                if (toolTip1 == null)
                {
                    toolTip1 = new ToolTip();
                }
                else
                {
                    toolTip1.RemoveAll();
                }
                toolTip1.SetToolTip(pictureBox, toolTipText);
            }
        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            if (CurrentNode == null) return;

            AddTooltipForPictureBox(pictureBox1);
        }

        private void pictureBox2_MouseHover(object sender, EventArgs e)
        {
            if (CurrentNode == null) return;

            AddTooltipForPictureBox(pictureBox2);
        }

        private void pictureBox3_MouseHover(object sender, EventArgs e)
        {
            if (CurrentNode == null) return;

            AddTooltipForPictureBox(pictureBox3);
        }

        private void pictureBox4_MouseHover(object sender, EventArgs e)
        {
            if (CurrentNode == null) return;

            AddTooltipForPictureBox(pictureBox4);
        }

        private void pictureBox5_MouseHover(object sender, EventArgs e)
        {
            if (CurrentNode == null) return;

            AddTooltipForPictureBox(pictureBox5);
        }

        private void pictureBox6_MouseHover(object sender, EventArgs e)
        {
            if (CurrentNode == null) return;

            AddTooltipForPictureBox(pictureBox6);
        }

        private void pictureBox7_MouseHover(object sender, EventArgs e)
        {
            if (CurrentNode == null) return;

            AddTooltipForPictureBox(pictureBox7);
        }

        private void pictureBox8_MouseHover(object sender, EventArgs e)
        {
            if (CurrentNode == null) return;

            AddTooltipForPictureBox(pictureBox8);
        }

        private void pictureBox9_MouseHover(object sender, EventArgs e)
        {
            if (CurrentNode == null) return;

            AddTooltipForPictureBox(pictureBox9);
        }

        private void pictureBox10_MouseHover(object sender, EventArgs e)
        {
            if (CurrentNode == null) return;

            AddTooltipForPictureBox(pictureBox10);
        }

        private void pictureBox11_MouseHover(object sender, EventArgs e)
        {
            if (CurrentNode == null) return;

            AddTooltipForPictureBox(pictureBox11);
        }

        private void pictureBox12_MouseHover(object sender, EventArgs e)
        {
            if (CurrentNode == null) return;

            AddTooltipForPictureBox(pictureBox12);
        }

        private void pictureBox13_MouseHover(object sender, EventArgs e)
        {
            if (CurrentNode == null) return;

            AddTooltipForPictureBox(pictureBox13);
        }

        private void pictureBox14_MouseHover(object sender, EventArgs e)
        {
            if (CurrentNode == null) return;

            AddTooltipForPictureBox(pictureBox14);
        }

        private void pictureBox15_MouseHover(object sender, EventArgs e)
        {
            if (CurrentNode == null) return;

            AddTooltipForPictureBox(pictureBox15);
        }

        private void pictureBox16_MouseHover(object sender, EventArgs e)
        {
            if (CurrentNode == null) return;

            AddTooltipForPictureBox(pictureBox16);
        }

        private void ShowPictureBoxContextMenu(PictureBox pictureBox, Point p)
        {
            if (contextMenu2 == null)
            {
                contextMenu2 = new ContextMenu();
            }
            contextMenu2.MenuItems.Clear();

            CurrentPictureBox = pictureBox;

            // Add Node
            MenuItem menuItem1 = new MenuItem("Edit &Augments", MenuItem_EditAugments_Click);
            contextMenu2.MenuItems.Add(menuItem1);

            contextMenu2.Show(pictureBox1, p);
        }

        private string GetAugmentForPictureBox(PictureBox pictureBox)
        {
            SlotType slotType = GetSlotTypeForPictureBox(pictureBox);
            if (Sets[CurrentNode][slotType].Item1 >= 0)
            {
                string item_augment = Sets[CurrentNode][slotType].Item3;

                return item_augment;
            }
            return "";
        }

        private void SetAugmentForPictureBox(PictureBox pictureBox, string text)
        {
            SlotType slotType = GetSlotTypeForPictureBox(pictureBox);
            if (Sets[CurrentNode][slotType].Item1 >= 0)
            {
                int item_id = Sets[CurrentNode][slotType].Item1;
                string item_name = Sets[CurrentNode][slotType].Item2;

                Sets[CurrentNode][slotType] = new Tuple<int, string, string>(item_id, item_name, text);
            }
        }

        private void MenuItem_EditAugments_Click(Object sender, System.EventArgs e)
        {
            Form2 form = new Form2();
            string text = GetAugmentForPictureBox(CurrentPictureBox);
            if (text.Length > 0)
            {
                form.SetText(text);
            }
            form.ShowDialog(this);

            text = form.GetText();

            if (text.Length > 0)
            {
                SetAugmentForPictureBox(CurrentPictureBox, text);
            }
            else
            {
                SetAugmentForPictureBox(CurrentPictureBox, "");
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (CurrentNode == null) return;

            if (e.Button == MouseButtons.Left)
            {
                SlotType slotType = GetSlotTypeForPictureBox(pictureBox1);
                SelectItem(slotType, pictureBox1);
            }

            if (e.Button != MouseButtons.Right) return;

            ShowPictureBoxContextMenu(pictureBox1, new Point(e.X, e.Y));
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (CurrentNode == null) return;

            if (e.Button == MouseButtons.Left)
            {
                SlotType slotType = GetSlotTypeForPictureBox(pictureBox2);
                SelectItem(slotType, pictureBox2);
            }

            if (e.Button != MouseButtons.Right) return;

            ShowPictureBoxContextMenu(pictureBox2, new Point(e.X, e.Y));
        }

        private void pictureBox3_MouseDown(object sender, MouseEventArgs e)
        {
            if (CurrentNode == null) return;

            if (e.Button == MouseButtons.Left)
            {
                SlotType slotType = GetSlotTypeForPictureBox(pictureBox3);
                SelectItem(slotType, pictureBox3);
            }

            if (e.Button != MouseButtons.Right) return;

            ShowPictureBoxContextMenu(pictureBox3, new Point(e.X, e.Y));
        }

        private void pictureBox4_MouseDown(object sender, MouseEventArgs e)
        {
            if (CurrentNode == null) return;

            if (e.Button == MouseButtons.Left)
            {
                SlotType slotType = GetSlotTypeForPictureBox(pictureBox4);
                SelectItem(slotType, pictureBox4);
            }

            if (e.Button != MouseButtons.Right) return;

            ShowPictureBoxContextMenu(pictureBox4, new Point(e.X, e.Y));
        }

        private void pictureBox5_MouseDown(object sender, MouseEventArgs e)
        {
            if (CurrentNode == null) return;

            if (e.Button == MouseButtons.Left)
            {
                SlotType slotType = GetSlotTypeForPictureBox(pictureBox5);
                SelectItem(slotType, pictureBox5);
            }

            if (e.Button != MouseButtons.Right) return;

            ShowPictureBoxContextMenu(pictureBox5, new Point(e.X, e.Y));
        }

        private void pictureBox6_MouseDown(object sender, MouseEventArgs e)
        {
            if (CurrentNode == null) return;

            if (e.Button == MouseButtons.Left)
            {
                SlotType slotType = GetSlotTypeForPictureBox(pictureBox6);
                SelectItem(slotType, pictureBox6);
            }

            if (e.Button != MouseButtons.Right) return;

            ShowPictureBoxContextMenu(pictureBox6, new Point(e.X, e.Y));
        }

        private void pictureBox7_MouseDown(object sender, MouseEventArgs e)
        {
            if (CurrentNode == null) return;

            if (e.Button == MouseButtons.Left)
            {
                SlotType slotType = GetSlotTypeForPictureBox(pictureBox7);
                SelectItem(slotType, pictureBox7);
            }

            if (e.Button != MouseButtons.Right) return;

            ShowPictureBoxContextMenu(pictureBox7, new Point(e.X, e.Y));
        }

        private void pictureBox8_MouseDown(object sender, MouseEventArgs e)
        {
            if (CurrentNode == null) return;

            if (e.Button == MouseButtons.Left)
            {
                SlotType slotType = GetSlotTypeForPictureBox(pictureBox8);
                SelectItem(slotType, pictureBox8);
            }

            if (e.Button != MouseButtons.Right) return;

            ShowPictureBoxContextMenu(pictureBox8, new Point(e.X, e.Y));
        }

        private void pictureBox9_MouseDown(object sender, MouseEventArgs e)
        {
            if (CurrentNode == null) return;

            if (e.Button == MouseButtons.Left)
            {
                SlotType slotType = GetSlotTypeForPictureBox(pictureBox9);
                SelectItem(slotType, pictureBox9);
            }

            if (e.Button != MouseButtons.Right) return;

            ShowPictureBoxContextMenu(pictureBox9, new Point(e.X, e.Y));
        }

        private void pictureBox10_MouseDown(object sender, MouseEventArgs e)
        {
            if (CurrentNode == null) return;

            if (e.Button == MouseButtons.Left)
            {
                SlotType slotType = GetSlotTypeForPictureBox(pictureBox10);
                SelectItem(slotType, pictureBox10);
            }

            if (e.Button != MouseButtons.Right) return;

            ShowPictureBoxContextMenu(pictureBox10, new Point(e.X, e.Y));
        }

        private void pictureBox11_MouseDown(object sender, MouseEventArgs e)
        {
            if (CurrentNode == null) return;

            if (e.Button == MouseButtons.Left)
            {
                SlotType slotType = GetSlotTypeForPictureBox(pictureBox11);
                SelectItem(slotType, pictureBox11);
            }

            if (e.Button != MouseButtons.Right) return;

            ShowPictureBoxContextMenu(pictureBox11, new Point(e.X, e.Y));
        }

        private void pictureBox12_MouseDown(object sender, MouseEventArgs e)
        {
            if (CurrentNode == null) return;

            if (e.Button == MouseButtons.Left)
            {
                SlotType slotType = GetSlotTypeForPictureBox(pictureBox12);
                SelectItem(slotType, pictureBox12);
            }

            if (e.Button != MouseButtons.Right) return;

            ShowPictureBoxContextMenu(pictureBox12, new Point(e.X, e.Y));
        }

        private void pictureBox13_MouseDown(object sender, MouseEventArgs e)
        {
            if (CurrentNode == null) return;

            if (e.Button == MouseButtons.Left)
            {
                SlotType slotType = GetSlotTypeForPictureBox(pictureBox13);
                SelectItem(slotType, pictureBox13);
            }

            if (e.Button != MouseButtons.Right) return;

            ShowPictureBoxContextMenu(pictureBox13, new Point(e.X, e.Y));
        }

        private void pictureBox14_MouseDown(object sender, MouseEventArgs e)
        {
            if (CurrentNode == null) return;

            if (e.Button == MouseButtons.Left)
            {
                SlotType slotType = GetSlotTypeForPictureBox(pictureBox14);
                SelectItem(slotType, pictureBox14);
            }

            if (e.Button != MouseButtons.Right) return;

            ShowPictureBoxContextMenu(pictureBox14, new Point(e.X, e.Y));
        }

        private void pictureBox15_MouseDown(object sender, MouseEventArgs e)
        {
            if (CurrentNode == null) return;

            if (e.Button == MouseButtons.Left)
            {
                SlotType slotType = GetSlotTypeForPictureBox(pictureBox15);
                SelectItem(slotType, pictureBox15);
            }

            if (e.Button != MouseButtons.Right) return;

            ShowPictureBoxContextMenu(pictureBox15, new Point(e.X, e.Y));
        }

        private void pictureBox16_MouseDown(object sender, MouseEventArgs e)
        {
            if (CurrentNode == null) return;

            if (e.Button == MouseButtons.Left)
            {
                SlotType slotType = GetSlotTypeForPictureBox(pictureBox16);
                SelectItem(slotType, pictureBox16);
            }

            if (e.Button != MouseButtons.Right) return;

            ShowPictureBoxContextMenu(pictureBox16, new Point(e.X, e.Y));
        }
    }
}