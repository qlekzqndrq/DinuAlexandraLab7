using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DinuAlexandraLab7
{
    class ValidationBehaviour : Behavior<Editor>
    {
        protected override void OnAttachedTo(Editor entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }
        protected override void OnDetachingFrom(Editor entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }
        void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            Editor editor = (Editor)sender;

            if (string.IsNullOrEmpty(args.NewTextValue))
            {
                editor.BackgroundColor = Color.FromRgba("#AA4A44");
                editor.TextColor = Colors.White;
            }
            else
            {
                editor.BackgroundColor = Colors.White;
                editor.TextColor = Colors.Black;
            }
        }
    }
}
