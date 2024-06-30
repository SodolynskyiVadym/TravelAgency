# Используем официальный образ Node.js
FROM node:alpine

# Устанавливаем рабочую директорию для установки зависимостей и копирования исходного кода
WORKDIR /usr/src/app

# Копируем файлы package.json и package-lock.json (если есть)
COPY ./package*.json .

# Устанавливаем зависимости
RUN npm install

# Копируем исходный код приложения
COPY . .

# Экспонируем порт, который используется вашим приложением Vue.js
EXPOSE 8080

# Команда для запуска сервера Vue.js
CMD npm run serve