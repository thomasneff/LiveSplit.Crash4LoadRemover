using LiveSplit.Model;
using LiveSplit.UI.Components;
using System;
using LiveSplit.CTRNitroFueledLoadRemover;

[assembly: ComponentFactory(typeof(CTRNitroFueledLoadRemoverFactory))]

namespace LiveSplit.CTRNitroFueledLoadRemover
{
    public class CTRNitroFueledLoadRemoverFactory : IComponentFactory
    {
        public string ComponentName
        {
            get { return "CTR Nitro Fueled Load Remover"; }
        }

        public ComponentCategory Category
        {
            get { return ComponentCategory.Control; }
        }

        public string Description
        {
            get { return "Automatically detects and removes loads (GameTime) for Crash Team Racing Nitro Fueled."; }
        }

        public IComponent Create(LiveSplitState state)
        {
            return new CTRNitroFueledLoadRemoverComponent(state);
        }

        public string UpdateName
        {
            get { return ComponentName; }
        }
        
        public string UpdateURL => "https://raw.githubusercontent.com/thomasneff/LiveSplit.CTRNitroFueledLoadRemover/master/";
        
        public string XMLURL => UpdateURL + "update.LiveSplit.CTRNitroFueledLoadRemover.xml";
		

        public Version Version
        {
            get { return Version.Parse("1.2"); }
        }
    }
}
