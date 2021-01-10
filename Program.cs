using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ppvis2_wf
{
    static class Program
    {

        public static Simulator simulator;
        static Form1 myForm;
        public static ListView console;
        public static Label baseCount;
        public static Label carCount;
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
                //StartSimulation();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());

            
        }
        
        public static string PrintBases()
        {
            string s = "";
            foreach (var b in simulator.map.bases)
            {
                s += $"base #{b.id}:\n queue={b.awaitingCars.Count}\n loadingStations={b.loadingStations.Count}\n resourses::";
                for (int i = 0; i < b.resourses.Length; i++)
                {
                    s += "resource #" + i + "::=" + b.resourses[i];
                    s += "\n";
                }
                s += "____________________\n";
                for (int i = 0; i < b.cars.Count; i++)
                {
                    s += $"car #{i}:\n";
                    s += "cargo:" + b.cars[i].cargo.type+"\n";
                    s += "state:" + (b.cars[i].currentState.homeToTargetProgress==0?"idle":"in trip");
                    s += "\n";
                }                 
                
            }

            return s;
        }

        public static string PrintCars()
        {
            string s = "";
            foreach (var b in simulator.map.bases)
            {               
                for (int i = 0; i < b.cars.Count; i++)
                {
                    s += $"car #{b.id}-{i}:\n";
                    s += "cargo:" + b.cars[i].cargo.type + "\n";
                    s += "state:" + (b.cars[i].currentState.homeToTargetProgress == 0 ? "idle" : "in trip");
                    s += "target base:" + (b.cars[i].targetBase==null ? "no" : ("#"+ b.cars[i].targetBase.id));
                }

            }

            return s;
        }

        public static void StartSimulation()
        {           
            Simulator s = new Simulator();
            s.map = new Map();
            s.map.bases = new List<Base>();
            s.map.AddBase(new Base
            {
                awaitingCars = new Queue<Car>(),
                cars = new List<Car>(),
                id = 0,
                loadingStations = new List<LoadingStation>() { }
            });

            s.map.bases[0].loadingStations.Add(new LoadingStation() { myBase = s.map.bases[0] });


            s.map.bases[0].cars.Add(new Car() { myBase = s.map.bases[0] });

            simulator = s;
        }
    }




    class Simulator
    {
        public Map map;
        public List<Request> requests = new List<Request>();
        public void Simlate(double time)
        {
            foreach (var b in map.bases)
            {
                foreach (var c in b.cars)
                {
                    c.currentState.NextState(1f);


                }
            }
        }
    }

    class Request
    {
        public int count;
        public Resource resource;
    }

    class Map
    {
        public List<Base> bases = new List<Base>();

        public void AddBase(Base newBase)
        {
            bases.Add(newBase);
        }

        public void AddBase(string str)
        {

        }
    }

    class Base
    {
        public int[] resourses = new int[5];
        public int id = 0;
        public List<LoadingStation> loadingStations = new List<LoadingStation>();
        public List<Car> cars = new List<Car>();
        public Queue<Car> awaitingCars = new Queue<Car>();

        public LoadingStation GetLoadingStation()
        {
            return null;
        }

        public Car GetNotBusyCar()
        {
            return null;
        }

        public void SendToBase(Car car, Base targetBase)
        {            
            car.targetBase = targetBase;
            car.currentState.currentState++;
        }

        public void GetCar(Car car)
        {
            awaitingCars.Enqueue(car);
            car.currentState.awaitingInQueue = true;
        }
    }

    class Car
    {
        public Base myBase;
        public Base targetBase;

        public CarState currentState = new CarState();

        public Resource cargo= new Resource();
    }

    class LoadingStation
    {
        public Base myBase;
        public void LoadCar(Car car)
        {
            int resType = 2;
            myBase.resourses[resType]--;
            car.cargo.type = resType;
        }

        public void UnloadCar(Car car)
        {
            myBase.resourses[car.cargo.type]++;
            car.cargo.type = 0;
        }
    }

    class Resource
    {
        public int type;
    }

    class CarState
    {
        public float homeToTargetProgress;
        public float targetToHomeProgress;
        public int currentState;
        public bool awaitingInQueue = false;

        public void NextState(double time)
        {
            switch (currentState)
            {
                case 0: break;//idle at base
                case 1: homeToTargetProgress += (float)time; break; // base to target
                case 2:  break;
                case 3: targetToHomeProgress += (float)time; break;
                case 4: break;
                default:
                    break;
            }
        }

    }
}
