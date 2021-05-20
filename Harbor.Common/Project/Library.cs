using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
// ReSharper disable InconsistentNaming
// ReSharper disable ClassNeverInstantiated.Local
// ReSharper disable UnusedMember.Global

namespace Harbor.Common.Project
{
    public class NamePathPair
    {
        public string name { get; set; }
        public string path { get; set; }
    }

    public sealed class AllLibrary
    {
        public static string HarborHome => Environment.GetEnvironmentVariable("HARBOR_HOME");
        
        public static List<Library> Libraries { get; set; }

        static AllLibrary()
        {
            var libInfoYmlFile = Path.Combine(HarborHome, "template", "library", "library.info.yml");
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            var libInfo = deserializer.Deserialize<LibraryInfo>(File.OpenText(libInfoYmlFile));
            Libraries = libInfo.libraries.Select(p => new Library(p)).ToList();
        }

        public static Library GetLibrary(string name)
        {
            return Libraries.FirstOrDefault(l => l.Name == name);
        }

        public static Library GetLibrary(ProjectInfo projectInfo)
        {
            var lib = GetLibrary(projectInfo.Library);
            // 根据ProjectInfo中引用的StdCell、IO剔除多余的StdCell、IO

            lib.StdCell = projectInfo.StdCell != null ? lib.StdCell.Where(s => projectInfo.StdCell.Contains(s.Name)).ToList() : new List<Library.LibraryStdCell>();
            lib.Io = projectInfo.Io != null ? lib.Io.Where(s => projectInfo.Io.Contains(s.Name)).ToList() : new List<Library.LibraryIo>();

            return lib;
        }

        private class LibraryInfo
        {
            public NamePathPair[] libraries { get; set; }
        }
    }

    public sealed class Library
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public LibraryPdk Pdk { get; set; }
        public List<LibraryStdCell> StdCell { get; set; }
        public LibraryStdCell PrimaryStdCell => StdCell.First();
        public List<LibraryMemory> Memory { get; set; }
        public List<LibraryIo> Io { get; set; }
        public List<LibraryIp> Ip { get; set; }

        public Library(NamePathPair pair)
        {
            Name = pair.name;

            // 读取PDK
            var pdkPath = Path.Combine(AllLibrary.HarborHome, "template", "library", pair.path, "library.pdk.yml");
            FileInfo pdkFi = new FileInfo(pdkPath);

            var deserializer2 = new DeserializerBuilder()
                .IgnoreUnmatchedProperties()
                .Build();

            Pdk = deserializer2.Deserialize<LibraryPdk>(File.OpenText(pdkFi.FullName));

            // 读取StdCell
            var stdCellPath = Path.Combine(AllLibrary.HarborHome, "template", "library", pair.path, "library.stdcell.yml");
            var stdCellFi = new FileInfo(stdCellPath);

            if (stdCellFi.Exists)
            {
                var stdCell = deserializer2.Deserialize<LibraryStdCell>(File.OpenText(stdCellFi.FullName));
                if (stdCell.refs == null)
                {
                    stdCell.Name = stdCell.cdk_name;
                    // 只有一个StdCell
                    StdCell = new List<LibraryStdCell>
                    {
                        stdCell
                    };
                }
                else
                {
                    StdCell = stdCell.refs.Select(r =>
                    {
                        var stdCellPath2 = Path.Combine(stdCellFi.DirectoryName!, r.path);
                        FileInfo stdCellFi2 = new FileInfo(stdCellPath2);

                        var stdCell2 = deserializer2.Deserialize<LibraryStdCell>(File.OpenText(stdCellFi2.FullName));
                        stdCell2.Name = r.name;
                        return stdCell2;
                    }).ToList();
                }
            }

            // 读取Io
            var ioPath = Path.Combine(AllLibrary.HarborHome, "template", "library", pair.path, "library.io.yml");
            var ioFi = new FileInfo(ioPath);

            if (ioFi.Exists)
            {
                var io = deserializer2.Deserialize<LibraryIo>(File.OpenText(ioFi.FullName));
                if (io.refs == null)
                {
                    io.Name = io.cdk_name;
                    // 只有一个StdCell
                    Io = new List<LibraryIo>()
                    {
                        io
                    };
                }
                else
                {
                    Io = io.refs.Select(r =>
                    {
                        var ioPath2 = Path.Combine(ioFi.DirectoryName!, r.path);
                        FileInfo ioFi2 = new FileInfo(ioPath2);

                        var io2 = deserializer2.Deserialize<LibraryIo>(File.OpenText(ioFi2.FullName));
                        io2.Name = r.name;
                        return io2;
                    }).ToList();
                }
            }

            //读取Ip

        }

