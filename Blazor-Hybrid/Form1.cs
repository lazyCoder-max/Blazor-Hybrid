using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.DependencyInjection;
using PromaAITextSrv.Helpers;
using PromaAITextSrv.Interfaces;
using PromaAITextSrv.Models.Endpoints.Chat;
using Shared.Components;
using Shared.Interfaces;

namespace Blazor_Hybrid
{
    public partial class Form1 : Form
    {
        private int _animationTargetX;
        private ServiceProvider? _serviceProvider;
        private bool _disposed = false;
        private bool _isInitialized;
        private IChatUIService _chatUIService;
        private Shared.Interfaces.IMessageBrokerService _messageBrokerService;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _serviceProvider = Program.Services.BuildServiceProvider();
            _messageBrokerService = _serviceProvider.GetRequiredService<Shared.Interfaces.IMessageBrokerService>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!_isInitialized)
            {
                _serviceProvider = Program.Services.BuildServiceProvider();

                blazorWebView1.HostPage = "wwwroot/index.html";
                blazorWebView1.Services = _serviceProvider;
                blazorWebView1.RootComponents.Add<ChatUI>("#app");

                _chatUIService = _serviceProvider.GetRequiredService<IChatUIService>();
                _chatUIService.OnCloseRequested += OnChatCloseRequested;
                _chatUIService.InputFieldChanged += NavService_InputFieldChanged;
                _isInitialized = true;
            }

            SlideToTarget(12);
        }

        private void NavService_InputFieldChanged(string? obj)
        {
            textBox1.Text = obj;
        }

        private void OnChatCloseRequested()
        {
            // Marshal back to the UI thread since the event fires from Blazor's render thread
            if (InvokeRequired)
            {
                Invoke(OnChatCloseRequested);
                return;
            }
            _disposed = true;
            SlideToTarget(-(blazorWebView1.Width + 14));

        }

        private void SlideToTarget(int targetX)
        {
            _animationTargetX = targetX;
            timer1.Interval = 10;
            timer1.Tick -= Timer1_Tick;
            timer1.Tick += Timer1_Tick;
            timer1.Start();
        }

        private async void Timer1_Tick(object? sender, EventArgs e)
        {
            var currentX = blazorWebView1.Location.X;
            var remaining = _animationTargetX - currentX;

            if (Math.Abs(remaining) < 2)
            {
                blazorWebView1.Location = new Point(_animationTargetX, 12);
                timer1.Stop();
                timer1.Tick -= Timer1_Tick;

                return;
            }

            // Ease-out: move 12% of remaining distance each tick (decelerates near target)
            var step = (int)Math.Ceiling(remaining * 0.12);
            blazorWebView1.Location = new Point(currentX + step, 12);
        }

        private void Form1_MaximumSizeChanged(object sender, EventArgs e)
        {
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Text = checkBox1.Checked ? "Disable Theme" : "Enable Theme";
            if (checkBox1.Checked)
                _chatUIService.AllowMultipleThemes = true;
            else
                _chatUIService.AllowMultipleThemes = false;

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            
        }
    }
}
