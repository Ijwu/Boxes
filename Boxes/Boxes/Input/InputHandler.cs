using Microsoft.Xna.Framework.Input;
/*
 * I got this class from https://github.com/MarioE/Raptor/blob/master/Raptor/Input.cs
 * and subsequently butchered it for my needs.
 * 
 * Credit and props to MarioE.
 */
namespace Boxes.Input
{
    /// <summary>
    /// Manages input.
    /// </summary>
    public static class Input
    {
        static KeyboardState lastKeyboard = Microsoft.Xna.Framework.Input.Keyboard.GetState();
        static MouseState lastMouse = Microsoft.Xna.Framework.Input.Mouse.GetState();
        static KeyboardState keyboard = Microsoft.Xna.Framework.Input.Keyboard.GetState();
        static MouseState mouse = Microsoft.Xna.Framework.Input.Mouse.GetState();

        /// <summary>
        /// Gets if an alt key is down.
        /// </summary>
        public static bool Alt
        {
            get { return keyboard.IsKeyDown(Keys.LeftAlt) || keyboard.IsKeyDown(Keys.RightAlt); }
        }
        internal static int CursorType { get; set; }
        /// <summary>
        /// Gets if a control key is down.
        /// </summary>
        public static bool Control
        {
            get { return keyboard.IsKeyDown(Keys.LeftControl) || keyboard.IsKeyDown(Keys.RightControl); }
        }
        /// <summary>
        /// Whether or not the keyboard for Terraria should be disabled.
        /// </summary>
        public static bool DisabledKeyboard;
        /// <summary>
        /// Whether or not the mouse for Terraria should be disabled.
        /// </summary>
        public static bool DisabledMouse;
        /// <summary>
        /// Gets the mouse delta scroll wheel value.
        /// </summary>
        public static int MouseDScroll
        {
            get { return mouse.ScrollWheelValue - lastMouse.ScrollWheelValue; }
        }
        /// <summary>
        /// Gets the mouse delta X position.
        /// </summary>
        public static int MouseDX
        {
            get { return mouse.X - lastMouse.X; }
        }
        /// <summary>
        /// Gets the mouse delta Y position.
        /// </summary>
        public static int MouseDY
        {
            get { return mouse.Y - lastMouse.Y; }
        }
        /// <summary>
        /// Gets if the LMB is clicked.
        /// </summary>
        public static bool MouseLeftClick
        {
            get { return mouse.LeftButton == ButtonState.Pressed && lastMouse.LeftButton == ButtonState.Released; }
        }
        /// <summary>
        /// Gets if the LMB is pressed.
        /// </summary>
        public static bool MouseLeftDown
        {
            get { return mouse.LeftButton == ButtonState.Pressed; }
        }
        /// <summary>
        /// Gets if the LMB is released.
        /// </summary>
        public static bool MouseLeftRelease
        {
            get { return mouse.LeftButton != ButtonState.Pressed && lastMouse.LeftButton == ButtonState.Pressed; }
        }
        /// <summary>
        /// Gets if the RMB is clicked.
        /// </summary>
        public static bool MouseRightClick
        {
            get { return mouse.RightButton == ButtonState.Pressed && lastMouse.RightButton == ButtonState.Released; }
        }
        /// <summary>
        /// Gets if the RMB is pressed.
        /// </summary>
        public static bool MouseRightDown
        {
            get { return mouse.RightButton == ButtonState.Pressed; }
        }
        /// <summary>
        /// Gets if the RMB is released.
        /// </summary>
        public static bool MouseRightRelease
        {
            get { return mouse.RightButton != ButtonState.Pressed && lastMouse.RightButton == ButtonState.Pressed; }
        }
        /// <summary>
        /// Gets the mouse scroll wheel value.
        /// </summary>
        public static int MouseScroll
        {
            get { return mouse.ScrollWheelValue; }
        }
        /// <summary>
        /// Gets the mouse X position.
        /// </summary>
        public static int MouseX
        {
            get { return mouse.X; }
        }
        /// <summary>
        /// Gets the mouse Y position.
        /// </summary>
        public static int MouseY
        {
            get { return mouse.Y; }
        }
        /// <summary>
        /// Gets if a shift key is down.
        /// </summary>
        public static bool Shift
        {
            get { return keyboard.IsKeyDown(Keys.LeftShift) || keyboard.IsKeyDown(Keys.RightShift); }
        }
        /// <summary>
        /// Gets if a key is down.
        /// </summary>
        public static bool IsKeyDown(Keys key)
        {
            return keyboard.IsKeyDown(key);
        }
        /// <summary>
        /// Gets if a key was released; that is, if the key is currently depressed but was pressed before.
        /// </summary>
        public static bool IsKeyReleased(Keys key)
        {
            return keyboard.IsKeyUp(key) && lastKeyboard.IsKeyDown(key);
        }
        /// <summary>
        /// Gets if a key was tapped; that is, if the key is currently pressed but was depressed before.
        /// </summary>
        public static bool IsKeyTapped(Keys key)
        {
            return keyboard.IsKeyDown(key) && lastKeyboard.IsKeyUp(key);
        }
        /// <summary>
        /// Gets all keys that are currently pressed down on the keyboard.
        /// </summary>
        public static Keys[] GetPressedKeys()
        {
            return keyboard.GetPressedKeys();
        }
        
        internal static void Update()
        {
            lastKeyboard = keyboard;
            keyboard = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            lastMouse = mouse;
            mouse = Microsoft.Xna.Framework.Input.Mouse.GetState();
        }
    }
}