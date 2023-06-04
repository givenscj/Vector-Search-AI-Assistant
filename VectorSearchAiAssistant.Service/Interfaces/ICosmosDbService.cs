﻿using VectorSearchAiAssistant.Service.Models.Chat;
using VectorSearchAiAssistant.Service.Models.Search;

namespace VectorSearchAiAssistant.Service.Interfaces;

public interface ICosmosDbService
{
    /// <summary>
    /// Gets a list of all current chat sessions.
    /// </summary>
    /// <returns>List of distinct chat session items.</returns>
    Task<List<Session>> GetSessionsAsync();

    /// <summary>
    /// Gets a list of all current chat messages for a specified session identifier.
    /// </summary>
    /// <param name="sessionId">Chat session identifier used to filter messsages.</param>
    /// <returns>List of chat message items for the specified session.</returns>
    Task<List<Message>> GetSessionMessagesAsync(string sessionId);

    /// <summary>
    /// Creates a new chat session.
    /// </summary>
    /// <param name="session">Chat session item to create.</param>
    /// <returns>Newly created chat session item.</returns>
    Task<Session> InsertSessionAsync(Session session);

    /// <summary>
    /// Creates a new chat message.
    /// </summary>
    /// <param name="message">Chat message item to create.</param>
    /// <returns>Newly created chat message item.</returns>
    Task<Message> InsertMessageAsync(Message message);

    /// <summary>
    /// Updates an existing chat session.
    /// </summary>
    /// <param name="session">Chat session item to update.</param>
    /// <returns>Revised created chat session item.</returns>
    Task<Session> UpdateSessionAsync(Session session);

    /// <summary>
    /// Batch create or update chat messages and session.
    /// </summary>
    /// <param name="messages">Chat message and session items to create or replace.</param>
    Task UpsertSessionBatchAsync(params dynamic[] messages);

    /// <summary>
    /// Batch deletes an existing chat session and all related messages.
    /// </summary>
    /// <param name="sessionId">Chat session identifier used to flag messages and sessions for deletion.</param>
    Task DeleteSessionAndMessagesAsync(string sessionId);

    /// <summary>
    /// Inserts a product into the product container.
    /// </summary>
    /// <param name="product">Product item to create.</param>
    /// <returns>Newly created product item.</returns>
    Task<Product> InsertProductAsync(Product product);

    /// <summary>
    /// Deletes a product by its Id and category (its partition key).
    /// </summary>
    /// <param name="productId">The Id of the product to delete.</param>
    /// <param name="categoryId">The category Id of the product to delete.</param>
    /// <returns></returns>
    Task DeleteProduct(string productId, string categoryId);

    /// <summary>
    /// Reads all documents retrieved by Vector Search.
    /// </summary>
    /// <param name="vectorDocuments">List string of JSON documents from vector search results</param>
    Task<string> GetVectorSearchDocumentsAsync(List<DocumentVector> vectorDocuments);
}