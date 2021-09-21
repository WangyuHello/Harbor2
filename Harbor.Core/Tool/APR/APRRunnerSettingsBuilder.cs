using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cake.Common.IO;
using Cake.Core;
using Cake.Core.IO;
using Harbor.Common.Model;
using Harbor.Common.Project;
using Harbor.Core.Tool.APR.Model;
using Newtonsoft.Json.Linq;
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace Harbor.Core.Tool.APR
{
    public class APRRunnerSettingsBuilder
    {
        public APRRunnerSettings Settings { get; set; } = new APRRunnerSettings();

        private readonly ICakeContext context;

        public APRRunnerSettingsBuilder(ICakeContext context)
        {
            this.context = context;
            Settings.Context = context;
        }

        public APRRunnerSettingsBuilder ProjectInfo(ProjectInfo _)
        {
            return this;
        }

        public APRRunnerSettingsBuilder SynProjectPath(string _)
        {
            return this;
        }

        public APRRunnerSettingsBuilder Verilog(FilePathCollection verilog)
        {
            Settings.Verilog = verilog;
            return this;
        }

        public APRRunnerSettingsBuilder Verilog(string pattern)
        {
            var vs = context.GetFiles(pattern);
            Settings.Verilog = vs;
            return this;
        }

        public APRRunnerSettingsBuilder AddAdditionalRefLib(DirectoryPathCollection reflibs)
        {
            if (Settings.AdditionalRefLib == null)
            {
                Settings.AdditionalRefLib = new DirectoryPathCollection();
            }

            Settings.AdditionalRefLib.Add(reflibs);
            return this;
        }

        public APRRunnerSettingsBuilder AddAdditionalRefLib(string reflib)
        {
            if (Settings.AdditionalRefLib == null)
            {
                Settings.AdditionalRefLib = new DirectoryPathCollection();
            }
            
            Settings.AdditionalRefLib.Add(context.MakeAbsolute(new DirectoryPath(reflib)));
            return this;
        }

        public APRRunnerSettingsBuilder AddTimingDB(FilePathCollection db)
        {
            if (Settings.AdditionalTimingDb == null)
            {
                Settings.AdditionalTimingDb = new FilePathCollection();
            }
            Settings.AdditionalTimingDb.Add(db);
            return this;
        }

        public APRRunnerSettingsBuilder AddTimingDB(string pattern)
        {
            if (Settings.AdditionalTimingDb == null)
            {
                Settings.AdditionalTimingDb = new FilePathCollection();
            }
            var dbs = context.GetFiles(pattern);
            Settings.AdditionalTimingDb.Add(dbs);
            return this;
        }

        public APRRunnerSettingsBuilder UseICC(bool UseICC)
        {
            Settings.UseICC = UseICC;
            return this;
        }

        public APRRunnerSettingsBuilder UseICC()
        {
            return UseICC(true);
        }

        public APRRunnerSettingsBuilder UseICC2(bool UseICC2)
        {
            Settings.UseICC2 = UseICC2;
            return this;
        }

        public APRRunnerSettingsBuilder UseInnovus(bool UseInnovus)
        {
            Settings.UseInnovus = UseInnovus;
            return this;
        }

        public APRRunnerSettingsBuilder MaxRoutingLayer(int routing, int preRoute)
        {
            Settings.MaxRoutingLayer = routing;
            Settings.MaxPreRouteLayer = preRoute;
            return this;
        }

        public APRRunnerSettingsBuilder FloorPlanOnly(bool floorPlanOnly = true)
        {
            Settings.FloorPlanOnly = floorPlanOnly;
            return this;
        }

        public APRRunnerSettingsBuilder AddPG()
        {
            Settings.AddPG = true;
            return this;
        }

        public APRRunnerSettingsBuilder OpenGUI()
        {
            Settings.OpenGUI = true;
            return this;
        }

        public APRRunnerSettingsBuilder FormalVerify()
        {
            Settings.FormalVerify = true;
            return this;
        }

        public APRRunnerSettingsBuilder LVS()
        {
            Settings.LVS = true;
            return this;
        }

        public APRRunnerSettingsBuilder FloorPlan(Action<FloorPlanSettingsBuilder> FloorPlan)
        {
            var floorplanbuilder = new FloorPlanSettingsBuilder();
            FloorPlan?.Invoke(floorplanbuilder);
            Settings.FloorPlanSettings = floorplanbuilder.Settings;
            return this;
        }

        public APRRunnerSettingsBuilder Pin(Action<PinSettingsBuilder> pin)
        {
            var pinBuilder = new PinSettingsBuilder(context);
            pin?.Invoke(pinBuilder);
            Settings.PinSettings = pinBuilder.Settings;
            return this;
        }

        public APRRunnerSettingsBuilder Place(Action<PlaceSettingsBuilder> action)
        {
            var builder = new PlaceSettingsBuilder();
            action?.Invoke(builder);
            Settings.PlaceSettings = builder.Settings;
            return this;
        }
    }

    public class FloorPlanSettingsBuilder
    {
        public FloorPlanSettings Settings { get; set; } = new FloorPlanSettings();

        public FloorPlanSettingsBuilder LeftIO2Core(double LeftIO2Core)
        {
            Settings.LeftIO2Core = LeftIO2Core;
            return this;
        }

        public FloorPlanSettingsBuilder RightIO2Core(double RightIO2Core)
        {
            Settings.RightIO2Core = RightIO2Core;
            return this;
        }

        public FloorPlanSettingsBuilder TopIO2Core(double TopIO2Core)
        {
            Settings.TopIO2Core = TopIO2Core;
            return this;
        }

        public FloorPlanSettingsBuilder BottomIO2Core(double BottomIO2Core)
        {
            Settings.BottomIO2Core = BottomIO2Core;
            return this;
        }

        public FloorPlanSettingsBuilder Type(FloorPlanType type)
        {
            Settings.FloorPlanType = type;
            return this;
        }

        public FloorPlanSettingsBuilder CoreUtilization(double CoreUtilization)
        {
            Settings.CoreUtilization = CoreUtilization;
            return this;
        }

        public FloorPlanSettingsBuilder AspectRatio(double AspectRatio)
        {
            Settings.AspectRatio = AspectRatio;
            return this;
        }

        public FloorPlanSettingsBuilder HeightWidthRatio(double AspectRatio)
        {
            Settings.AspectRatio = AspectRatio;
            return this;
        }

        public FloorPlanSettingsBuilder CoreWidth(double width)
        {
            Settings.CoreWidth = width;
            return this;
        }

        public FloorPlanSettingsBuilder CoreHeight(double height)
        {
            Settings.CoreHeight = height;
            return this;
        }

        public FloorPlanSettingsBuilder Width(double width)
        {
            Settings.Width = width;
            return this;
        }

        public FloorPlanSettingsBuilder Height(double height)
        {
            Settings.Height = height;
            return this;
        }

        public FloorPlanSettingsBuilder Padding(string margin)
        {
            var parts = margin.Split(' ');
            if (parts.Length == 2)
            {
                //eg. 4 2
                var left_right = double.Parse(parts[0]);
                var top_bottom = double.Parse(parts[1]);
                Settings.LeftIO2Core = left_right;
                Settings.RightIO2Core = left_right;
                Settings.TopIO2Core = top_bottom;
                Settings.BottomIO2Core = top_bottom;
            }
            else if (parts.Length == 4)
            {
                //eg. 4 2 4 2
                var left = double.Parse(parts[0]);
                var top = double.Parse(parts[1]);
                var right = double.Parse(parts[2]);
                var bottom = double.Parse(parts[3]);
                Settings.LeftIO2Core = left;
                Settings.RightIO2Core = right;
                Settings.TopIO2Core = top;
                Settings.BottomIO2Core = bottom;
            }
            return this;
        }

        public FloorPlanSettingsBuilder Padding(double left_right, double top_bottom)
        {
            Settings.LeftIO2Core = left_right;
            Settings.RightIO2Core = left_right;
            Settings.TopIO2Core = top_bottom;
            Settings.BottomIO2Core = top_bottom;
            return this;
        }

        public FloorPlanSettingsBuilder Padding(double left, double top, double right, double bottom)
        {
            Settings.LeftIO2Core = left;
            Settings.RightIO2Core = right;
            Settings.TopIO2Core = top;
            Settings.BottomIO2Core = bottom;
            return this;
        }

        public FloorPlanSettingsBuilder PowerRing(double powerWidth = 1, double groundWidth = 1, double verticalOffset = 0.5,
            double horizontalOffset = 0.5, double verticalSpace = 0.3, double horizontalSpace = 0.3)
        {
            Settings.PowerWidth = powerWidth;
            Settings.GroundWidth = groundWidth;
            Settings.VerticalOffset = verticalOffset;
            Settings.HorizontalOffset = horizontalOffset;
            Settings.VerticalSpace = verticalSpace;
            Settings.HorizontalSpace = horizontalSpace;
            return this;
        }

        public FloorPlanSettingsBuilder PowerStrap(double start = 20, double step = 20, double? stop = null, double width = 2, double? space = null, string layer = "", params string[] nets)
        {
            var powerStrap = new PowerStrapSettings
            {
                Orientation = false,
                Start = start,
                Step = step,
                Stop = stop,
                Width = width,
                Space = space,
                Layer = layer
            };
            if (nets.Length > 0)
            {
                powerStrap.Nets = nets.ToList();
            }
            Settings.PowerStraps.Add(powerStrap);
            return this;
        }

        public FloorPlanSettingsBuilder HorizontalPowerStrap(double start = 20, double step = 20, double? stop = null, double width = 2, double? space = null, string layer = "", params string[] nets)
        {
            var powerStrap = new PowerStrapSettings
            {
                Orientation = true,
                Start = start,
                Step = step,
                Stop = stop,
                Space = space,
                Width = width,
                Layer = layer
            };
            if (nets.Length > 0)
            {
                powerStrap.Nets = nets.ToList();
            }
            Settings.PowerStraps.Add(powerStrap);
            return this;
        }
    }

    public class PinGroupBuilder
    {
        public PinGroupSettings Settings = new();

        public PinGroupBuilder PinSpace(decimal pinSpace)
        {
            Settings.PinSpace = pinSpace;
            return this;
        }

        public PinGroupBuilder PinsSpace(string pinSpace)
        {
            Settings.PinSpace = decimal.Parse(pinSpace);
            return this;
        }

        public PinGroupBuilder PinsSpace(double pinSpace)
        {
            Settings.PinSpace = (decimal)pinSpace;
            return this;
        }

        public PinGroupBuilder Order(int order)
        {
            Settings.Order = order;
            return this;
        }

        public PinGroupBuilder Offset(decimal offset)
        {
            Settings.Offset = offset;
            return this;
        }

        public PinGroupBuilder Offset(string offset)
        {
            Settings.Offset = decimal.Parse(offset);
            return this;
        }

        public PinGroupBuilder Offset(double offset)
        {
            Settings.Offset = (decimal)offset;
            return this;
        }

        public PinGroupBuilder Position(PortPosition position)
        {
            Settings.Position = position;
            return this;
        }

        public PinGroupBuilder Pin(string name, int order = -1)
        {
            Settings.Ports.Add((name, order));
            return this;
        }
    }

    public class PinSettingsBuilder
    {
        public PinSettings Settings { get; set; } = new PinSettings();
        private readonly ICakeContext context;

        public PinSettingsBuilder(ICakeContext context)
        {
            this.context = context;
        }

        public PinSettingsBuilder PinSpace(decimal pinSpace)
        {
            Settings.PinSpace = pinSpace;
            return this;
        }

        public PinSettingsBuilder PinsSpace(string pinSpace)
        {
            Settings.PinSpace = decimal.Parse(pinSpace);
            return this;
        }

        public PinSettingsBuilder PinsSpace(double pinSpace)
        {
            Settings.PinSpace = (decimal) pinSpace;
            return this;
        }

        public PinSettingsBuilder PlaceMode(PinPlaceMode mode)
        {
            Settings.PinPlaceMode = mode;
            return this;
        }

        public PinSettingsBuilder Pin(string name, PortPosition portPosition, int order = -1)
        {
            Settings.PinPair.Add((name.ToUpper(), portPosition, order));
            return this;
        }

        public PinSettingsBuilder Group(Action<PinGroupBuilder> group)
        {
            var pinGroupBuilder = new PinGroupBuilder();
            group?.Invoke(pinGroupBuilder);
            Settings.Groups.Add(pinGroupBuilder.Settings);
            return this;
        }

        public PinSettingsBuilder Constraint(string file)
        {
            Settings.ConstraintFile = context.MakeAbsolute(new FilePath(file));
            return this;
        }
    }

    public class PlaceSettingsBuilder
    {
        public PlaceSettings Settings { get; } = new PlaceSettings();

        public PlaceSettingsBuilder PlaceMacro(string name, string type, double x, double y, Orientation orientation = Orientation.N, bool createRing = false, bool reverseRoutingDirection = false)
        {
            return PlaceMacro(name, type, x, y, (8,8,8,8), orientation, createRing, reverseRoutingDirection);
        }

        public PlaceSettingsBuilder PlaceMacro(string name, string type, double x, double y, (double, double) margin, Orientation orientation = Orientation.N, bool createRing = false, bool reverseRoutingDirection = false)
        {
            return PlaceMacro(name, type, x, y, (margin.Item1, margin.Item2, margin.Item1, margin.Item2), orientation, createRing, reverseRoutingDirection);
        }

        public PlaceSettingsBuilder PlaceMacro(string name, string type, double x, double y, (double, double, double, double) margin, Orientation orientation = Orientation.N, bool createRing = false, bool reverseRoutingDirection = false)
        {
            Settings.MacroPlaceSettings.Add(new MacroPlaceSettings
            {
                Name = name,
                Type = type,
                X = x,
                Y = y,
                Orientation = orientation,
                MarginLeft = margin.Item1,
                MarginRight = margin.Item3,
                MarginTop = margin.Item2,
                MarginBottom = margin.Item4,
                CreateRing = createRing,
                ReverseRoutingDirection = reverseRoutingDirection
            });
            return this;
        }
    }
}
