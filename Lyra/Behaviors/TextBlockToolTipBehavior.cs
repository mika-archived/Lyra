using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace Lyra.Behaviors
{
    /// <summary>
    /// TextBlock.Text が描画幅を超えている際に、 TextBlock.Text の内容を ToolTip で表示するようにします。
    /// (Text が Trimming されている時のみ、 ToolTip を表示します)
    /// </summary>
    public class TextBlockToolTipBehavior : Behavior<TextBlock>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.SizeChanged += AssociatedObject_SizeChanged;
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.SizeChanged -= AssociatedObject_SizeChanged;
            base.OnDetaching();
        }

        private void AssociatedObject_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var text = (TextBlock)sender;
            var formattedText = new FormattedText(text.Text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                new Typeface(text.FontFamily, text.FontStyle, text.FontWeight, text.FontStretch), text.FontSize,
                text.Foreground);

            if (text.ActualWidth < formattedText.Width)
            {
                var tooltip = new TextBlock
                {
                    Foreground = (Brush)text.FindResource("ForegroundBrushKey"),
                    Text = text.Text
                };
                text.ToolTip = tooltip;
            }
            else
                text.ToolTip = null;
        }
    }
}