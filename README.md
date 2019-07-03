# How to set it up

Extract the .zip into your LiveSplit/Components folder, such that it contains all .ctrnfdata files and LiveSplit.CTRNitroFueledLoadRemover.dll.

Add this to LiveSplit by going into your Layout Editor -> Add -> Control -> CTRNitroFueledLoadRemover.

You can then configure your capture settings (window/display) and capture region within the component settings in the Layout Settings.

If you set it up correctly, you can select the database file for your language in the "Database" drop down menu in the component settings.

Make sure that sure game footage is fully and accurately captured and that you don't distort/change the aspect ratio, color saturation, contrast or do any other aggressive filtering on the image footage, this will lead to detection issues.

Make sure that the "LOADING" text in the bottom right of each load screen is not covered in any way, otherwise you might encounter issues in load screen detection.

The "Threshold" in the settings is a value between 0 and 1 that can be fine-tuned if you have issues with detection.

If you don't detect load screens that you should detect (= timer runs during load screens) decrease the threshold. If you detect load screens that you shouldn't detect (= timer freezes during gameplay) increase the threshold.


The following screens show how it would be correctly set up:

![Setup Layout Settings](setup_layout_editor.png "Layout Settings")
![Setup Component Settings](setup_component_editor.png "Component Settings")
![Compare Against GameTime](compare_against_gametime.png "Compare Against GameTime")


About the "Scaling" setting: You should only need to change this if you have the Display Scaling set in Windows. (Display Settings -> "Change Size of text, apps and other items"). Otherwise you can leave this at 100%.



# LiveSplit.CrashNSTLoadRemoval
LiveSplit component to automatically detect and remove loads from Crash Team Racing: Nitro Fueled.

This is adapted from my Crash NST vision-based load remover: https://github.com/thomasneff/LiveSplit.CrashNSTLoadRemoval
and from https://github.com/Maschell/LiveSplit.PokemonRedBlue for the base component code.

# Settings
The files LiveSplit.CTRNitroFueledLoadRemover.dll as well as LiveSplit.CTRNitroFueledLoadRemover.data go into your "Components" folder in your LiveSplit folder.

Add this to LiveSplit by going into your Layout Editor -> Add -> Control -> CTRNitroFueledLoadRemover.

You can specify to capture either the full primary Display (default) or an open window. This window has to be open (not minimized) but does not have to be in the foreground.

This might not work for windows with DirectX/OpenGL surfaces, nothing I can do about that. (Use Display capture for those cases, sorry, although even that might not work in some cases). In those cases, you will probably get a black image in the capture preview in the component settings.

