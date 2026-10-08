using System;
using System.Collections.Generic;

namespace _02_Builder_Automarke
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Vehicle> vehicles = new List<Vehicle>();

            IVehicleBuilder vwBuilder = new VWBuilder();
            VehicleDirector vwDirector = new VehicleDirector(vwBuilder);
            vwDirector.ConstructBasicVW();          
            vehicles.Add(vwBuilder.Build());     

            IVehicleBuilder seatBuilder = new SeatBuilder();
            new VehicleDirector(seatBuilder).ConstructSportSeat();
            vehicles.Add(seatBuilder.Build());

            vehicles.Add(new VehicleBuilder()
                .SetBrand("Audi")
                .SetModel("A4 Avant")
                .SetEngine("2.0 TDI, 150 PS")
                .SetColor("Grau")
                .SetDoors(5)
                .Build());

            foreach (Vehicle vehicle in vehicles)
            {
                Console.WriteLine(vehicle);
            }

            Console.ReadKey();
        }
    }

    public class Vehicle
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Engine { get; set; }
        public string Color { get; set; }
        public int Doors { get; set; }

        public override string ToString()
        {
            return $"{Brand} {Model} | Motor: {Engine} | Farbe: {Color} | Türen: {Doors}";
        }
    }

    public interface IVehicleBuilder
    {
        IVehicleBuilder SetBrand(string brand);
        IVehicleBuilder SetModel(string model);
        IVehicleBuilder SetEngine(string engine);
        IVehicleBuilder SetColor(string color);
        IVehicleBuilder SetDoors(int number);

        void SetBrand();
        void SetModel();
        void SetEngine();
        void SetColor();
        void SetDoors();

        Vehicle Build();
    }

    public class VehicleBuilder : IVehicleBuilder
    {
        private Vehicle _vehicle = new Vehicle();

        public IVehicleBuilder SetBrand(string brand)
        {
            _vehicle.Brand = brand;
            return this;
        }

        public IVehicleBuilder SetModel(string model)
        {
            _vehicle.Model = model;
            return this;
        }

        public IVehicleBuilder SetEngine(string engine)
        {
            _vehicle.Engine = engine;
            return this;
        }

        public IVehicleBuilder SetColor(string color)
        {
            _vehicle.Color = color;
            return this;
        }

        public IVehicleBuilder SetDoors(int number)
        {
            _vehicle.Doors = number;
            return this;
        }

        public void SetBrand()
        {
            _vehicle.Brand = "notSet";
        }

        public void SetModel()
        {
            _vehicle.Model = "notSet";
        }

        public void SetEngine()
        {
            _vehicle.Engine = "Standardmotor";
        }

        public void SetColor()
        {
            _vehicle.Color = "Weiß";
        }

        public void SetDoors()
        {
            _vehicle.Doors = 5;
        }

        public Vehicle Build()
        {
            Vehicle result = _vehicle;
            _vehicle = new Vehicle();   
            return result;
        }
    }

    public class VWBuilder : IVehicleBuilder
    {
        private Vehicle _vehicle = new Vehicle();

        public VWBuilder()
        {
            SetBrand();
        }

        public IVehicleBuilder SetBrand(string brand)
        {
            _vehicle.Brand = brand;
            return this;
        }

        public IVehicleBuilder SetModel(string model)
        {
            _vehicle.Model = model;
            return this;
        }

        public IVehicleBuilder SetEngine(string engine)
        {
            _vehicle.Engine = engine;
            return this;
        }

        public IVehicleBuilder SetColor(string color)
        {
            _vehicle.Color = color;
            return this;
        }

        public IVehicleBuilder SetDoors(int number)
        {
            _vehicle.Doors = number;
            return this;
        }

        public void SetBrand()
        {
            _vehicle.Brand = "VW";
        }

        public void SetModel()
        {
            _vehicle.Model = "Polo";
        }

        public void SetEngine()
        {
            _vehicle.Engine = "1.0 TSI, 80 PS";
        }

        public void SetColor()
        {
            _vehicle.Color = "Weiß";
        }

        public void SetDoors()
        {
            _vehicle.Doors = 5;
        }

        public Vehicle Build()
        {
            Vehicle result = _vehicle;
            _vehicle = new Vehicle();   
            SetBrand();                 
            return result;
        }
    }

    public class SeatBuilder : IVehicleBuilder
    {
        private Vehicle _vehicle = new Vehicle();

        public SeatBuilder()
        {
            SetBrand();   
        }

        public IVehicleBuilder SetBrand(string brand)
        {
            _vehicle.Brand = brand;
            return this;
        }

        public IVehicleBuilder SetModel(string model)
        {
            _vehicle.Model = model;
            return this;
        }

        public IVehicleBuilder SetEngine(string engine)
        {
            _vehicle.Engine = engine;
            return this;
        }

        public IVehicleBuilder SetColor(string color)
        {
            _vehicle.Color = color;
            return this;
        }

        public IVehicleBuilder SetDoors(int number)
        {
            _vehicle.Doors = number;
            return this;
        }

        public void SetBrand()
        {
            _vehicle.Brand = "Seat";
        }

        public void SetModel()
        {
            _vehicle.Model = "Leon";
        }

        public void SetEngine()
        {
            _vehicle.Engine = "1.0 TSI, 110 PS";
        }

        public void SetColor()
        {
            _vehicle.Color = "Weiß";
        }

        public void SetDoors()
        {
            _vehicle.Doors = 5;
        }

        public Vehicle Build()
        {
            Vehicle result = _vehicle;
            _vehicle = new Vehicle();   
            SetBrand();                 
            return result;
        }
    }

    public class VehicleDirector
    {
        private IVehicleBuilder _builder;

        public VehicleDirector(IVehicleBuilder builder)
        {
            _builder = builder;
        }

        public void ConstructBasicVW()
        {
            _builder.SetBrand();
            _builder.SetModel();
            _builder.SetEngine();
            _builder.SetColor();
            _builder.SetDoors();
        }

        public void ConstructSportSeat()
        {
            _builder.SetBrand();
            _builder.SetModel();
            _builder.SetEngine("2.0 TSI, 300 PS");
            _builder.SetColor("Rot");
            _builder.SetDoors();
        }
    }
}