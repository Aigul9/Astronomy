﻿using System;
using System.Globalization;
using System.Linq;

namespace Astronomy
{
    /// <summary>
    /// The class contains different methods that allow to calculate astronomical properties of the planet.
    /// </summary>
    public class Planets
    {
        /// <summary>
        /// List of planets with astronomical constants.
        /// </summary>
        static readonly Planet[] planets = new Planet[]
        {
            new Planet(PlanetName.Mercury, 174.7948, 4.09233445, 132.3282, 6.1385025, 23.4400, 2.9818, 0.5255, 0.1058, 0.0241, 0.0055, 230.3265, 0.0351),
            new Planet(PlanetName.Venus, 50.4161, 1.60213034, 104.9067, -1.481368, 0.7758, 0.0033, 0, 0, 0, 0, 73.7576, 2.6376),
            new Planet(PlanetName.Earth, 357.5291, 0.98560028, 280.1470,  360.9856235, 1.9148, 0.0200, 0.0003, 0, 0, 0, 102.9373, 23.4393),
            new Planet(PlanetName.Mars, 19.3730, 0.52402068, 313.3827, 350.89198226, 10.6912, 0.6228, 0.0503, 0.0046, 0.0005, 0, 71.0041, 25.1918),
            new Planet(PlanetName.Jupiter, 20.0202, 0.08308529, 145.9722, 870.5360000, 5.5549, 0.1683, 0.0071, 0.0003, 0, 0, 237.1015, 3.1189),
            new Planet(PlanetName.Saturn, 317.0207, 0.03344414, 174.3508, 810.7939024, 6.3585, 0.2204, 0.0106, 0.0006, 0, 0, 99.4587, 26.7285),
            new Planet(PlanetName.Uranus, 141.0498, 0.01172834, 29.6474, -501.1600928, 5.3042, 0.1534, 0.0062, 0.0003, 0, 0, 5.4634, 82.2298),
            new Planet(PlanetName.Neptune, 256.2250, 0.00598103, 52.4160, 536.3128662, 1.0302, 0.0058, 0, 0, 0, 0, 182.2100, 27.8477)
        };

        /// <summary>
        /// Calculates the mean anomaly of the planet.
        /// </summary>
        /// <returns>
        /// The mean anomaly of the planet.
        /// </returns>
        /// <param name="planetName">Solar System planet name.</param>
        /// <param name="date">Date.</param>
        public static double GetMeanAnomaly(PlanetName planetName, DateTime date)
        {
            Planet planet = getPlanet(planetName);
          
            double calcDate = BaseCalculator.ToDays(date),
                M = planet.M0 + planet.M1 * calcDate;

            return Math.Round(M % 360, 4);
        }

        /// <summary>
        /// Calculates the equation of center of the planet.
        /// </summary>
        /// <returns>
        /// The difference between the true anomaly and the mean anomaly.
        /// </returns>
        /// <param name="planetName">Solar System planet name.</param>
        /// <param name="date">Date.</param>
        public static double GetEquationOfCenter(PlanetName planetName, DateTime date)
        {
            Planet planet = getPlanet(planetName);

            double M = GetMeanAnomaly(planetName, date) * BaseCalculator.rad;
           
            double eq = planet.C1 * Math.Sin(M) + planet.C2 * Math.Sin(2 * M) + planet.C3 * Math.Sin(3 * M) +
                planet.C4 * Math.Sin(4 * M) + planet.C5 * Math.Sin(5 * M) + planet.C6 * Math.Sin(6 * M);

            return Math.Round(eq, 4);
        }

        /// <summary>
        /// Calculates the sidereal time of the planet.
        /// </summary>
        /// <returns>
        /// The sidereal time of the planet.
        /// </returns>
        /// <param name="planetName">Solar System planet name.</param>
        /// <param name="date">Date.</param>
        /// <param name="lw">Longtitude of the location.</param>
        public static double GetSiderealTime(PlanetName planetName, DateTime date, double lw)
        {
            Planet planet = getPlanet(planetName);
          
            double calcDate = BaseCalculator.ToDays(date),
                theta = planet.Theta0 + planet.Theta1 * calcDate - lw;

            return Math.Round(theta % 360, 4);
        }

        /// <summary>
        /// Calculates ecliptical coordinates of the planet.
        /// </summary>
        /// <returns>
        /// The ecliptic longitude of the planet as seen from the Sun.
        /// </returns>
        /// <param name="planetName">Solar System planet name.</param>
        /// <param name="date">Date.</param>
        public static double getEclipticalCoordinates(PlanetName planetName, DateTime date)
        {
            Planet planet = getPlanet(planetName);
            double eclLng = GetMeanAnomaly(planetName, date) + planet.P + 180 + GetEquationOfCenter(planetName, date);

            return Math.Round(eclLng % 360, 4);
        }

        /// <summary>
        /// Calculates equatorial coordinates of the planet.
        /// </summary>
        /// <returns>
        /// The right ascension and the declination of the planet.
        /// </returns>
        /// <param name="planetName">Solar System planet name.</param>
        /// <param name="date">Date.</param>
        public static Tuple<double, double> GetEquatorialCoordinates(PlanetName planetName, DateTime date)
        {
            Planet planet = getPlanet(planetName);

            double eclLng = getEclipticalCoordinates(planetName, date) * BaseCalculator.rad,
                eps = planet.Eps * BaseCalculator.rad;

            // right ascension
            double alpha = Math.Atan2(Math.Sin(eclLng) * Math.Cos(eps), Math.Cos(eclLng)) / BaseCalculator.rad;

            // declination
            double delta = Math.Asin(Math.Sin(eclLng) * Math.Sin(eps)) / BaseCalculator.rad;

            return new Tuple<double, double>(Math.Round(alpha, 4), Math.Round(delta, 4));
        }

        /// <summary>
        /// Gets planet by given name.
        /// </summary>
        /// <returns>
        /// The instance of the planet.
        /// </returns>
        /// <param name="planetName">Solar System planet name.</param>
        static Planet getPlanet(PlanetName planetName)
        {
            return planets.SingleOrDefault(item => item.Name == planetName);
        }
    }
}
