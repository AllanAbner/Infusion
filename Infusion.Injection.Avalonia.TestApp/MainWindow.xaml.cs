﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Infusion.Config;
using Infusion.Injection.Avalonia.Scripts;
using InjectionScript.Runtime;
using System.Threading.Tasks;

namespace Infusion.Injection.Avalonia.TestApp
{
    public class MainWindow : Window
    {
        private readonly InjectionConfiguration injectionConfiguration;
        private readonly TestInjectionObjectServices objectServices;
        private readonly TestScriptServices scriptServices;
        private readonly TestMainServices mainServices;

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            mainServices = new TestMainServices();
            injectionConfiguration = new InjectionConfiguration(configBag, new InjectionOptions());
            injectionWindowHandler = new InjectionWindowHandler();

            configBag = new ConfigBag(new MemoryConfigBagRepository());
            this.FindControl<Button>("OpenInjectionWindow").Click += (sender, e) => OpenWindow();

            objectServices = new TestInjectionObjectServices();
            objectServices.Set("Initial Object", 0x0000DEAD);
            objectServices.Set("Second Initial Object", 0x0000BEEF);

            this.FindControl<Button>("AddObjectButton").Click += (sender, e) => AddObject();
            this.FindControl<Button>("RemoveObjectButton").Click += (sender, e) => RemoveObject();

            scriptServices = new TestScriptServices();
            this.FindControl<Button>("AddRunning").Click += (sender, e) => AddRunning();
            this.FindControl<Button>("AddAvailable").Click += (sender, e) => AddAvailable();
            this.FindControl<Button>("RemoveRunning").Click += (sender, e) => RemoveRunning();
            this.FindControl<Button>("RemoveAvailable").Click += (sender, e) => RemoveAvailable();

            OpenWindow();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OpenWindow() => injectionWindowHandler.Open(objectServices, scriptServices, mainServices, injectionConfiguration);
        public TextBox ObjectName => this.FindControl<TextBox>("ObjectName");
        public TextBox ObjectId => this.FindControl<TextBox>("ObjectId");

        public InjectionWindowHandler injectionWindowHandler { get; private set; }
        public ConfigBag configBag { get; private set; }

        public void AddObject() => Task.Run(() => objectServices.Set(ObjectName.Text, int.Parse(ObjectId.Text)));
        public void RemoveObject() => Task.Run(() => objectServices.Remove(ObjectName.Text));

        public void AddRunning() => Task.Run(() => scriptServices.AddRunning(this.FindControl<TextBox>("ScriptName").Text));
        public void AddAvailable() => Task.Run(() => scriptServices.AddAvailable(this.FindControl<TextBox>("ScriptName").Text));
        public void RemoveRunning() => Task.Run(() => scriptServices.RemoveRunning(this.FindControl<TextBox>("ScriptName").Text));
        public void RemoveAvailable() => Task.Run(() => scriptServices.RemoveAvailable(this.FindControl<TextBox>("ScriptName").Text));
    }
}
