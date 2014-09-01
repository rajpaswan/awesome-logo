using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AwesomeLogoPhone
{
    class LogoTurtle
    {
        #region variables

        public static string Token_0;
        public static string Token_1;
        public static string Token_X;
        //public static string Operator_1;
        public static string Operator_2;

        public static Dictionary<string, string> Variables;
        public static Dictionary<string, string> Procedures;

        double Angle;
        double X1;
        double Y1;
        double X2;
        double Y2;

        int PenStrokeThickness;
        double FontSize;
        bool PenStatus;
        bool TurtleStatus;

        System.Windows.Media.Color PenColor;
        System.Windows.Media.Color BgColor;

        Polygon Turtle;
        Canvas Board;
        Grid Container;
        TextBlock InfoBlock;

        public string Message;

        #endregion

        public LogoTurtle(Grid container, Canvas board, TextBlock textblock)
        {
            Token_0 = " home cs pu pd st ht ";//taking no arguments
            Token_1 = " fd bk rt lt pc str bg fs wt "; //taking one argument. [use] not supported
            Token_X = " to repeat var mov"; //other keywords

            //Operator_1 = " not ";
            Operator_2 = " + - * / = "; //> < == >= <= != and or ";

            Variables = new Dictionary<string, string>(); //for storing variables
            Procedures = new Dictionary<string, string>(); //for storing procedures

            Board = board;
            InfoBlock = textblock;
            Container = container;

            Reset();
        }

        public void Reset()
        {
            Variables.Clear();
            Procedures.Clear();

            Angle = 90;
            X1 = Board.Width / 2;
            Y1 = Board.Height / 2;
            X2 = X1;
            Y2 = Y1;

            PenStrokeThickness = 2;
            FontSize = 20;
            PenColor = Colors.Black;
            PenStatus = true;
            TurtleStatus = true;
            BgColor = Colors.Black;
            Turtle = new Polygon();
            DrawTurtle();
            Message = "";
        }

        public void DrawTurtle()
        {
            if (TurtleStatus == true)
            {
                if (Board.Children.Contains(Turtle))
                    Board.Children.Remove(Turtle);

                Turtle.Points.Clear();
                Turtle.StrokeThickness = PenStrokeThickness;
                Turtle.Stroke = new SolidColorBrush(PenColor);
                Turtle.Points.Add(new Point(X1 + Math.Sqrt(200.0) * Math.Cos(Angle * Math.PI / 180.0), Y1 - Math.Sqrt(200.0) * Math.Sin(Angle * Math.PI / 180.0)));
                Turtle.Points.Add(new Point(X1 + Math.Sqrt(50.0) * Math.Cos((Angle - 120.0) * Math.PI / 180.0), Y1 - Math.Sqrt(50.0) * Math.Sin((Angle - 120.0) * Math.PI / 180.0)));
                Turtle.Points.Add(new Point(X1 + Math.Sqrt(50.0) * Math.Cos((Angle + 120.0) * Math.PI / 180.0), Y1 - Math.Sqrt(50.0) * Math.Sin((Angle + 120.0) * Math.PI / 180.0)));

                Board.Children.Add(Turtle);
            }
        }

        public void AdjustAngle()
        {
            while (Angle >= 360)
            {
                Angle -= 360;
            }

            while (Angle < 0)
            {
                Angle += 360;
            }
        }

        private bool isValidName(string name)
        {
            if (!char.IsLetter(name[0]) && name[0] != '_')
                return false;
            for (int i = 1; i < name.Length; ++i)
                if (!char.IsLetter(name[i]) && !char.IsDigit(name[i]) && name[i] != '_')
                    return false;
            return true;
        }

        private bool isReservedToken(string token)
        {
            token = " " + token.ToLower() + " ";
            if (Token_0.Contains(token) || Token_1.Contains(token) || Token_X.Contains(token))
                return true;
            else
                return false;
        }

        #region Drawing Functions

        #region Functions with no parameters

        public void ClearScreen()
        {
            Board.Children.Clear();
        }

        public void Home()
        {
            Line myLine = new Line();
            myLine.X1 = X1;
            myLine.Y1 = Y1;
            X2 = Board.Width / 2;
            Y2 = Board.Height / 2;
            myLine.X2 = X2;
            myLine.Y2 = Y2;

            if (PenStatus == true)
            {
                myLine.StrokeThickness = PenStrokeThickness;
                myLine.Stroke = new SolidColorBrush(PenColor);
                Board.Children.Add(myLine);
            }
            X1 = X2;
            Y1 = Y2;
        }

        public void PenUp()
        {
            PenStatus = false;
        }

        public void PenDown()
        {
            PenStatus = true;
        }

        public void ShowTurtle()
        {
            if (!Board.Children.Contains(Turtle))
                Board.Children.Remove(Turtle);
            TurtleStatus = true;
        }

        public void HideTurtle()
        {
            if (Board.Children.Contains(Turtle))
                Board.Children.Remove(Turtle);
            TurtleStatus = false;
        }

        #endregion

        #region Functions with one parameter

        public void Forward(String arg)
        {
            try
            {
                double x;
                x = double.Parse(arg);
            }
            catch (FormatException e)
            {
                Message = "Error! <distance> should be a Number for [FD]";
                return;
            }

            Line myLine = new Line();
            myLine.X1 = X1;
            myLine.Y1 = Y1;
            X2 = (double)(X1 + Math.Cos(Angle * Math.PI / 180.0) * double.Parse(arg));
            Y2 = (double)(Y1 - Math.Sin(Angle * Math.PI / 180.0) * double.Parse(arg));
            myLine.X2 = X2;
            myLine.Y2 = Y2;

            if (PenStatus == true)
            {
                myLine.StrokeThickness = PenStrokeThickness;
                myLine.Stroke = new SolidColorBrush(PenColor);
                Board.Children.Add(myLine);
            }
            X1 = X2;
            Y1 = Y2;
        }

        public void Backward(String arg)
        {
            try
            {
                double x;
                x = double.Parse(arg);
            }
            catch (FormatException e)
            {
                Message = "Error! <distance> should be a Number for [BK]";
                return;
            }

            Line myLine = new Line();
            myLine.X1 = X1;
            myLine.Y1 = Y1;
            X2 = (double)(X1 - Math.Cos(Angle * Math.PI / 180.0) * double.Parse(arg));
            Y2 = (double)(Y1 + Math.Sin(Angle * Math.PI / 180.0) * double.Parse(arg));
            myLine.X2 = X2;
            myLine.Y2 = Y2;
            if (PenStatus == true)
            {
                myLine.StrokeThickness = PenStrokeThickness;
                myLine.Stroke = new SolidColorBrush(PenColor);
                Board.Children.Add(myLine);
            }
            X1 = X2;
            Y1 = Y2;
        }

        public void Right(String arg)
        {
            try
            {
                double x;
                x = double.Parse(arg);
            }
            catch (FormatException e)
            {
                Message = "Error! <Angle> should be a Number for [RT]";
                return;
            }

            Angle -= double.Parse(arg);
        }

        public void Left(String arg)
        {
            try
            {
                double x;
                x = double.Parse(arg);
            }
            catch (FormatException e)
            {
                Message = "Error! <Angle> should be a Number for [LT]";
                return;
            }

            Angle += double.Parse(arg);
        }

        public void PenColorChange(String arg)
        {
            switch (arg)
            {
                case "red": PenColor = Colors.Red; break;
                case "orange": PenColor = Colors.Orange; break;
                case "brown": PenColor = Colors.Brown; break;
                case "magenta": PenColor = Colors.Magenta; break;
                case "green": PenColor = Colors.Green; break;
                case "yellow": PenColor = Colors.Yellow; break;
                case "blue": PenColor = Colors.Blue; break;
                case "black": PenColor = Colors.Black; break;
                case "white": PenColor = Colors.White; break;
                case "gray": PenColor = Colors.Gray; break;
                case "cyan": PenColor = Colors.Cyan; break;
                case "darkgray": PenColor = Colors.DarkGray; break;
                case "lightgray": PenColor = Colors.LightGray; break;
                case "purple": PenColor = Colors.Purple; break;
                case "transparent": PenColor = Colors.Transparent; break;
                //case "pink": PenColor = Colors.Pink; break;
                //case "sky": PenColor = Colors.SkyBlue; break;
                //case "violet": PenColor = Colors.Violet; break;
                //case "snow": PenColor = Colors.Snow; break;
                //case "teal": PenColor = Colors.Teal; break;
                //case "tomato": PenColor = Colors.Tomato; break;
                //case "silver": PenColor = Colors.Silver; break;
                //case "gold": PenColor = Colors.Gold; break;
                //case "aqua": PenColor = Colors.Aqua; break;
                default: Message = "Error : Unknown Color [" + arg + "]"; break;
            }
        }

        public void BgColorChange(String arg)
        {
            switch (arg)
            {
                case "red": BgColor = Colors.Red; break;
                case "orange": BgColor = Colors.Orange; break;
                case "brown": BgColor = Colors.Brown; break;
                case "magenta": BgColor = Colors.Magenta; break;
                case "green": BgColor = Colors.Green; break;
                case "yellow": BgColor = Colors.Yellow; break;
                case "blue": BgColor = Colors.Blue; break;
                case "black": BgColor = Colors.Black; break;
                case "white": BgColor = Colors.White; break;
                case "gray": BgColor = Colors.Gray; break;
                case "cyan": BgColor = Colors.Cyan; break;
                case "darkgray": BgColor = Colors.DarkGray; break;
                case "lightgray": BgColor = Colors.LightGray; break;
                case "purple": BgColor = Colors.Purple; break;
                case "transparent": BgColor = Colors.Transparent; break;
                //case "pink": BgColor = Colors.Pink; break;
                //case "sky": BgColor = Colors.SkyBlue; break;
                //case "violet": BgColor = Colors.Violet; break;
                //case "snow": BgColor = Colors.Snow; break;
                //case "teal": BgColor = Colors.Teal; break;
                //case "tomato": BgColor = Colors.Tomato; break;
                //case "silver": BgColor = Colors.Silver; break;
                //case "gold": BgColor = Colors.Gold; break;
                //case "aqua": BgColor = Colors.Aqua; break;
                default: Message = "Error : Unknown Color [" + arg + "]"; break;
            }
            if (!Message.Contains("Error"))
                Container.Background = new SolidColorBrush(BgColor);
        }

        //public async void UseFile(string arg)
        //{
        //    StorageFolder folder = null;
        //    String code = "";

        //    try
        //    {
        //        folder = await KnownFolders.DocumentsLibrary.GetFolderAsync("AwesomeLogo");
        //        folder = await folder.GetFolderAsync("Scripts");
        //    }
        //    catch (Exception e)
        //    {
        //        Message = "Error : Could not access the [Scripts] folder";
        //        return;
        //    }

        //    try
        //    {
        //        StorageFile file = await folder.GetFileAsync(arg + ".lgo");
        //        IBuffer buffer = await FileIO.ReadBufferAsync(file);

        //        using (DataReader dataReader = DataReader.FromBuffer(buffer))
        //        {
        //            code = dataReader.ReadString(buffer.Length);
        //        }

        //        Execute(code);
        //        Message = "Success : [" + arg + "] used.";
        //    }
        //    catch (Exception e)
        //    {
        //        Message = "Error : File [" + arg + "] not found.";
        //    }
        //}

        private void WriteText(String arg)
        {
            Thickness margin = new Thickness((X1 - (10 + arg.Length * FontSize) / 4), (Y1 - FontSize / 2), 0, 0);
            TextBlock textblock = new TextBlock();

            textblock.Text = arg;
            textblock.FontSize = FontSize;
            textblock.Foreground = new SolidColorBrush(PenColor);

            textblock.Margin = margin;
            textblock.VerticalAlignment = VerticalAlignment.Center;
            textblock.HorizontalAlignment = HorizontalAlignment.Center;

            Board.Children.Add(textblock);
        }

        private void FontSizeChange(string arg)
        {
            try
            {
                double size;
                size = double.Parse(arg);
                if (size <= 0)
                    throw new Exception();
                FontSize = size;
            }
            catch (Exception e)
            {
                Message = "Error! <Size> should be a Number for [FS]";
            }
        }

        private void PenStrokeChange(string arg)
        {
            try
            {
                int thick;
                thick = int.Parse(arg);
                if (thick <= 0)
                    throw new Exception();
                PenStrokeThickness = thick;
            }
            catch (Exception e)
            {
                Message = "Error! <Size> should be a Positive Integer for [STR]";
            }
        }

        #endregion

        #region Miscellenous Functions

        private void Moveto(string x, string y)
        {
            Line myLine = new Line();
            myLine.X1 = X1;
            myLine.Y1 = Y1;


            X2 = Board.Width / 2 + double.Parse(x);
            Y2 = Board.Height / 2 - double.Parse(y);

            myLine.X2 = X2;
            myLine.Y2 = Y2;

            if (PenStatus == true)
            {
                myLine.StrokeThickness = PenStrokeThickness;
                myLine.Stroke = new SolidColorBrush(PenColor);
                Board.Children.Add(myLine);
            }

            X1 = X2;
            Y1 = Y2;
        }

        private void Repeat(string times, string code)
        {
            int count = 0;
            try
            {
                count = int.Parse(times);
                for (int i = 0; i < count && !Message.Contains("Error"); ++i)
                {
                    Execute(code);
                    Update();
                }
            }
            catch (Exception e)
            {
                Message = "Error : <Times> must be a Positive Integer for [REPEAT]!";
            }
        }

        private void DefineProcedure(string procname, string body)
        {
            if (!isValidName(procname))
            {
                Message = "Error : Invalid <Procedure> name. [ " + procname + " ]";
                return;
            }
            else if (isReservedToken(procname))
            {
                Message = "Error : Reserved tokens cannot be overridden. [ " + procname + " ]";
                return;
            }

            if (Procedures.ContainsKey(procname))
                Procedures[procname] = body;
            else
                Procedures.Add(procname, body);

            Message = "Success : <Procedure> [" + procname + "] defined!";
        }

        private void DefineVariables(string varname, string value)
        {
            if (!isValidName(varname))
            {
                Message = "Error : Invalid <Variable> name. [ " + varname + " ]";
                return;
            }
            else if (isReservedToken(varname))
            {
                Message = "Error : Reserved tokens cannot be overridden. [ " + varname + " ]";
                return;
            }

            while (Variables.ContainsKey(value))
                value = Variables[value];

            if (Variables.ContainsKey(varname))
                Variables[varname] = value;
            else
                Variables.Add(varname, value);

            Message = "Success : <Variable> [" + varname + "] defined!";
        }

        #endregion

        #endregion

        public void Execute(string script)
        {
            string code = "";

            for (int i = 0; i < script.Length; ++i)
            {
                char ch = script[i];
                if (char.IsControl(ch) || char.IsWhiteSpace(ch) || char.IsSeparator(ch) || ch.Equals('\n'))
                    code += ' ';
                else
                    code += ch;
            }

            char[] separators = { ' ', '\n' };

            String[] Tokens = code.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            int maxPar = Tokens.Length;

            String curToken = "";

            for (int curPar = 0; curPar < maxPar && !Message.Contains("Error"); )
            {
                curToken = Tokens[curPar];

                if (curToken.Length == 0)
                {
                    curPar++;
                }
                else if (Token_0.Contains(" " + curToken + " "))
                {
                    curPar++;

                    switch (curToken)
                    {
                        case "cs": ClearScreen(); break;
                        case "home": Home(); break;
                        case "pu": PenUp(); break;
                        case "pd": PenDown(); break;
                        case "st": ShowTurtle(); break;
                        case "ht": HideTurtle(); break;
                    }
                }
                else if (Token_1.Contains(" " + curToken + " "))
                {
                    String curArg = "";

                    try
                    {
                        curArg = Tokens[curPar + 1];

                        if (curArg.Equals("["))
                        {
                            string expression = "";

                            int i = curPar + 1;

                            for (; i < maxPar && Tokens[i] != "]"; ++i)
                                expression += Tokens[i] + " ";


                            if (i > maxPar)
                            {
                                Message = "Error : ']' missing after <Expression>";
                                return;
                            }

                            curArg = Evaluate(expression);

                            Tokens[curPar + 1] = curArg;

                            for (int j = curPar + 2; j <= i; ++j)
                                Tokens[j] = "";

                        }

                        while (Variables.ContainsKey(curArg))
                            curArg = Variables[curArg];

                    }
                    catch (Exception e)
                    {
                        Message = "Error : Parameter missing for [" + curToken + "]\n";
                        return;
                    }

                    switch (curToken)
                    {
                        case "fd": Forward(curArg); break;
                        case "bk": Backward(curArg); break;
                        case "lt": Left(curArg); break;
                        case "rt": Right(curArg); break;
                        case "pc": PenColorChange(curArg); break;
                        case "bg": BgColorChange(curArg); break;
                        case "str": PenStrokeChange(curArg); break;
                        //case "use": UseFile(curArg); break;
                        case "fs": FontSizeChange(curArg); break;
                        case "wt": WriteText(curArg); break;
                    }

                    curPar += 2;
                }
                else if (Token_X.Contains(" " + curToken + " "))
                {
                    if (curToken.Equals("repeat"))
                    {
                        string times;
                        string inlinecode = "";

                        try
                        {
                            times = Tokens[curPar + 1];

                            while (Variables.ContainsKey(times))
                                times = Variables[times];
                        }
                        catch (Exception e)
                        {
                            Message = "Error : <Times> not found!";
                            return;
                        }

                        int j, br = 1;

                        curPar++;

                        try
                        {
                            if (Tokens[curPar + 1] != "[")
                            {
                                Message = "Error : '[' missing for [REPEAT]";
                                return;
                            }
                        }
                        catch (Exception e)
                        {
                            Message = "Error : '[' missing for [REPEAT]";
                            return;
                        }

                        for (j = curPar + 2; j < maxPar && br > 0; j++)
                        {
                            if (Tokens[j] == "[")
                                br++;
                            else if (Tokens[j] == "]")
                                br--;

                            inlinecode += Tokens[j] + " ";
                        }

                        if (br != 0)
                        {
                            Message = "Error : ']' missing for REPEAT.";
                            return;
                        }

                        curPar = j;

                        inlinecode = inlinecode.Substring(0, inlinecode.Length - 2);

                        Repeat(times, inlinecode);
                    }
                    else if (curToken.Equals("to"))
                    {
                        string proc;
                        string body = "";
                        //string[] args;

                        try
                        {
                            proc = Tokens[curPar + 1];
                        }
                        catch (Exception e)
                        {
                            Message = "Error : <Procedure Name> not found!";
                            return;
                        }

                        // get Arguments here

                        try
                        {
                            if (Tokens[curPar + 2] != "[")
                                throw new Exception();
                        }
                        catch (Exception e)
                        {
                            Message = "Error : '[' missing for [TO]";
                            return;
                        }

                        int j, br = 0;

                        for (j = curPar + 3; j < maxPar && Tokens[j] != "]" || br > 0; j++)
                        {
                            if (Tokens[j] == "[")
                                br++;
                            else if (Tokens[j] == "]")
                                br--;

                            body += Tokens[j] + " ";
                        }

                        if (br > 0)
                        {
                            Message = "Error : ']' missing for [TO].";
                            return;
                        }
                        curPar = j + 1;
                        DefineProcedure(proc, body);
                    }
                    else if (curToken.Equals("var"))
                    {
                        try
                        {
                            string varname = Tokens[curPar + 1];
                            if (Tokens[curPar + 2] == "=")
                            {
                                string value = Tokens[curPar + 3];
                                DefineVariables(varname, value);
                                curPar = curPar + 4;
                            }
                            else
                            {
                                Message = "Error : '=' token is missing for [VAR]";
                                return;
                            }
                        }
                        catch (Exception e)
                        {
                            Message = "Error : <Variable Name> or <Variable Value> is missing!";
                            return;
                        }
                    }
                }
                else if (curToken.Equals("mov"))
                {
                    string x, y;
                    try
                    {
                        x = Tokens[curPar + 1];
                        y = Tokens[curPar + 2];
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        Message = "Error : <X> or <Y> is missing for [MOV].";
                        return;
                    }

                    while (Variables.ContainsKey(x))
                        x = Variables[x];
                    while (Variables.ContainsKey(y))
                        y = Variables[y];

                    if (isNumber(x) && isNumber(y))
                        Moveto(x, y);
                    else
                        Message = "Error : <X> and <Y> must be numbers";
                    curPar += 3;
                }
                else if (Procedures.ContainsKey(curToken))
                {
                    Execute(Procedures[curToken]);
                    ++curPar;
                }
                else if (curToken.Equals("["))
                {
                    string expression = "";

                    int i = curPar + 1;

                    for (; i < maxPar && Tokens[i] != "]"; ++i)
                    {
                        expression += Tokens[i] + " ";
                    }

                    if (i >= maxPar)
                    {
                        Message = "Error : ']' missing after <Expression>";
                        return;
                    }

                    Tokens[curPar] = Evaluate(expression);

                    for (int j = curPar + 1; j <= i; ++j)
                        Tokens[j] = "";
                }
                else if (Variables.ContainsKey(curToken))
                {
                    while (Variables.ContainsKey(curToken))
                        curToken = Variables[curToken];

                    Tokens[curPar] = curToken;
                }
                else
                {
                    if (isNumber(curToken))
                        Message = "Success : Value is " + Tokens[curPar].ToString();
                    else
                        Message = "Error : Unknown Token [" + curToken + "] found!";
                    return;
                }
            }
        }

        public void Update()
        {
            DrawTurtle();
            AdjustAngle();
            if (InfoBlock != null)
                InfoBlock.Text = "(" + (int)(X1 - Board.Width / 2) + "," + -(int)(Y1 - Board.Height / 2) + "):" + (int)(Angle);
            Board.UpdateLayout();
        }

        private string Evaluate(string expression)
        {
            char[] separators = { ' ', '\n' };
            expression = expression.Replace("[", " [ ").Replace("]", " ] ").Replace("+", " + ").Replace("-", " - ").Replace("*", " * ").Replace("/", " / ").Replace("=", " = ");

            String[] exp = expression.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            if (exp.Length == 1)
                return expression;

            Stack<string> var = new Stack<string>();
            Stack<string> opr = new Stack<string>();

            string var1, var2, result;

            for (int i = 0; i < exp.Length; ++i)
            {
                if (isNumber(exp[i]) || isValidName(exp[i]))
                    var.Push(exp[i]);
                else if (Operator_2.Contains(exp[i]))
                {
                    if (opr.Count == 0)
                        opr.Push(exp[i]);
                    else if (Precedence(exp[i]) > Precedence(opr.Peek()))
                        opr.Push(exp[i]);
                    else
                    {
                        while (opr.Count > 0 && (Precedence(exp[i]) <= Precedence(opr.Peek())))
                        {
                            string operation = opr.Pop();
                            try
                            {
                                var2 = var.Pop();
                                var1 = var.Pop();
                                result = Solve(var1, operation, var2);
                                var.Push(result);
                            }
                            catch (Exception e)
                            {
                                Message = "Error : Wrong <Expression> [ " + expression + "]";
                                return "";
                            }
                        }
                        opr.Push(exp[i]);
                    }
                }
            }

            while (opr.Count > 0)
            {
                try
                {
                    string operation = opr.Pop();
                    var2 = var.Pop();
                    var1 = var.Pop();
                    result = Solve(var1, operation, var2);
                    var.Push(result);
                }
                catch (Exception e)
                {
                    Message = "Error : <Operand> missing in <Expression> [ " + expression + "]";
                    return "";
                }
            }

            if (var.Count == 1)
                return var.Pop();
            else
            {
                Message = "Error : Wrong <Expression> [ " + expression + "]";
                return "";
            }
        }

        int Precedence(string opr)
        {
            int t;
            switch (opr)
            {
                //case "^": t = 4; break;
                case "/":
                case "*": t = 3; break;
                case "+":
                case "-": t = 2; break;
                case "=": t = 1; break;
                default: t = 0; break;
            }
            return t;
        }

        string Solve(string var1, string opr, string var2)
        {
            string varname = var1;

            while (Variables.ContainsKey(var1))
                var1 = Variables[var1];

            while (Variables.ContainsKey(var2))
                var2 = Variables[var2];

            double val1, val2, value = 0;

            try
            {
                val1 = double.Parse(var1);
                val2 = double.Parse(var2);
            }
            catch (Exception e)
            {
                Message = "Error : <Operands> must be a Number for Operation [" + opr + "]!";
                return "";
            }

            switch (opr)
            {
                case "+": value = val1 + val2; break;

                case "-": value = val1 - val2; break;

                case "*": value = val1 * val2; break;

                case "/":
                    try
                    {
                        value = val1 / val2;
                    }
                    catch (Exception e)
                    {
                        Message = "Error : Cannot divide by Zero!";
                        return "";
                    }
                    break;

                case "=":
                    if (Variables.ContainsKey(varname))
                    {
                        Variables[varname] = val2.ToString();
                        return "";
                    }
                    else
                    {
                        Message = "Error : <Variable> not defined [" + varname + "]";
                        return "";
                    }
            }

            return value.ToString();
        }

        private bool isNumber(string number)
        {
            try
            {
                double x = Convert.ToDouble(number);
                return true;
            }
            catch (FormatException e)
            {
                return false;
            }
        }
    }
}
