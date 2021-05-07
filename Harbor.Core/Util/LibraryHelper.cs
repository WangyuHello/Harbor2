using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Harbor.Core.Util
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class LibraryHelper
    {
        public static string HarborHome => Environment.GetEnvironmentVariable("HARBOR_HOME");

        public static (LibraryStdCell stdCell, LibraryPDK pdk, List<LibraryIO> io) GetLibraryParams(JObject projectInfo)
        {
            return (GetStdCell(projectInfo), GetPdk(projectInfo), GetIO(projectInfo));
        }

        public static LibraryStdCell GetStdCell(JObject projectInfo)
        {
            var libInfo = GetLibInfo();
            var libName = projectInfo["Library"].Value<string>();

            if (projectInfo.ContainsKey("StdCell")) // 如果标准单元库存在
            {
                var stdCellName = projectInfo["StdCell"].Value<string>();
                var libPath = GetLibraryPath(libInfo, libName);
                var stdCellInfoFile = Path.Combine(libPath, "library.stdcell.yml");
                if (File.Exists(stdCellInfoFile)) // 如果标准单元库存在
                {   
                    var deserializer2 = new DeserializerBuilder()
                        .IgnoreUnmatchedProperties()
                        .Build();
                    var stdCell = deserializer2.Deserialize<LibraryStdCell>(File.OpenText(stdCellInfoFile));
                    if (stdCell.refs == null)
                    {
                        return stdCell;
                    }
                    var targetLibRelativePath = stdCell.refs.First(l => l.name == stdCellName);
                    var comb = Path.Combine(new FileInfo(stdCellInfoFile).DirectoryName ?? throw new InvalidOperationException(), targetLibRelativePath.path);
                    var stdCellFile = (new DirectoryInfo(comb)).FullName;

                    stdCell = deserializer2.Deserialize<LibraryStdCell>(File.OpenText(stdCellFile));
                    return stdCell;
                }
            }

            return null;
        }

        public static List<LibraryIO> GetIO(JObject projectInfo)
        {
            var libInfo = GetLibInfo();
            var libName = projectInfo["Library"].Value<string>();
            if (projectInfo.ContainsKey("IO")) // 如果IO库存在
            {
                var libPath = GetLibraryPath(libInfo, libName);
                var ioInfoFile = Path.Combine(libPath, "library.io.yml");

                if (projectInfo["IO"] is JArray)
                { //多个IO库
                    var ioNames = ((JArray) projectInfo["IO"]).Select(i => i.Value<string>());
                    return ioNames.Select(ioName => GetIOOne(ioName, ioInfoFile)).ToList();
                }
                else
                {
                    var ioName = projectInfo["IO"].Value<string>();
                    if (File.Exists(ioInfoFile)) // 如果IO库存在
                    {
                        return new List<LibraryIO>
                        {
                            GetIOOne(ioName, ioInfoFile)
                        };
                    }
                }


            }

            LibraryIO GetIOOne(string ioName, string ioInfoFile)
            {
                var deserializer2 = new DeserializerBuilder()
                    .IgnoreUnmatchedProperties()
                    .Build();
                var io = deserializer2.Deserialize<LibraryIO>(File.OpenText(ioInfoFile));
                if (io.refs == null)
                {
                    return io;
                }
                var targetLibRelativePath = io.refs.First(l => l.name == ioName);
                var comb = Path.Combine(new FileInfo(ioInfoFile).DirectoryName ?? throw new InvalidOperationException(), targetLibRelativePath.path);
                var ioFile = (new DirectoryInfo(comb)).FullName;

                io = deserializer2.Deserialize<LibraryIO>(File.OpenText(ioFile));
                return io;
            }

            return null;
        }

        public static LibraryPDK GetPdk(JObject projectInfo)
        {
            var libInfo = GetLibInfo();
            var libName = projectInfo["Library"].Value<string>();
            var libPath = GetLibraryPath(libInfo, libName);
            var deserializer2 = new DeserializerBuilder()
                .IgnoreUnmatchedProperties()
                .Build();

            var pdk = deserializer2.Deserialize<LibraryPDK>(File.OpenText(Path.Combine(libPath, "library.pdk.yml")));
            return pdk;
        }

        public static string GetLibraryPath(LibraryInfo libInfo, string libName)
        {
            var libInfoYmlFile = Path.Combine(HarborHome, "template", "library", "library.info.yml");
            var targetLibRelativePath = libInfo.libraries.First(l => l.name == libName);
            var comb = Path.Combine(new FileInfo(libInfoYmlFile).DirectoryName ?? throw new InvalidOperationException(), targetLibRelativePath.path);
            var libPath = (new DirectoryInfo(comb)).FullName;
            return libPath;
        }

        public static LibraryInfo GetLibInfo()
        {
            var libInfoYmlFile = Path.Combine(HarborHome, "template", "library", "library.info.yml");
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            var libInfo = deserializer.Deserialize<LibraryInfo>(File.OpenText(libInfoYmlFile));
            return libInfo;
        }

        public class LibraryInfo
        {
            public NamePathPair[] libraries { get; set; }
        }

        public class LibraryStdCell
        {
            public NamePathPair[] refs { get; set; }
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
            public MemoryCompiler[] memory_compiler { get; set; }
        }

        public class LibraryPDK
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
            public LayerPair[] layer_map { get; set; }

            public int GetLayerNumber(string layer_name)
            {
                return layer_map.FirstOrDefault(l => l.layer_name == layer_name).layer_number;
            }
        }

        public class LayerPair
        {
            public string layer_name { get; set; }
            public int layer_number { get; set; }
        }

        public class NamePathPair
        {
            public string name { get; set; }
            public string path { get; set; }
        }

        public class LibraryIO
        {
            public NamePathPair[] refs { get; set; }
            public string timing_db_path { get; set; }
            public string timing_db_name { get; set; }
            public string timing_db_name_abbr { get; set; }

            public string icc_ref_path { get; set; }
            public string cdk_name { get; set; }
            public string cdk_path { get; set; }
        }

        public class MemoryCompiler
        {
            public string name { get; set; }
            public string executable { get; set; }
        }
    }
}
