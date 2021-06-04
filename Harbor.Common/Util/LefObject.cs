using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable UnusedMember.Global

namespace Harbor.Common.Util
{
    public class LefObject
    {
        public string LefFile { get; set; }
        public Dictionary<string, Macro> Macros { get; } = new();
        private Dictionary<string, Layer> _layerDict = new();
        private Dictionary<string, Via> _viaDict = new();
        private Stack<Statement> _stack = new();
        private List<Statement> _statements = new();
        private double CellHeight = -1;

        public static LefObject Parse(string lefFile)
        {
            var lef = new LefObject
            {
                LefFile = lefFile
            };
            lef.Parse();
            return lef;
        }

        //private double GetCellHeight()
        //{

        //}

        private void Parse()
        {
            var f = File.ReadAllLines(LefFile);
            foreach (var line in f)
            {
                var info = StrToList(line.Trim());
                if (info.Length != 0)
                {
                    // if info is a blank line, then move to next line
                    // check if the program is processing a statement
                    (int code, Statement s) nextState;
                    if (_stack.Count != 0)
                    {
                        var curState = _stack.Peek();
                        nextState = curState.ParseNext(info);
                    }
                    else
                    {
                        var curState = new Statement();
                        nextState = curState.ParseNext(info);
                    }

                    switch (nextState.code)
                    {
                        case 0:
                            break;
                        case 1:
                        {
                            //remove the done statement from stack, and add it to the statements
                            if (_stack.Count != 0)
                            {
                                var doneObj = _stack.Pop();
                                switch (doneObj)
                                {
                                    case Macro m:
                                        Macros.Add(m.Name, m);
                                        break;
                                    case Layer l:
                                        _layerDict.Add(l.Name, l);
                                        break;
                                    case Via v:
                                        _viaDict.Add(v.Name, v);
                                        break;
                                }
                                _statements.Add(doneObj);
                            }

                            break;
                        }
                        case -1:
                            break;
                        default:
                            _stack.Push(nextState.s);
                            break;
                    }
                }
            }


        }

        public string[] StrToList(string s) => s.Split(' ');

        public class Statement
        {
            public string Name { get; }

            protected Statement(string name)
            {
                Name = name;
            }

            public Statement()
            {

            }

            internal virtual (int code, Statement s) ParseNext(string[] data)
            {
                switch (data[0])
                {
                    case "MACRO":
                    {
                        var name = data[1];
                        var newState = new Macro(name);
                        return (999, newState);
                    }
                    case "LAYER" when data.Length == 2:
                    {
                        var name = data[1];
                        var newState = new Layer(name);
                        return (999, newState);
                    }
                    case "VIA":
                    {
                        var name = data[1];
                        var newState = new Via(name);
                        return (999, newState);
                    }
                    case "PROPERTYDEFINITIONS":
                    {
                        var newState = new PropertyDefinitions();
                        return (999, newState);
                    }
                    case "END":
                        return (1, null);
                    default:
                        return (0, null);
                }
            }
        }

        public class Macro : Statement
        {
            private readonly Dictionary<string, object> _info = new();
            private Dictionary<string, Pin> _pinDict = new();

            public string Foreign => _info["FOREIGN"] as string;
            public string Symmetry => _info["SYMMETRY"] as string;
            public (double w, double h)? Size => _info["SIZE"] as (double w, double h)?;
            public string Class => _info["CLASS"] as string;
            public List<Pin> Pins => _info["PIN"] as List<Pin>;

            public Macro(string name) : base(name)
            {
            }

            internal override (int code, Statement s) ParseNext(string[] data)
            {
                switch (data[0])
                {
                    case "CLASS":
                        _info["CLASS"] = data[1];
                        break;
                    case "ORIGIN":
                        var xCor = double.Parse(data[1]);
                        var yCor = double.Parse(data[2]);
                        _info["ORIGIN"] = (xCor, yCor);
                        break;
                    case "FOREIGN":
                        _info["FOREIGN"] = data[1..];
                        break;
                    case "SIZE":
                        var width = double.Parse(data[1]);
                        var height = double.Parse(data[3]);
                        _info["SIZE"] = (width, height);
                        break;
                    case "SYMMETRY":
                        _info["SYMMETRY"] = data[1..];
                        break;
                    case "SITE":
                        _info["SITE"] = data[1];
                        break;
                    case "PIN":
                        var newPin = new Pin(data[1]);
                        _pinDict[data[1]] = newPin;
                        if (_info.ContainsKey("PIN"))
                        {
                            ((List<Pin>)_info["PIN"]).Add(newPin);
                        }
                        else
                        {
                            _info["PIN"] = new List<Pin> {newPin};
                        }

                        return (999, newPin);
                    case "OBS":
                        var newObs = new Obs();
                        _info["OBS"] = newObs;
                        return (999, newObs);
                    case "PROPERTY":
                        _info[data[1]] = data[2];
                        break;
                    case "END":
                        if (data[1] == Name)
                        {
                            return (1, null);
                        }
                        else
                        {
                            return (-1, null);
                        }
                }
                return (0, null);
            }

        }

