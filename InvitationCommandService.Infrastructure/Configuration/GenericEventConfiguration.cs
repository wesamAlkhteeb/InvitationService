using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using InvitationCommandService.Domain.Entities.Events;

namespace InvitationCommandService.Infrastructure.Configuration
{
    public class GenericEventConfiguration<TEntity, TData> : IEntityTypeConfiguration<TEntity>
            where TEntity : EventEntity<TData>
            where TData : class
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(e => e.Data).HasConversion(
                 x => Serialize(x),
                 x => Deserialize(x)
            ).HasColumnName("Data");
        }

        private static JsonSerializerOptions GetJsonSerializerOptions() => new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        private static string Serialize(TData data) => JsonSerializer.Serialize(data, GetJsonSerializerOptions());
        private static TData Deserialize(string data) => JsonSerializer.Deserialize<TData>(data, GetJsonSerializerOptions())
            ?? throw new InvalidOperationException("Failed to deserialize JSON data");
    }

}

