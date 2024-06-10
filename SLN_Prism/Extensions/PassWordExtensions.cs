using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace SLN_Prism.Extensions
{
    /// <summary>
    /// 扩展密码框功能，使其可以被绑定到ViewModel的属性上
    /// </summary>
    public class PassWordExtensions
    {
        public static string GetPassWord(DependencyObject obj)
        {
            // 获取附加属性 PassWord 的值
            return (string)obj.GetValue(PassWordProperty);
        }

        public static void SetPassWord(DependencyObject obj, string value)
        {
            // 设置附加属性 PassWord 的值
            obj.SetValue(PassWordProperty, value);
        }

        public static readonly DependencyProperty PassWordProperty =
            // 注册附加属性 PassWord，指定默认值和属性更改回调方法
            DependencyProperty.RegisterAttached("PassWord", typeof(string), typeof(PassWordExtensions), new FrameworkPropertyMetadata(string.Empty, OnPassWordPropertyChanged));

        static void OnPassWordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var passWord = sender as PasswordBox;
            string password = (string)e.NewValue;

            // 当属性值发生改变时，更新关联的 PasswordBox 控件的 Password 属性
            if (passWord != null && passWord.Password != password)
                passWord.Password = password;
        }
    }

    public class PasswordBehavior : Behavior<PasswordBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            // 当附加到 PasswordBox 控件时，订阅 PasswordChanged 事件
            AssociatedObject.PasswordChanged += AssociatedObject_PasswordChanged;
        }

        private void AssociatedObject_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            string password = PassWordExtensions.GetPassWord(passwordBox);

            // 当 PasswordBox 的密码发生改变时，更新附加属性 PassWord 的值
            if (passwordBox != null && passwordBox.Password != password)
                PassWordExtensions.SetPassWord(passwordBox, passwordBox.Password);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            // 当 Behavior 从 PasswordBox 控件上移除时，取消订阅 PasswordChanged 事件
            AssociatedObject.PasswordChanged -= AssociatedObject_PasswordChanged;
        }
    }
}
