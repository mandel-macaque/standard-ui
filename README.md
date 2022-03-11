# .NET Standard Controls

.NET Standard Controls is a project, currently experimental, that enables building controls and other UI that can be used across multiple .NET UI frameworks - WPF, .NET MAUI, WinForms, WinUI, macOS Cocoa, and potentially Blazor, Uno, Avalonia, and more.

Today the .NET UI ecosystem is fragmented - lots of the frameworks above work similarly, but they
are all slightly different and incompatible. Control authors that want to support multiple frameworks
need to write their own abstraction layer. Many do that, but it's a pain and new frameworks aren't
automatically supported.  Rather than everyone writing their own abstraction layer, it would be better
if Microsoft provided a standard set of APIs, where if you use those to author your control it'll
work everywhere. That's the intention of .NET Standard Controls.

Here's how it works:
1. You create a .NET Standard Control by defining an interface for the control's public API, including
properties exposed and events emitted.

2. You also provide a control implementation class, which provides the core functionality of the control.
There you can respond to input events, using standard event APIs (based on those in WPF/WinUI/MAUI today, but standardized).
In response to events, you can update the control state and regenerate the control visual output.


    Standard Control visuals follow the same model as WPF/WinUI/MAUI today - a control is basically a tree of UI objects,
shape UI objects used for retained mode drawn UI or other controls that are composed together. The control
defines what this tree is, and updates it, in order to define its visual look.

3. Now say a user wants to use a Standard Control in their WPF app. So, like any other control, they start by
adding the assembly to their client app, say via NuGet. At the point the magic of source generators comes into play,
generating the WPF specific glue code, where the standard control properties turn into WPF DependencyProperties,
to be a proper native WPF control. The Standard Control is just a .NET Standard assembly - it works
everywhere (normally, though it can use multitargetting and include framework specific code if needed). It's the source
generator that turns the Standard Control into a WPF native control, functioning like any other WPF native control.

    At that point, the user can use the control in their WPF XAML, set control properties including thru bindings,
    define control styles, etc. It works like any other WPF native control, because it is. But because it's a
standard control, it can also be used in WinForms, MAUI, and other frameworks, acting like native controls there too.

# Value Propositions

.NET Standard Controls aims to help solve these problems:

