﻿
namespace DataStoreLib.Storage
{
    using DataStoreLib.Models;
    using Microsoft.WindowsAzure.Storage.Table;

    internal class AffilationTable : Table
    {
        protected AffilationTable(CloudTable table)
            : base(table)
        {
        }

        internal static Table CreateTable(CloudTable table)
        {
            return new AffilationTable(table);
        }

        protected override string GetParitionKey()
        {
            return AffilationEntity.PARTITION_KEY;
        }
    }
}
