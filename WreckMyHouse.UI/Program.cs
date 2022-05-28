using Ninject;
using System;

namespace WreckMyHouse.UI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            NinjectContainer.Configure(); 
            Controller controller = NinjectContainer.kernel.Get<Controller>(); 
            controller.Run(); 
        }
    }
}
