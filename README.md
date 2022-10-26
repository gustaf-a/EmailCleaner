# EmailCleaner

An application which collects emails and groups them in different ways to make it easy to clean the inbox.

Built using an API gateway microservices pattern.

Currently only supporting GMail.

## About

### MailCollectorService

Runs a collector which retrieves emails and publishes the emails to the RabbitMQ event queue.

## Installation and setup

### RabbitMQ between MailCollectorService and MailProviderService

Here's a good guide for more indepth instructions:
https://itnext.io/how-to-build-an-event-driven-asp-net-core-microservice-architecture-e0ef2976f33f

1. Start a docker container with RabbitMQ using enabling port 5672
2. Setup RabbitMQ queue.
3. Add EventQueue options to appsettings in MailCollectorService

Example EventQueue-section from appsettings.json in MailCollectorService:
```
     "EventQueue": {
        "Exchange": "emails",
        "RoutingKey": "emails.collected"
     }
```

### Google Cloud Project Setup

1. Go to https://console.cloud.google.com/ and create a new project
2. Add Google GMail API
3. Setup OAuth consent screen and add "Gmail API" scope to read and delete emails
4. Add yourself as a test user
5. Create OAuth2 credentials and download json-file and add to MailCollectorService
6. Add Gmail options in appsettings.json in MailCollectorService

Example Gmail-section from appsettings.json in MailCollectorService:
```
      "Gmail": {
        "ApplicationName": "EmailCleaner",
        "CredentialsFileName": "credentials.json",
        "UserId": "me"
      }
```

## Future development

### MailCollectorService
- Use batch requests instead. No difference for limit per minute, but faster.

