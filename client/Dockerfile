### STAGE 1: Build ###
FROM node:15.7.0-alpine AS build
WORKDIR /usr/src/app
COPY package.json package-lock.json ./
RUN npm install
COPY . .
RUN npm run build --prod

### STAGE 2: Run ###
FROM nginx:1.19.6-alpine
COPY nginx.conf /etc/nginx/nginx.conf
COPY --from=build /usr/src/app/dist /usr/share/nginx/html

#FROM node:15.4.0-alpine3.10

#WORKDIR /app/

#COPY package.json package-lock.json ./

#RUN npm install -g @angular/cli @angular-devkit/build-angular && npm install

#COPY . .
#EXPOSE 4201

#RUN npm run build --prod

#CMD ["npm", "start"]
