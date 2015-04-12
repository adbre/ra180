using System;
using System.Windows.Forms;
using Ra180.Devices.Dart380;
using Ra180.UI;
using PaintEventArgs = System.Windows.Forms.PaintEventArgs;

namespace Ra180.Client.WinForms
{
    public partial class Dart380Control : UserControl
    {
        private readonly Dart380Drawable _drawable;
        private readonly DelayedSynchronizationContext _synchronizationContext = new DelayedSynchronizationContext();
        private IDart380 _dart;

        public Dart380Control()
        {
            InitializeComponent();

            _dart = new Dart380(_synchronizationContext);
            _drawable = new Dart380Drawable(new WinFormsGraphic(this), _dart);
        }

        public IDart380 Dart380
        {
            get { return _dart; }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (var canvas = new WinFormsCanvas(e.Graphics, ClientRectangle))
                _drawable.OnDraw(canvas);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            if (_drawable != null)
                _drawable.SizeChanged(new SizeChangedEventArgs(ClientRectangle.Width, ClientRectangle.Height));
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (_drawable != null)
                _drawable.OnTouchEvent(new MotionEventArgs(e.X, e.Y, MotionEventActions.Down));
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (_drawable != null)
                _drawable.OnTouchEvent(new MotionEventArgs(e.X, e.Y, MotionEventActions.Up));
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            if (_drawable != null)
                _drawable.OnKeyPress(e.KeyChar);
        }

        private void synchronizationContextTimer_Tick(object sender, System.EventArgs e)
        {
            _synchronizationContext.Tick(synchronizationContextTimer.Interval);

            if (_drawable != null)
                _drawable.Tick();
        }
    }
}
