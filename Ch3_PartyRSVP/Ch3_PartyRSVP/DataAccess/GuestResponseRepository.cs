using Ch3_PartyRSVP.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ch3_PartyRSVP.DataAccess
{
    public class GuestResponseRepository : Repository<GuestResponse>
    {
        private readonly ILogger<GuestResponseRepository> _logger;
        private readonly IConfiguration _configuration;

        public GuestResponseRepository(ILogger<GuestResponseRepository> logger,
            IConfiguration configuration) : base(logger, configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public override async Task AddAsync(GuestResponse entity)
        {
            if(await HasRsvped(entity.Email))
            {
                throw new ArgumentException($"{entity.Email} has already RSVPed", nameof(entity));
            }    

            var queryResult = await QuerySingleAsync<int>($"INSERT INTO {TableName} (Name, Email, WillAttend) " +
                $"VALUES (@Name, @Email, @WillAttend); select last_insert_rowid();", entity);

            entity.Id = queryResult;
        }

        public override async Task DeleteAsync(GuestResponse entity)
        {
            await ExecuteAsync($"DELETE FROM {TableName} WHERE Id = @Id;", entity);
        }

        public override async Task EditAsync(GuestResponse entity)
        {
            await ExecuteAsync($"UPDATE {TableName} SET Name = @Name, Email = @Email, WillAttend = @WillAttend " +
                $"WHERE Id = @Id", entity);
        }

        public async Task<bool> HasRsvped(string email)
        {
            var queryResult = await QuerySingleAsync<int>($"SELECT COUNT(Email) FROM {TableName} WHERE Email = @Email", new { Email = email });
            if (queryResult > 0)
            {
                return true;
            }

            return false;
        }
    }
}