        public bool HasStdCell => StdCell != null && StdCell.Count != 0;
        public bool HasIo => Io != null && Io.Count != 0;
        public bool HasMemory => Memory != null && Memory.Count != 0;
        public bool HasIp => Ip != null && Ip.Count != 0;

        public LibraryStdCell GetStdCell(string name)
        {
            return StdCell?.FirstOrDefault(s => s.Name == name);
        }

        public LibraryIo GetIo(string name)
        {
            return Io?.FirstOrDefault(s => s.Name == name);
        }

        public class LibraryPdk
        {
            public string pdk_name { get; set; }
            public string pdk_path { get; set; }
            public string pdk_define { get; set; }
            public string std_cdk_name { get; set; }
            public string std_cdk_path { get; set; }
            public string std_cdk_define { get; set; }
            public string layer_map_in_full_name { get; set; }
            public string layer_map_out_full_name { get; set; }
            public string cds_init_addition { get; set; }
            public string drc_full_name { get; set; }
            public string lvs_full_name { get; set; }
            public string empty_circuit_full_name { get; set; }
            public LayerPair[] layer_map { get; set; }

            public int GetLayerNumber(string layer_name)
            {
                var pair = layer_map.FirstOrDefault(l => l.layer_name == layer_name);
                if (pair == null)
                {
                    return -1;
                }
                return pair.layer_number;
            }
        }

        public class LayerPair
        {
            public string layer_name { get; set; }
            public int layer_number { get; set; }
        }

        public class LibraryStdCell
        {
            public string Name { get; set; }
            public NamePathPair[] refs { get; set; }
            public string description { get; set; }
            public string cdk_name { get; set; }
            public string cdk_path { get; set; }
            public string timing_db_path { get; set; }
            public string timing_db_name { get; set; }
            public string timing_db_name_abbr { get; set; }
            public string timing_db_full_name { get; set; }
            public string symbol_full_name { get; set; }
            public string antenna_full_name { get; set; }
            public string techfile_full_name { get; set; }
            public string techlef_file_full_name { get; set; }
            public string ref_path { get; set; }
            public string tluplus_worst_full_name { get; set; }
            public string tluplus_best_full_name { get; set; }
            public string tluplus_map_full_name { get; set; }
            public string primitive_inv { get; set; }
            public string primitive_inv_port { get; set; }
            public string[] fill_cells { get; set; }
            public string[] antenna_cells { get; set; }
            public string[] delay_cells { get; set; }
            public string tie_high_cell { get; set; }
            public string tie_low_cell { get; set; }
            public string[] cap_cells { get; set; }
            public string[] wire_only_cells { get; set; }
            public string filltie_cell { get; set; }
            public string cell_list { get; set; }
            public string m1_routing_direction { get; set; }
            public int power_rail_layer { get; set; }
            public string power_pin { get; set; }
            public string ground_pin { get; set; }
            public string gds_layer_map { get; set; }
            public double m2_width { get; set; }
            public string cdl_full_name { get; set; }

            public string[] GetCellList()
            {
                return File.ReadAllLines(cell_list);
            }
        }

        public class LibraryMemory
        {
            
        }

        public class LibraryIo
        {
            public string Name { get; set; }
            public NamePathPair[] refs { get; set; }
            public string timing_db_path { get; set; }
            public string timing_db_name { get; set; }
            public string timing_db_name_abbr { get; set; }

            public string icc_ref_path { get; set; }
            public string cdk_name { get; set; }
            public string cdk_path { get; set; }
        }

        public class NamePathDescriptionPair
        {
            public string name { get; set; }
            public string path { get; set; }
            public string desciption { get; set; }
        }

        public class LibraryIp
        {
            public NamePathDescriptionPair[] ips { get; set; }
        }
    }

    public sealed class LibraryConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string stringValue)
            {
                return AllLibrary.Libraries.FirstOrDefault(l => l.Name == stringValue);
            }
            throw new NotSupportedException("无法识别工艺库");
        }
    }
}