        public class Pin : Statement
        {
            private readonly Dictionary<string, object> _info = new();

            public string Direction
            {
                get => _info["DIRECTION"] as string;
                set => _info["DIRECTION"] = value;
            }
            public string Use
            {
                get => _info["USE"] as string;
                set => _info["USE"] = value;
            }
            public string ShapeS
            {
                get => _info["SHAPE"] as string;
                set => _info["SHAPE"] = value;
            }

            public List<Port> Ports
            {
                get => _info["PORT"] as List<Port>;
                set => _info["PORT"] = value;
            }

            public Pin(string name) : base(name)
            {
            }

            internal override (int code, Statement s) ParseNext(string[] data)
            {
                switch (data[0])
                {
                    case "DIRECTION":
                        _info["DIRECTION"] = data[1];
                        break;
                    case "USE":
                        _info["USE"] = data[1];
                        break;
                    case "PORT":
                        var newPort = new Port();
                        if (_info.ContainsKey("PORT"))
                        {
                            ((List<Port>)_info["PORT"]).Add(newPort);
                        }
                        else
                        {
                            _info["PORT"] = new List<Port> { newPort };
                        }
                        return (999, newPort);
                    case "SHAPE":
                        _info["SHAPE"] = data[1];
                        break;
                    case "ANTENNAGATEAREA":
                        _info["ANTENNAGATEAREA"] = double.Parse(data[1]);
                        break;
                    case "ANTENNADIFFAREA":
                        _info["ANTENNADIFFAREA"] = double.Parse(data[1]);
                        break;
                    case "END":
                        if (data[1] == Name)
                        {
                            return (1, null);
                        }
                        else
                        {
                            return (-1, null);
                        }
                }
                return (0, null);
            }
        }

        public class Port : Statement
        {
            private Dictionary<string, List<LayerDef>> _info = new();

            internal override (int code, Statement s) ParseNext(string[] data)
            {
                switch (data[0])
                {
                    case "END":
                        return (1, null);
                    case "LAYER":
                        var newLayerDef = new LayerDef(data[1]);
                        if (_info.ContainsKey("LAYER"))
                        {
                            _info["LAYER"].Add(newLayerDef);
                        }
                        else
                        {
                            _info.Add("LAYER", new List<LayerDef> { newLayerDef });
                        }
                        break;
                    case "RECT":
                        _info["LAYER"][^1].AddRect(data);
                        break;
                    case "POLYGON":
                        _info["LAYER"][^1].AddPolygon(data);
                        break;
                }
                return (0, null);
            }

            public Port(string name) : base(name)
            {
            }

            public Port() : base("")
            {

            }
        }

        class Obs : Statement
        {
            private Dictionary<string, List<LayerDef>> _info = new();

            public override string ToString()
            {
                return string.Join(Environment.NewLine, _info["Layer"].Select(layer => typeof(LayerDef) + " " + layer.Name));
            }

            internal override (int code, Statement s) ParseNext(string[] data)
            {
                switch (data[0])
                {
                    case "END":
                        return (1, null);
                    case "LAYER":
                        var newLayerDef = new LayerDef(data[1]);
                        if (_info.ContainsKey("LAYER"))
                        {
                            _info["LAYER"].Add(newLayerDef);
                        }
                        else
                        {
                            _info.Add("LAYER", new List<LayerDef> {newLayerDef});
                        }
                        break;
                    case "RECT":
                        _info["LAYER"][^1].AddRect(data);
                        break;
                    case "POLYGON":
                        _info["LAYER"][^1].AddPolygon(data);
                        break;
                }

                return (0, null);
            }

            public Obs(string name) : base(name)
            {
            }

            public Obs() : base("")
            {
            }
        }

