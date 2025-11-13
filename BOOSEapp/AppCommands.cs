using BOOSE;

namespace BOOSEapp
{
    // ----------------- MoveTo -----------------
    public class MyMoveTo : CommandTwoParameters
    {
        public MyMoveTo() : base() { }

        public override void Execute()
        {
            base.Execute();

            int x = Paramsint[0];
            int y = Paramsint[1];

            Canvas.MoveTo(x, y);
        }
    }

    // ----------------- DrawTo -----------------
    public class MyDrawTo : CommandTwoParameters
    {
        public MyDrawTo() : base() { }

        public override void Execute()
        {
            base.Execute();

            int x = Paramsint[0];
            int y = Paramsint[1];

            Canvas.DrawTo(x, y);
        }
    }

    // ----------------- Circle -----------------
    public class MyCircle : CommandOneParameter
    {
        public MyCircle() : base() { }

        public override void Execute()
        {
            base.Execute();

            int radius = Paramsint[0];
            Canvas.Circle(radius, false);
        }
    }

    // ----------------- Rectangle -----------------
    public class MyRect : CommandTwoParameters
    {
        public MyRect() : base() { }

        public override void Execute()
        {
            base.Execute();

            int w = Paramsint[0];
            int h = Paramsint[1];

            Canvas.Rect(w, h, false);
        }
    }

    // ----------------- Triangle -----------------
    public class MyTri : CommandTwoParameters
    {
        public MyTri() : base() { }

        public override void Execute()
        {
            base.Execute();

            int w = Paramsint[0];
            int h = Paramsint[1];

            Canvas.Tri(w, h);
        }
    }

    // ----------------- Command Factory -----------------
    public class AppCommandFactory : CommandFactory
    {
        public override ICommand MakeCommand(string commandType)
        {
            switch (commandType.ToLower())
            {
                case "moveto": return new MyMoveTo();
                case "drawto": return new MyDrawTo();
                case "circle": return new MyCircle();
                case "rect":
                case "rectangle": return new MyRect();
                case "tri":
                case "triangle": return new MyTri();

                default:
                    return base.MakeCommand(commandType);
            }
        }
    }
}
