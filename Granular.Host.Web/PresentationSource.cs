using System;
using System.Collections.Generic;
using static Retyped.dom;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Granular.Host.Render;
using System.Windows.Media;

namespace Granular.Host
{
    public class PresentationSourceFactory : IPresentationSourceFactory
    {
        private List<PresentationSource> presentationSources;
        private HtmlRenderElementFactory htmlRenderElementFactory;
        private HtmlValueConverter htmlValueConverter;
        private ImageElementContainer imageElementContainer;
        private SvgDefinitionContainer svgDefinitionContainer;

        public PresentationSourceFactory(HtmlRenderElementFactory htmlRenderElementFactory, HtmlValueConverter htmlValueConverter, ImageElementContainer imageElementContainer, SvgDefinitionContainer svgDefinitionContainer)
        {
            this.htmlRenderElementFactory = htmlRenderElementFactory;
            this.htmlValueConverter = htmlValueConverter;
            this.imageElementContainer = imageElementContainer;
            this.svgDefinitionContainer = svgDefinitionContainer;

            presentationSources = new List<PresentationSource>();
        }

        public IPresentationSource CreatePresentationSource(UIElement rootElement)
        {
            PresentationSource presentationSource = new PresentationSource(rootElement, htmlRenderElementFactory, htmlValueConverter, imageElementContainer, svgDefinitionContainer);
            presentationSources.Add(presentationSource);

            return presentationSource;
        }

        public IPresentationSource GetPresentationSourceFromElement(UIElement element)
        {
            while (element.VisualParent is FrameworkElement)
            {
                element = (FrameworkElement)element.VisualParent;
            }

            return presentationSources.FirstOrDefault(presentationSource => presentationSource.RootElement == element);
        }
    }

    public class PresentationSource : IPresentationSource
    {
        public event EventHandler HitTestInvalidated { add { } remove { } }

        public UIElement RootElement { get; private set; }

        public MouseDevice MouseDevice { get; private set; }
        public KeyboardDevice KeyboardDevice { get; private set; }

        public string Title
        {
            get { return window.document.title; }
            set { window.document.title = value; }
        }

        private HtmlValueConverter converter;

        private bool mouseDownHandled;
        private bool mouseMoveHandled;
        private bool mouseUpHandled;

        private bool keyDownHandled;
        private bool keyUpHandled;

        public PresentationSource(UIElement rootElement, HtmlRenderElementFactory htmlRenderElementFactory, HtmlValueConverter converter, ImageElementContainer imageElementContainer, SvgDefinitionContainer svgDefinitionContainer)
        {
            this.RootElement = rootElement;
            this.converter = converter;

            RootElement.IsRootElement = true;

            MouseDevice = new MouseDevice(this);
            KeyboardDevice = new KeyboardDevice(this);

            MouseDevice.CursorChanged += (sender, e) => window.document.body.SetHtmlStyleProperty("cursor", converter.ToCursorString(MouseDevice.Cursor, htmlRenderElementFactory));
            window.document.body.SetHtmlStyleProperty("cursor", converter.ToCursorString(MouseDevice.Cursor, htmlRenderElementFactory));

            window.onkeydown = OnKeyDown;
            window.onkeyup = OnKeyUp;
            window.onkeypress = PreventKeyboardHandled;
            window.onmousemove = OnMouseMove;
            window.onmousedown = OnMouseDown;
            window.onmouseup = OnMouseUp;
            window.onscroll = OnScroll;
            window.onfocus = e => { MouseDevice.Activate(); return null; };
            window.onblur = e => { MouseDevice.Deactivate(); return null; };
            window.onresize = e => { SetRootElementSize(); return null; };
            window.onclick = PreventMouseHandled;
            window.oncontextmenu = PreventMouseHandled;
            window.addEventListener("ondblclick", OnDblClick);
            window.addEventListener("wheel", OnMouseWheel);

            SetRootElementSize();
            ((FrameworkElement)RootElement).Arrange(new Rect(window.innerWidth, window.innerHeight));

            IHtmlRenderElement renderElement = ((IHtmlRenderElement)RootElement.GetRenderElement(htmlRenderElementFactory));
            renderElement.Load();

            window.document.body.style.overflow = "hidden";
            window.document.body.appendChild(imageElementContainer.HtmlElement);
            window.document.body.appendChild(svgDefinitionContainer.HtmlElement);
            window.document.body.appendChild(renderElement.HtmlElement);

            MouseDevice.Activate();
            KeyboardDevice.Activate();
        }

        private void SetRootElementSize()
        {
            ((FrameworkElement)RootElement).Width = window.innerWidth;
            ((FrameworkElement)RootElement).Height = window.innerHeight;
        }

