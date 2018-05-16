using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using qt.zccf_p2p.Common;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            Car c1 = new Car("甲壳虫",300,200);

            c1.RegisterWithCarEngine(new Car.CarEngineHandler(OnCarEngineEvent));

            Car.CarEngineHandler handler2 = new Car.CarEngineHandler(OnCarEngineEvent);
            c1.RegisterWithCarEngine(handler2);

            for (int i = 0; i < 100; i++)
            {
                c1.Accelerate(10);
            }
            //c1.DisplayCurrSp();

            Console.ReadKey();
        }

        public static void OnCarEngineEvent(string msg)
        {
            Console.WriteLine("=> "+msg);
        }
    }

    public class Car
    {
        public int CurrentSpeed { get; set; }
        public int MaxSpeed { get; set; }
        public string PetName { get; set; }
        private bool carIsDead;
        public Car()
        {
            MaxSpeed = 100;
            carIsDead = false;
            CurrentSpeed = 0;
        }
        public Car(string name,int maxSp,int currSp)
        {
            PetName = name;
            MaxSpeed = maxSp;
            carIsDead = false;
            CurrentSpeed = currSp;
        }
        public void DisplayCurrSp()
        {
            if (carIsDead)
            {
                Console.WriteLine("时速：0");
            }
            else
            {
                Console.WriteLine("时速：" + CurrentSpeed);
            }
            
        }

        // 1) 定义委托类型
        public delegate void CarEngineHandler(string msgForCaller);
        // 2）定义每个委托类型的成员变量
        private CarEngineHandler listOfHandlers;
        // 3) 向调用者添加注册函数
        public void RegisterWithCarEngine(CarEngineHandler methodToCall)
        {
            listOfHandlers = methodToCall;
        }

        public void Accelerate(int delta)
        {
            if (carIsDead)
            {
                //listOfHandlers?.Invoke("车子挂掉了！！");
            }
            else
            {
                CurrentSpeed += delta;
                if (10 == (MaxSpeed-CurrentSpeed)&& listOfHandlers!=null)
                {
                    listOfHandlers("车子快不行了！！！");
                }
                if (CurrentSpeed>MaxSpeed)
                {
                    listOfHandlers?.Invoke("车子挂掉了！！");
                    CurrentSpeed = 0;
                    carIsDead = true;
                }
                else
                {
                    //listOfHandlers("时速：" + CurrentSpeed);
                    Console.WriteLine("时速："+CurrentSpeed);
                }
            }
        }
    }
    
}
