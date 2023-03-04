using LiveSplit.Model;
using LiveSplit.Options;
using LiveSplit.TimeFormatters;
using LiveSplit.UI.Components;
using LiveSplit.Web;
using LiveSplit.Web.Share;
using LiveSplit.ComponentUtil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using System.IO;

namespace LiveSplit.UI.Components
{
    class Component : IComponent
    {
        public static ComponentSettings settings;
        
        public string ComponentName
        {
            get { return "JazzSplitter"; }
        }

        public float PaddingBottom { get { return 0; } }
        public float PaddingTop { get { return 0; } }
        public float PaddingLeft { get { return 0; } }
        public float PaddingRight { get { return 0; } }

        public string FilePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "Low\\Necrophone Games\\LoadPtr";

        public bool Refresh { get; set; }

        public IDictionary<string, Action> ContextMenuControls { get; protected set; }
        TimerModel _timer;

        public Process Game { get; set; }
        public IntPtr LoadPtr;
        public IntPtr CustomSplitPtr;
        public IntPtr LevelPtr;

        public Component()
        {
            settings = new ComponentSettings();
            JazzLoadRemover.JazzpunkFinder.InstallAssembly();
        }

        int CPS = -1;
        int PreviousLevel = 0;

        int cd = 0;
        public void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {
            if(_timer == null)
            {
                _timer = new TimerModel();
                _timer.CurrentState = state;
            }

            if(cd-- > 0) return;

            if (Game == null || Game.HasExited)
            {
                Game = null;
                string[] split;
                int addr;
                var process = Process.GetProcessesByName("Jazzpunk").FirstOrDefault();

                if (process == null || !File.Exists(FilePath) || (split = File.ReadAllLines(FilePath)).Length < 2 || split[1] != process.Id.ToString() || !Int32.TryParse(split[0], out addr))
                {
                    cd = 20;
                    return;
                }

                Game = process;
                LoadPtr = new IntPtr(addr);
                LevelPtr = new IntPtr(addr + 1);
                CustomSplitPtr = new IntPtr(addr + 5);
            }

            byte Paused;
            if (Game.ReadValue(LoadPtr, out Paused))
                state.IsGameTimePaused = Paused > 0;

            if (!settings.ASEnabled) return;

            int SE;
            if(Game.ReadValue(CustomSplitPtr, out SE) && CPS != SE)
            {
                CPS = SE;
                if (CPS != 0 && settings.CanSplit(SE))
                {
                    if (state.CurrentPhase == TimerPhase.NotRunning)
                        _timer.Start();
                    else
                        _timer.Split();
                }


                Game.WriteBytes(CustomSplitPtr, new byte[4]);
            }

            int CurrentLevel;
            if(Game.ReadValue(LevelPtr, out CurrentLevel) && CurrentLevel != PreviousLevel)
            {
                if (!settings.LevelTransitions.TrueForAll(x => !x.Valid(CurrentLevel, PreviousLevel)))
                    _timer.Split();

                if (CurrentLevel == 1) // Darlington Station
                    _timer.Reset();

                PreviousLevel = CurrentLevel;
            }
        }

        public void DrawHorizontal(Graphics g, LiveSplitState state, float height, Region clipRegion)
        {
        }

        public void DrawVertical(Graphics g, LiveSplitState state, float width, Region clipRegion)
        {
        }

        public float VerticalHeight
        {
            get { return 0; }
        }

        public float MinimumWidth
        {
            get { return 0; }
        }

        public float HorizontalWidth
        {
            get { return 0; }
        }

        public float MinimumHeight
        {
            get { return 0; }
        }

        public System.Xml.XmlNode GetSettings(System.Xml.XmlDocument document)
        {
            return settings.GetSettings(document);
        }

        public System.Windows.Forms.Control GetSettingsControl(UI.LayoutMode mode)
        {
            return settings;
        }

        public void SetSettings(System.Xml.XmlNode node)
        {
            settings.ApplySettings(node);
        }

        public void RenameComparison(string oldName, string newName)
        {
        }

        public void Dispose()
        {
            
        }
    }
}
