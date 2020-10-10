using LiveSplit.Model;
using LiveSplit.UI.Components;
using System;
using LiveSplit.Crash4LoadRemover;

[assembly: ComponentFactory(typeof(Crash4LoadRemoverFactory))]

namespace LiveSplit.Crash4LoadRemover
{
    public class Crash4LoadRemoverFactory : IComponentFactory
    {
        public string ComponentName
        {
            get { return "Crash Bandicoot 4: It's About Time - Load Remover"; }
        }

        public ComponentCategory Category
        {
            get { return ComponentCategory.Control; }
        }

        public string Description
        {
            get { return "Automatically detects and removes loads (GameTime) for Crash Bandicoot 4: It's About Time."; }
        }

        public IComponent Create(LiveSplitState state)
        {
            return new Crash4LoadRemoverComponent(state);
        }

        public string UpdateName
        {
            get { return ComponentName; }
        }
        
        public string UpdateURL => "https://raw.githubusercontent.com/thomasneff/LiveSplit.Crash4LoadRemover/master/";
        
        public string XMLURL => UpdateURL + "update.LiveSplit.Crash4LoadRemover.xml";
		

        public Version Version
        {
            get { return Version.Parse("1.1"); }
        }
    }
}
