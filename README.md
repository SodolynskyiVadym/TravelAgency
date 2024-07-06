# Travel agency site

## Description

<p> his project aims to create a convenient and accessible platform for tourists looking for unforgettable travel experiences. The main features include a catalogue of tours with detailed descriptions and photos of holiday 
destinations, a convenient search engine to find the perfect option, the ability to book tours online. In addition, users can create personal accounts to easily track their 
bookings, manage their favourite tours and save their bucket lists. This website is integrated with secure payment systems, which guarantees the reliability of transactions and the convenience of the payment process.</p>

## Technologies

- ![.NET](https://img.shields.io/badge/dotnet-8F2D97?style=for-the-badge&logo=dotnet&logoColor=white)
- ![VUE.JS](https://img.shields.io/badge/VUE.JS-1AC82F?style=for-the-badge&logo=vuedotjs&logoColor=white)
- ![MSSQL](https://img.shields.io/badge/MSSQL-red?style=for-the-badge&logo=link&logoColor=white)
- ![REDIS](https://img.shields.io/badge/REDIS-FF0000?style=for-the-badge&logo=link&logoColor=white)
- ![DOCKER](https://img.shields.io/badge/DOCKER-blue?style=for-the-badge&logo=docker&logoColor=white)
- ![STRIPE](https://img.shields.io/badge/Stripe-2871EA?style=for-the-badge&logo=stripe&logoColor=white)

## Run

- Download the repository to your PC
```
git clone https://github.com/SodolynskyiVadym/TravelAgency.git
```
<br>

- Enter your data in file appsettings.json and config.env

File [appsettings.json](TravelAgencyAPI/appsettings.json)
> - MailSetting
> - Stripe(private key)
> - ConnectionStrings(if you run by docker you haven't to do it)
> - **Don't change AuthSetting(passwords of existed users will be incorrect)**

File [config.env](travel_agency_front/config.env)
> - STRIPE_PUBLIC_KEY
<br>

- Run docker-compose
```
docker-compose up -d
```
