using System.Windows;
using Microsoft.StandardUI;

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
                                     //(used if a resource is not found in the page,
                                     // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
                                              //(used if a resource is not found in the page,
                                              // app, or any theme specific resource dictionaries)
)]

[assembly: ImportStandardControl(typeof(Microcharts.IChart))]
[assembly: ImportStandardControl(typeof(AlohaKit.StandardControls.IToggleSwitch))]
