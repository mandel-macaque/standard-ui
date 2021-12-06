namespace Microsoft.StandardUI.Wpf
{
    public class WpfStandardUIEnvironment : IStandardUIEnvironment
    {
        private readonly StandardUIFactory _uiElementFactory = new StandardUIFactory();
        private readonly IVisualEnvironment _visualEnvironment;

        public static void Init(IVisualEnvironment visualEnvironment)
        {
            StandardUIEnvironment.Init(new WpfStandardUIEnvironment(visualEnvironment));
        }

        public WpfStandardUIEnvironment(IVisualEnvironment visualEnvironment)
        {
            _visualEnvironment = visualEnvironment;
        }

        public IVisualEnvironment VisualEnvironment => _visualEnvironment;

        public IStandardUIFactory Factory => _uiElementFactory;
    }
}
