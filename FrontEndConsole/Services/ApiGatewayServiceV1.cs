﻿using EmailCleaner.Client.Data;
using FrontEndConsole.Model.Configuration;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace FrontEndConsole.Services;

public class ApiGatewayServiceV1 : IApiGatewayService
{
    private readonly HttpClient _httpClient;

    private readonly ApplicationOptions _applicationOptions;

    public ApiGatewayServiceV1(IConfiguration configuration, HttpClient httpClient)
    {
        _applicationOptions = configuration.GetSection(ApplicationOptions.Application).Get<ApplicationOptions>();

        _httpClient = httpClient;
    }

    public async Task<List<EmailData>> GetEmails()
    {
        var result = new List<EmailData>();

        var randomNumber = Random.Shared.Next(0, 200);
        var randomNumber2 = Random.Shared.Next(randomNumber, 250);

        for (int i = randomNumber; i < randomNumber2; i++)
        {
            result.Add(new EmailData
            {
                Id = i,
                SenderAddress = $"sender{i}@email.com",
                ThreadId = i,
                Tags = new() { "Tag1", "Tag2" },
                Subject = $"Message from me to you {i}"
            });
        }

        return result;
    }

    public async Task StartCollecting()
    {
        Log.Information("Sending request to start collecting emails.");

        Log.Information("Fake request sent.");
        //TODO
        //_httpClient.BaseAddress = new Uri($"http:/{_applicationOptions.ApiGatewayHostName}/{port}/v1/emailcleaner/collect/start");
        return;
    }

    public async Task StopCollecting()
    {
        Log.Information("Sending request to stop collecting emails.");
        //Todo Call ApiGatewayService
        
        Log.Information("Fake request sent.");
        
        return;
    }
}