        private object OnKeyDown(Event e)
        {
            KeyboardEvent keyboardEvent = (KeyboardEvent)e;

            Key key = converter.ConvertBackKey(keyboardEvent.keyCode, keyboardEvent.location);

            keyDownHandled = ProcessKeyboardEvent(new RawKeyboardEventArgs(key, KeyStates.Down, keyboardEvent.repeat, GetTimestamp()));

            if (keyDownHandled)
            {
                e.preventDefault();
            }

            return e;
        }

        private object OnKeyUp(Event e)
        {
            KeyboardEvent keyboardEvent = (KeyboardEvent)e;

            Key key = converter.ConvertBackKey(keyboardEvent.keyCode, keyboardEvent.location);

            keyUpHandled = ProcessKeyboardEvent(new RawKeyboardEventArgs(key, KeyStates.None, keyboardEvent.repeat, GetTimestamp()));

            if (keyDownHandled || keyUpHandled)
            {
                e.preventDefault();
            }

            return e;
        }

        private object PreventKeyboardHandled(Event e)
        {
            if (keyDownHandled || keyUpHandled)
            {
                e.preventDefault();
            }

            return e;
        }

        private object OnMouseDown(Event e)
        {
            MouseEvent mouseEvent = (MouseEvent)e;

            Point position = new Point(mouseEvent.pageX, mouseEvent.pageY);
            MouseButton button = converter.ConvertBackMouseButton(mouseEvent.button);

            mouseDownHandled = ProcessMouseEvent(new RawMouseButtonEventArgs(button, MouseButtonState.Pressed, position, GetTimestamp()));

            if (mouseDownHandled || MouseDevice.CaptureTarget != null)
            {
                e.preventDefault();
            }

            return e;
        }

        private object OnMouseUp(Event e)
        {
            MouseEvent mouseEvent = (MouseEvent)e;

            Point position = new Point(mouseEvent.pageX, mouseEvent.pageY);
            MouseButton button = converter.ConvertBackMouseButton(mouseEvent.button);

            mouseUpHandled = ProcessMouseEvent(new RawMouseButtonEventArgs(button, MouseButtonState.Released, position, GetTimestamp()));

            if (mouseDownHandled || mouseMoveHandled || mouseUpHandled || MouseDevice.CaptureTarget != null)
            {
                e.preventDefault();
            }

            return e;
        }
        private object OnScroll(Event e)
        {
            MouseEvent uiEvent = (MouseEvent)e;
            WheelEvent wheelEvent = (WheelEvent)e;

            Point position = new Point(uiEvent.pageX, uiEvent.pageY);
            int delta = (wheelEvent).deltaY > 0 ? -100 : 100;

            if (ProcessMouseEvent(new RawMouseWheelEventArgs(delta, position, GetTimestamp())))
            {
                e.preventDefault();
            }

            return e;
        }

        private void OnMouseWheel(Event e) => OnScroll(e);

        private object OnMouseMove(Event e)
        {
            if (!(e is MouseEvent))
            {
                return e;
            }

            MouseEvent mouseEvent = (MouseEvent)e;

            Point position = new Point(mouseEvent.pageX, mouseEvent.pageY);

            mouseMoveHandled = ProcessMouseEvent(new RawMouseEventArgs(position, GetTimestamp()));

            if (mouseDownHandled || mouseMoveHandled || MouseDevice.CaptureTarget != null)
            {
                e.preventDefault();
            }

            return e;
        }

        private void OnDblClick(Event e) => PreventMouseHandled(e);

        private object PreventMouseHandled(Event e)
        {
            if (mouseDownHandled || mouseMoveHandled || mouseUpHandled || MouseDevice.CaptureTarget != null)
            {
                e.preventDefault();
            }

            return e;
        }

        public IInputElement HitTest(Point position)
        {
            return RootElement.HitTest(position) as IInputElement;
        }

        public int GetTimestamp()
        {
            return 0;//(int)(DateTime.Now.GetTime());
        }

        private bool ProcessKeyboardEvent(RawKeyboardEventArgs keyboardEventArgs)
        {
            return Dispatcher.CurrentDispatcher.Invoke(() => KeyboardDevice.ProcessRawEvent(keyboardEventArgs), DispatcherPriority.Input);
        }

        private bool ProcessMouseEvent(RawMouseEventArgs mouseEventArgs)
        {
            return Dispatcher.CurrentDispatcher.Invoke(() => MouseDevice.ProcessRawEvent(mouseEventArgs), DispatcherPriority.Input);
        }
    }
}