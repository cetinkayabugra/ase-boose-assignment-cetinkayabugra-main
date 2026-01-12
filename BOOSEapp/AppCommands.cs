using System;
using System.Diagnostics;
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
            Debug.WriteLine($"Factory.MakeCommand called with: '{commandType}'");

            string lowerCommand = commandType.ToLower();

            switch (lowerCommand)
            {
                case "moveto":
                    Debug.WriteLine("  -> Returning MyMoveTo");
                    return new MyMoveTo();

                case "drawto":
                    Debug.WriteLine("  -> Returning MyDrawTo");
                    return new MyDrawTo();

                case "circle":
                    Debug.WriteLine("  -> Returning MyCircle");
                    return new MyCircle();

                case "rect":
                case "rectangle":
                    Debug.WriteLine("  -> Returning MyRect");
                    return new MyRect();

                case "tri":
                case "triangle":
                    Debug.WriteLine("  -> Returning MyTri");
                    return new MyTri();

                case "int":
                    Debug.WriteLine("  -> Returning UnrestrictedInt");
                    return new UnrestrictedInt();

                case "real":
                    Debug.WriteLine("  -> Returning UnrestrictedReal");
                    return new UnrestrictedReal();

                default:
                    Debug.WriteLine("  -> Trying base factory...");
                    try
                    {
                        var cmd = base.MakeCommand(commandType);
                        Debug.WriteLine($"  -> Base factory returned: {cmd.GetType().Name}");
                        return cmd;
                    }
                    catch (FactoryException ex)
                    {
                        Debug.WriteLine($"  -> Base factory threw exception: {ex.Message}");
                        Debug.WriteLine("  -> Returning UnrestrictedEvaluation");
                        return new UnrestrictedEvaluation();
                    }
            }
        }
    }
}