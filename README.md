# Travel agency site

## Description

<p> This project aims to create a convenient and accessible platform for tourists looking for unforgettable travel experiences. The main features include a catalogue of tours with detailed descriptions and photos of holiday 
destinations, a convenient search engine to find the perfect option, the ability to book tours online. In addition, users can create personal accounts to easily track their 
bookings, manage their favourite tours and save their bucket lists. This website is integrated with secure payment systems, which guarantees the reliability of transactions and the convenience of the payment process.</p>

## Technologies

- ![.NET](https://img.shields.io/badge/dotnet-8F2D97?style=for-the-badge&logo=dotnet&logoColor=white)
- ![VUE.JS](https://img.shields.io/badge/vue.js-%2335495e.svg?style=for-the-badge&logo=vuedotjs&logoColor=%234FC08D)
- ![MicrosoftSQLServer](https://img.shields.io/badge/Microsoft%20SQL%20Server-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white)
- ![REDIS](https://img.shields.io/badge/redis-%23DD0031.svg?style=for-the-badge&logo=redis&logoColor=white)
- ![DOCKER](https://img.shields.io/badge/DOCKER-blue?style=for-the-badge&logo=docker&logoColor=white)
- ![RabbitMQ](https://img.shields.io/badge/Rabbitmq-FF6600?style=for-the-badge&logo=rabbitmq&logoColor=white)
- ![Azure](https://img.shields.io/badge/azure-%230072C6.svg?style=for-the-badge&logo=microsoftazure&logoColor=white)
- ![STRIPE](https://img.shields.io/badge/Stripe-2871EA?style=for-the-badge&logo=stripe&logoColor=white)
- ![Postman](https://img.shields.io/badge/Postman-FF6C37?style=for-the-badge&logo=postman&logoColor=white)

## Run

- Download the repository to your PC
```
git clone https://github.com/SodolynskyiVadym/TravelAgency.git
```
<br>

- Enter your data in files appsettings.json and config.env

File [appsettings.json](TravelAgencyAPI/TravelAgencyAPIServer/appsettings.json) (TravelAgencyAPI)
> - Stripe(private key)
> - ConnectionStrings(if you run by docker you haven't to do it)
> - AuthSetting
> - RabbitMqSetting

File [appsettings.json](TravelAgencyService/appsettings.json) (TravelAgencyService)
> - MailSetting
> - RabbitMqSetting

File [config.env](travel-agency-front/config.env) (travel_agency_front)
> - STRIPE_PUBLIC_KEY
<br>

- Run docker-compose
```
docker-compose up -d
```
