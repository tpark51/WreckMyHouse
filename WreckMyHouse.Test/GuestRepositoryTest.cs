using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using WreckMyHouse.Core.Models;

namespace WreckMyHouse.DAL.Test
{
    public class GuestRepositoryTest
    {
        const string SEED_DIRECTORY = "testdata";
        const string SEED_FILE = "guests-seed.csv";
        const string TEST_DIRECTORY = "test";
        const string TEST_FILE = "guests-test.csv";

        string SEED_PATH = Path.Combine(SEED_DIRECTORY, SEED_FILE);
        string TEST_PATH = Path.Combine(TEST_DIRECTORY, TEST_FILE);

        GuestRepository repo;

        [SetUp]
        public void SetUp()
        {
            if(!Directory.Exists(TEST_DIRECTORY))
            {
                Directory.CreateDirectory(TEST_DIRECTORY);
            }
            File.Copy(SEED_PATH, TEST_PATH, true);
            repo = new GuestRepository(TEST_PATH);
        }

        [Test]
        public void ShouldFind1000()
        {
            List<Guest> all = repo.FindAll();
            Assert.AreEqual(1000, all.Count);
        }
    }
}
