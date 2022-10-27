# EmailCleaner

An application which collects emails and groups them in different ways to make it easy to clean the inbox.
This is a practice project built with a tech stack and architecture best suited for my own curiosity 
and learning rather than performance.

Features: 
- Microservices in docker containers built around an API Gateway pattern
- RabbitMQ between two microservices for async communication
- Service tests

Currently only supporting GMail.

## About

### MailCollectorService

Runs a collector which retrieves emails and publishes the emails to the RabbitMQ event queue.

## Installation and setup

### RabbitMQ between MailCollectorService and MailProviderService

1. Start a docker container with RabbitMQ using enabling port 5672
2. Setup RabbitMQ queue.

Here's a good guide for more indepth instructions:
https://itnext.io/how-to-build-an-event-driven-asp-net-core-microservice-architecture-e0ef2976f33f

### MailProviderService

1. Add MessageQueue options to appsettings in MailProviderService. Get exchange from RabbitMQ queue created.

Example MessageQueue-section from appsettings.json in MailProviderService:
```
      "MessageQueue": {
        "Exchange": "emails",
        "HostName": "localhost",
        "RoutingKeyCollected": "emails.collected"
      }
```

### MailCollectorService

1. Add MessageQueue options to appsettings in MailCollectorService. Get exchange from RabbitMQ queue created.

Example MessageQueue-section from appsettings.json in MailCollectorService:
```
      "MessageQueue": {
        "Exchange": "emails",
        "HostName": "localhost",
        "RoutingKeyCollected": "emails.collected"
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

- Create docker-compose.yml 
- Create Platform library for microservices startup

### API Gateway

- Implement an authentication microservice for when connecting with the gateway, for example with JWT auth.


### MailCollectorService

- Use batch requests instead. No difference for limit per minute, but faster.
- Download attachments?
- 

### MailProviderService

- 

