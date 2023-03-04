using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace LiveSplit.UI.Components
{
    [Flags]
    public enum SplitterEvent
    {
        Kidney_Pickup = 1,
        Last_Golf_Hole = 2,
        Gravy_Race_Finish = 4,
        Pill_Swallow = 8
    }

    public class LevelTransition
    {
        public void Enable()
        {
            Enabled = true;
        }

        public void Disable()
        {
            Enabled = false;
        }

        public void SetState(bool enabled)
        {
            Enabled = enabled;
        }

        public string Name; 
        public bool Enabled;
        public int From, To;

        public LevelTransition(string name, int from, int to, bool enabled = false)
        {
            Name = name;
            From = from;
            To = to;
            Enabled = enabled;
        }

        public bool Valid(int CurrentLevel, int PreviousLevel)
        {
            if (From < 0)
                return CurrentLevel == To;

            return CurrentLevel == To && PreviousLevel == From && Enabled;
        }
    }

    public partial class ComponentSettings : UserControl
    {
        public bool ASEnabled => AutoSplitterCB.Checked;
        public SplitterEvent SplitOptions = SplitterEvent.Pill_Swallow;

        public List<LevelTransition> LevelTransitions = new List<LevelTransition>()
        {
            new LevelTransition("Darlington Station", 1,2),
            new LevelTransition("Soviet Consulate", 2, 3),
            new LevelTransition("Ikayaki Alley", 5,6),
            new LevelTransition("The Temple",6,7),
            new LevelTransition("Kai Tak Day", 8,27),
            new LevelTransition("Kat Tak Night", 10, 9),
            new LevelTransition("The Wetworks", 9,12),
            new LevelTransition("Darlington Station 2",12, 13),
            new LevelTransition("Bachelor Pad", 13, 16)
        };

        public bool CanSplit(SplitterEvent e) => (SplitOptions & e) > 0;
        public bool CanSplit(int n) => ((int)SplitOptions & n) > 0;

        public ComponentSettings()
        {
            InitializeComponent();
        }

        private void thirdSplit_CheckedChanged(object sender, EventArgs e)
        {

        }

        public void ApplySettings(XmlNode node)
        {
            SplitOptions = (SplitterEvent)SettingsHelper.ParseInt( node["SettingsFlags"] );
            AutoSplitterCB.Checked = SettingsHelper.ParseBool( node["EnableAutoSplitter"] );
            foreach (var trans in LevelTransitions)
                trans.Disable();

            foreach(var entry in SettingsHelper.ParseString(node["LevelTransitionsSplits"], "").Split('|'))
                LevelTransitions.Find(x=>x.Name == entry).Enable();
        }

        public XmlNode GetSettings(XmlDocument Document)
        {
            var jp = Document.CreateElement("Jazzpunk");

            SettingsHelper.CreateSetting(Document,jp,"SettingsFlags",(int)SplitOptions);
            SettingsHelper.CreateSetting(Document,jp,"LevelTransitionsSplits", string.Join("|", LevelTransitions.Where(x=>x.Enabled).Select(x=>x.Name)));
            SettingsHelper.CreateSetting(Document,jp,"EnableAutoSplitter",ASEnabled);

            return jp;
        }

        private void CheckChange(object sender, EventArgs e)
        {
            var box = sender as CheckBox;
            if (box == null) return;

            SplitterEvent ret;
            if(Enum.TryParse(box.Text.Replace(' ', '_'), out ret))
            {
                SplitOptions &= ~ret;
                if(box.Checked)
                    SplitOptions |= ret;
            }
        }

        private void LevelCheckChanged(object sender, EventArgs e)
        {
            var box = sender as CheckBox;
            if (box == null) return;

            LevelTransitions.Find(x => x.Name == box.Text).SetState(box.Checked);
        }

        private void ComponentSettings_Load(object sender, EventArgs e)
        {
            int y = 0, m = 25;
            int width = AutoSplitGroupBox.Width - 15;

            foreach (var transition in LevelTransitions)
            {
                CheckBox cb = new CheckBox();
                cb.Location = new Point(5, y += m);
                cb.Text = transition.Name;
                cb.Width = width;
                cb.Checked = transition.Enabled;
                cb.CheckedChanged += LevelCheckChanged;

                LevelSplitsBox.Controls.Add(cb);
            }
            LevelSplitsBox.Height = y+m;
            AutoSplitGroupBox.Top = LevelSplitsBox.Bottom + m;

            y = 0; m = 25;

            string[] Names = Enum.GetNames(typeof(SplitterEvent));
            for(int i = 0; i < Names.Length; i++)
            {
                CheckBox cb = new CheckBox()
                {
                    Text = Names[i].Replace('_', ' '),
                    Location = new Point(5, y += m),
                    Width = width,
                    Checked = CanSplit(1 << i)
                };

                cb.CheckedChanged += CheckChange;
                AutoSplitGroupBox.Controls.Add(cb);
            }

            AutoSplitGroupBox.Height = y+m;
            Height = AutoSplitGroupBox.Bottom + m;
        }
    }
}
