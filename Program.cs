using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{

    static string FindNearestVehicle(List<(string, double, double)> vehiclePositions, (double, double) testCoord)
    {
        double minDistance = double.MaxValue;
        string nearestVehicle = "";

        foreach (var (registration, latitude, longitude) in vehiclePositions)
        {
            double distance = CalculateDistance(latitude, longitude, testCoord.Item1, testCoord.Item2);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestVehicle = registration;
            }
        }

        return nearestVehicle;
    }

    /*calculate distance between two points using Haversine formula
     * https://community.esri.com/t5/coordinate-reference-systems-blog/distance-on-a-sphere-the-haversine-formula/ba-p/902128#:~:text=For%20example%2C%20haversine(%CE%B8),longitude%20of%20the%20two%20points.
     * 
     * */

    static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double EarthRadiusKm = 6371;
        double dLat = (lat2 - lat1) * Math.PI / 180;
        double dLon = (lon2 - lon1) * Math.PI / 180;
        double lat1Rad = lat1 * Math.PI / 180;
        double lat2Rad = lat2 * Math.PI / 180;

        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1Rad) * Math.Cos(lat2Rad);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        double distance = EarthRadiusKm * c;
        return distance;
    }


    static void Main(string[] args)
    {
        // Parse data file
        var vehiclePositions = new List<(string, double, double)>();
        using (StreamReader reader = new StreamReader("VehiclePositions.dat"))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                
                 //VehicleId,VehicleRegistration,Latitude,Longitude,RecordedTimeUTC
                string[] parts = line.Split('\t');
                string registration = parts[1];
                double latitude = double.Parse(parts[2]);
                double longitude = double.Parse(parts[3]);
                vehiclePositions.Add((registration, latitude, longitude));
            }
        }

        // Test coordinates
        var testCoordinates = new (double, double)[]
        {
            (34.544909,-102.100843),
            (32.345544,-99.123124),
            (33.234235,-100.214124),
            (35.195739,-95.348899),
            (31.895839,-97.789573),
            (32.895839,-101.789573),
            (34.115839,-100.225732),
            (32.335839,-99.992232),
            (33.535339,-94.792232),
            (32.234235,-100.222222)
        };

        // Find nearest vehicle for each test coordinate
        for (int i = 0; i < testCoordinates.Length; i++)
        {
            var testCoord = testCoordinates[i];
            var nearestVehicle = FindNearestVehicle(vehiclePositions, testCoord);
            Console.WriteLine($"Nearest vehicle to test coordinate {i + 1}: {nearestVehicle}");
        }
    }

    
}
