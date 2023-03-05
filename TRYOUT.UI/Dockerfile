FROM node:14 AS build
WORKDIR /app

COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build

FROM nginx:alpine
COPY --from=build /app/dist/tryout.ui /usr/share/nginx/html
EXPOSE 80
