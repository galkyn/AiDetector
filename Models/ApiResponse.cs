using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace AiDetector.Models
{
    public class ApiResponse
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string? InputText { get; set; }
        public string? Result { get; set; }
        public bool Success { get; set; }
        public int Code { get; set; }
        public string? Message { get; set; }
        public ApiData? Data { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; }
    }

    public class ApiData
    {
        public int TextWords { get; set; }
        public int AiWords { get; set; }
        public double FakePercentage { get; set; }
    }
}
