using System.Windows.Input;

namespace GidraSIM.GUI
{
    public class MainWindowCommands
    {
        public static RoutedCommand Arrow { get; set; }
        public static RoutedCommand Procedure { get; set; }
        public static RoutedCommand Resourse { get; set; }
        public static RoutedCommand Connect { get; set; }
        public static RoutedCommand SubProcess { get; set; }
        public static RoutedCommand StartCheck { get; set; }
        public static RoutedCommand StartModeling { get; set; }
        public static RoutedCommand BlackTheme { get; set; }
        public static RoutedCommand WhiteTheme { get; set; }

        static MainWindowCommands()
        {
            InputGestureCollection inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.Q,ModifierKeys.Alt, "Alt+Q"));
            Arrow = new RoutedCommand("Arrow", typeof(MainWindowCommands), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.W, ModifierKeys.Alt, "Alt+W"));
            Procedure = new RoutedCommand("Procedure", typeof(MainWindowCommands), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.E, ModifierKeys.Alt, "Alt+E"));
            Resourse = new RoutedCommand("Resourse", typeof(MainWindowCommands), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.R, ModifierKeys.Alt, "Alt+R"));
            Connect = new RoutedCommand("Connect", typeof(MainWindowCommands), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.T, ModifierKeys.Alt, "Alt+T"));
            SubProcess = new RoutedCommand("SubProcess", typeof(MainWindowCommands), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.F4, ModifierKeys.None, "F4"));
            StartCheck = new RoutedCommand("StartCheck", typeof(MainWindowCommands), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.F5, ModifierKeys.None, "F5"));
            StartModeling = new RoutedCommand("StartCheck", typeof(MainWindowCommands), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.B, ModifierKeys.Control, "Ctrl+B"));
            BlackTheme = new RoutedCommand("BlackTheme", typeof(MainWindowCommands), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.K, ModifierKeys.Control, "Ctrl+B"));
            WhiteTheme = new RoutedCommand("WhiteTheme", typeof(MainWindowCommands), inputs);
        }
    }
}