**Grow the .NET UI control ecosystem** - Writing a single control that can target several UI
frameworks means it's easier to write controls and they can target a bigger set of users. This
helps control vendors, community members that build controls, and Microsoft as it builds out first
party controls - cheaper + wider reach should mean more controls in the ecosystem. For Microsoft controls, possibilities include cross platform Fluent UI or controls that interoperate with MS services, like the MS Graph controls [here](https://docs.microsoft.com/en-us/windows/communitytoolkit/graph/controls/peoplepicker).

**Reduce .NET UI Fragementation** - Today there are multiple XAML UI frameworks (WPF, UWP, WinUI, Xamarin.Forms, .NET MAUI, Uno, Avalonia, etc.). All are pretty similar, though slightly different.
For the most part they don't share code. The naming differences are annoying.
This project is similar in some ways to [XAML Standard](https://github.com/microsoft/xaml-standard), but this is a binary standard, not just aligned naming conventions. A binary standard is much more useful, at it allows writing shared code.
As the standard is based on WPF/UWP/WinUI, it means that it isn't a big leap to take an existing WPF/UWP/WinUI control definition (something like [this](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/controls/button-styles-and-templates?view=netframeworkdesktop-4.8) for instance, contructed out of shape primitives, visual states, and storyboards) and make it a cross platform control.

# Documentation

Doc is a work in progress. The latest doc is here (though currently only accessible by Microsoft internals, unfortunately):

[Reference (API) doc](https://review.docs.microsoft.com/en-us/dotnet/api/microsoft.standardui?view=dotnet-standard-ui&branch=pr-en-us-4)

[Conceptual doc](https://review.docs.microsoft.com/en-us/dotnet/standard-ui/?branch=main)


# Architecture and APIs

The API is interface based. For instance, an ellipse is `Microsoft.StandardUI.Shapes.IEllipse`. Users of the API always use the interface.

In terms of implementation, UI platforms can implement the interface directly OR it can be implemented by a wrapper object (which typically lives in this repo). Having both options available provides maximum flexibility.

For new UI platforms, like WinUI3 and .NET MAUI, ideally they have their native
`Ellipse` object implement `IEllipse` directly. That helps enforce API naming consistency and is slightly more efficient.

Or the interface can be implemented via a wrapper, which requires no changes to the underlying UI platform at all. That's a good choice for platforms like WPF.

The API interfaces are all defined [here](src/StandardUI). Implementations for the different UI frameworks are created through a mix of [code generation](src/StandardUI.CodeGenerator) from those interfaces and hand coding.

This project is an evolution of my [XGraphics](https://github.com/BretJohnson/XGraphics) project, taking it beyond just shapes.

### Control hierarchy

[IUIElement](src/StandardUI/IUIElement.cs),
[IUIElementCollection](src/StandardUI/Controls/IUIElementCollection.cs),
[IControl](src/StandardUI/Controls/IControl.cs),
[IUserControl](src/StandardUI/Controls/IUserControl.cs)

### Shapes and Drawing

_Shapes:_
[IShape](src/StandardUI/Shapes/IShape.cs),
[IEllipse](src/StandardUI/Shapes/IEllipse.cs),
[ILine](src/StandardUI/Shapes/ILine.cs),
[IPath](src/StandardUI/Shapes/IPath.cs),
[IPolygon](src/StandardUI/Shapes/IPolygon.cs),
[IPolyline](src/StandardUI/Shapes/IPolyline.cs),
[IRectangle](src/StandardUI/Shapes/IRectangle.cs)

_Geometries:_
[IGeometry](src/StandardUI/Media/IGeometry.cs),
[IArcSegement](src/StandardUI/Media/IArcSegement.cs),
[IBezierSegment](src/StandardUI/Media/IBezierSegment.cs),
[ILineSegment](src/StandardUI/Media/ILineSegment.cs),
[IPathFigure](src/StandardUI/Media/IPathFigure.cs),
[IPathGeometry](src/StandardUI/Media/IPathGeometry.cs),
[IPathSegment](src/StandardUI/Media/IPathSegment.cs),
[IPolyBezierSegment](src/StandardUI/Media/IPolyBezierSegment.cs)
[IPolyQuadraticBezierSegment](src/StandardUI/Media/IPolyQuadraticBezierSegment.cs)
[IQuadraticBezierSegment](src/StandardUI/Media/IQuadraticBezierSegment.cs)

_Transforms:_
[ITransform](src/StandardUI/Media/ITransform.cs),
[IRotateTransform](src/StandardUI/Media/IRotateTransform.cs),
[IScaleTransform](src/StandardUI/Media/IScaleTransform.cs),
[ITransformGroup](src/StandardUI/Media/ITransformGroup.cs),
[ITranslateTransform](src/StandardUI/Media/ITranslateTransform.cs)

_Brushes and Strokes:_
[BrushMappingMode](src/StandardUI/Media/BrushMappingMode.cs),
[FillMode](src/StandardUI/Media/FillMode.cs),
[GradientStreamMethod](src/StandardUI/Media/GradientStreamMethod.cs),
[IGradientBrush](src/StandardUI/Media/IGradientBrush.cs),
[ILinearGradientBrush](src/StandardUI/Media/ILinearGradientBrush.cs),
[IRadialGradientBrush](src/StandardUI/Media/IRadialGradientBrush.cs),
[ISolidColorBrush](src/StandardUI/Media/ISolidColorBrush.cs),
[PenLineCap](src/StandardUI/Media/PenLineCap.cs),
[PenLineJoin](src/StandardUI/Media/PenLineJoin.cs),
[SweepDirection](src/StandardUI/Media/SweepDirection.cs)

All of these APIs are nearly identical to UWP, WPF, and Xamarin.Forms 4.8 (which added shape and brush support).

Shapes are [IUIElements](src/StandardUI/IUIElement.cs) that can be used as children to build the visual representation of a control, often as part of a control template. That's the same model used by UWP/WPF/Forms.

Geometries, transforms, and brushes all help support the drawing.

### Text

[ITextBlock](src/StandardUI/Controls/ITextBlock.cs),
[FontStyle](src/StandardUI/Text/FontStyle.cs),
[FontWeight](src/StandardUI/Text/FontWeight.cs),
[FontWeights](src/StandardUI/Text/FontWeights.cs)

### Other controls

[IBorder](src/StandardUI/Controls/IBorder.cs)

### Layout

[IPanel](src/StandardUI/Controls/IPanel.cs),
[IStackPanel](src/StandardUI/Controls/IStackPanel.cs),
[IGrid](src/StandardUI/Controls/IGrid.cs),
[ICanvas](src/StandardUI/Controls/ICanvas.cs)



