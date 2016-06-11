using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPF_Windows_Spotlight.Foundation.ItemType
{
    public class AnswerItem : Item
    {
        private string _expression;
        public AnswerItem(string title, string exprssion, string name = "", int weight = 0)
            : base(title, name, weight)
        {
            _expression = exprssion;
        }

        public string Expression
        {
            get { return _expression; }
        }

        public override void Open()
        {
        }

        public override void GenerateContent(StackPanel contentView)
        {
            contentView.Children.Clear();
            Label expression = new Label();
            expression.Content = Expression;
            expression.FontSize = 24;
            Color fontColor = (Color)ColorConverter.ConvertFromString("#434343");
            expression.BorderBrush = new SolidColorBrush(fontColor);

            expression.Foreground = expression.BorderBrush;
            expression.HorizontalContentAlignment = HorizontalAlignment.Center;
            Thickness expMargin = new Thickness(0, 100, 0, 10);
            expression.Margin = expMargin;
            contentView.Children.Add(expression);

            Border hr = new Border();
            Color hrColor = (Color)ColorConverter.ConvertFromString("#FFD7D7D7");
            hr.Height = 1;
            hr.BorderBrush = new SolidColorBrush(hrColor);
            hr.BorderThickness = new Thickness(0, 1, 0, 0);
            hr.Width = contentView.Width - 30;
            hr.Margin = new Thickness(15, 10, 15, 10);
            contentView.Children.Add(hr);

            Label answer = new Label();
            answer.Content = Title;
            answer.FontSize = 36;
            answer.HorizontalContentAlignment = HorizontalAlignment.Center;
            contentView.Children.Add(answer);
        }
    }
}
