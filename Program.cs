using System.Xml.Linq;

namespace GPXLoad_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Enter name of mission from CLI
                Console.WriteLine("Enter name of aircraft:");
                string name = Console.ReadLine();

                // Enter file location from CLI
                Console.WriteLine("Enter full path of file to convert:");
                string file = Console.ReadLine().Replace("\"", "");

                // Open file and start parsing for converstion
                string[] source = File.ReadAllLines(file);

                //Create XML elements
                XElement cust = new XElement("trk",
                    new XElement("name", name),
                    new XElement("trkseg",

                        //Skip headers and iterate through string array
                        from str in source.Skip(2)

                            //Split into fields via commas
                        let fields = str.Split(',')

                        //Take new XML element and add fields to it
                        select new XElement("trkpt",
                        new XAttribute("lat", fields[2]),
                        new XAttribute("lon", fields[3]),
                        new XElement("ele", fields[4]),
                        new XElement("time", (new DateTime(DateTime.Now.Year, 1, 1).Add(new TimeSpan((int.Parse(fields[0].Substring(0, 3)) - 1), (int.Parse(fields[0].Substring(4, 2))), (int.Parse(fields[0].Substring(7, 2))), (int.Parse(fields[0].Substring(10, 2)))))).ToString("s") + "Z"),
                        new XElement("speed", fields[8])
                        )
                    )
                );

                //Save file and write completion message
                File.WriteAllText(name + ".gpx", cust.ToString());
                Console.WriteLine("Conversion complete, press ENTER to exit");
                Console.ReadLine();
            }
            //Catch if CSV file is open
            catch (IOException ex)
            {
                Console.WriteLine("Conversion failed");
                Console.WriteLine("Please close the CSV file before running and try again");
                Console.WriteLine("Press ENTER to exit");
                Console.ReadLine();
            }
            //Catch any general exception
            catch (Exception ex)
            {
                Console.WriteLine("Conversion failed");
                Console.WriteLine("Press ENTER to exit and please try again");
                Console.ReadLine();
            }
        }
    }
}
