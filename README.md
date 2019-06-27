# LiveSplit.CrashNSTLoadRemoval
LiveSplit component to automatically detect and remove loads from Crash Team Racing: Nitro Fueled.

This is adapted from my Crash NST vision-based load remover: https://github.com/thomasneff/LiveSplit.CrashNSTLoadRemoval
and from https://github.com/Maschell/LiveSplit.PokemonRedBlue for the base component code.

# Settings
The files LiveSplit.CTRNitroFueledLoadRemover.dll as well as LiveSplit.CTRNitroFueledLoadRemover.data go into your "Components" folder in your LiveSplit folder.

Add this to LiveSplit by going into your Layout Editor -> Add -> Control -> CTRNitroFueledLoadRemover.

You can specify to capture either the full primary Display (default) or an open window. This window has to be open (not minimized) but does not have to be in the foreground.

This might not work for windows with DirectX/OpenGL surfaces, nothing I can do about that. (Use Display capture for those cases, sorry, although even that might not work in some cases). In those cases, you will probably get a black image in the capture preview in the component settings.

