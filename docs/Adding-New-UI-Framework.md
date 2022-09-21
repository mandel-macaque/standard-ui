# Adding a New UI Framework

In brief, to add StandardUI support for a new UI Framework, do this:

1. **Source generation:** Create a new subclass of [UIFramework](../src/Microsoft.StandardUI.Analyzers/UIFrameworks/UIFramework.cs),
subclassing [XamlUIFramework](../src/Microsoft.StandardUI.Analyzers/UIFrameworks/XamlUIFramework.cs) or 
[NonXamlUIFramework](../src/Microsoft.StandardUI.Analyzers/UIFrameworks/NonXamlUIFramework.cs).
Implement the class's methods to generate the right source code for the framework.

2. **Runtime support**: Create a new project under `src`. You can copy
[Microsoft.StandardUI.Wpf](../src/Microsoft.StandardUI.Wpf/), using it as an example.
All the code in the [generated](../src/Microsoft.StandardUI.Wpf/generated/)
subdirectory there is generated, the rest hand authored. Run the
[CommandLineSourceGenerator](../src/Microsoft.StandardUI.CommandLineSourceGenerator/)
project to create/update all the generated code. Update the hand authored code
as appropriate for the framework.

3. **Visual (drawing) support**: With Standard UI, much like the entire UI environment is
abstracted with `UIFramework`, the graphics rendering is abstracted with
[VisualFramework](../src/Microsoft.StandardUI/Visual/IVisualFramework.cs). A given UI
framework can support multiple visual frameworks. For example, WPF supports
[WpfNativeVisualFramework](../src/Microsoft.StandardUI.Wpf/NativeVisualFramework/WpfNativeVisualFramework.cs),
which uses built in WPF drawing APIs, and also supports [SkiaVisualFramework](../src/Microsoft.StandardUI.SkiaVisualFramework/SkiaVisualFramework.cs), which uses SkiaSharp. Standard UI, like most XAML GUI frameworks, uses
"retained mode" (as opposed to "immediate mode") graphics. So while you can draw into 
an [IDrawingContext](../src/Microsoft.StandardUI/Visual/IDrawingContext.cs), drawn
graphics objects aren't actually rendered to the screen then; they are retained in an
[IVisual](../src/Microsoft.StandardUI/Visual/IVisual.cs) which is rendered later. That
allows batching drawing operations, improving performance in some scenarios.


    When adding a new UIFramework, you'll need to
provide at least one VisualFramework. Typically drawing using native / built in APIs
is just built into the StandardUI runtime assembly ([here](../src/Microsoft.StandardUI.Wpf/NativeVisualFramework) for WPF). 
heavier weight drawing, with more dependencies, like using SkiaSharp, is a separate assembly.