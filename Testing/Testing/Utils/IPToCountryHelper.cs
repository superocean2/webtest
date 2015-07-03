using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Web;

namespace Testing.Utils
{
    public class IPv4Data
    {
        public long IpFrom { get; set; }
        public long IpTo { get; set; }
        public string Registry { get; set; }
        public long Assigned { get; set; }
        public string Iso3166TwoLetterCode { get; set; }
        public string Iso3166ThreeLetterCode { get; set; }
        public string Country { get; set; }
    }
    public static class IPToCountryHelper
    {
        #region Fields

        private static IList<IPv4Data> _dataList;
        private static object _syncObject = new object();

        #endregion

        #region Properties
        public static IPv4Data GetIPv4CustomData(this IPAddress ipAddress)
        {
            return GetIPv4Data(ipAddress);
        }
        private static IList<IPv4Data> DataList
        {
            get
            {
                if (_dataList == null)
                {
                    lock (_syncObject)
                    {
                        if (_dataList == null)
                        {
                            _dataList = ReadInCSVFile();
                        }
                    }
                }

                return _dataList;
            }
        }

        /// <summary>
        /// The Regional Internet Registery.
        /// </summary>
        /// <remarks>What the hell is this? Wiki knows all :- http://en.wikipedia.org/wiki/Regional_Internet_Registry/<br /><br />
        /// Expected values are: Reserved, IANA, IETF, APCNIC, ARIN, LACNIC, RIPENCC and AFRINIC.</remarks>
        /// <returns>The RIR code.</returns>
        public static string Registry(this IPAddress ipAddress)
        {
            IPv4Data ipv4Data;


            ipv4Data = GetIPv4Data(ipAddress);
            return ipv4Data == null ? null : ipv4Data.Registry;
        }

        /// <summary>
        /// The date this IP or block was assigned.
        /// </summary>
        /// <remarks>If the date was unknown when assigned, a null value is returned.</remarks>
        /// <returns>The date and time in UTC if known, otherwise null.</returns>
        public static DateTimeOffset? AssignedDateTime(this IPAddress ipAddress)
        {
            IPv4Data ipv4Data;


            ipv4Data = GetIPv4Data(ipAddress);
            if (ipv4Data == null)
            {
                return null;
            }
            else
            {
                return new DateTimeOffset(1970,
                1,
                1,
                0,
                0,
                0,
                0,
                new TimeSpan(0, 0, 0)).AddSeconds(ipv4Data.Assigned);
            }
        }

        /// <summary>
        /// The two letter ISO 3166 code.
        /// </summary>
        /// <remarks>What the hell is this? Wiki knows all :- http://en.wikipedia.org/wiki/ISO_3166/</remarks>
        /// <returns>The two letter code.</returns>
        public static string Iso3166TwoLetterCode(this IPAddress ipAddress)
        {
            IPv4Data ipv4Data;


            ipv4Data = GetIPv4Data(ipAddress);
            return ipv4Data == null ? null : ipv4Data.Iso3166TwoLetterCode;
        }

        /// <summary>
        /// The three letter ISO 3166 code.
        /// </summary>
        /// <remarks>What the hell is this? Wiki knows all :- http://en.wikipedia.org/wiki/ISO_3166/</remarks>
        /// <param name="ipAddress"></param>
        /// <returns>The three letter code.</returns>
        public static string Iso3166ThreeLetterCode(this IPAddress ipAddress)
        {
            IPv4Data ipv4Data;


            ipv4Data = GetIPv4Data(ipAddress);
            return ipv4Data == null ? null : ipv4Data.Iso3166ThreeLetterCode;
        }

        /// <summary>
        /// The full country name.
        /// </summary>
        /// <returns>The full country name.</returns>
        public static string Country(this IPAddress ipAddress)
        {
            IPv4Data ipv4Data;


            ipv4Data = GetIPv4Data(ipAddress);
            return ipv4Data == null ? null : ipv4Data.Country;
        }

        #endregion

        #region Methods

        private static IPv4Data GetIPv4Data(IPAddress ipAddress)
        {
            byte[] ipAddressBytes;
            long ipAddressLong;
            IPv4Data ipv4Data = null;


            if (ipAddress == null)
            {
                throw new ArgumentNullException("ipAddress");
            }
            else if (ipAddress.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
            {
                throw new ArgumentException("Only Internet Protocol version 4 (IPv4) addresses are accepted.");
            }

            if (DataList == null)
            {
                throw new InvalidOperationException("Internet Protocol (IPv4) address list was not successfully loaded into memory. Unable to process your request.");
            }

            ipAddressBytes = ipAddress.GetAddressBytes();
            ipAddressLong = (long)ipAddressBytes[3] +
                ((long)ipAddressBytes[2] * 256L) +
                ((long)ipAddressBytes[1] * 256L * 256L) +
                ((long)ipAddressBytes[0] * 256L * 256L * 256L);

            // Now retrieve the country specific data.
            ipv4Data = (from d in DataList
                        where d.IpFrom <= ipAddressLong &&
                        d.IpTo >= ipAddressLong
                        select d).SingleOrDefault();

            return ipv4Data;
        }

        private static IList<IPv4Data> ReadInCSVFile()
        {
            string row;
            string[] rowArray;
            IList<IPv4Data> ipv4DataList = null;
            long ipFrom;
            long ipTo;
            long assigned;

            // First, decompress the gzipped data file.
            //assembly = Assembly.GetExecutingAssembly();

            //using (GZipStream gzipStream = new GZipStream(assembly.GetManifestResourceStream("WorldDomination.Net.Data.csv.gz"),
            //    CompressionMode.Decompress))
            //string csvfile = HttpContext.Current.Server.MapPath("~/content/IpToCountry.csv");
            var path = AppDomain.CurrentDomain.BaseDirectory.Replace('\\', '/') + "Content/IpToCountry.csv.gz";
            using (var csvStream = new StreamReader(path))
            {
                using (var gzipStream = new GZipStream(csvStream.BaseStream, CompressionMode.Decompress))
                {
                    using (TextReader textReader = new StreamReader(gzipStream))
                    {
                        while ((row = textReader.ReadLine()) != null)
                        {
                            if (!string.IsNullOrEmpty(row) &&
                                !row.StartsWith("#",
                                StringComparison.OrdinalIgnoreCase))
                            {
                                // We have a row, so lets split it then add it to our collection.
                                rowArray = row.Replace("\"", string.Empty).Split(',');
                                if (rowArray != null &&
                                    rowArray.Length == 7 &&
                                    long.TryParse(rowArray[0], out ipFrom) &&
                                    long.TryParse(rowArray[1], out ipTo) &&
                                    long.TryParse(rowArray[3], out assigned))
                                {
                                    // We have a legit row, so lets add this to our collection.
                                    if (ipv4DataList == null)
                                    {
                                        ipv4DataList = new List<IPv4Data>();
                                    }

                                    ipv4DataList.Add(new IPv4Data
                                    {
                                        IpFrom = ipFrom,
                                        IpTo = ipTo,
                                        Registry = rowArray[2],
                                        Assigned = assigned,
                                        Iso3166TwoLetterCode = rowArray[4],
                                        Iso3166ThreeLetterCode = rowArray[5],
                                        Country = rowArray[6]
                                    });
                                }
                            }
                        }
                    }
                }
            }

            return ipv4DataList == null ||
                ipv4DataList.Count <= 0 ? null : ipv4DataList;
        }

        #endregion
    }

}