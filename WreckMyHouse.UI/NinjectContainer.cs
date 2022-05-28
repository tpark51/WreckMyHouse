using Ninject;
using System;
using System.IO;
using WreckMyHouse.BLL;
using WreckMyHouse.Core.Repositories;
using WreckMyHouse.DAL;

namespace WreckMyHouse.UI
{
    class NinjectContainer
    {
        public static StandardKernel kernel { get; private set; }

        public static void Configure()
        {
            kernel = new StandardKernel();

            kernel.Bind<ConsoleIO>().To<ConsoleIO>();
            kernel.Bind<View>().To<View>();

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            string reservationFileDirectory = @"C:\Users\Toi Parker\Code\WreckMyHouse\WreckMyHouse.DAL\Data\reservations\";
            string hostFilePath = @"C:\Users\Toi Parker\Code\WreckMyHouse\WreckMyHouse.DAL\Data\hosts.csv";
            string guestFilePath = @"C:\Users\Toi Parker\Code\WreckMyHouse\WreckMyHouse.DAL\Data\guests.csv";

            kernel.Bind<IHostRepository>().To<HostRepository>().WithConstructorArgument(hostFilePath);
            kernel.Bind<IGuestRepository>().To<GuestRepository>().WithConstructorArgument(guestFilePath);
            kernel.Bind<IReservationRepository>().To<ReservationRepository>().WithConstructorArgument(reservationFileDirectory);

            kernel.Bind<ReservationService>().To<ReservationService>();
            kernel.Bind<HostService>().To<HostService>();
            kernel.Bind<GuestService>().To<GuestService>();
        }
    }
}
