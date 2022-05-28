using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using WreckMyHouse.Core.Models;

namespace WreckMyHouse.DAL.Test
{
    public class ReservationRepositoryTest
    {
        const string SEED_DIRECTORY = "testdata";
        const string SEED_FILE = "reservation-seed-2e72f86c-b8fe-4265-b4f1-304dea8762db.csv";
        const string TEST_DIRECTORY = "test";
        const string TEST_FILE = "2e72f86c-b8fe-4265-b4f1-304dea8762db.csv";

        string SEED_PATH = Path.Combine(SEED_DIRECTORY, SEED_FILE);
        string TEST_PATH = Path.Combine(TEST_DIRECTORY, TEST_FILE);

        ReservationRepository repo;

        DateTime startDate = new DateTime(2022, 4, 1);
        DateTime endDate = new DateTime(2022, 4, 7);

        [SetUp]
        public void SetUp()
        {
            if (!Directory.Exists(TEST_DIRECTORY))
            {
                Directory.CreateDirectory(TEST_DIRECTORY);
            }
            File.Copy(SEED_PATH, TEST_PATH, true);
            repo = new ReservationRepository(TEST_DIRECTORY);
        }

        [Test]
        public void ShouldFindByHost()
        {
            string hostId = "2e72f86c-b8fe-4265-b4f1-304dea8762db";
            List<Reservation> all = repo.FindHostReservationById(hostId);
            Assert.AreEqual(12, all.Count);
        }



    }
}
