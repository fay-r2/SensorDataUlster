using Newtonsoft.Json;
using System.Collections.Generic;

namespace TodoApi.Models
{
    public class BlobJson
    {
        public long? Width { get; set; } = null;


        public long? Height { get; set; } = null;


        public string Unit { get; set; } = null;


        public double? Reading { get; set; } = null;

        public List<long> FrameData { get; set; } 
    }

    public class SensorDataItem
    {
        public string SensorUUID { get; set; }

        public string SensorHardwareID { get; set; }

        public double TimeStamp { get; set; }

        public long DeviceMfg { get; set; }

        public long SensorClass { get; set; }

        public BlobJson BlobJson { get; set; } 

    }
}
