using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PersonBalance.Web.Models;
using PersonBalance.Web.Models.Data;
using PersonBalance.Web.Services;
using PersonBalance.Web.Exceptions;

namespace PersonBalance.Tests.Controllers
{
    [TestClass]
    public class ValuesControllerTest
    {
        [TestMethod]
        public async Task MultiThreadedTest()
        {
            //Идентификатор базы к которой будем создавать PersistentConnection
            var dbId = Guid.NewGuid();

            //Запоминаем идентификаторы пользователей
            var personIds = new List<Guid>();
            using (var connection = Effort.DbConnectionFactory.CreatePersistent(dbId.ToString()))
            {
                //Регистрация пользователей
                using (var context = new ApplicationDbContext(connection))
                {
                    IPersonService service = new PersonService(context);

                    for (int i = 0; i < 50; i++)
                    {
                        var id = await service.CreateNewPerson(new PersonDto
                        {
                            Name = $"{i}",
                            BirthDate = DateTime.Now
                        });
                        personIds.Add(id);
                    }
                }
            }

            var rand = new Random();

            //Запускаем 10 потоков
            var taskNum = 10;
            var tasks = new Task[taskNum];
            var factory = new TaskFactory();
            for (int i = 0; i < taskNum; i++)
            {
                tasks[i] = factory.StartNew(async () =>
                {
                    using (var curCon = Effort.DbConnectionFactory.CreatePersistent(dbId.ToString()))
                    using (var curContext = new ApplicationDbContext(curCon))
                    {
                        IPersonService curService = new PersonService(curContext);
                        for (int j = 0; j < 10; j++)
                        {
                            //Берем случайного пользователя
                            var personId = personIds[rand.Next(0, 49)];

                            using (var transaction = curContext.Database.BeginTransaction())
                            {
                                //Запоминаем баланс до изменения для сравнения
                                var balance = await curService.CheckBalance(personId);
                                
                                //Запоминаем версию пользователя для сравнения
                                var personVer = await curService.GetPersonVersion(personId);
                                try
                                {
                                    //Пытаемся поменять баланс
                                    await curService.ChangeBalance(personId, 10);
                                    //Пытаемся получить новый баланс для сравнения
                                    var newBalance = await curService.CheckBalance(personId);
                                    transaction.Commit();
                                    //Транзакция прошла − баланс должен был изменится на 10, сравниваем
                                    Assert.AreEqual(balance + 10, newBalance);
                                }
                                catch (EntityHasChangedException)
                                {
                                    //Произошел конфликт − другой поток внес изменения, значит версия пользователя должна поменяться
                                    var changedPersonVer = await curService.GetPersonVersion(personId);
                                    //Сравниваем версии до и после конфликта записи
                                    Assert.AreNotEqual(personVer, changedPersonVer);
                                }
                            }
                        }
                    }
                });
            }

            Task.WaitAll(tasks);
        }
    }
}
