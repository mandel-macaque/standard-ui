namespace Microsoft.StandardUI.WinUI
{
    public class WinUIStandardUIEnvironment : IStandardUIEnvironment
    {
        private readonly StandardUIFactory _uiElementFactory = new StandardUIFactory();
        private readonly IVisualEnvironment _visualEnvironment;

        public static void Init(IVisualEnvironment visualEnvironment)
        {
            StandardUIEnvironment.Init(new WinUIStandardUIEnvironment(visualEnvironment));
        }

        public WinUIStandardUIEnvironment(IVisualEnvironment visualEnvironment)
        {
            _visualEnvironment = visualEnvironment;
        }

        public IVisualEnvironment VisualEnvironment => _visualEnvironment;

        public IStandardUIFactory Factory => _uiElementFactory;
    }
}
