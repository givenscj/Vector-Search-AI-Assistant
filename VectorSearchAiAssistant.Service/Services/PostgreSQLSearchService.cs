using Azure;
using Azure.Core.Serialization;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel.Connectors.Postgres;
using Newtonsoft.Json;
using Npgsql;
using Pgvector;
using System.Data.Common;
using System.Text.Json;
using VectorSearchAiAssistant.SemanticKernel.Models;
using VectorSearchAiAssistant.Service.Interfaces;
using VectorSearchAiAssistant.Service.Models.ConfigurationOptions;

namespace VectorSearchAiAssistant.Service.Services
{
    public class PostgreSQLSearchService : ICognitiveSearchService
    {
        private PostgresDbClient _client;
        private NpgsqlDataSource _dataSource;
        readonly PostgreSQLSearchSettings _settings;
        readonly ILogger _logger;

        public PostgreSQLSearchService(
            IOptions<PostgreSQLSearchSettings> options,
            ILogger<PostgreSQLSearchSettings> logger)
        {
            _settings = options.Value;
            _logger = logger;

            NpgsqlDataSourceBuilder dataSourceBuilder = new(_settings.ConnectionString);
            dataSourceBuilder.EnableDynamicJson();
            dataSourceBuilder.UseVector();
            _dataSource = dataSourceBuilder.Build();
            
            _client = new PostgresDbClient(_dataSource, "public", _settings.VectorSize);

            _client.CreateTableAsync(_settings.IndexName).Wait();
        }

        /// <summary>
        /// Initialize the underlying Azure Cognitive Search index.
        /// </summary>
        /// <param name="typesToIndex">The object types supported by the index.</param>
        /// <returns></returns>
        public async Task Initialize(List<Type> types)
        {
            try
            {
                //https://github.com/pgvector/pgvector-dotnet

                //create the table
                using var cmd = new NpgsqlCommand();
                cmd.Connection = _dataSource.CreateConnection();

                _client.CreateTableAsync(_settings.IndexName).Wait();

                _logger.LogInformation($"Created the {_settings.IndexName} index.");
            }
            catch (Exception e)
            {
                _logger.LogError($"An error occurred while trying to build the {_settings.IndexName} index: {e}");
            }
        }

        public async Task IndexItem(object item)
        {
            await using (var cmd = new NpgsqlCommand($"INSERT INTO {_settings.IndexName} (id, content, embedding) VALUES ($1, $2, $3)", _dataSource.CreateConnection()))
            {
                //embed the content...
                var embedding = new Vector(new float[] { 1, 1, 1 });

                cmd.Parameters.AddWithValue("embedding", embedding);
                cmd.Parameters.AddWithValue("content", JsonConvert.SerializeObject(item));
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<Response<SearchResults<SearchDocument>>> SearchAsync(SearchOptions options)
        {
            return null;

            var connection = _dataSource.CreateConnection();

            await using (var cmd = new NpgsqlCommand($"SELECT * FROM {_settings.IndexName} ORDER BY embedding <-> $1 LIMIT 5", connection))
            {
                foreach (var vq in options.VectorQueries)
                {
                    float[] in_vectors = null;
                    var embedding = new Vector(in_vectors);
                    cmd.Parameters.AddWithValue(embedding);

                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Console.WriteLine(reader.GetValue(0));
                        }
                    }
                }
            }

            return null;
        }
    }
}