        class Layer : Statement
        {
            private string _layerType;
            private double _spacing;
            private double _width;
            private double _pitch;
            private string _direction;
            private (double, double) _offset;
            private (string, double) _resistance;
            private double _thickness;
            private double _height;
            private (string, double) _capacitance;
            private double _edgeCap;
            private (string, double) _property;


            internal override (int code, Statement s) ParseNext(string[] data)
            {
                switch (data[0])
                {
                    case "TYPE":
                        _layerType = data[1];
                        break;
                    case "SPACINGTABLE":
                        break;
                    case "SPACING":
                        _spacing = double.Parse(data[1]);
                        break;
                    case "WIDTH":
                        _width = double.Parse(data[1]);
                        break;
                    case "PITCH":
                        _pitch = double.Parse(data[1]);
                        break;
                    case "DIRECTION":
                        _direction = data[1];
                        break;
                    case "OFFSET":
                        _offset = (double.Parse(data[1]), double.Parse(data[2]));
                        break;
                    case "RESISTANCE":
                        _resistance = _layerType == "ROUTING" ? (data[1], double.Parse(data[2])) : ("", double.Parse(data[1]));
                        break;
                    case "THICKNESS":
                        _thickness = double.Parse(data[1]);
                        break;
                    case "HEIGHT":
                        _height = double.Parse(data[1]);
                        break;
                    case "CAPACITANCE":
                        _capacitance = (data[1], double.Parse(data[2]));
                        break;
                    case "EDGECAPACITANCE":
                        _edgeCap = double.Parse(data[1]);
                        break;
                    case "PROPERTY":
                        _property = (data[1], double.Parse(data[2]));
                        break;
                    case "END":
                        if (data[1] == Name)
                        {
                            return (1, null);
                        }
                        else
                        {
                            return (-1, null);
                        }
                }
                return (0, null);
            }

            public Layer(string name) : base(name)
            {
            }
        }

        class Via : Statement
        {
            private List<LayerDef> Layers { get; } = new();

            internal override (int code, Statement s) ParseNext(string[] data)
            {
                switch (data[0])
                {
                    case "END":
                        return (1, null);
                    case "LAYER":
                    {
                        var name = data[1];
                        var newLayerDef = new LayerDef(name);
                        Layers.Add(newLayerDef);
                        break;
                    }
                    case "RECT":
                        Layers[^1].AddRect(data); //[-1] means the latest layer
                        break;
                    case "POLYGON":
                        Layers[^1].AddPolygon(data);
                        break;
                }

                return (0, null);
            }

            public Via(string name) : base(name)
            {
            }
        }

        public class LayerDef
        {
            public string Name { get; }
            public List<Shape> Shapes { get; } = new();

            public LayerDef(string name)
            {
                Name = name;
            }

            public void AddRect(string[] data)
            {
                var x0 = double.Parse(data[1]);
                var y0 = double.Parse(data[2]);
                var x1 = double.Parse(data[3]);
                var y1 = double.Parse(data[4]);
                var points = new[] { (x0, y0), (x1, y1) };
                var rect = new Rect(points);
                Shapes.Add(rect);
            }

            public void AddPolygon(string[] data)
            {
                List<(double x, double y)> points = new List<(double x, double y)>();
                for (int idx = 1; idx < data.Length - 2; idx+=2)
                {
                    var x_cor = double.Parse(data[idx]);
                    var y_cor = double.Parse(data[idx + 1]);
                    points.Add((x_cor, y_cor));
                }

                var polygon = new Polygon(points);
                Shapes.Add(polygon);
            }
        }

        public class Rect : Shape
        {
            public (double x, double y)[] Points { get; set; }

            public Rect((double x, double y)[] points)
            {
                Points = points;
            }
        }

        public class Polygon : Shape
        {
            public List<(double x, double y)> Points { get; set; }

            public Polygon(List<(double x, double y)> points)
            {
                Points = points;
            }
        }

        public class Shape
        {
            
        }

        public class PropertyDefinitions : Statement
        {
            private readonly Dictionary<string, object> _info = new();

            internal override (int code, Statement s) ParseNext(string[] data)
            {
                switch (data[0])
                {
                    case "MACRO":
                        _info["MACRO"] = data[1..];
                        break;
                    case "END":
                        if (data[1] == "PROPERTYDEFINITIONS")
                        {
                            return (1, null);
                        }
                        else
                        {
                            return (-1, null);
                        }
                }
                return (0, null);
            }
        }
    }
}
