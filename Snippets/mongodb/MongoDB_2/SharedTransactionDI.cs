﻿using System.Threading.Tasks;
using NServiceBus.Storage.MongoDB;

namespace MongoDB_2
{
    public class SharedTransactionDI
    {
        #region MongoDBSharedTransactionDI
        class MyService
        {
            IMongoSynchronizedStorageSession sharedSession;

            // Resolved from DI container
            public MyService(IMongoSynchronizedStorageSession sharedSession)
            {
                this.sharedSession = sharedSession;
            }

            public Task Create()
            {
                return sharedSession.MongoSession.Client
                    .GetDatabase("mydatabase")
                    .GetCollection<MyBusinessObject>("mycollection")
                    .InsertOneAsync(sharedSession.MongoSession, new MyBusinessObject());
            }
        }
        #endregion

        class MyBusinessObject
        {
        }
    }
}