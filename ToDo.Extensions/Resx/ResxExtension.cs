using System.Reflection;
using System.Windows;
using System.Windows.Markup;

namespace ToDo.Extensions.Resx
{
    public class ResxExtension : MarkupExtension
    {
        public string Key { get; set; }
        private static readonly Dictionary<WeakReference<FrameworkElement>, string> _targets = new();

        public ResxExtension(string key)
        {
            Key = key;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var target = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
            if (target.TargetObject is FrameworkElement fe)
                _targets[new WeakReference<FrameworkElement>(fe)] = Key;

            return GetValue();
        }

        private object GetValue()
        {
            var type = typeof(ToDo.Lang.Lang); // 指向 ToDo.Lang 的强类型类
            var prop = type.GetProperty(Key, BindingFlags.Public | BindingFlags.Static);
            return prop?.GetValue(null) ?? $"[{Key}]";
        }

        /// <summary>
        /// 切换语言后刷新所有绑定控件
        /// </summary>
        public static void UpdateTargets()
        {
            foreach (var kv in _targets.ToArray())
            {
                if (kv.Key.TryGetTarget(out var fe))
                {
                    PropertyInfo propInfo = fe.GetType().GetProperty("Text")
                                         ?? fe.GetType().GetProperty("Content");
                    if (propInfo != null)
                        propInfo.SetValue(fe, typeof(ToDo.Lang.Lang).GetProperty(kv.Value)?.GetValue(null));
                }
            }
        }
    }
}
